export interface IRequestAuthorizationCodeGrantPayload {
   grant_type: string;
   client_id: string;
   client_secret: string;
   code: string;
   state?: string;
   redirect_uri: string;
}

export interface IAuthorizationCodeResponse {
   access_token: string;
   token_type: string;
   expires_in: number;
   refresh_token: string;
   scope: string;
   id_token: string;
}
