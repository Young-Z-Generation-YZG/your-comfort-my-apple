'use client';

import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { logout, setTimerId } from '~/infrastructure/redux/features/auth.slice';
import { useAppSelector } from '~/infrastructure/redux/store';

export const useTokenExpirationCheck = () => {
   const dispatch = useDispatch();
   const { AT_expireIn, isAuthenticated, timerId } = useAppSelector(
      (state) => state.auth.value,
   );

   console.log('timerId:', timerId);

   useEffect(() => {
      if (!isAuthenticated || !AT_expireIn) return;

      // Assuming AT_expireIn is in seconds and represents the expiration time from now
      const expirationTime = new Date().getTime() + parseInt('300') * 1000; // Convert seconds to milliseconds

      // console.log('Expiration Time:', expirationTime);

      const intervalId = setInterval(() => {
         const currentTime = new Date().getTime();
         // console.log('Current Time:', currentTime);

         const timeLeft = expirationTime - currentTime;

         // console.log('Time Left:', timeLeft);

         if (timeLeft <= 0) {
            clearInterval(intervalId);
            dispatch(logout()); // Dispatch logout action when token expires
            dispatch(setTimerId(null)); // Clear the timer ID in the store
         } else {
            // Update the timer ID in the store if needed
            dispatch(setTimerId(intervalId.toString()));
         }
      }, 1000); // Check every second

      return () => clearInterval(intervalId);
   }, [AT_expireIn, dispatch, isAuthenticated]);
};
