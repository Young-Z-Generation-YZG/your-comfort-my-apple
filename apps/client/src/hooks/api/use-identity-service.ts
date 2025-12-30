import { useCallback, useMemo } from 'react';
import { useCheckApiError } from '~/hooks/use-check-error';
import {
   IAddressPayload,
   IChangePasswordPayload,
} from '~/domain/interfaces/identity.interface';
import { IUpdateProfilePayload } from '~/domain/interfaces/identity.interface';
import {
   useUpdateProfileMutation,
   useAddAddressMutation,
   useUpdateAddressMutation,
   useSetDefaultAddressMutation,
   useDeleteAddressMutation,
   useLazyGetMeQuery,
   useLazyGetAddressesQuery,
} from '~/infrastructure/services/identity.service';
import {
   useCancelOrderMutation,
   useConfirmOrderMutation,
} from '~/infrastructure/services/ordering.service';
import { useChangePasswordMutation } from '~/infrastructure/services/auth.service';
import { useCheckApiSuccess } from '~/hooks/use-check-success';

const useIdentityService = () => {
   const [getMeTrigger, getMeQueryState] = useLazyGetMeQuery();
   const [getAddressesTrigger, getAddressesQueryState] =
      useLazyGetAddressesQuery();

   const [updateProfileMutation, updateProfileMutationState] =
      useUpdateProfileMutation();
   const [addAddressMutation, addAddressMutationState] =
      useAddAddressMutation();
   const [updateAddressMutation, updateAddressMutationState] =
      useUpdateAddressMutation();
   const [setDefaultAddressMutation, setDefaultAddressMutationState] =
      useSetDefaultAddressMutation();
   const [deleteAddressMutation, deleteAddressMutationState] =
      useDeleteAddressMutation();
   const [changePasswordMutation, changePasswordMutationState] =
      useChangePasswordMutation();
   const [cancelOrderMutation, cancelOrderMutationState] =
      useCancelOrderMutation();
   const [confirmOrderMutation, confirmOrderMutationState] =
      useConfirmOrderMutation();

   useCheckApiError([
      {
         title: 'Update Profile failed',
         error: updateProfileMutationState.error,
      },
      { title: 'Add Address failed', error: addAddressMutationState.error },
      {
         title: 'Update Address failed',
         error: updateAddressMutationState.error,
      },
      {
         title: 'Set Default Address failed',
         error: setDefaultAddressMutationState.error,
      },
      {
         title: 'Delete Address failed',
         error: deleteAddressMutationState.error,
      },
      {
         title: 'Change Password failed',
         error: changePasswordMutationState.error,
      },
      {
         title: 'Cancel Order failed',
         error: cancelOrderMutationState.error,
      },
      {
         title: 'Confirm Order failed',
         error: confirmOrderMutationState.error,
      },
   ]);

   useCheckApiSuccess([
      {
         title: 'Profile updated successfully',
         isSuccess: updateProfileMutationState.isSuccess,
      },
      {
         title: 'Address added successfully',
         isSuccess: addAddressMutationState.isSuccess,
      },
      {
         title: 'Address updated successfully',
         isSuccess: updateAddressMutationState.isSuccess,
      },
      {
         title: 'Default address updated',
         isSuccess: setDefaultAddressMutationState.isSuccess,
      },
      {
         title: 'Address deleted successfully',
         isSuccess: deleteAddressMutationState.isSuccess,
      },
      {
         title: 'Password changed successfully',
         isSuccess: changePasswordMutationState.isSuccess,
      },
      {
         title: 'Order canceled successfully',
         isSuccess: cancelOrderMutationState.isSuccess,
      },
      {
         title: 'Order confirmed successfully',
         isSuccess: confirmOrderMutationState.isSuccess,
      },
   ]);

   const updateProfileAsync = useCallback(
      async (data: IUpdateProfilePayload) => {
         try {
            const result = await updateProfileMutation(data).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [updateProfileMutation],
   );

   const addAddressAsync = useCallback(
      async (data: IAddressPayload) => {
         try {
            const result = await addAddressMutation(data).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [addAddressMutation],
   );

   const updateAddressAsync = useCallback(
      async (addressId: string, payload: IAddressPayload) => {
         try {
            const result = await updateAddressMutation({
               addressId,
               payload,
            }).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [updateAddressMutation],
   );

   const setDefaultAddressAsync = useCallback(
      async (addressId: string) => {
         try {
            const result = await setDefaultAddressMutation(addressId).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [setDefaultAddressMutation],
   );

   const deleteAddressAsync = useCallback(
      async (addressId: string) => {
         try {
            const result = await deleteAddressMutation(addressId).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [deleteAddressMutation],
   );

   const changePasswordAsync = useCallback(
      async (data: IChangePasswordPayload) => {
         try {
            const result = await changePasswordMutation(data).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [changePasswordMutation],
   );

   const cancelOrderAsync = useCallback(
      async (orderId: string) => {
         try {
            const result = await cancelOrderMutation(orderId).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [cancelOrderMutation],
   );

   const confirmOrderAsync = useCallback(
      async (orderId: string) => {
         try {
            const result = await confirmOrderMutation(orderId).unwrap();
            return {
               isSuccess: true,
               isError: false,
               data: result,
               error: null,
            };
         } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
         }
      },
      [confirmOrderMutation],
   );

   const getMeAsync = useCallback(async () => {
      try {
         const result = await getMeTrigger().unwrap();
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
   }, [getMeTrigger]);

   const getAddressesAsync = useCallback(async () => {
      try {
         const result = await getAddressesTrigger().unwrap();
         return {
            isSuccess: true,
            isError: false,
            data: result,
            error: null,
         };
      } catch (error) {
         return { isSuccess: false, isError: true, data: null, error };
      }
   }, [getAddressesTrigger]);

   const isLoading = useMemo(() => {
      return (
         getMeQueryState.isLoading ||
         getAddressesQueryState.isLoading ||
         updateProfileMutationState.isLoading ||
         addAddressMutationState.isLoading ||
         updateAddressMutationState.isLoading ||
         setDefaultAddressMutationState.isLoading ||
         deleteAddressMutationState.isLoading ||
         changePasswordMutationState.isLoading ||
         cancelOrderMutationState.isLoading ||
         confirmOrderMutationState.isLoading
      );
   }, [
      getMeQueryState.isLoading,
      getAddressesQueryState.isLoading,
      updateProfileMutationState.isLoading,
      addAddressMutationState.isLoading,
      updateAddressMutationState.isLoading,
      setDefaultAddressMutationState.isLoading,
      deleteAddressMutationState.isLoading,
      changePasswordMutationState.isLoading,
      cancelOrderMutationState.isLoading,
      confirmOrderMutationState.isLoading,
   ]);

   return {
      // states
      isLoading,
      getMeState: getMeQueryState,
      getAddressesState: getAddressesQueryState,
      updateProfileState: updateProfileMutationState,
      addAddressState: addAddressMutationState,
      updateAddressState: updateAddressMutationState,
      setDefaultAddressState: setDefaultAddressMutationState,
      deleteAddressState: deleteAddressMutationState,
      changePasswordState: changePasswordMutationState,
      cancelOrderState: cancelOrderMutationState,
      confirmOrderState: confirmOrderMutationState,

      // actions
      updateProfileAsync,
      addAddressAsync,
      updateAddressAsync,
      setDefaultAddressAsync,
      deleteAddressAsync,
      changePasswordAsync,
      cancelOrderAsync,
      confirmOrderAsync,
      getMeAsync,
      getAddressesAsync,
   };
};

export default useIdentityService;
