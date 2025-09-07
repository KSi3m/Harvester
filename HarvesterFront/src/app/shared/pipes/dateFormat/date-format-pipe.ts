import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateFormatPipe',
})
export class DateFormatPipe implements PipeTransform {
  transform(value: Date | string | number, format = 'YYYY-MM-DD'): string {
    if (!value) return '';

    const date = new Date(value);

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    switch (format) {
      case 'DD.MM.YYYY':
        return `${day}.${month}.${year}`;
      case 'YYYY-MM-DD':
        return `${year}-${month}-${day}`;
      default:
        return date.toDateString(); // fallback
    }
  }
}
