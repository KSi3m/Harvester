import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function fieldIdentifierValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (!value) return null;

    const regex = /^\d{2}\d{2}\d{2}_\d\.\d{4}\.\d+(\/\d+)?$/;
    return regex.test(value) ? null : { invalidFieldId: { value } };
  };
}
