export interface IAddressPayload {
   label: string;
   contact_name: string;
   contact_phone_number: string;
   address_line: string;
   district: string;
   province: string;
   country: string;
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
