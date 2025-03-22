import createWebStorage from 'redux-persist/lib/storage/createWebStorage';
import { WebStorage } from 'redux-persist/lib/types';

export function createPersistStorage(): WebStorage {
   const isServer = typeof window === 'undefined';

   // Returns noop (dummy) storage.
   if (isServer) {
      return {
         getItem() {
            return Promise.resolve(null);
         },
         setItem() {
            return Promise.resolve();
         },
         removeItem() {
            return Promise.resolve();
         },
      };
   }

   return createWebStorage('local');
}
