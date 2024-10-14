import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from 'src/environment/environment ';
import { ResponseDto } from '../models/responseDto';
@Injectable({
  providedIn: 'root',
})
export class GroupService {
  private url = 'group/getAllId';
  constructor(private http: HttpClient) {}

  public getAllId(): Number[] {
    const list: Number[] = [];
    this.http
      .get<ResponseDto>(`${environment.urlApi}/${this.url}`)
      .subscribe((res: ResponseDto) => {
        res.data.forEach((element: number) => {
          list.push(element);
        });
      });
    return list;
  }
  public TestHandleError(): Observable<ResponseDto> {
    return this.http
      .get<ResponseDto>(`${environment.urlApi}/${this.url}`)
      .pipe(catchError(this.handleError));
  }
  private handleError(error: HttpErrorResponse) {
    const errorCode = error.status;
    switch (errorCode) {
      case 0: {
        // A client-side or network error occurred. Handle it accordingly.
        console.error('An error occurred:', error.error);
        break;
      }
      case 400: {
        console.log('Wrong request');
        break;
      }
      case 401: {
        console.log('Must be login');
        break;
      }
      case 403: {
        console.log('Do not have unauthorized');
        break;
      }
      case 404: {
        console.log('Not found');
        break;
      }
      case 409: {
        console.log('Conflict');
        break;
      }
      case 500: {
        console.log('Server have error');
        break;
      }
    }
    // The backend returned an unsuccessful response code.
    // The response body may contain clues as to what went wrong.
    console.error(
      `Backend returned code ${error.status}, body was: `,
      error.error
    );
    // Return an observable with a user-facing error message.
    return throwError(
      () => new Error('Something bad happened; please try again later.')
    );
  }
}
