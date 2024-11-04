/** @type {import('next').NextConfig} */

const nextConfig = {
   output: 'standalone',
   webpack: (config) => {
      // Set the exportLocalsConvention to camelCase for CSS modules (transform everything to camelCase)
      config.module.rules
         .find(({ oneOf }) => !!oneOf)
         .oneOf.filter(({ use }) => JSON.stringify(use)?.includes('css-loader'))
         .reduce((acc, { use }) => acc.concat(use), [])
         .forEach(({ options }) => {
            if (options.modules) {
               options.modules.exportLocalsConvention = 'camelCase';
            }
         });

      return config;
   },
   images: {
      remotePatterns: [
        {
          protocol: 'https',
          hostname: 'shopdunk.com',
          port: '',
          pathname: '/**'
        },
      ],
   },
  
};

export default nextConfig;
