import { VERIFICATION_TYPES } from '~/domain/enums/verification-type.enum';

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
   verification_type: VERIFICATION_TYPES;
   params: Object | null;
}
