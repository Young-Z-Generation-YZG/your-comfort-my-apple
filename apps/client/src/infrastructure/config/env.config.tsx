const envConfig = () => {
   return {
      API_ENDPOINT: process.env.API_ENDPOINT || '',
      APP_URL: process.env.APP_URL || '',
      DEFAULT_TENANT_ID: process.env.DEFAULT_TENANT_ID || '',
   };
};

export default envConfig();
