import { NextRequest, NextResponse } from 'next/server';
import { AuthMiddleware } from './middlewares/auth.middleware';

export function middleware(req: NextRequest) {
   return AuthMiddleware(req);
}

export const config = {
   matcher: [
      // '/verify/otp',
      // '/((?!api|_next/static|_next/image|favicon.ico|sitemap.xml|robots.txt).*)',
   ],
};
