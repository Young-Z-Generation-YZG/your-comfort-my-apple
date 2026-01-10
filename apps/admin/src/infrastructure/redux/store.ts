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
import tenantReducer from './features/tenant.slice';
import { promotionApi } from '~/src/infrastructure/services/promotion.service';
import { authApi } from '~/src/infrastructure/services/auth.service';
import { keycloakApi } from '~/src/infrastructure/services/keycloak.service';
import { uploadImageApi } from '~/src/infrastructure/services/upload.service';
import { TypedUseSelectorHook, useSelector } from 'react-redux';
import { createPersistStorage } from './persist-storage';
import { orderingApi } from '~/src/infrastructure/services/ordering.service';
import { identityApi } from '~/src/infrastructure/services/identity.service';
import { inventoryApi } from '~/src/infrastructure/services/inventory.service';
import { tenantApi } from '~/src/infrastructure/services/tenant.service';
import { productApi } from '~/src/infrastructure/services/product.service';
import { categoryApi } from '~/src/infrastructure/services/category.service';
import { userApi } from '~/src/infrastructure/services/user.service';
import { notificationApi } from '~/src/infrastructure/services/notification.service';
import { reviewApi } from '~/src/infrastructure/services/review.service';
import { requestApi } from '~/src/infrastructure/services/request.service';

const storage = createPersistStorage();

const persistConfig: PersistConfig<ReturnType<typeof reducers>> = {
   key: 'admin-root',
   version: 1,
   storage: storage,
   blacklist: [
      authApi.reducerPath,
      keycloakApi.reducerPath,
      uploadImageApi.reducerPath,
      promotionApi.reducerPath,
      orderingApi.reducerPath,
      identityApi.reducerPath,
      inventoryApi.reducerPath,
      tenantApi.reducerPath,
      productApi.reducerPath,
      categoryApi.reducerPath,
      userApi.reducerPath,
      notificationApi.reducerPath,
      reviewApi.reducerPath,
      requestApi.reducerPath,
   ],
   whitelist: ['auth', 'tenant'],
};

const reducers = combineReducers({
   app: appReducer,
   auth: authReducer,
   tenant: tenantReducer,
   [tenantApi.reducerPath]: tenantApi.reducer,
   [authApi.reducerPath]: authApi.reducer,
   [keycloakApi.reducerPath]: keycloakApi.reducer,
   [uploadImageApi.reducerPath]: uploadImageApi.reducer,
   [promotionApi.reducerPath]: promotionApi.reducer,
   [orderingApi.reducerPath]: orderingApi.reducer,
   [identityApi.reducerPath]: identityApi.reducer,
   [inventoryApi.reducerPath]: inventoryApi.reducer,
   [productApi.reducerPath]: productApi.reducer,
   [categoryApi.reducerPath]: categoryApi.reducer,
   [userApi.reducerPath]: userApi.reducer,
   [notificationApi.reducerPath]: notificationApi.reducer,
   [reviewApi.reducerPath]: reviewApi.reducer,
   [requestApi.reducerPath]: requestApi.reducer,
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
         authApi.middleware,
         uploadImageApi.middleware,
         promotionApi.middleware,
         keycloakApi.middleware,
         orderingApi.middleware,
         identityApi.middleware,
         inventoryApi.middleware,
         tenantApi.middleware,
         productApi.middleware,
         categoryApi.middleware,
         userApi.middleware,
         notificationApi.middleware,
         reviewApi.middleware,
         requestApi.middleware,
      ),
});

export const persistor = persistStore(reduxStore);

export type RootState = ReturnType<typeof reduxStore.getState>;
export type AppDispatch = typeof reduxStore.dispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
