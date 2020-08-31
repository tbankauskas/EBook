import { IJsonWebToken } from './json-web-token';

export interface AuthChangeEvent {
    event: 'login' | 'logout';
    rawJwt?: string;
    jwt?: IJsonWebToken;
    reason?: 'logout' | 'token_expired';
}
