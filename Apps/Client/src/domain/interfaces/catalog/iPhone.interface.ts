import { IColorResponse } from '../common/color-response.interface';
import { IImageResponse } from '../common/image-response.interface';
import { IStorageResponse } from '../common/storage-response.interface';

export interface IIphonePromotionResponse {
   product_id: string;
   product_model: string;
   product_color: IColorResponse;
   product_storage: IStorageResponse;
   product_unit_price: number;
   product_available_in_stock: number;
   product_description: string;
   product_images: IImageResponse[];
   product_slug: string;
}
