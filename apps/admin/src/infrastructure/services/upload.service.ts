import { createApi } from '@reduxjs/toolkit/query/react';
import { ICloudinaryImage } from '~/src/domain/interfaces/common/ICloudinaryImage';
import { baseQueryHandler } from '~/src/infrastructure/services/base-query';

export const uploadImageApi = createApi({
   reducerPath: 'upload-image-api',
   tagTypes: ['Upload'],
   baseQuery: (args, api, extraOptions) => {
      return baseQueryHandler(args, api, extraOptions, 'catalog-services');
   },
   endpoints: (builder) => ({
      getImagesAsync: builder.query<ICloudinaryImage[], void>({
         query: () => '/api/v1/upload/images',
         providesTags: ['Upload'],
      }),
   }),
});

export const { useGetImagesAsyncQuery } = uploadImageApi;
