import { catchError, map } from 'rxjs/operators';
import { SpinnerService } from '../../shared-components-module/spinner/spinner.service';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {

    constructor(private spinner: SpinnerService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.spinner.show();
        return next.handle(req)
            .pipe(
                catchError((err) => {
                    this.spinner.hide();
                    return err;
                })
            ).pipe(map<HttpEvent<any>, any>((evt: HttpEvent<any>) => {
                if (evt instanceof HttpResponse) {
                    this.spinner.hide();
                }
                return evt;
            }));
    }

}