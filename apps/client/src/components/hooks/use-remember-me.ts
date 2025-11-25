import { useCallback } from 'react';
import { useLocalStorage } from './use-storage';
import {
   encryptPassword,
   decryptPassword,
} from '~/infrastructure/utils/crypto.util';

interface RememberMeData {
   email: string;
   encryptedPassword: string;
}

export function useRememberMe() {
   const [rememberMeData, setRememberMeData, removeRememberMeData] =
      useLocalStorage('remember-me', null);

   const saveCredentials = useCallback(
      (email: string, password: string) => {
         const encryptedPassword = encryptPassword(password);
         setRememberMeData({ email, encryptedPassword });
      },
      [setRememberMeData],
   );

   const getCredentials = useCallback((): {
      email: string;
      password: string;
   } | null => {
      if (!rememberMeData) return null;

      try {
         const { email, encryptedPassword } = rememberMeData as RememberMeData;
         const password = decryptPassword(encryptedPassword);
         return { email, password };
      } catch (error) {
         console.error('Failed to decrypt password:', error);
         removeRememberMeData();
         return null;
      }
   }, [rememberMeData, removeRememberMeData]);

   const clearCredentials = useCallback(() => {
      removeRememberMeData();
   }, [removeRememberMeData]);

   const hasRememberedCredentials = useCallback((): boolean => {
      return rememberMeData !== null && rememberMeData !== undefined;
   }, [rememberMeData]);

   return {
      saveCredentials,
      getCredentials,
      clearCredentials,
      hasRememberedCredentials,
   };
}
