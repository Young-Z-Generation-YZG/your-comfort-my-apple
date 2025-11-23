import { useCallback, useMemo } from 'react';
import { useCheckApiError } from '~/src/hooks/use-check-error';

import { toast } from 'sonner';

import {
   IUpdateProfileByIdPayload,
   useLazyGetUserByUserIdQuery,
   useUpdateProfileByUserIdMutation,
} from '~/src/infrastructure/services/user.service';

const useUserService = () => {
   const [getUserByUserIdTrigger, getUserByUserIdQueryState] =
      useLazyGetUserByUserIdQuery();
   const [updateProfileByUserIdTrigger, updateProfileByUserIdQueryState] =
      useUpdateProfileByUserIdMutation();

   useCheckApiError([]);

   const getUserByUserIdAsync = useCallback(
      async (params: any) => {
         try {
            const result = await getUserByUserIdTrigger(params).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return {
               isSuccess: false,
               isError: true,
               data: null,
               error,
            };
         }
      },
      [getUserByUserIdTrigger],
   );

   const updateProfileByUserIdAsync = useCallback(
      async (userId: string, body: IUpdateProfileByIdPayload) => {
         try {
            const result = await updateProfileByUserIdTrigger({
               userId,
               body,
            }).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return {
               isSuccess: false,
               isError: true,
               data: null,
               error,
            };
         }
      },
      [updateProfileByUserIdTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         getUserByUserIdQueryState.isLoading ||
         updateProfileByUserIdQueryState.isLoading
      );
   }, [
      getUserByUserIdQueryState.isLoading,
      updateProfileByUserIdQueryState.isLoading,
   ]);

   return {
      // states
      isLoading,
      getUserByUserIdQueryState,
      updateProfileByUserIdQueryState,

      // actions
      getUserByUserIdAsync,
      updateProfileByUserIdAsync,
   };
};

export default useUserService;
