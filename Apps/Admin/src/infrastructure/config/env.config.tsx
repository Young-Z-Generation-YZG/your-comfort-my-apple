const envConfig = () => {
   return {
      API_ENDPOINT: process.env.API_ENDPOINT || '',
   };
};

export default envConfig();
