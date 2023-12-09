import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environment/environment ';
import { User } from '../models/user';
import { ResponseDto } from '../models/responseDto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private httpClient: HttpClient) {}
  public login(username: string, password: string): Observable<ResponseDto> {
    const user: User = new User(username, password);
    return this.httpClient.post<ResponseDto>(
      `${environment.urlApi}/Auth`,
      user
    );
  }
  public getToken() : string | null {
    return localStorage.getItem('access_token');
  }
  public isLoggedIn() {
    return !!localStorage.getItem('access_token');
  }
  public logout() {
    localStorage.removeItem('access_token');
  }
}
