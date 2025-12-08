const envConfig = () => {
   return {
      API_ENDPOINT: process.env.API_ENDPOINT || '',
      APP_URL: process.env.APP_URL || '',
   };
};

export default envConfig();
