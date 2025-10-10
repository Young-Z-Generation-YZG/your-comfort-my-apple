export interface IColor {
   name: string;
   normalized_name: string;
   hex_code: string;
   showcase_image_id: string;
   order: number;
}

export interface IStorage {
   name: string;
   normalized_name: string;
   order: number;
}

export interface IModel {
   name: string;
   normalized_name: string;
   order: number;
}

export interface ISKUPrice {
   normalized_model: string;
   normalized_color: string;
   normalized_storage: string;
   unit_price: number;
}

export interface IImage {
   image_id: string;
   image_url: string;
   image_name: string;
   image_description: string;
   image_width: number;
   image_height: number;
   image_bytes: number;
   image_order: number;
}

export interface IRatingStar {
   star: number;
   count: number;
}

export interface IAverageRating {
   rating_average_value: number;
   rating_count: number;
}
