import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';

import { Observable } from 'rxjs';
import { timeout } from 'rxjs/operators';

import { AuthService } from './auth.service';
import { environment } from '../../../environments/environment';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

    constructor(
        private authService: AuthService
    ) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (!req.headers.get('authorization'))
            req = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${this.authService.token}`
                }
            });
        return next.handle(req);
    }
}