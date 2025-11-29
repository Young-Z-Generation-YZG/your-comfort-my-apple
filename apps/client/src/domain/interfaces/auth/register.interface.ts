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

export interface IRegisterResponse {
   params: Array<object>;
   verification_type: string;
   token_expired_in: number;
}
