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
