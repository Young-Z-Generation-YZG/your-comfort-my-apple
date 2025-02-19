import NextAuth, { AuthOptions } from 'next-auth';
import KeycloakProvider from 'next-auth/providers/keycloak';
import { environments } from '~/environments';

export const authOptions: AuthOptions = {
   providers: [
      KeycloakProvider({
         clientId: environments.IDENTITY_PROVIDER_CLIENT_ID ?? '',
         clientSecret: environments.IDENTITY_PROVIDER_CLIENT_SECRET ?? '',
         issuer: environments.IDENTITY_PROVIDER_ISSUER ?? '',
      }),
   ],
};

const handler = NextAuth(authOptions);
export { handler as GET, handler as POST };
