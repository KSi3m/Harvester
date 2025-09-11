import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import * as L from 'leaflet';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { SelectModule } from 'primeng/select';
import { FieldDto } from '../../../core/models/dtos/field-dto';
import { GeoMultiPolygonDto } from '../../../core/models/geoJson/geo-multiPolygon-dto';
import { GeoPointDto } from '../../../core/models/geoJson/geo-point-dto';
import { ErrorResponse } from '../../../core/models/responses/error-response';
import { GeoJsonDataForFieldResponse } from '../../../core/models/responses/geoJson-data-for-field-response';
import { FieldService } from '../../../core/services/fieldService/field-service';
import { fieldIdentifierValidator } from '../../../shared/validators/field-identifier-validator';

@Component({
  selector: 'app-add-field-form-component',
  imports: [
    CardModule,
    ReactiveFormsModule,
    InputTextModule,
    InputNumberModule,
    SelectModule,
    ButtonModule,
    MessageModule,
    DialogModule,
  ],
  templateUrl: './add-field-form-component.html',
  styleUrl: './add-field-form-component.scss',
})
export class AddFieldFormComponent implements OnInit {
  fieldService = inject(FieldService);
  messageService = inject(MessageService);
  fb = inject(FormBuilder);
  isValid = false;
  private map!: L.Map | null;

  tempResponse: GeoJsonDataForFieldResponse | null = null;

  modalVisible = false;
  form!: FormGroup;
  cropOptions = [
    { label: 'Corn', value: 'Corn' },
    { label: 'Wheat', value: 'Wheat' },
    { label: 'Rapeseed', value: 'Rapeseed' },
    { label: 'Barley', value: 'Barley' },
    { label: 'Oat', value: 'Oat' },
    { label: 'Rye', value: 'Rye' },
    { label: 'Triticale', value: 'Triticale' },
  ];
  terrainOptions = [
    { label: 'Steep', value: 0.75 },
    { label: 'Undulating', value: 0.6 },
    { label: 'Flat', value: 1.0 },
  ];

  shapeOptions = [
    { label: 'Rectangle', value: 1.0 },
    { label: 'Irregular', value: 0.6 },
  ];

  get f() {
    return this.form!.controls;
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      identifierName: [
        null,
        [
          Validators.required,
          Validators.maxLength(100),
          fieldIdentifierValidator(),
        ],
      ],
      commonName: [null, [Validators.maxLength(100)]],
      areaHectares: [
        null as number | null,
        [Validators.required, Validators.min(0.01)],
      ],
      terrainCoeff: [null, [Validators.required]],
      shapeCoeff: [null, [Validators.required]],
      cropType: [null, [Validators.required]],
      centerPoint: [null],
      boundary: [null],
    });
    this.form!.get('identifierName')?.valueChanges.subscribe(() => {
      this.form!.get('areaHectares')?.setValue(null, { emitEvent: false });
      this.form!.get('centerPoint')?.setValue(null, { emitEvent: false });
      this.form!.get('boundary')?.setValue(null, { emitEvent: false });
      this.tempResponse = null;
    });
  }
  submit() {
    if (this.form!.invalid) {
      this.form!.markAllAsTouched();
      return;
    }

    const payload: FieldDto = {
      identifierName: this.f['identifierName'].value!,
      commonName: this.f['commonName'].value!,
      areaHectares: Number(this.f['areaHectares'].value),
      terrainCoeff: Number(this.f['terrainCoeff'].value),
      shapeCoeff: Number(this.f['shapeCoeff'].value),
      cropType: this.f['cropType'].value!,
      centerPoint: this.f['centerPoint'].value!,
      boundary: this.f['boundary'].value!,
    };
    //console.log(payload);

    this.isValid = true;
    this.fieldService.createField(payload).subscribe({
      next: () => {
        this.form!.reset();
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Operation completed!',
        });
      },
      error: (error: HttpErrorResponse) => {
        const err = error.error as ErrorResponse;

        this.messageService.add({
          severity: 'error',
          summary: err.title,
          detail: err.detail,
        });
      },
      complete: () => (this.isValid = false),
    });
  }

  onDialogShow() {
    const mapContainer = document.getElementById(`map`);
    if (!mapContainer) return;
    const centerPoint = this.tempResponse!.centerPoint;
    const boundary = this.tempResponse!.boundary;
    if (!this.map) {
      const map = L.map(`map`, {
        center: [centerPoint.coordinates[1], centerPoint.coordinates[0]],
        zoom: 14,
      });

      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
      }).addTo(map);

      const latLngs = boundary.coordinates.map((polygon) =>
        polygon.map((ring) =>
          ring.map(([lng, lat]) => [lat, lng] as [number, number]),
        ),
      );

      const polygon = L.polygon(latLngs);
      polygon.addTo(map);

      L.marker([centerPoint.coordinates[1], centerPoint.coordinates[0]]).addTo(
        map,
      );

      this.map = map;
    } else {
      setTimeout(() => this.map!.invalidateSize(), 200);
    }
  }
  onDialogHide() {
    if (this.map) {
      this.map.remove();
      this.map = null;
    }
  }

  showDialog() {
    this.fillArea();
  }

  setGeoJsonData() {
    const value = Number(this.tempResponse!.areaHectares);
    const centerPoint = this.tempResponse!.centerPoint as GeoPointDto;
    const boundary = this.tempResponse!.boundary as GeoMultiPolygonDto;
    this.form!.get('areaHectares')?.setValue(value);
    this.form!.get('centerPoint')?.setValue(centerPoint);
    this.form!.get('boundary')?.setValue(boundary);
    this.modalVisible = false;
  }

  fillArea() {
    const identifier = this.form!.get('identifierName')
      ?.value as unknown as string;
    if (!identifier) {
      this.f['identifierName'].markAsTouched();
      return;
    }
    if (this.tempResponse) {
      this.modalVisible = true;
      return;
    }

    this.fieldService.getGeoJsonDataForField(identifier).subscribe({
      next: (res) => {
        this.tempResponse = res;
        this.modalVisible = true;
      },
      error: (error: HttpErrorResponse) => {
        const err = error.error as ErrorResponse;

        this.messageService.add({
          severity: 'error',
          summary: err.title,
          detail: err.detail,
        });
      },
    });
  }
}
