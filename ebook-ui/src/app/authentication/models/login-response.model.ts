export class Response {
    message: string;
    success: boolean;
}

export class LoginResponse extends Response {
    token: string;
}