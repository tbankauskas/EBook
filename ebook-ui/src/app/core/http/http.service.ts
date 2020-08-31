import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHandler } from '@angular/common/http';


@Injectable({ providedIn: 'root' })
export class HttpService extends HttpClient {

    apiUrl = environment.apiUrl;
    constructor(protected httpHandler: HttpHandler) {
        super(httpHandler);
    }
}
