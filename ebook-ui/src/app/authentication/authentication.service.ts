import { RegisterRequest } from './models/register-request.model';
import { AuthChangeEvent } from './interfaces/auth-change.event';
import { LoginResponse } from './models/login-response.model';
import { IJsonWebToken } from './interfaces/json-web-token';
import { environment } from '../../environments/environment';
import { Injectable, NgZone } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ReplaySubject, Observable } from 'rxjs';
import * as decode from 'jwt-decode';

import { Router } from '@angular/router';

const KEY_TOKEN = 'EBOOK:TOKEN';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

    private jwt: IJsonWebToken;
    public authChanged = new ReplaySubject<AuthChangeEvent>(1);
    private expireTimeout: number;

    public get userName(): string {
        return this.jwt && this.jwt.sub || null;
    }

    constructor(private http: HttpClient,
        private zone: NgZone,
        private router: Router) {
        if (this.getTokenFromLocalStorage()) {
            this.processToken(this.getTokenFromLocalStorage());
        }
    }

    login(login: string, password: string): ReplaySubject<IJsonWebToken> {
        const subj = new ReplaySubject<IJsonWebToken>();

        this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login/`, { login, password },
            {
                headers: new HttpHeaders({ 'Content-Type': 'application/json' })
            })
            .subscribe(response => {
                if (!response.success) {
                    subj.error(response.message);
                }
                if (this.processToken(response.token)) {
                    subj.next(this.jwt);
                    subj.complete();
                } else {
                    subj.error('Cannot decode JWT');
                }
            });
        return subj;
    }

    register(request: RegisterRequest): Observable<Response> {
        return this.http.post<Response>(`${environment.apiUrl}/auth/register/`, request);
    }

    logout(expired: boolean): void {
        localStorage.removeItem(KEY_TOKEN);
        this.jwt = null;
        this.authChanged.next({
            event: 'logout',
            reason: expired ? 'token_expired' : 'logout'
        });
    }

    isAuthenticated(): boolean {
        if (!this.jwt && this.isTokenExpired) {
            return false;
        }
        return true;
    }

    getTokenFromLocalStorage(): string {
        return localStorage.getItem(KEY_TOKEN);
    }

    private isTokenExpired(): boolean {
        if (this.jwt.exp < Date.now() / 1000) {
            return true;
        }
        return false;
    }

    private processToken(token: string): boolean {
        const { success, jwt } = this.decodeToken(token);
        if (success && !(jwt.exp < Date.now() / 1000)) {
            this.jwt = jwt;
            localStorage.setItem(KEY_TOKEN, token);
            this.setExpireTimeout(this.jwt.exp - Date.now() / 1000);
            this.authChanged.next({
                event: 'login',
                jwt: this.jwt,
                rawJwt: token
            });
            return true;
        }
        return false;
    }

    private decodeToken(jwt: string): { success: boolean, jwt: IJsonWebToken } {
        try {
            const decoded: IJsonWebToken = decode(jwt);

            return { success: true, jwt: decoded };
        } catch (err) {
            return { success: false, jwt: undefined };
        }
    }

    private clearExpireTimeout(): void {
        if (this.expireTimeout) {
            clearTimeout(this.expireTimeout);
        }
    }

    private setExpireTimeout(expireTimeSeconds: number): void {
        this.zone.runOutsideAngular(() => {
            this.clearExpireTimeout();
            this.expireTimeout = window.setTimeout(() => this.onTokenExpired(), expireTimeSeconds * 1000);
        });
    }
    private onTokenExpired(): void {
        this.zone.run(() => {
            this.logout(true);
            this.router.navigateByUrl('/login', { state: { expired: 'Session has expired. Please sign in again!' } });
        });
    }
}