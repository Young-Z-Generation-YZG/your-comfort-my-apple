export interface IIphoneModel {
  name: string;
  order: number;
}

export interface IIphoneColor {
  name: string;
  color_hash: string;
  order: number;
}

export interface IIphoneStorage {
  name: string;
  value: number;
}

export interface IImage {
  url: string;
  id: string;
  order: number;
}

export interface IAverageRating {
  value: number;
  num_ratings: number;
}

export interface IStarRating {
  star: number;
  num_ratings: number;
}

export interface IGetAllIphoneModelsResponse {
  id: string;
  categoryId: string;
  promotionId: string;
  name: string;
  description: string;
  models: IIphoneModel[];
  colors: IIphoneColor[];
  storages: IIphoneStorage[];
  average_rating: IAverageRating;
  star_ratings: IStarRating[];
  product_items: [];
  images: IImage[];
  slug: string;
  state: string;
  created_at: string;
  updated_at: string;
}

export interface ICreateNewIPhoneModelPayload {
  name: string;
  description: string;
  models: IIphoneModel[];
  colors: IIphoneColor[];
  storages: number[];
  images: IImage[];
  categoryId: string;
  promotionId: string;
}

export interface IIphoneItemResponse {
  model: string;
  color: string;
  storage: number;
  description: string;
  price: number;
  quantity_in_stock: number;
  images: IImage[];
  productId: string;
  promotionId: string;
}

export interface ICreateNewIphoneItemPayload {
  model: string;
  color: string;
  storage: number;
  description: string;
  price: number;
  quantity_in_stock: number;
  images: IImage[];
  productId: string;
  promotionId: string;
}
