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
import { promotionApi } from '~/src/infrastructure/services/promotion.service';
import { authApi } from '~/src/infrastructure/services/auth.service';
import { keycloakApi } from '~/src/infrastructure/services/keycloak.service';
import { uploadImageApi } from '~/src/infrastructure/services/upload.service';
import { TypedUseSelectorHook, useSelector } from 'react-redux';
import { createPersistStorage } from './persist-storage';
import { orderingApi } from '~/src/infrastructure/services/order.service';
import { identityApi } from '~/src/infrastructure/services/identity.service';
import { inventoryApi } from '~/src/infrastructure/services/inventory.service';

const storage = createPersistStorage();

const persistConfig: PersistConfig<ReturnType<typeof reducers>> = {
   key: 'root',
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
   ],
   whitelist: ['auth'],
};

const reducers = combineReducers({
   app: appReducer,
   auth: authReducer,
   [authApi.reducerPath]: authApi.reducer,
   [keycloakApi.reducerPath]: keycloakApi.reducer,
   [uploadImageApi.reducerPath]: uploadImageApi.reducer,
   [promotionApi.reducerPath]: promotionApi.reducer,
   [orderingApi.reducerPath]: orderingApi.reducer,
   [identityApi.reducerPath]: identityApi.reducer,
   [inventoryApi.reducerPath]: inventoryApi.reducer,
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
      ),
});

export const persistor = persistStore(reduxStore);

export type RootState = ReturnType<typeof reduxStore.getState>;
export type AppDispatch = typeof reduxStore.dispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
