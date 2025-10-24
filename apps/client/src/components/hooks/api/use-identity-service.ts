import { useCallback, useMemo } from 'react';
import { useCheckApiError } from '@components/hooks/use-check-error';
import { IAddressPayload } from '~/domain/interfaces/identity/address';
import { IProfilePayload } from '~/domain/interfaces/identity/profile';
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
import { changePasswordFormType } from '~/domain/schemas/auth.schema';
import { toast } from 'sonner';

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

   const updateProfileAsync = useCallback(
      async (data: IProfilePayload) => {
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
      async (data: { id: string; payload: IAddressPayload }) => {
         try {
            const result = await updateAddressMutation({
               id: data.id,
               payload: data.payload,
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
      async (id: string) => {
         try {
            const result = await setDefaultAddressMutation(id).unwrap();
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
      async (id: string) => {
         try {
            const result = await deleteAddressMutation(id).unwrap();
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
      async (data: changePasswordFormType) => {
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

   // Centrally track success with toasts
   useMemo(() => {
      const successToastStyle = {
         style: {
            backgroundColor: '#DCFCE7',
            color: '#166534',
            border: '1px solid #86EFAC',
         },
         cancel: {
            label: 'Close',
            onClick: () => {},
            actionButtonStyle: {
               backgroundColor: '#16A34A',
               color: '#FFFFFF',
            },
         },
      };
      if (updateProfileMutationState.isSuccess) {
         toast.success('Profile updated successfully', successToastStyle);
      }
      if (addAddressMutationState.isSuccess) {
         toast.success('Address added successfully', successToastStyle);
      }
      if (updateAddressMutationState.isSuccess) {
         toast.success('Address updated successfully', successToastStyle);
      }
      if (setDefaultAddressMutationState.isSuccess) {
         toast.success('Default address updated', successToastStyle);
      }
      if (deleteAddressMutationState.isSuccess) {
         toast.success('Address deleted successfully', successToastStyle);
      }
      if (changePasswordMutationState.isSuccess) {
         toast.success('Password changed successfully', successToastStyle);
      }
      if (cancelOrderMutationState.isSuccess) {
         toast.success('Order canceled successfully', successToastStyle);
      }
      if (confirmOrderMutationState.isSuccess) {
         toast.success('Order confirmed successfully', successToastStyle);
      }
   }, [
      updateProfileMutationState.isSuccess,
      addAddressMutationState.isSuccess,
      updateAddressMutationState.isSuccess,
      setDefaultAddressMutationState.isSuccess,
      deleteAddressMutationState.isSuccess,
      changePasswordMutationState.isSuccess,
      cancelOrderMutationState.isSuccess,
      confirmOrderMutationState.isSuccess,
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
