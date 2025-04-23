import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import envConfig from '~/infrastructure/config/env.config';
import { RootState } from '../redux/store';

export const baseQuery = fetchBaseQuery({
   baseUrl: envConfig.API_ENDPOINT,
   prepareHeaders: (headers, { getState }) => {
      const accessToken = (getState() as RootState).auth.value.accessToken;

      if (accessToken) {
         headers.set('Authorization', `Bearer ${accessToken}`);
      }

      headers.set('ngrok-skip-browser-warning', 'true');

      return headers;
   },
});
