'use client';

import { useSearchParams } from 'next/navigation';
import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { LoadingOverlay } from '~/components/loading-overlay';
import { AUTH_RESULT_TYPE } from '~/constants/auth';
import { loginFailed, loginSuccess } from '~/redux/slices/auth.slice';
import { AppDispatch } from '~/redux/store';
import { requestAuthorizationCodeGrant } from '~/services/auth.service';
import { authChannel } from '~/utils/broadcast-channel';

const CallbackPage = () => {
   const searchParams = useSearchParams();
   const state = searchParams.get('state') ?? '';
   const code = searchParams.get('code') ?? '';
   const iss = searchParams.get('iss') ?? '';

   const dispatch = useDispatch<AppDispatch>();

   const handleCallback = async () => {
      if (!state && !code && !iss) {
         dispatch(loginFailed());
      }

      const result = await requestAuthorizationCodeGrant({
         code: code,
         state: state,
      });

      console.log(result);

      if (result?.error) {
         authChannel.postMessage({
            type: 'AUTH_LOGIN',
            message: {
               result: AUTH_RESULT_TYPE.AUTH_FAILED,
               token: null,
            },
         });
      } else {
         authChannel.postMessage({
            type: 'AUTH_LOGIN',
            message: {
               result: AUTH_RESULT_TYPE.AUTH_SUCCESS,
               token: result.access_token,
            },
         });
      }
   };

   useEffect(() => {
      handleCallback();
   }, []);

   // const dispatch = useDispatch<AppDispatch>();

   // const handleCallback = async () => {
   //    if (!state && !code && !iss) {
   //       dispatch(loginFailed());
   //    }

   //    const result = await requestAuthorizationCodeGrant({
   //       code: code,
   //       state: state,
   //    });

   //    console.log(result);

   //    if (result?.error) {
   //       dispatch(loginFailed());
   //    } else {
   //       console.log('[DISPATCH] Login success', result.access_token);
   //       dispatch(loginSuccess(result.access_token));

   //       // window.close();
   //    }
   // };

   // useEffect(() => {
   //    handleCallback();
   // }, []);

   return (
      <div>
         <LoadingOverlay isLoading={true} textStyles="text-primary" />
      </div>
   );
};

export default CallbackPage;
