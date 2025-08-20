import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { SelectModule } from 'primeng/select';
import {
  FieldDto,
  FieldService,
} from '../../../core/services/fieldService/field-service';
import { fieldIdentifierValidator } from '../../../shared/validators/field-validator';

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
  ],
  templateUrl: './add-field-form-component.html',
  styleUrl: './add-field-form-component.scss',
})
export class AddFieldFormComponent implements OnInit {
  fieldService = inject(FieldService);
  messageService = inject(MessageService);
  fb = inject(FormBuilder);
  isValid = false;

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
      name: [
        null,
        [
          Validators.required,
          Validators.maxLength(100),
          fieldIdentifierValidator(),
        ],
      ],
      areaHectares: [
        null as number | null,
        [Validators.required, Validators.min(0.01)],
      ],
      terrainCoeff: [null, [Validators.required]],
      shapeCoeff: [null, [Validators.required]],
      cropType: [null, [Validators.required]],
    });
    this.form!.get('name')?.valueChanges.subscribe(() => {
      this.form!.get('areaHectares')?.setValue(null, { emitEvent: false });
    });
  }
  submit() {
    if (this.form!.invalid) {
      this.form!.markAllAsTouched();
      return;
    }
    const payload: FieldDto = {
      name: this.f['name'].value!,
      areaHectares: Number(this.f['areaHectares'].value),
      terrainCoeff: Number(this.f['terrainCoeff'].value),
      shapeCoeff: Number(this.f['shapeCoeff'].value),
      cropType: this.f['cropType'].value!,
    };
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
      error: (err) => {
        console.error(err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Operation failed!',
        });
      },
      complete: () => (this.isValid = false),
    });
  }
  fillArea() {
    const identifier = this.form!.get('name')?.value as unknown as string;
    if (!identifier) {
      this.f['name'].markAsTouched();
      return;
    }
    this.fieldService.getAreaForField(identifier).subscribe({
      next: (res) => {
        const value = Number(res.areaInHectares);
        this.form!.get('areaHectares')?.setValue(value);
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed  failed!',
        });
      },
    });
  }
}
