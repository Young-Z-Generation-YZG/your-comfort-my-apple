import { get } from "~/utils/http-request";

export const getAllProductAsync = async () => {
  const response = await get("products");

  return response;
};
