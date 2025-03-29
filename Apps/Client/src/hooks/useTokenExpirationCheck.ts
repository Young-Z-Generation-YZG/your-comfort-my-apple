'use client';

import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import {
   setLogout,
   setTimerId,
} from '~/infrastructure/redux/features/auth.slice';
import { useAppSelector } from '~/infrastructure/redux/store';

export const useTokenExpirationCheck = () => {
   const dispatch = useDispatch();
   const { AT_expireIn, isAuthenticated, timerId } = useAppSelector(
      (state) => state.auth.value,
   );

   useEffect(() => {
      if (!isAuthenticated || !AT_expireIn) return;

      // Assuming AT_expireIn is in seconds and represents the expiration time from now
      const expirationTime = new Date().getTime() + parseInt('300') * 1000; // Convert seconds to milliseconds

      console.info(
         'Expiration Time:',
         new Date(expirationTime).toLocaleString(),
      );

      const intervalId = setInterval(() => {
         const currentTime = new Date().getTime();

         const timeLeft = expirationTime - currentTime;

         if (timeLeft <= 0) {
            clearInterval(intervalId);
            dispatch(setLogout());
            dispatch(setTimerId(null)); // Clear the timer ID in the store
         } else {
            dispatch(setTimerId(intervalId.toString()));
         }
      }, 1000);

      return () => clearInterval(intervalId);
   }, [AT_expireIn, dispatch, isAuthenticated]);
};
