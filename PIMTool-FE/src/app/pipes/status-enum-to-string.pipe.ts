import { Pipe, PipeTransform } from '@angular/core';
import { getStatusString } from '../utils/getStatusString';

@Pipe({
  name: 'statusEnumToString',
})
export class StatusEnumToStringPipe implements PipeTransform {
  transform(value: any): any {
    return getStatusString(value);
  }
}
