import { useCallback, useMemo } from 'react';
import {
   useLazyGetSkuRequestsQuery,
   useLazyGetSkuRequestByIdQuery,
   useCreateSkuRequestMutation,
   useUpdateSkuRequestMutation,
   IGetSkuRequestsParams,
   ICreateSkuRequestRequest,
   IUpdateSkuRequestRequest,
} from '~/src/infrastructure/services/request.service';
import { useCheckApiError } from '../use-check-error';
import { useCheckApiSuccess } from '../use-check-success';

const useRequestService = () => {
   const [getSkuRequestsTrigger, getSkuRequestsState] =
      useLazyGetSkuRequestsQuery();
   const [getSkuRequestByIdTrigger, getSkuRequestByIdState] =
      useLazyGetSkuRequestByIdQuery();
   const [createSkuRequestTrigger, createSkuRequestState] =
      useCreateSkuRequestMutation();
   const [updateSkuRequestTrigger, updateSkuRequestState] =
      useUpdateSkuRequestMutation();

   useCheckApiError([
      {
         title: 'Get SKU Requests Error',
         error: getSkuRequestsState.error,
      },
      {
         title: 'Get SKU Request Error',
         error: getSkuRequestByIdState.error,
      },
      {
         title: 'Create SKU Request Error',
         error: createSkuRequestState.error,
      },
      {
         title: 'Update SKU Request Error',
         error: updateSkuRequestState.error,
      },
   ]);

   useCheckApiSuccess([
      {
         title: 'Success',
         description: 'SKU request created successfully',
         isSuccess: createSkuRequestState.isSuccess,
      },
      {
         title: 'Success',
         description: 'SKU request updated successfully',
         isSuccess: updateSkuRequestState.isSuccess,
      },
   ]);

   const getSkuRequestsAsync = useCallback(
      async (params: IGetSkuRequestsParams) => {
         try {
            const result = await getSkuRequestsTrigger(params).unwrap();
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
      [getSkuRequestsTrigger],
   );

   const getSkuRequestByIdAsync = useCallback(
      async (id: string) => {
         try {
            const result = await getSkuRequestByIdTrigger(id).unwrap();
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
      [getSkuRequestByIdTrigger],
   );

   const createSkuRequestAsync = useCallback(
      async (body: ICreateSkuRequestRequest) => {
         try {
            const result = await createSkuRequestTrigger(body).unwrap();
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
      [createSkuRequestTrigger],
   );

   const updateSkuRequestAsync = useCallback(
      async (id: string, body: IUpdateSkuRequestRequest) => {
         try {
            const result = await updateSkuRequestTrigger({ id, body }).unwrap();
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
      [updateSkuRequestTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         getSkuRequestsState.isLoading ||
         getSkuRequestsState.isFetching ||
         getSkuRequestByIdState.isLoading ||
         getSkuRequestByIdState.isFetching ||
         createSkuRequestState.isLoading ||
         updateSkuRequestState.isLoading
      );
   }, [
      getSkuRequestsState.isLoading,
      getSkuRequestsState.isFetching,
      getSkuRequestByIdState.isLoading,
      getSkuRequestByIdState.isFetching,
      createSkuRequestState.isLoading,
      updateSkuRequestState.isLoading,
   ]);

   return {
      isLoading,
      getSkuRequestsState,
      getSkuRequestByIdState,
      createSkuRequestState,
      updateSkuRequestState,

      getSkuRequestsAsync,
      getSkuRequestByIdAsync,
      createSkuRequestAsync,
      updateSkuRequestAsync,
   };
};

export default useRequestService;
