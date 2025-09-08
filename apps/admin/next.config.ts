import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
   output: 'standalone',
   reactStrictMode: false,
   env: {
      API_ENDPOINT: process.env.API_ENDPOINT,
      IDENTITY_PROVIDER_CLIENT_ID: process.env.IDENTITY_PROVIDER_CLIENT_ID,
      IDENTITY_PROVIDER_CLIENT_SECRET:
         process.env.IDENTITY_PROVIDER_CLIENT_SECRET,
      IDENTITY_PROVIDER_ISSUER: process.env.IDENTITY_PROVIDER_ISSUER,
      IDENTITY_PROVIDER_LOGIN_REDIRECT_URL:
         process.env.IDENTITY_PROVIDER_LOGIN_REDIRECT_URL,
      IDENTITY_PROVIDER_CALLBACK_URL:
         process.env.IDENTITY_PROVIDER_CALLBACK_URL,
   },
   images: {
      remotePatterns: [
         {
            protocol: 'https',
            hostname: 'res.cloudinary.com',
            port: '',
            pathname: '/**',
         },
      ],
   },
};

export default nextConfig;
