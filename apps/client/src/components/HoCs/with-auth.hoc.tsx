import useAuth from '@components/hooks/use-auth';
import { usePathname, useRouter } from 'next/navigation';
import { ComponentType, useEffect, useMemo } from 'react';

// Auth pages that should redirect to home when authenticated
const AUTH_PAGES = ['/sign-in', '/sign-up', '/verify/otp', '/forgot-password'];

const DEFAULT_AUTH_REDIRECT = '/sign-in';
const DEFAULT_HOME_REDIRECT = '/home';

type WithAuthOptions = {
   authRedirect?: string;
   homeRedirect?: string;
};

const withAuth = <P extends object>(
   WrappedComponent: ComponentType<P>,
   options?: WithAuthOptions,
) => {
   const WithAuth = (props: P) => {
      const { isAuthenticated } = useAuth();
      const pathname = usePathname();
      const router = useRouter();

      const authRedirect = options?.authRedirect || DEFAULT_AUTH_REDIRECT;
      const homeRedirect = options?.homeRedirect || DEFAULT_HOME_REDIRECT;

      // Check if current page is an auth page
      const isAuthPage = useMemo(
         () => AUTH_PAGES.includes(pathname),
         [pathname],
      );

      // Handle redirects
      useEffect(() => {
         // Authenticated users on auth pages → redirect to home
         if (isAuthenticated && isAuthPage) {
            router.replace(homeRedirect);
            return;
         }

         // Unauthenticated users on protected pages → redirect to auth
         if (!isAuthenticated && !isAuthPage) {
            router.replace(authRedirect);
         }
      }, [isAuthenticated, isAuthPage, router, authRedirect, homeRedirect]);

      // Don't render while redirecting
      const shouldRender = useMemo(() => {
         return (
            (isAuthenticated && !isAuthPage) || (!isAuthenticated && isAuthPage)
         );
      }, [isAuthenticated, isAuthPage]);

      if (!shouldRender) {
         return null;
      }

      return <WrappedComponent {...props} />;
   };

   // Preserve component name for debugging
   WithAuth.displayName = `WithAuth(${WrappedComponent.displayName || WrappedComponent.name || 'Component'})`;

   return WithAuth;
};

export default withAuth;
