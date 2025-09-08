import { useAppSelector } from '~/infrastructure/redux/store';
import { useRouter } from 'next/navigation';
import { usePathname } from 'next/navigation';
import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { setLogout } from '~/infrastructure/redux/features/auth.slice';

const withAuth = (WrappedComponent: any) => {
   const WithAuth = (props: any) => {
      const auth = useAppSelector((state) => state.auth.value);
      const router = useRouter();
      const pathname = usePathname();

      const dispatch = useDispatch();

      useEffect(() => {
         console.log('auth', auth);

         if (auth.accessToken) {
            if (pathname === '/sign-in' || pathname === '/sign-up') {
               router.back();
            }
         }

         if (!auth.accessToken) {
            // Redirect to sign-in page if not authenticated

            dispatch(setLogout());

            if (pathname !== '/sign-in' && pathname !== '/sign-up') {
               router.push('/sign-in');
            }
         }
      }, [auth.accessToken, router]); // Dependencies for useEffect

      if (auth.accessToken) {
         if (pathname === '/sign-in' || pathname === '/sign-up') {
            return null; // Render nothing while redirecting
         }
      }

      if (
         !auth.accessToken &&
         pathname !== '/sign-in' &&
         pathname !== '/sign-up'
      ) {
         return null; // Render nothing while redirecting
      }

      return <WrappedComponent {...props} />;
   };

   WithAuth.getInitialProps = async (ctx: any) => {
      const wrappedComponentInitialProps = WrappedComponent.getInitialProps
         ? await WrappedComponent.getInitialProps(ctx)
         : {};

      return { ...wrappedComponentInitialProps };
   };

   return WithAuth;
};

export default withAuth;
