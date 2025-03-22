import { get } from '~/infrastructure/utils/http/server.http';
import { IPostResponse } from './post.type';

export const getPosts = async () => {
   return await get<IPostResponse[]>('');
};
