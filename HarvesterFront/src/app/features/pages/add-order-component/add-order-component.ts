import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MessageService } from 'primeng/api';
import { CardModule } from 'primeng/card';
import { DatePickerModule } from 'primeng/datepicker';
import { MessageModule } from 'primeng/message';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectModule } from 'primeng/select';
import { SelectButtonModule } from 'primeng/selectbutton';
import { Combine } from '../../../core/models/Combine';
import { CreateOrderDto } from '../../../core/models/dtos/CreateOrderDto';
import { Field } from '../../../core/models/Field';
import { CreateOrderResponse } from '../../../core/models/responses/create-order-response';
import { CombineService } from '../../../core/services/combineService/combine-service';
import { FieldService } from '../../../core/services/fieldService/field-service';
import { OrderService } from '../../../core/services/orderService/order-service';
import { dateNotInPastValidator } from '../../../shared/validators/dateNotInPastValidator';

@Component({
  selector: 'app-add-order-component',
  imports: [
    SelectModule,
    ReactiveFormsModule,
    MultiSelectModule,
    CardModule,
    DatePickerModule,
    SelectButtonModule,
    MessageModule,
  ],
  templateUrl: './add-order-component.html',
  styleUrl: './add-order-component.scss',
})
export class AddOrderComponent implements OnInit {
  fieldService = inject(FieldService);
  combineService = inject(CombineService);
  orderService = inject(OrderService);
  messageService = inject(MessageService);
  fb = inject(FormBuilder);
  form!: FormGroup;

  combines: Combine[] = [];
  fields: Field[] = [];

  minDate = new Date();
  maxDate: Date = new Date(new Date().getFullYear(), 11, 31);

  strawMethods = [
    { label: 'Chop straw', value: 'CHOP' },
    { label: 'Leave straw in windrows', value: 'LEAVE' },
  ];

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
      selectedCombine: [null, [Validators.required]],
      selectedField: [null, [Validators.required]],
      selectedDate: [null, [Validators.required, dateNotInPastValidator()]],
      selectedStrawMethod: [null, [Validators.required]],
    });
  }
  get f() {
    return this.form!.controls;
  }
  onSubmit() {
    const control = this.f['selectedDate'] as FormControl;
    control?.updateValueAndValidity();

    if (this.form!.invalid) {
      this.form!.markAllAsTouched();
      return;
    }
    const payload: CreateOrderDto = {
      fieldId: Number(this.f['selectedField'].value!),
      combineId: Number(this.f['selectedCombine'].value),
      orderDate: this.f['selectedDate'].value,
      strawProcessingMethod: this.f['selectedStrawMethod'].value,
    };

    this.orderService.createOrder(payload).subscribe({
      next: (res: CreateOrderResponse) => {
        if (res.success) {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Order added!',
          });
          this.form!.reset();
        } else {
          this.messageService.add({
            severity: 'error',
            summary: res.ruleName,
            detail: res.details,
          });
        }
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 422) {
          const err = error.error as CreateOrderResponse;
          this.messageService.add({
            severity: 'error',
            summary: err.ruleName || 'Validation Error',
            detail: err.details || 'Check your input',
          });
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: error.message || 'Server error',
          });
        }
      },
    });
  }
}
