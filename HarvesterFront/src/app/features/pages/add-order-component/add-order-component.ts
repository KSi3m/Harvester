import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DatePickerModule } from 'primeng/datepicker';
import { MessageModule } from 'primeng/message';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectModule } from 'primeng/select';
import { SelectButtonModule } from 'primeng/selectbutton';
import { Combine } from '../../../core/models/Combine';
import { CreateOrderDto } from '../../../core/models/dtos/create-order-dto';
import { FieldDto } from '../../../core/models/dtos/field-dto';
import { StrawProcessingMethod } from '../../../core/models/enums/straw-processing-method';
import { Field } from '../../../core/models/Field';
import { CreateOrderResponse } from '../../../core/models/responses/create-order-response';
import { CombineService } from '../../../core/services/combineService/combine-service';
import { FieldService } from '../../../core/services/fieldService/field-service';
import { OrderService } from '../../../core/services/orderService/order-service';
import { dateNotInPastValidator } from '../../../shared/validators/date-not-in-past-validator';

@Component({
  selector: 'app-add-order-component',
  imports: [
    SelectModule,
    ReactiveFormsModule,
    ButtonModule,
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
  estimatedPrice = 0;

  activatedRoute = inject(ActivatedRoute);
  idFromRoute!: number;
  isEditing = false;
  formReady = false;
  hasError = false;

  combines: Combine[] = [];
  fields: Field[] = [];

  minDate = new Date();
  maxDate: Date = new Date(new Date().getFullYear(), 11, 31);

  strawMethods = [
    { label: 'Chop straw', value: 'CHOP', disabled: false },
    { label: 'Leave straw in windrows', value: 'LEAVE', disabled: false },
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

    this.activatedRoute.paramMap.subscribe((res) => {
      this.formReady = false;
      const id = res.get('id');
      if (id != null) {
        this.isEditing = true;
        this.idFromRoute = Number(id);
        this.orderService.getOrderById(this.idFromRoute).subscribe({
          next: (res) => {
            if (res != null) {
              this.form.patchValue({
                selectedCombine: res.combine.id,
                selectedField: res.field.id,
                selectedDate: new Date(res.scheduledDate),
                selectedStrawMethod: res.strawProcessingMethod,
              });
            }
            this.formReady = true;
          },
          error: (err) => {
            console.log(err);
            this.hasError = true;
            this.formReady = false;
            //this.route.navigate(['/'])
          },
        });
      } else {
        this.isEditing = false;
        this.formReady = true;
      }
    });

    this.form.valueChanges.subscribe((formValue) => {
      const {
        selectedCombine,
        selectedField,
        selectedDate,
        selectedStrawMethod,
      } = formValue;

      if (!selectedCombine || !selectedField || !selectedStrawMethod) {
        this.estimatedPrice = 0;
        return;
      }

      const combine = this.combines.find((c) => c.id === selectedCombine);
      const field = this.fields.find((c) => c.id === selectedField);
      if (!combine || !field) {
        this.estimatedPrice = 0;
        return;
      }

      this.strawMethods = this.strawMethods.map((opt) => {
        if (opt.value === 'CHOP') {
          return { ...opt, disabled: !combine?.hasStrawChopper };
        }
        return opt;
      });
      if (
        !combine?.hasStrawChopper &&
        this.form.value.selectedStrawMethod === 'CHOP'
      ) {
        this.form.patchValue({ selectedStrawMethod: null });
      }

      this.estimatedPrice = this.calculatePrice(
        combine,
        field,
        selectedStrawMethod,
      );
    });
  }
  calculatePrice(
    combine: Combine,
    field: FieldDto,
    selectedStrawMethod: string,
  ): number {
    const priceForStrawMethod =
      selectedStrawMethod === StrawProcessingMethod.Chop ? 50 : 0;
    const total =
      field.areaHectares * (combine.pricePerHectare + priceForStrawMethod);

    return total;
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

    if (this.isEditing) {
      this.orderService.updateOrder(this.idFromRoute, payload).subscribe({
        next: (res: CreateOrderResponse) => {
          if (res.success) {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Order edited succesfully!',
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
    } else {
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
}
