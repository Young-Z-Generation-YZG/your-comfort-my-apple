import { useCallback, useMemo } from 'react';
import { LoginFormType } from '~/domain/schemas/auth.schema';
import { useLoginMutation } from '~/infrastructure/services/auth.service';
import { useAppSelector } from '~/infrastructure/redux/store';
// import {
// } from '~/infrastructure/utils/http/rtk-query-error-handler';

// import { useToast } from '@components/hooks/use-toast';
import { toast } from 'sonner';

const useAuth = () => {
   const [loginMutation, loginMutationState] = useLoginMutation();

   const authAppState = useAppSelector((state) => state.auth);

   const isAuthenticated = useMemo(() => {
      return Boolean(authAppState.accessToken && authAppState.isAuthenticated);
   }, [authAppState.accessToken, authAppState.isAuthenticated]);

   //  const { toast } = useToast();

   const login = useCallback(
      async (data: LoginFormType) => {
         try {
            const result = await loginMutation(data).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [loginMutation],
   );

   // centrally track the loading state
   const isLoading = useMemo(() => {
      return loginMutationState.isLoading;
   }, [loginMutationState.isLoading]);

   // centrally track the error
   useMemo(() => {
      let title = null;
      let description = null;
      const style = {
         backgroundColor: '#FEE2E2',
         color: '#991B1B',
         border: '1px solid #FCA5A5',
      };
      const cancel = {
         label: 'Close',
         onClick: () => {},
         actionButtonStyle: {
            backgroundColor: '#991B1B',
            color: '#FFFFFF',
         },
      };

      if (loginMutationState.isError) {
         title = 'Login failed';
         description = 'Wrong email or password';

         toast.error(title, {
            description: description,
            style: style,
            cancel: cancel,
         });

         console.error(
            '[useAuth]::loginMutationState.isError',
            loginMutationState.isError,
         );
      }
   }, [loginMutationState]);

   // centrally track the success
   useMemo(() => {
      if (loginMutationState.isSuccess) {
         toast.success('Welcome!', {
            style: {
               backgroundColor: '#DCFCE7',
               color: '#166534',
               border: '1px solid #86EFAC',
            },
            cancel: {
               label: 'Close',
               onClick: () => {},
               actionButtonStyle: {
                  backgroundColor: '#16A34A',
                  color: '#FFFFFF',
               },
            },
         });
      }
   }, [loginMutationState]);

   // check type of error

   return {
      // States
      isLoading,
      isAuthenticated,

      // Actions
      login,
   };
};

export default useAuth;
