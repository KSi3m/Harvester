import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function dateNotInPastValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (!value) return null;

    const selected = new Date(control.value);
    selected.setHours(0, 0, 0, 0);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return selected < today ? { dateInPast: true } : null;
  };
}
