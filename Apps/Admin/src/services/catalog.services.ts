import {
  ICreateNewIPhoneModelPayload,
  IGetAllIphoneModelsResponse,
} from "~/types/api-types/catalog.type";
import { get, post } from "~/utils/http-request";

export const getAllProductAsync = async (): Promise<
  IGetAllIphoneModelsResponse[]
> => {
  const response = await get("products");

  return response;
};

export const createIphoneModelAsync = async (
  payload: ICreateNewIPhoneModelPayload
): Promise<boolean> => {
  const response = await post("products", payload);

  return response;
};
