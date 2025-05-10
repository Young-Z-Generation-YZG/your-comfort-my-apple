import { NextRequest, NextResponse } from 'next/server';
import { AuthMiddleware } from './middlewares/auth.middleware';

export function middleware(req: NextRequest) {
   // const { pathname } = req.nextUrl;

   console.log('middleware', req.nextUrl.pathname);

   return AuthMiddleware(req);
}

export const config = {
   matcher: [
      '/account/:path*',
      // '/verify/otp',
      // '/((?!api|_next/static|_next/image|favicon.ico|sitemap.xml|robots.txt).*)',
   ],
};
