import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { SelectModule } from 'primeng/select';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { CreateCombineDto } from '../../../core/models/dtos/create-combine-dto';
import { ErrorResponse } from '../../../core/models/responses/error-response';
import { CombineService } from '../../../core/services/combineService/combine-service';

@Component({
  selector: 'app-add-combine-form-component',
  imports: [
    CardModule,
    ReactiveFormsModule,
    InputTextModule,
    InputNumberModule,
    SelectModule,
    ButtonModule,
    ToggleSwitchModule,
    MessageModule,
  ],
  templateUrl: './add-combine-form-component.html',
  styleUrl: './add-combine-form-component.scss',
})
export class AddCombineFormComponent implements OnInit {
  combineService = inject(CombineService);
  messageService = inject(MessageService);
  activatedRoute = inject(ActivatedRoute);
  fb = inject(FormBuilder);
  form!: FormGroup;
  router = inject(Router);

  idFromRoute!: number;
  isEditing = false;

  formReady = false;
  hasError = false;

  get f() {
    return this.form!.controls;
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      model: [null, [Validators.required, Validators.maxLength(100)]],
      baseHaPerHour: [
        null as number | null,
        [Validators.required, Validators.min(0.01)],
      ],
      headerLength: [
        null as number | null,
        [Validators.required, Validators.min(0.01)],
      ],
      isAvailable: [true],
      pricePerHectare: [
        null as number | null,
        [Validators.required, Validators.min(0), Validators.max(5000)],
      ],
      hasStrawChopper: [null, [Validators.required]],
      availableWorkHours: [
        null as number | null,
        [Validators.required, Validators.min(1), Validators.max(24)],
      ],
      baseEfficency: [1, [Validators.min(0.0), Validators.max(1)]],
    });

    this.activatedRoute.paramMap.subscribe((res) => {
      this.formReady = false;
      const id = res.get('id');
      if (id != null) {
        this.isEditing = true;
        this.idFromRoute = Number(id);
        this.combineService.getCombineById(this.idFromRoute).subscribe({
          next: (res) => {
            if (res != null) {
              this.form.patchValue({
                model: res.model,
                baseHaPerHour: res.baseHaPerHour,
                headerLength: res.headerLength,
                isAvailable: res.isAvailable,
                pricePerHectare: res.pricePerHectare,
                hasStrawChopper: res.hasStrawChopper,
                availableWorkHours: res.availableWorkHours,
                baseEfficency: res.baseEfficency,
              });
            }
            this.formReady = true;
          },
          error: (err) => {
            console.log(err);
            this.formReady = false;
            this.hasError = true;
            this.router.navigate(['/combines']);
          },
        });
      } else {
        this.isEditing = false;
        this.formReady = true;
      }
    });
  }
  submit() {
    if (this.form!.invalid) {
      this.form!.markAllAsTouched();
      return;
    }

    const payload: CreateCombineDto = {
      model: this.f['model'].value!,
      baseHaPerHour: Number(this.f['baseHaPerHour'].value),
      headerLength: Number(this.f['headerLength'].value),
      isAvailable: this.f['isAvailable'].value,
      pricePerHectare: Number(this.f['pricePerHectare'].value),
      hasStrawChopper: this.f['hasStrawChopper'].value!,
      availableWorkHours: this.f['availableWorkHours'].value!,
      baseEfficency: this.f['baseEfficency'].value,
    };

    if (this.isEditing) {
      this.combineService.updateCombine(this.idFromRoute, payload).subscribe({
        next: () => {
          this.form!.reset();
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: `Combine with id: ${this.idFromRoute} updated!`,
          });
          this.router.navigate(['/combines']);
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
    } else {
      this.combineService.createCombine(payload).subscribe({
        next: () => {
          this.form!.reset();
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'New combine added!',
          });
          this.router.navigate(['/combines']);
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
}
