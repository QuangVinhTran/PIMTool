import { ResponseDto } from './../../shared/models/responseDto';
import { catchError, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpHandler, HttpInterceptor, HttpRequest, HttpEvent, HttpResponse, HttpErrorResponse } from "@angular/common/http";
import { AuthService } from 'src/shared/services/auth.service';
@Injectable({
    providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor{
    constructor(private authService : AuthService) {}
    intercept(req: HttpRequest<any>, next : HttpHandler){
        const access_token = this.authService.getToken();
        const authReq = req.clone( {
            headers : req.headers.set('Authorization', "Bearer " + access_token || '')
        });
        return next.handle(authReq)
        // .pipe(
        //     catchError((err : HttpErrorResponse) => {
        //         const responseDto = new ResponseDto(null, false, "");
        //         return of(new HttpResponse());
        //     })
        // );
    }
}