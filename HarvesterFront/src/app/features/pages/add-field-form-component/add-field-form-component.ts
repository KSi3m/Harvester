import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { Field } from '../../../core/models/Field';
import { FieldService } from '../../../core/services/field-service';

@Component({
  selector: 'app-add-field-form-component',
  imports: [
    CardModule,
    ReactiveFormsModule,
    InputTextModule,
    InputNumberModule,
    SelectModule,
    ButtonModule,
  ],
  templateUrl: './add-field-form-component.html',
  styleUrl: './add-field-form-component.scss',
})
export class AddFieldFormComponent implements OnInit {
  fieldService = inject(FieldService);
  fb = inject(FormBuilder);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    areaHectares: [null, [Validators.required, Validators.min(0.01)]],
    terrainCoeff: [
      1,
      [Validators.required, Validators.min(0), Validators.max(1)],
    ],
    shapeCoeff: [
      1,
      [Validators.required, Validators.min(0), Validators.max(1)],
    ],
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
    console.log('Submit');
  }
}
