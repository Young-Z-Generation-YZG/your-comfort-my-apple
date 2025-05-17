'use client';

import { LoadingOverlay } from '@components/loading-overlay';
import { useEffect } from 'react';
import { useSearchParams } from 'next/navigation';
import { useAuthorizationCodeMutation } from '~/src/infrastructure/services/keycloak.service';

const AuthCallbackPage = () => {
   const searchParams = useSearchParams();
   const state = searchParams.get('state') ?? '';
   const code = searchParams.get('code') ?? '';
   const iss = searchParams.get('iss') ?? '';

   const [authorizationCode, { isSuccess, isError, error }] =
      useAuthorizationCodeMutation();

   //    useEffect(() => {
   //       // Close the opener window and redirect to the main application
   //       if (window.opener) {

   //          // Close the popup window
   //          window.opener.postMessage('AUTH_COMPLETED', window.location.origin);
   //          // Optionally redirect the main window if needed
   //          // window.opener.location.href = '/dashboard';

   //          // Close this window
   //          window.close();
   //       }
   //    }, []);

   useEffect(() => {
      const fetchAuthorizationCode = async () => {
         await authorizationCode({
            code: code,
         }).unwrap();
      };

      fetchAuthorizationCode();
   }, []);
   useEffect(() => {
      if (window.opener) {
         if (isSuccess) {
            // Send success message to main window
            window.opener.postMessage(
               { status: 'AUTH_SUCCESS' },
               window.location.origin,
            );
            // Close this window after a short delay to ensure message is sent
            setTimeout(() => window.close(), 500);
         } else if (isError && error) {
            // Send error message with details to main window
            window.opener.postMessage(
               {
                  status: 'AUTH_FAILED',
                  error: error.toString(),
               },
               window.location.origin,
            );
            // Close this window
            setTimeout(() => window.close(), 500);
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
