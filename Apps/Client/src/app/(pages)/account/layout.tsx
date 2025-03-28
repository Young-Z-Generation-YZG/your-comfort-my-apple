'use client';

import { useRouter } from 'next/navigation';
import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useTokenExpirationCheck } from '~/hooks/useTokenExpirationCheck';
import { setRouter } from '~/infrastructure/redux/features/app.slice';
import { useAppSelector } from '~/infrastructure/redux/store';

const AccountLayout = ({ children }: { children: React.ReactNode }) => {
   useTokenExpirationCheck();
   const router = useRouter();
   const dispatch = useDispatch();

   const { isAuthenticated, timerId, isLogoutTriggered } = useAppSelector(
      (state) => state.auth.value,
   );

   const currentPath = window.location.pathname;

   const appRouter = useAppSelector((state) => state.app.value.router);

   useEffect(() => {
      if (!isAuthenticated) {
         if (isLogoutTriggered) {
            dispatch(
               setRouter({
                  previousPath: null,
               }),
            );

            router.push('/sign-in');
         }

         if (!isLogoutTriggered && appRouter.previousPath) {
            router.push(appRouter.previousPath);
         } else {
            dispatch(
               setRouter({
                  previousPath: currentPath,
               }),
            );
            router.push('/sign-in');
         }
      }
   }, [isAuthenticated, timerId, router]);

   return <div>{children}</div>;
};

export default AccountLayout;
