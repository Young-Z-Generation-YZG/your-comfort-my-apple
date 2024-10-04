'use server';

import { get } from '~/utils/http/server.http';
import { cookies } from 'next/headers';

interface IPostResponse {
   userId: number;
   id: number;
   title: string;
   body: string;
}

export const getUser = async (url: string) => {
   return await get<IPostResponse[]>(url, {});
};
