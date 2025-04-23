import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
   output: 'standalone',
   env: {
      API_ENDPOINT: process.env.API_ENDPOINT,
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
