import { AuthenticationService } from './../../authentication/authentication.service';
import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable} from 'rxjs';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(public authService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        if (this.authService.getTokenFromLocalStorage()) {
            request = this.addToken(request, this.authService.getTokenFromLocalStorage());
        }

        return next.handle(request);
    }

    private addToken(request: HttpRequest<any>, token: string): any {
        return request.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
        });
    }
}