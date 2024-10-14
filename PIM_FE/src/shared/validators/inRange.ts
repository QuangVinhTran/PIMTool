import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function inRange(startNumber: number, endNumber: number): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const number = parseInt(control.value);
    const inRange = number < startNumber || number > endNumber;
    return inRange ? { inRange: { min: startNumber, max: endNumber } } : null;
  };
}
