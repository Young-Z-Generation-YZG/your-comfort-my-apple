import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
   output: 'standalone',
   reactStrictMode: true, // Enable to catch memory leaks and issues
   poweredByHeader: false, // Remove X-Powered-By header
   compress: true, // Enable gzip compression
   // swcMinify is enabled by default in Next.js 15, no need to specify
   // Configure cache directory to avoid permission issues
   distDir: '.next',
   // Disable experimental features that might cause issues and spawn workers
   experimental: {
      serverActions: {
         bodySizeLimit: '2mb',
      },
   },
   // Production optimizations
   productionBrowserSourceMaps: false, // Disable source maps in production
   optimizeFonts: true, // Optimize font loading
   env: {
      API_ENDPOINT: process.env.API_ENDPOINT,
      DEFAULT_TENANT_ID: process.env.DEFAULT_TENANT_ID,
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
