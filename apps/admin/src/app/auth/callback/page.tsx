'use client';

import { LoadingOverlay } from '@components/loading-overlay';
import { useEffect, useRef, useState } from 'react';
import { useSearchParams } from 'next/navigation';
import { useAuthorizationCodeMutation } from '~/src/infrastructure/services/keycloak.service';
import useAuthService from '~/src/hooks/api/use-auth-service';
import { useDispatch } from 'react-redux';
import {
   setIdentity,
   setRoles,
} from '~/src/infrastructure/redux/features/auth.slice';
import { setTenant } from '~/src/infrastructure/redux/features/tenant.slice';
import { ERole } from '~/src/domain/enums/role.enum';

const AuthCallbackPage = () => {
   const searchParams = useSearchParams();
   const state = searchParams.get('state') ?? '';
   const code = searchParams.get('code') ?? '';
   const iss = searchParams.get('iss') ?? '';

   // Use a ref to track if the API has been called
   const hasCalledApi = useRef(false);
   const [identityFetched, setIdentityFetched] = useState(false);

   const [authorizationCode, { isSuccess, isError, error, data }] =
      useAuthorizationCodeMutation();
   const { getIdentityAsync } = useAuthService();
   const dispatch = useDispatch();

   console.log('isSuccess', isSuccess);
   console.log('isError', isError);
   console.log('error', error);
   console.log('data', data);

   useEffect(() => {
      const fetchAuthorizationCode = async () => {
         if (hasCalledApi.current) {
            console.log('API call skipped: Already called');
            return;
         }

         try {
            console.log('CALLBACK:: code:', code, 'state:', state, 'iss:', iss);
            hasCalledApi.current = true;
            const data = await authorizationCode({ code }).unwrap();
            console.log('data', data);
         } catch (err) {
            console.error('Error fetching authorization code:', err);
            hasCalledApi.current = false; // Allow retry on error if needed
         }
      };

      if (code) {
         fetchAuthorizationCode();
      } else {
         console.error('No authorization code provided');
      }
      // eslint-disable-next-line react-hooks/exhaustive-deps
   }, [code, authorizationCode]);

   // Fetch identity after successful token exchange
   useEffect(() => {
      const fetchIdentity = async () => {
         if (!isSuccess || identityFetched) {
            return;
         }

         try {
            console.log('Fetching identity after Keycloak login...');
            const identityResult = await getIdentityAsync();

            if (identityResult.isSuccess && identityResult.data) {
               // Set roles
               dispatch(
                  setRoles({
                     currentUser: {
                        roles: identityResult.data.roles,
                     },
                     impersonatedUser: null,
                  }),
               );

               // Set identity (tenant_id, branch_id, etc.)
               dispatch(
                  setIdentity({
                     currentUser: {
                        userId: identityResult.data.id,
                        tenantId: identityResult.data.tenant_id,
                        branchId: identityResult.data.branch_id,
                        tenantSubDomain: identityResult.data.tenant_sub_domain,
                     },
                     impersonatedUser: null,
                  }),
               );

               // Set tenant if user is not super admin
               if (!identityResult.data.roles.includes(ERole.ADMIN_SUPER)) {
                  dispatch(
                     setTenant({
                        tenantId: identityResult.data.tenant_id,
                        branchId: identityResult.data.branch_id,
                        tenantSubDomain: identityResult.data.tenant_sub_domain,
                     }),
                  );
               }

               console.log(
                  'Identity fetched successfully:',
                  identityResult.data,
               );
               setIdentityFetched(true);
            } else {
               console.error('Failed to fetch identity:', identityResult.error);
               // Still proceed even if identity fetch fails (user can still access)
               setIdentityFetched(true);
            }
         } catch (err) {
            console.error('Error fetching identity:', err);
            // Still proceed even if identity fetch fails
            setIdentityFetched(true);
         }
      };

      if (isSuccess && !identityFetched) {
         fetchIdentity();
      }
      // eslint-disable-next-line react-hooks/exhaustive-deps
   }, [isSuccess, getIdentityAsync, dispatch]);

   useEffect(() => {
      console.log('Success handler effect:', {
         isSuccess,
         identityFetched,
         isError,
      });

      if (isSuccess && identityFetched) {
         console.log('Auth success, window.opener:', !!window.opener);
         if (window.opener) {
            // Popup flow: Send success message to main window
            console.log('Sending AUTH_SUCCESS message to parent window');
            window.opener.postMessage(
               { status: 'AUTH_SUCCESS' },
               window.location.origin,
            );
            // Close this window after a short delay to ensure message is sent
            setTimeout(() => {
               console.log('Closing popup window');
               window.close();
            }, 500);
         } else {
            // Full-page redirect flow: Redirect to dashboard
            console.log('Redirecting to dashboard (full-page flow)');
            window.location.href = '/dashboard';
         }
      } else if (isError && error) {
         if (window.opener) {
            // Popup flow: Send error message to main window
            window.opener.postMessage(
               {
                  status: 'AUTH_FAILED',
               },
               window.location.origin,
            );
            // Close this window
            setTimeout(() => window.close(), 500);
         } else {
            // Full-page redirect flow: Redirect back to sign-in page
            window.location.href = '/auth/sign-in';
         }
      }
   }, [isSuccess, isError, error, identityFetched]);

   return (
      <div>
         <LoadingOverlay isLoading={true} fullScreen />
      </div>
   );
};

export default AuthCallbackPage;
