import NextAuth, { AuthOptions } from 'next-auth';
import KeycloakProvider from 'next-auth/providers/keycloak';
import dayjs from 'dayjs';
import envConfig from '~/src/infrastructure/config/env.config';
import { urlSerializer } from '~/src/infrastructure/utils/url-serializer';

export const authOptions: AuthOptions = {
   providers: [
      KeycloakProvider({
         clientId: envConfig.IDENTITY_PROVIDER_CLIENT_ID ?? '',
         clientSecret: envConfig.IDENTITY_PROVIDER_CLIENT_SECRET ?? '',
         issuer: envConfig.IDENTITY_PROVIDER_ISSUER ?? '',
      }),
   ],
};

export const redirectToIdentityProvider = () => {
   const url = envConfig.IDENTITY_PROVIDER_LOGIN_REDIRECT_URL ?? '';
   const client_id = envConfig.IDENTITY_PROVIDER_CLIENT_ID ?? '';
   const redirect_uri = envConfig.IDENTITY_PROVIDER_CALLBACK_URL ?? '';
   const response_type = 'code';
   const state = btoa(dayjs().format());

   return urlSerializer(url, {
      response_type: response_type,
      client_id: client_id,
      redirect_uri: redirect_uri,
      state: state,
   });
};

const handler = NextAuth(authOptions);
export { handler as GET, handler as POST };
