import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class CsrfHeaderInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (!request.headers.has("AnyX-CSRF")) {
      request = request.clone({
        headers: request.headers.set("AnyX-CSRF", "7"),
      });
    }

    return next.handle(request);
  }
}



