import { fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import envConfig from '~/src/infrastructure/config/env.config';
import { RootState } from '../redux/store';

const baseQuery = (args: any, api: any, extraOptions: any, service: string) => {
   console.log('envConfig', envConfig.API_ENDPOINT);

   return fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + service,
      prepareHeaders: (headers, { getState }) => {
         const accessToken = (getState() as RootState).auth.value.accessToken;

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
      responseHandler: (response) => {
         return response.json();
      },
   });
};

export const baseQueryHandler = async (
   args: any,
   api: any,
   extraOptions: any,
   service: string,
) => {
   const baseQueryFn = baseQuery(args, api, extraOptions, service);
   const response = await baseQueryFn(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (response.error && response.error.status === 401) {
      // Dispatch logout action to clear auth state
      //   api.dispatch(setLogout());
   }

   return response;
};
