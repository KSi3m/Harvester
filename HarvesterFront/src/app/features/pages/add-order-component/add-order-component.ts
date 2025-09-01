import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectModule } from 'primeng/select';
import { Combine } from '../../../core/models/Combine';
import { CombineService } from '../../../core/services/combineService/combine-service';
import { FieldService } from '../../../core/services/fieldService/field-service';
import { CardModule } from 'primeng/card';
import { Field } from '../../../core/models/Field';
import { DatePickerModule } from 'primeng/datepicker';

@Component({
  selector: 'app-add-order-component',
  imports: [
    SelectModule,
    ReactiveFormsModule,
    MultiSelectModule,
    CardModule,
    DatePickerModule,
  ],
  templateUrl: './add-order-component.html',
  styleUrl: './add-order-component.scss',
})
export class AddOrderComponent implements OnInit {
  fieldService = inject(FieldService);
  combineService = inject(CombineService);
  messageService = inject(MessageService);
  fb = inject(FormBuilder);
  form!: FormGroup;

  combines: Combine[] = [];
  fields: Field[] = [];

  minDate = new Date();
  maxDate: Date = new Date(new Date().getFullYear(), 11, 31);

  ngOnInit(): void {
    this.combineService.getCombines().subscribe({
      next: (res) => {
        this.combines = res;
        console.log(res);
        console.log(this.combines);
      },
      error: (err) => {
        console.log(err);
      },
    });
    this.fieldService.getFields().subscribe({
      next: (res) => {
        this.fields = res;
        console.log(this.fields);
      },
      error: (err) => {
        console.log(err);
      },
    });

    this.form = this.fb.group({
      selectedCombine: [null],
      selectedField: [null],
      selectedDate: [null],
    });
  }
  get f() {
    return this.form!.controls;
  }
  onSubmit() {
    console.log(this.f['selectedCombine'].value!);
  }
}
