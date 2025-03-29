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
import cartReducer from './features/cart-demo.slice';
import { CategoryApi } from '~/infrastructure/services/category.service';
import { AuthApi } from '../services/auth.service';
import { TypedUseSelectorHook, useSelector } from 'react-redux';
import { createPersistStorage } from './persist-storage';

const storage = createPersistStorage();

const persistConfig: PersistConfig<ReturnType<typeof reducers>> = {
   key: 'root',
   version: 1,
   storage: storage, // Add the storage option or any other required options
   blacklist: [CategoryApi.reducerPath, AuthApi.reducerPath], // Add the blacklist option
   whitelist: ['auth', 'cart'],
};

const reducers = combineReducers({
   app: appReducer,
   auth: authReducer,
   cart: cartReducer,
   [CategoryApi.reducerPath]: CategoryApi.reducer,
   [AuthApi.reducerPath]: AuthApi.reducer,
});

const persistedReducer = persistReducer(persistConfig, reducers);

export const reduxStore = configureStore({
   reducer: persistedReducer,
   middleware: (getDefaultMiddleware: any) =>
      getDefaultMiddleware({
         serializableCheck: {
            ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
         },
      }).concat(CategoryApi.middleware, AuthApi.middleware),
});

export const persistor = persistStore(reduxStore);

export type RootState = ReturnType<typeof reduxStore.getState>;
export type AppDispatch = typeof reduxStore.dispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
