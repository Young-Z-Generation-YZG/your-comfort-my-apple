import { useCallback, useMemo } from 'react';
import {
   IGetWarehousesQueryParams,
   useLazyGetWarehousesQuery,
} from '~/src/infrastructure/services/inventory.service';

const useInventoryService = () => {
   const [getWarehousesTrigger, getWarehousesState] =
      useLazyGetWarehousesQuery();

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

   const isLoading = useMemo(() => {
      return getWarehousesState.isLoading || getWarehousesState.isFetching;
   }, [getWarehousesState.isLoading, getWarehousesState.isFetching]);

   return {
      getWarehousesState,

      getWarehousesAsync,

      isLoading,
   };
};

export default useInventoryService;
