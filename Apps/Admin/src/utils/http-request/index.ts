import { environments } from '~/environments';
import { urlSerializer } from '../url-serializer';
import { IHeaderOptions } from './header-options.interface';
import Cookies from 'js-cookie';

export const get = async (
   url: string,
   params: Record<string, string> = {},
   options: IHeaderOptions = {},
) => {
   const serializeUrl = urlSerializer(url, params);

   const env = process.env.NODE_ENV;

   // const baseUrl =
   //   env === "production"
   //     ? environments.PROD_BACKEND_CATALOG_SERVICE_API_URL
   //     : environments.BACKEND_CATALOG_SERVICE_API_URL;

   const baseUrl = 'http://localhost:17070';

   const fullUrl = `${baseUrl}/${serializeUrl}`;

   const response = await fetch(fullUrl, {
      method: 'GET',
      headers: {
         'Content-Type': 'application-json',
         Authorization: `Bearer ${Cookies.get('access-token')}`,
         ...options,
      },
   });

   return await response.json();
};

export const post = async (
   url: string,
   payload: {} | FormData,
   params?: Record<string, string>,
   options: IHeaderOptions = {},
) => {
   const serializeUrl = urlSerializer(url, params);
   const env = process.env.NODE_ENV;

   // const baseUrl =
   //   env === "production"
   //     ? environments.PROD_BACKEND_CATALOG_SERVICE_API_URL
   //     : environments.BACKEND_CATALOG_SERVICE_API_URL;

   const baseUrl = 'http://localhost:17070';

   const requestUrl = `${baseUrl}/${serializeUrl}`;

   const isFormData = payload instanceof FormData;

   const response = await fetch(requestUrl, {
      method: 'POST',
      headers: {
         'Content-Type': isFormData
            ? 'multipart/form-data'
            : 'application/json',
         Authorization: `Bearer ${Cookies.get('access-token')}`,
         ...options,
      },
      body: isFormData ? (payload as FormData) : JSON.stringify(payload),
   });

   const data = await response.json();

   return data;
};

export const postToken = async (payload: Record<string, any>) => {
   const baseUrl = environments.IDENTITY_PROVIDER_TOKEN_URL ?? '';

   const response = await fetch(baseUrl, {
      method: 'POST',
      headers: {
         'Content-Type': 'application/x-www-form-urlencoded',
      },
      body: new URLSearchParams(payload).toString(),
   });

   const data = await response.json();

   return data;
};
