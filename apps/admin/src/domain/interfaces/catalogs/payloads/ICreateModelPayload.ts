export interface IModelItemPayload {
   model_name: string;
   model_order: number;
}

export interface IColorItemPayload {
   color_name: string;
   color_hex: string;
   color_image: string;
   color_order: number;
}

export interface IImagePayload {
   image_id: string;
   image_url: string;
   image_name: string;
   image_description: string;
   image_width: number;
   image_height: number;
   image_bytes: number;
   image_order: number;
}

export interface ICreateModelPayload {
   name: string;
   models: IModelItemPayload[];
   colors: IColorItemPayload[];
   storages: number[];
   description: string;
   description_images: IImagePayload[];
   category_id: string;
}
