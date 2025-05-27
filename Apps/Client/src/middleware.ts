import { NextRequest } from 'next/server';
import { AuthMiddleware } from './middlewares/auth.middleware';

export function middleware(req: NextRequest) {
   return AuthMiddleware(req);
}
