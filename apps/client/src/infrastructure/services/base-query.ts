import { fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { BaseQueryApi, FetchArgs } from '@reduxjs/toolkit/query';
import envConfig from '~/infrastructure/config/env.config';
import { RootState } from '~/infrastructure/redux/store';
import {
   setLogin,
   setLogout,
} from '~/infrastructure/redux/features/auth.slice';
import { setPreviousUnAuthenticatedPath } from '~/infrastructure/redux/features/app.slice';

/**
 * Force delete access token from localStorage
 * This ensures tokens are cleared even if Redux persist hasn't synced yet
 */
const forceClearAccessToken = () => {
   if (typeof window === 'undefined') return;

   try {
      const persistKey = 'persist:client-root';
      const persistedState = localStorage.getItem(persistKey);

      if (persistedState) {
         const parsed = JSON.parse(persistedState);
         if (parsed.auth) {
            const authState = JSON.parse(parsed.auth);
            // Clear access token specifically
            authState.accessToken = null;
            authState.refreshToken = null;
            authState.isAuthenticated = false;
            // Update persisted state
            parsed.auth = JSON.stringify(authState);
            localStorage.setItem(persistKey, JSON.stringify(parsed));
         }
      }
   } catch (error) {
      console.error(
         '[baseQuery] Failed to clear access token from localStorage:',
         error,
      );
   }
};

export const baseQuery = (service: string) => {
   const baseQueryWithAuth = fetchBaseQuery({
      baseUrl: envConfig.API_ENDPOINT + service,
      prepareHeaders: (headers, { getState, endpoint }) => {
         const accessToken = (getState() as RootState).auth.accessToken;
         const refreshToken = (getState() as RootState).auth.refreshToken;

         console.log('[Fn:baseQuery]::ENDPOINT: ', endpoint);

         // Only set Authorization header if not already set (allows explicit override)
         if (!headers.get('Authorization')) {
            const isLogoutRequest =
               endpoint === 'logout' ||
               (typeof endpoint === 'string' && endpoint.includes('logout'));
            const isRefreshRequest =
               endpoint === 'refresh' ||
               (typeof endpoint === 'string' && endpoint.includes('refresh'));

            if (isLogoutRequest || isRefreshRequest) {
               headers.set('Authorization', `Bearer ${refreshToken}`);
            } else {
               headers.set('Authorization', `Bearer ${accessToken}`);
            }
         }

         headers.set('ngrok-skip-browser-warning', 'true');

         return headers;
      },
      paramsSerializer: (params: Record<string, any>) => {
         const searchParams = new URLSearchParams();

         Object.entries(params).forEach(([key, value]) => {
            if (value === null || value === undefined) {
               // Skip null/undefined values
               return;
            }

            if (Array.isArray(value)) {
               // For arrays, append each value separately
               value.forEach((item) => {
                  if (item !== null && item !== undefined) {
                     searchParams.append(key, String(item));
                  }
               });
            } else {
               searchParams.append(key, String(value));
            }
         });

         return searchParams.toString();
      },
   });

   return async (
      args: string | FetchArgs,
      api: BaseQueryApi,
      extraOptions: {},
   ) => {
      // Check if this is a refresh or logout request and ensure correct token is used
      if (typeof args !== 'string' && args.url) {
         const isRefreshRequest = args.url.includes('/auth/refresh');
         const isLogoutRequest = args.url.includes('/auth/logout');

         if (isRefreshRequest || isLogoutRequest) {
            const state = api.getState() as RootState;
            const refreshToken = state.auth.refreshToken;

            // Explicitly set refresh token header for refresh/logout requests
            if (refreshToken && !args.headers) {
               args.headers = {};
            }
            if (refreshToken && args.headers) {
               (args.headers as Record<string, string>).Authorization =
                  `Bearer ${refreshToken}`;
            }
         }
      }

      let result = await baseQueryWithAuth(args, api, extraOptions);

      // If we get a 401 and it's not a refresh or logout request, try to refresh
      if (
         result.error &&
         result.error.status === 401 &&
         typeof args !== 'string' &&
         args.url &&
         !args.url.includes('/auth/refresh') &&
         !args.url.includes('/auth/logout')
      ) {
         const state = api.getState() as RootState;
         const refreshToken = state.auth.refreshToken;

         // Only attempt refresh if we have a refresh token
         if (refreshToken) {
            console.log('[baseQuery] Attempting to refresh token...');

            // Create refresh request with explicit refresh token header
            // IMPORTANT: Always use identity-services for refresh, regardless of current service
            const refreshArgs: FetchArgs = {
               url: '/api/v1/auth/refresh',
               method: 'POST',
               headers: {
                  Authorization: `Bearer ${refreshToken}`,
               },
            };

            // Use identity-services base query for refresh token endpoint
            const identityBaseQuery = baseQuery('/identity-services');
            const refreshResult = await identityBaseQuery(
               refreshArgs,
               api,
               extraOptions,
            );

            if (refreshResult.data) {
               // Update tokens in store
               const refreshData = refreshResult.data as {
                  access_token: string;
                  refresh_token: string;
                  access_token_expires_in: number;
               };

               api.dispatch(
                  setLogin({
                     userId: state.auth.userId!,
                     userEmail: state.auth.userEmail!,
                     username: state.auth.username!,
                     accessToken: refreshData.access_token,
                     refreshToken: refreshData.refresh_token,
                     accessTokenExpiredIn: refreshData.access_token_expires_in,
                  }),
               );

               // Retry the original request with new token
               console.log(
                  '[baseQuery] Token refreshed, retrying original request...',
               );
               result = await baseQueryWithAuth(args, api, extraOptions);
            } else {
               // Refresh failed, logout user and force clear access token
               console.log('[baseQuery] Token refresh failed, logging out...');
               const currentRoute = window.location.pathname;

               // Force delete access token from localStorage
               forceClearAccessToken();

               api.dispatch(setPreviousUnAuthenticatedPath(currentRoute));
               api.dispatch(setLogout());
            }
         } else {
            // No refresh token available, logout user and force clear access token
            console.log(
               '[baseQuery] No refresh token available, logging out...',
            );
            const currentRoute = window.location.pathname;

            // Force delete access token from localStorage
            forceClearAccessToken();

            api.dispatch(setPreviousUnAuthenticatedPath(currentRoute));
            api.dispatch(setLogout());
         }
      }

      return result;
   };
};
