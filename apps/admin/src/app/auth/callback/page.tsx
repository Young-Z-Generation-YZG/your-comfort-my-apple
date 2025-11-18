'use client';

import { LoadingOverlay } from '@components/loading-overlay';
import { useEffect, useRef } from 'react';
import { useSearchParams } from 'next/navigation';
import { useAuthorizationCodeMutation } from '~/src/infrastructure/services/keycloak.service';

const AuthCallbackPage = () => {
   const searchParams = useSearchParams();
   const state = searchParams.get('state') ?? '';
   const code = searchParams.get('code') ?? '';
   const iss = searchParams.get('iss') ?? '';

   // Use a ref to track if the API has been called
   const hasCalledApi = useRef(false);

   const [authorizationCode, { isSuccess, isError, error, data }] =
      useAuthorizationCodeMutation();

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

   useEffect(() => {
      if (isSuccess) {
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
   }, [isSuccess, isError, error]);

   return (
      <div>
         <LoadingOverlay isLoading={true} fullScreen />
      </div>
   );
};

export default AuthCallbackPage;
