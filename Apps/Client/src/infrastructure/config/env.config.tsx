const envConfig = () => {
   return {
      API_ENDPOINT: process.env.API_ENDPOINT || 'http://default:3000/api/v1/',
   };
};

export default envConfig();
