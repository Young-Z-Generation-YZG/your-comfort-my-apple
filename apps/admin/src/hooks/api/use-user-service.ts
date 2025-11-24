import { useCallback, useMemo } from 'react';
import { useCheckApiError } from '~/src/hooks/use-check-error';
import { useCheckApiSuccess } from '~/src/hooks/use-check-success';

import {
   IUpdateProfileByIdPayload,
   useLazyGetUserByUserIdQuery,
   useLazyGetAccountDetailsQuery,
   useUpdateProfileByUserIdMutation,
} from '~/src/infrastructure/services/user.service';

const useUserService = () => {
   const [getUserByUserIdTrigger, getUserByUserIdQueryState] =
      useLazyGetUserByUserIdQuery();
   const [getAccountDetailsTrigger, getAccountDetailsQueryState] =
      useLazyGetAccountDetailsQuery();

   const [updateProfileByUserIdTrigger, updateProfileByUserIdQueryState] =
      useUpdateProfileByUserIdMutation();

   useCheckApiError([
      {
         title: 'Failed to load user details',
         error: getUserByUserIdQueryState.error,
      },
      {
         title: 'Failed to update profile',
         error: updateProfileByUserIdQueryState.error,
      },
   ]);

   useCheckApiSuccess([
      {
         title: 'User details loaded successfully',
         isSuccess: getUserByUserIdQueryState.isSuccess,
      },
      {
         title: 'Profile updated successfully',
         isSuccess: updateProfileByUserIdQueryState.isSuccess,
      },
   ]);

   const getAccountDetailsAsync = useCallback(async () => {
      try {
         const result = await getAccountDetailsTrigger().unwrap();
         return {
            isSuccess: true,
            isError: false,
            data: result,
            error: null,
         };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [getAccountDetailsTrigger]);

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
      getAccountDetailsQueryState,

      // actions
      getUserByUserIdAsync,
      updateProfileByUserIdAsync,
      getAccountDetailsAsync,
   };
};

export default useUserService;
