export interface IJsonWebToken {
    iss?: string;
    sub?: string;
    aud?: string;
    exp?: number;
    nbf?: number;
    iat?: number;
    jti?: string;
    uid?: number;
    roles?: string[];
    [key: string]: any;
    scopes: string[];
}
