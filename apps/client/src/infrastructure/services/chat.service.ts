import { createApi } from '@reduxjs/toolkit/query/react';
import { baseQuery } from './base-query';
import { BaseQueryApi, FetchArgs } from '@reduxjs/toolkit/query';
import { ChatbotMessagesRequest } from '~/domain/interfaces/catalog.interface';
import { TChatbotMessage } from '~/domain/types/catalog.type';

const baseQueryHandler = async (
   args: string | FetchArgs,
   api: BaseQueryApi,
   extraOptions: unknown,
) => {
   const result = await baseQuery('/catalog-services')(
      args,
      api,
      extraOptions as unknown as any,
   );

   return result;
};

export const chatApi = createApi({
   reducerPath: 'chat-api',
   tagTypes: ['Chat'],
   baseQuery: baseQueryHandler,
   endpoints: (builder) => ({
      sendChat: builder.mutation<TChatbotMessage, ChatbotMessagesRequest>({
         query: (body) => ({
            url: '/api/v1/chatbot/messages',
            method: 'POST',
            body,
         }),
      }),
   }),
});

export const { useSendChatMutation } = chatApi;
