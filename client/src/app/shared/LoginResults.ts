export class LoginResults {
    token: string = "";
    expiration: Date = new Date();
}

export class LoginRequest {
    username: string | undefined;
    password: string | undefined;
}