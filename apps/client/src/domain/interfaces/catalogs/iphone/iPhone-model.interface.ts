import {
   IAverageRating,
   IColor,
   IImage,
   IModel,
   IRatingStar,
   ISKUPrice,
   IStorage,
} from '../../common/value-objects.interface';
import { ICategory } from '../categories/category.interface';

export interface IIphoneModel {
   id: string;
   category: ICategory;
   name: string;
   model_items: IModel[];
   color_items: IColor[];
   storage_items: IStorage[];
   sku_prices: ISKUPrice[];
   description: string;
   showcase_images: IImage[];
   overall_sold: number;
   rating_stars: IRatingStar[];
   average_rating: IAverageRating;
   slug: string;
}
