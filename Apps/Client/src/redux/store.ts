import { configureStore } from '@reduxjs/toolkit';
import { userApi } from '~/services/example/user.service';

export const store = configureStore({
   reducer: {
      [userApi.reducerPath]: userApi.reducer,
   },
   middleware: (getDefaultMiddleware) =>
      getDefaultMiddleware().concat(userApi.middleware),
});
