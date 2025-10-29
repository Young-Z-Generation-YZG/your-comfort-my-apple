import { createApi } from '@reduxjs/toolkit/query/react';
import { ICloudinaryImage } from '~/src/domain/interfaces/common/ICloudinaryImage';
import { baseQuery } from './base-query';
import { setLogout } from '../redux/features/auth.slice';

const baseQueryHandler = async (args: any, api: any, extraOptions: any) => {
   const result = await baseQuery('ordering-services')(args, api, extraOptions);

   // Check if we received a 401 Unauthorized response
   if (result.error && result.error.status === 401) {
      // Dispatch logout action to clear auth state
      api.dispatch(setLogout());
   }

   return result;
};

export const uploadImageApi = createApi({
   reducerPath: 'upload-image-api',
   tagTypes: ['Upload'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      getImagesAsync: builder.query<ICloudinaryImage[], void>({
         query: () => '/api/v1/upload/images',
         providesTags: ['Upload'],
      }),
   }),
});

export const { useGetImagesAsyncQuery } = uploadImageApi;
