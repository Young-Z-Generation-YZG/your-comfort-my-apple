import { useCallback, useMemo } from 'react';
import { useCheckApiError } from '~/src/hooks/use-check-error';
import { useCheckApiSuccess } from '~/src/hooks/use-check-success';
import { toast } from 'sonner';

import {
   IUpdateProfileByIdPayload,
   useLazyGetUserByUserIdQuery,
   useLazyGetAccountDetailsQuery,
   useUpdateProfileByUserIdMutation,
   useAssignRolesMutation,
   useLazyGetUserRolesQuery,
} from '~/src/infrastructure/services/user.service';
import { IAssignRolesPayload } from '~/src/domain/types/identity.type';

const useUserService = () => {
   const [getUserByUserIdTrigger, getUserByUserIdQueryState] =
      useLazyGetUserByUserIdQuery();
   const [getAccountDetailsTrigger, getAccountDetailsQueryState] =
      useLazyGetAccountDetailsQuery();

   const [updateProfileByUserIdTrigger, updateProfileByUserIdQueryState] =
      useUpdateProfileByUserIdMutation();
   const [assignRolesMutation, assignRolesState] = useAssignRolesMutation();
   const [getUserRolesTrigger, getUserRolesQueryState] =
      useLazyGetUserRolesQuery();

   useCheckApiError([
      {
         title: 'Failed to load user details',
         error: getUserByUserIdQueryState.error,
      },
      {
         title: 'Failed to update profile',
         error: updateProfileByUserIdQueryState.error,
      },
      {
         title: 'Failed to assign roles',
         error: assignRolesState.error,
      },
      {
         title: 'Failed to get user roles',
         error: getUserRolesQueryState.error,
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
      {
         title: 'Roles assigned successfully',
         isSuccess: assignRolesState.isSuccess,
      },
      {
         title: 'User roles loaded successfully',
         isSuccess: getUserRolesQueryState.isSuccess,
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

   const assignRolesAsync = useCallback(
      async (payload: IAssignRolesPayload) => {
         try {
            const result = await assignRolesMutation(payload).unwrap();
            toast.success('Roles assigned successfully');
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
      [assignRolesMutation],
   );

   const getUserRolesAsync = useCallback(
      async (userId: string) => {
         try {
            const result = await getUserRolesTrigger(userId).unwrap();
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
      [getUserRolesTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         getUserByUserIdQueryState.isLoading ||
         updateProfileByUserIdQueryState.isLoading ||
         assignRolesState.isLoading ||
         getUserRolesQueryState.isLoading
      );
   }, [
      getUserByUserIdQueryState.isLoading,
      updateProfileByUserIdQueryState.isLoading,
      assignRolesState.isLoading,
      getUserRolesQueryState.isLoading,
   ]);

   return {
      // states
      isLoading,
      getUserByUserIdQueryState,
      updateProfileByUserIdQueryState,
      getAccountDetailsQueryState,
      assignRolesState,
      getUserRolesQueryState,

      // actions
      getUserByUserIdAsync,
      updateProfileByUserIdAsync,
      getAccountDetailsAsync,
      assignRolesAsync,
      getUserRolesAsync,
   };
};

export default useUserService;
