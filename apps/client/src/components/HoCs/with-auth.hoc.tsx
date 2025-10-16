// import useAuth from '@components/hooks/use-auth';
import { usePathname, useRouter } from 'next/navigation';
import { useEffect } from 'react';
import useAuth from '~/app/(pages)/(auth)/_hooks/use-auth';

const withAuth = (WrappedComponent: any) => {
   const WithAuth = (props: any) => {
      const {} = useAuth();
      // const { isAuthenticated } = useAuth();
      // const pathname = usePathname();
      // const router = useRouter();

      // useEffect(() => {
      //    const isAuthPage =
      //       pathname === '/sign-in' ||
      //       pathname === '/sign-up' ||
      //       pathname === '/verify/otp';

      //    // If authenticated and on auth pages, redirect to home
      //    if (isAuthenticated && isAuthPage) {
      //       router.push('/home');
      //    }

      //    // If not authenticated and not on auth pages, redirect to sign-in
      //    if (!isAuthenticated && !isAuthPage) {
      //       router.push('/sign-in');
      //    }
      // }, [isAuthenticated, pathname, router]);

      // const isAuthPage =
      //    pathname === '/sign-in' ||
      //    pathname === '/sign-up' ||
      //    pathname === '/verify/otp';

      // if (
      //    (isAuthenticated && isAuthPage) ||
      //    (!isAuthenticated && !isAuthPage)
      // ) {
      //    return null;
      // }

      return <WrappedComponent {...props} />;
   };

   return WithAuth;
};

export default withAuth;
