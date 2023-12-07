import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http'
import {Observable} from "rxjs";
import {getLocalAccessToken} from "../utils/localStorage.util";

export class HttpClientInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    req = req.clone({
      setHeaders: {
        'Authorization': `Bearer ${getLocalAccessToken()}`
      }
    })

    return next.handle(req)
  }
}
