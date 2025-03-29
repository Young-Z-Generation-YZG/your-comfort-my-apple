export interface ILoginPayload {
   email: string;
   password: string;
}

export interface ILoginResponse {
   user_email: string;
   access_token: string;
   refresh_token: string;
   expiration: string;
}
