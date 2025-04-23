import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

export function AuthMiddleware(request: NextRequest) {
   return NextResponse.next();
}
