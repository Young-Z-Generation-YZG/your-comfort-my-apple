import { cookies } from 'next/headers';
import envConfig from '~/config/env.config';
import { IHeaderOptions } from './header-options.interface';
import { serializeUrl } from '../serialize';

const baseUrl = envConfig.client.NEXT_PUBLIC_API_URL + '/api/v1/';

export const get = async <IResponse>(
   url: string,
   params: Record<string, string> = {},
   options: IHeaderOptions = {},
) => {
   const serializedUrl = serializeUrl(url, params);

   const response = await fetch(baseUrl + serializedUrl, {
      method: 'GET',
      headers: {
         'Content-Type': 'application-json',
         ...(cookies().get('access-token')
            ? {
                 Authorization: `Bearer ${cookies().get('access-token')}`,
              }
            : {}),
         ...options,
      },
   });

   return (await response.json()) as IResponse;
};

export const post = async <IRequest>(
   url: string,
   body: string | FormData | IRequest,
   options: IHeaderOptions = {},
) => {
   const serializedUrl = serializeUrl(url);

   return await fetch(baseUrl + serializedUrl, {
      method: 'POST',
      headers: {
         'Content-Type': 'application-json',
         ...(cookies().get('access-token')
            ? {
                 Authorization: `Bearer ${cookies().get('access-token')}`,
              }
            : {}),
         ...options,
      },
      body: JSON.stringify(body),
   });
};
