import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { SelectModule } from 'primeng/select';
import { Field } from '../../../core/models/Field';
import { FieldDto, FieldService } from '../../../core/services/field-service';

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
  fb = inject(FormBuilder);
  isValid = false;

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    areaHectares: [null, [Validators.required, Validators.min(0.01)]],
    terrainCoeff: [null, [Validators.required]],
    shapeCoeff: [null, [Validators.required]],
    cropType: [null, [Validators.required]],
  });
  fields: Field[] = [];
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
    return this.form.controls;
  }

  ngOnInit(): void {
    this.fieldService.getFields().subscribe({
      next: (res) => {
        console.log(res);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
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
        this.form.reset();
      },
      error: (err) => {
        console.error(err);
      },
      complete: () => (this.isValid = false),
    });
  }
}
