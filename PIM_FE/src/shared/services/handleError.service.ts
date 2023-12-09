import { throwError, Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ResponseDto } from '../models/responseDto';

@Injectable({
  providedIn: 'root',
})
export class HandleError {
  // handleError(error: any) {
  //   let errorMessage = '';
  //   if (error.error instanceof ErrorEvent) {
  //     // client-side error
  //     errorMessage = `Error: ${error.error.message}`;
  //   } else {
  //     // server-side error
  //     errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
  //   }
  //   console.log(errorMessage);
  //   return throwError(() => {
  //     return errorMessage;
  //   });
  // }
  public handleError(error: HttpErrorResponse): Observable<ResponseDto> {
    const responseDto = new ResponseDto(null, false, '');
    const errorCode = error.status;
    switch (errorCode) {
      case 0:
        responseDto.error = "Client have some error !";
        break;
      case 400:
        responseDto.error = "Please check your data";
        break;
      case 401:
        responseDto.error = 'You must be login';
        break;
      case 403:
        responseDto.error = 'You are not have unauthorized';
        break;
      case 404:
        responseDto.error = 'Not found!';
        break;
      case 409:
        responseDto.error = 'Conflict data. Please try again';
        break;
      case 500:
        responseDto.error = 'Server have error!';
        break;
      default:
        break;
    }
    return of(responseDto);
  }
}
