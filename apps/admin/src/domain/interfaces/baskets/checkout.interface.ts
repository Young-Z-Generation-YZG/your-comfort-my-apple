export interface ICheckoutPayload {
   shipping_address: IShippingAddressPayload;
   payment_method: string;
   discount_code: string | null;
   sub_total_amount: number;
   discount_amount: number;
   total_amount: number;
}

export interface IShippingAddressPayload {
   contact_name: string;
   contact_phone_number: string;
   address_line: string;
   district: string;
   province: string;
   country: string;
}
