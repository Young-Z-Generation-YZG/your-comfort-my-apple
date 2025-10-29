import { fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import envConfig from '~/src/infrastructure/config/env.config';
import { RootState } from '../redux/store';

export const baseQuery = (service: string) =>
   fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + service,
      prepareHeaders: (headers, { getState }) => {
         const accessToken = (getState() as RootState).auth.accessToken;

         if (accessToken) {
            headers.set('Authorization', `Bearer ${accessToken}`);
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
   });
