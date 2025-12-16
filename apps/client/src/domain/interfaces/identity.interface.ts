import { EVerificationTypeEnum } from '~/domain/enums/verification-type.enum';

export interface IUpdateProfilePayload {
   first_name: string;
   last_name: string;
   email: string;
   phone_number: string;
   birth_day: string;
   gender: string;
}

export interface IAddressPayload {
   label: string;
   contact_name: string;
   contact_phone_number: string;
   address_line: string;
   district: string;
   province: string;
   country: string;
}

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
   verification_type: EVerificationTypeEnum;
   params: object | null;
}

export interface IOtpPayload {
   email: string;
   token: string;
   otp: string;
}

export interface ISendEmailResetPasswordPayload {
   email: string;
}

export interface IResetPasswordPayload {
   email: string;
   token: string;
   new_password: string;
   confirm_password: string;
}

export interface IChangePasswordPayload {
   old_password: string;
   new_password: string;
}

export interface IRegisterPayload {
   email: string;
   password: string;
   confirm_password: string;
   first_name: string;
   last_name: string;
   phone_number: string;
   birth_date: string;
   country: string;
}

export interface IRefreshTokenResponse {
   access_token: string;
   refresh_token: string;
   access_token_expires_in: number;
}

export interface IRegisterResponse {
   params: Array<object>;
   verification_type: string;
   token_expired_in: number;
}

export type TEmailVerificationResponse = {
   params: {
      _email: string;
      _token: string;
   };
   verification_type: string;
   token_expired_in: number;
};

export interface IVerifyOtpPayload {
   email: string;
   token: string;
   otp: string;
}
