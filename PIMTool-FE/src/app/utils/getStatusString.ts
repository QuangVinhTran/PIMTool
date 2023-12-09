import { statusEnum } from '../enums/statusEnum';

export function getStatusString(status: statusEnum): string {
  switch (status) {
    case statusEnum.NEW:
      return 'NEW';
    case statusEnum.PLA:
      return 'Planned';
    case statusEnum.INP:
      return 'In progress';
    case statusEnum.FIN:
      return 'Finish';
    default:
      return 'Unknown';
  }
}
