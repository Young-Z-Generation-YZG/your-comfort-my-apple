export interface IIphoneModel {
  name: string;
  order: number;
}

export interface IIphoneColor {
  name: string;
  color_hash: string;
  order: number;
}

export interface IImage {
  url: string;
  id: string;
  order: number;
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
