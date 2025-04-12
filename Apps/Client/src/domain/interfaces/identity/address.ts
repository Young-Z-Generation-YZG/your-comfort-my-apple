export interface IAddressPayload {
   contact_name: string;
   contact_email: string;
   contact_label: string;
   contact_phone_number: string;
   contact_address_line: string;
   contact_district: string;
   contact_province: string;
   contact_country: string;
}

export interface IAddressResponse {
   id: string;
   label: string;
   contact_name: string;
   contact_phone_number: string;
   address_line: string;
   district: string;
   province: string;
   country: string;
   is_default: boolean;
}
