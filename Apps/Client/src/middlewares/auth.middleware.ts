import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

// This function can be marked `async` if using `await` inside
export function AuthMiddleware(request: NextRequest) {
   // Get the pathname of the request
   //    const path = request.nextUrl.pathname;

   //    console.log('Middleware triggered for path:', path);

   //    // Get authentication status from cookies
   //    const isAuthenticating =
   //       request.cookies.get('auth_state')?.value === 'authenticating';
   //    const isAuthenticated = request.cookies.get('auth_token')?.value;

   //    // Define public paths that don't need authentication
   //    const publicPaths = ['/sign-in', '/sign-up', '/forgot-password'];
   //    const isPublicPath = publicPaths.includes(path);

   //    // Define authentication paths that need special handling
   //    const authPaths = ['/verify/otp', '/reset-password'];
   //    const isAuthPath = authPaths.includes(path);

   //    // Redirect logic
   //    if (isAuthPath) {
   //       // Only allow access to OTP page if user is in authenticating state
   //       if (!isAuthenticating) {
   //          return NextResponse.redirect(new URL('/sign-in', request.url));
   //       }
   //    } else if (!isPublicPath) {
   //       // For protected routes, redirect to sign-in if not authenticated
   //       if (!isAuthenticated) {
   //          return NextResponse.redirect(new URL('/sign-in', request.url));
   //       }
   //    } else if (isPublicPath) {
   //       // Redirect authenticated users away from public pages to dashboard
   //       if (isAuthenticated) {
   //          return NextResponse.redirect(new URL('/dashboard', request.url));
   //       }
   //    }

   return NextResponse.next();
}
