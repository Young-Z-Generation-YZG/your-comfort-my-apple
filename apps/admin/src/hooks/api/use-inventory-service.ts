import { useCallback, useMemo } from 'react';
import {
   IGetWarehousesQueryParams,
   useLazyGetSkuByIdQuery,
   useLazyGetSkuByIdWithImageQuery,
   useLazyGetWarehousesQuery,
} from '~/src/infrastructure/services/inventory.service';

const useInventoryService = () => {
   const [getWarehousesTrigger, getWarehousesState] =
      useLazyGetWarehousesQuery();

   const [getSkuByIdTrigger, getSkuByIdState] = useLazyGetSkuByIdQuery();

   const [getSkuByIdWithImageTrigger, getSkuByIdWithImageState] =
      useLazyGetSkuByIdWithImageQuery();

   const getWarehousesAsync = useCallback(
      async (params: IGetWarehousesQueryParams) => {
         try {
            const result = await getWarehousesTrigger(params).unwrap();
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
      [getWarehousesTrigger],
   );

   const getSkuByIdAsync = useCallback(
      async (id: string) => {
         try {
            const result = await getSkuByIdTrigger(id).unwrap();
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
      [getSkuByIdTrigger],
   );

   const getSkuByIdWithImageAsync = useCallback(
      async (id: string) => {
         try {
            const result = await getSkuByIdWithImageTrigger(id).unwrap();
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
      [getSkuByIdWithImageTrigger],
   );

   const isLoading = useMemo(() => {
      return (
         getWarehousesState.isLoading ||
         getWarehousesState.isFetching ||
         getSkuByIdState.isLoading ||
         getSkuByIdState.isFetching ||
         getSkuByIdWithImageState.isLoading ||
         getSkuByIdWithImageState.isFetching
      );
   }, [
      getWarehousesState.isLoading,
      getWarehousesState.isFetching,
      getSkuByIdState.isLoading,
      getSkuByIdState.isFetching,
      getSkuByIdWithImageState.isLoading,
      getSkuByIdWithImageState.isFetching,
   ]);

   return {
      getWarehousesState,
      getSkuByIdState,
      getSkuByIdWithImageState,

      getWarehousesAsync,
      getSkuByIdAsync,
      getSkuByIdWithImageAsync,

      isLoading,
   };
};

export default useInventoryService;
