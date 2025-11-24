const envConfig = () => {
   return {
      API_ENDPOINT: process.env.API_ENDPOINT || '',
      IDENTITY_PROVIDER_CLIENT_ID:
         process.env.IDENTITY_PROVIDER_CLIENT_ID || '',
      IDENTITY_PROVIDER_CLIENT_SECRET:
         process.env.IDENTITY_PROVIDER_CLIENT_SECRET || '',
      IDENTITY_PROVIDER_ISSUER: process.env.IDENTITY_PROVIDER_ISSUER || '',
      IDENTITY_PROVIDER_LOGIN_REDIRECT_URL:
         process.env.IDENTITY_PROVIDER_LOGIN_REDIRECT_URL || '',
      IDENTITY_PROVIDER_CALLBACK_URL:
         process.env.IDENTITY_PROVIDER_CALLBACK_URL || '',
      ORDERING_NOTIFICATION_HUB:
         process.env.ORDERING_NOTIFICATION_HUB ||
         'https://7b1bec6f44c4.ngrok-free.app/ordering-services/orderHub',
   };
};

export default envConfig();
