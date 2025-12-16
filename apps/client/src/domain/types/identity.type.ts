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

export type TAccount = {
   email: string;
   first_name: string;
   last_name: string;
   phone_number: string;
   birth_date: string;
   image_id: string;
   image_url: string;
   default_address_label: string;
   default_contact_name: string;
   default_contact_phone_number: string;
   default_address_line: string;
   default_address_district: string;
   default_address_province: string;
   default_address_country: string;
};

export type TAddress = {
   id: string;
   label: string;
   contact_name: string;
   contact_phone_number: string;
   address_line: string;
   district: string;
   province: string;
   country: string;
   is_default: boolean;
};
