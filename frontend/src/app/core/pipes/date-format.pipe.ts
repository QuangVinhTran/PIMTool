import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatUtcDate'
})
export class DateFormatPipe implements PipeTransform {
  DATE_FORMAT = 'yyyy-mm-dd'

  transform(value: any, ...args: unknown[]): unknown {
    const slicedDate = value.slice(0, this.DATE_FORMAT.length + 1)
    // format from yyyy-mm-dd => dd.mm.yyyy
    return `${slicedDate.slice(8, 10)}.${slicedDate.slice(5, 7)}.${slicedDate.slice(0, 4)}`
  }
}
