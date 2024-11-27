import { environments } from "~/environments";
import { urlSerializer } from "../url-serializer";
import { IHeaderOptions } from "./header-options.interface";
import Cookies from "js-cookie";

export const get = async (
  url: string,
  params: Record<string, string> = {},
  options: IHeaderOptions = {}
) => {
  const serializeUrl = urlSerializer(url, params);
  const baseUrl = environments.BACKEND_CATALOG_SERVICE_API_URL;
  const fullUrl = `${baseUrl}/${serializeUrl}`;

  const response = await fetch(fullUrl, {
    method: "GET",
    headers: {
      "Content-Type": "application-json",
      Authorization: `Bearer ${Cookies.get("access-token")}`,
      ...options,
    },
  });

  return await response.json();
};

export const post = () => {};
