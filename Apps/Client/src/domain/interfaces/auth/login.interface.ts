export interface ILoginPayload {
   email: string;
   password: string;
}

export interface ILoginResponse {
   access_token: string;
   refresh_token: string;
   expiration: string;
}
