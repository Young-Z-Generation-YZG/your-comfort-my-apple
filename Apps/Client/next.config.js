module.exports = {
   env: {
      API_ENDPOINT: process.env.API_ENDPOINT,
   },
   images: {
      remotePatterns: [
         {
            protocol: 'https',
            hostname: 'shopdunk.com',
            port: '',
            pathname: '/**',
         },
         {
            protocol: 'https',
            hostname: 'store.storeimages.cdn-apple.com',
            port: '',
            pathname: '/**',
         },
         {
            protocol: 'https',
            hostname: 'res.cloudinary.com',
            port: '',
            pathname: '/**',
         },
         {
            protocol: 'https',
            hostname: 'cdn.discordapp.com',
            port: '',
            pathname: '/**',
         },
         {
            protocol: 'https',
            hostname: 'images.unsplash.com',
            port: '',
            pathname: '/**',
         },
      ],
   },
};
