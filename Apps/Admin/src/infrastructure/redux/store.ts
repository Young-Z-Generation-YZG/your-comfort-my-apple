import {
   persistStore,
   persistReducer,
   FLUSH,
   PAUSE,
   PERSIST,
   PURGE,
   REGISTER,
   REHYDRATE,
} from 'redux-persist';
import { PersistConfig } from 'redux-persist/es/types';
import { combineReducers, configureStore } from '@reduxjs/toolkit';

import appReducer from './features/app.slice';
import authReducer from './features/auth.slice';
import { CategoryApi } from '~/src/infrastructure/services/category.service';
import { promotionApi } from '~/src/infrastructure/services/promotion.service';
import { authApi } from '~/src/infrastructure/services/auth.service';
import { uploadImageApi } from '~/src/infrastructure/services/upload.service';
import { catalogApi } from '~/src/infrastructure/services/catalog.service';
import { basketApi } from '~/src/infrastructure/services/basket.service';
import { identityApi } from '~/src/infrastructure/services/identity.service';
import { orderApi } from '~/src/infrastructure/services/order.service';
import { TypedUseSelectorHook, useSelector } from 'react-redux';
import { createPersistStorage } from './persist-storage';

const storage = createPersistStorage();

const persistConfig: PersistConfig<ReturnType<typeof reducers>> = {
   key: 'root',
   version: 1,
   storage: storage,
   blacklist: [
      CategoryApi.reducerPath,
      authApi.reducerPath,
      uploadImageApi.reducerPath,
      promotionApi.reducerPath,
      catalogApi.reducerPath,
      basketApi.reducerPath,
      orderApi.reducerPath,
      identityApi.reducerPath,
   ], // Add the blacklist option
   whitelist: ['auth'],
};

const reducers = combineReducers({
   app: appReducer,
   auth: authReducer,
   [CategoryApi.reducerPath]: CategoryApi.reducer,
   [authApi.reducerPath]: authApi.reducer,
   [uploadImageApi.reducerPath]: uploadImageApi.reducer,
   [promotionApi.reducerPath]: promotionApi.reducer,
   [catalogApi.reducerPath]: catalogApi.reducer,
   [basketApi.reducerPath]: basketApi.reducer,
   [orderApi.reducerPath]: orderApi.reducer,
   [identityApi.reducerPath]: identityApi.reducer,
});

const persistedReducer = persistReducer(persistConfig, reducers);

export const reduxStore = configureStore({
   reducer: persistedReducer,
   middleware: (getDefaultMiddleware: any) =>
      getDefaultMiddleware({
         serializableCheck: {
            ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
         },
      }).concat(
         CategoryApi.middleware,
         authApi.middleware,
         uploadImageApi.middleware,
         promotionApi.middleware,
         catalogApi.middleware,
         basketApi.middleware,
         orderApi.middleware,
         identityApi.middleware,
      ),
});

export const persistor = persistStore(reduxStore);

export type RootState = ReturnType<typeof reduxStore.getState>;
export type AppDispatch = typeof reduxStore.dispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
