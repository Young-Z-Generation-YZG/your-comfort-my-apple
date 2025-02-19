import { date } from 'zod';
import { environments } from '~/environments';
import dayjs from 'dayjs';
import { urlSerializer } from '~/utils/url-serializer';
import {
   IAuthorizationCodeResponse,
   IRequestAuthorizationCodeGrantPayload,
} from '~/types/api-types/auth.type';
import { post, postToken } from '~/utils/http-request';

export const redirectToIdentityProvider = () => {
   const url = environments.IDENTITY_PROVIDER_LOGIN_REDIRECT_URL ?? '';
   const client_id = environments.IDENTITY_PROVIDER_CLIENT_ID ?? '';
   const redirect_uri = environments.IDENTITY_PROVIDER_REDIRECT_URL ?? '';
   const response_type = 'code';
   const state = btoa(dayjs().format());

   return urlSerializer(url, {
      response_type: response_type,
      client_id: client_id,
      redirect_uri: redirect_uri,
      state: state,
   });
};

export const requestAuthorizationCodeGrant = async (payload: {
   code: string;
   state: string;
}): Promise<any> => {
   const formUrlEncodedRecord: Record<string, string> = {
      grant_type: 'authorization_code',
      client_id: environments.IDENTITY_PROVIDER_CLIENT_ID ?? '',
      client_secret: environments.IDENTITY_PROVIDER_CLIENT_SECRET ?? '',
      code: payload.code,
      state: payload.state,
      redirect_uri: environments.IDENTITY_PROVIDER_REDIRECT_URL ?? '',
   };

   const response = await postToken(formUrlEncodedRecord);

   return response;
};
