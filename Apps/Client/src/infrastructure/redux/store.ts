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

import authReducer from './features/auth.slice';
import appReducer from './features/app.slice';
import cartReducer from './features/cart.slice';
import searchReducer from './features/search.slice';
import { CategoryApi } from '~/infrastructure/services/category.service';
import { promotionApi } from '~/infrastructure/services/promotion.service';
import { AuthApi } from '~/infrastructure/services/auth.service';
import { catalogApi } from '~/infrastructure/services/catalog.service';
import { reviewApi } from '~/infrastructure/services/review.service';
import { basketApi } from '~/infrastructure/services/basket.service';
import { identityApi } from '~/infrastructure/services/identity.service';
import { orderApi } from '~/infrastructure/services/order.service';
import { TypedUseSelectorHook, useSelector } from 'react-redux';
import { createPersistStorage } from './persist-storage';

const storage = createPersistStorage();

const persistConfig: PersistConfig<ReturnType<typeof reducers>> = {
   key: 'root',
   version: 1,
   storage: storage, // Add the storage option or any other required options
   blacklist: [
      CategoryApi.reducerPath,
      AuthApi.reducerPath,
      promotionApi.reducerPath,
      catalogApi.reducerPath,
      reviewApi.reducerPath,
      basketApi.reducerPath,
      orderApi.reducerPath,
      identityApi.reducerPath,
   ], // Add the blacklist option
   whitelist: ['auth', 'cart', 'search'],
};

const reducers = combineReducers({
   app: appReducer,
   auth: authReducer,
   cart: cartReducer,
   search: searchReducer,
   [CategoryApi.reducerPath]: CategoryApi.reducer,
   [AuthApi.reducerPath]: AuthApi.reducer,
   [promotionApi.reducerPath]: promotionApi.reducer,
   [catalogApi.reducerPath]: catalogApi.reducer,
   [reviewApi.reducerPath]: reviewApi.reducer,
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
         AuthApi.middleware,
         promotionApi.middleware,
         catalogApi.middleware,
         reviewApi.middleware,
         basketApi.middleware,
         orderApi.middleware,
         identityApi.middleware,
      ),
});

export const persistor = persistStore(reduxStore);

export type RootState = ReturnType<typeof reduxStore.getState>;
export type AppDispatch = typeof reduxStore.dispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
