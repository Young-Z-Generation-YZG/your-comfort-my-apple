export interface ICloudinaryImage {
   original_filename: string | null;
   format: string;
   public_id: string;
   display_name: string;
   secure_url: string;
   length: number;
   bytes: number;
   width: number;
   height: number;
}

export interface IUploadPayload {
   images: File[];
}
