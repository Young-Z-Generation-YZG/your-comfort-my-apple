import { EVerificationType } from '~/domain/enums/verification-type.enum';

export interface ILoginPayload {
   email: string;
   password: string;
}

export interface ILoginResponse {
   user_email: string;
   username: string;
   access_token: string | null;
   refresh_token: string | null;
   access_token_expires_in: number | null;
   verification_type: EVerificationType;
   params: object | null;
}
