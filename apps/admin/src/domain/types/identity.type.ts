export type TUser = {
   id: string;
   tenant_id: string;
   branch_id: string;
   user_name: string;
   normalized_user_name: string;
   email: string;
   normalized_email: string;
   email_confirmed: boolean;
   phone_number: string;
   profile: TProfile;
   created_at: string;
   updated_at: string;
   updated_by: string;
   is_deleted: boolean;
   deleted_at: string;
   deleted_by: string;
};

export type TProfile = {
   id: string;
   user_id: string;
   first_name: string;
   last_name: string;
   full_name: string;
   phone_number: string;
   birth_day: string;
   gender: string;
   image_id: string;
   image_url: string | null;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

// Payloads
export interface IAddNewStaffPayload {
   email: string;
   password: string;
   first_name: string;
   last_name: string;
   birth_day: string;
   phone_number: string;
   role_name: string;
   tenant_id: string;
   branch_id: string;
}

export interface IAssignRolesPayload {
   user_id: string;
   roles: string[];
}
