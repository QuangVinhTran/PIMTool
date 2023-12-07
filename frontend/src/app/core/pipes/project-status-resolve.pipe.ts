import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'resolveStatus'
})
export class ProjectStatusResolvePipe implements PipeTransform {
  transform(value: string, ...args: unknown[]): string {
    switch (value.toUpperCase()) {
      case 'NEW':
        return 'NEW';
      case 'PLA':
        return 'PLANNED'
      case 'INP':
        return 'IN_PROGRESS'
      case 'FIN':
        return 'FINISHED'
      default:
        return 'UNKNOWN'
    }
  }
}
