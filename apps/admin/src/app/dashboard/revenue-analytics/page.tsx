'use client';

import { Card, CardContent } from '@components/ui/card';
import { useMemo, useEffect } from 'react';

import useOrderingService from '~/src/hooks/api/use-ordering-service';
import { LoadingOverlay } from '@components/loading-overlay';
import YearOverYearChart from './_components/year-over-year-chart';
import MultiTenantChart from './_components/multi-tenant-chart';
import RevenueOrdersChart from './_components/revenue-orders-chart';
import MetricsCards from './_components/metrics-cards';
import { useAppSelector } from '~/src/infrastructure/redux/store';

const RevenueAnalytics = () => {
   const {
      getRevenuesAsync,
      getRevenuesState,
      getRevenuesByYearsState,
      getRevenuesByTenantsState,
      isLoading,
   } = useOrderingService();

   const { currentUser } = useAppSelector((state) => state.auth);

   const isSuperAdmin = useMemo(() => {
      return currentUser?.roles?.includes('ADMIN_SUPER_YBZONE') || currentUser?.roles?.includes('ADMIN_YBZONE');
   }, [currentUser]);

   // Get orders from revenues API response
   const orders = useMemo(() => {
      return getRevenuesState.data || [];
   }, [getRevenuesState.data]);

   useEffect(() => {
      // Fetch revenues on component mount
      getRevenuesAsync();
   }, [getRevenuesAsync]);

   return (
      <div className="flex flex-col flex-1 gap-4 p-4">
         <LoadingOverlay isLoading={isLoading} />

         <h1 className="text-3xl font-bold tracking-tight">
            Revenue Analytics
         </h1>

         <p className="text-muted-foreground">
            View your revenue analytics and compare them with the previous
            month.
         </p>

         {/* Error State */}
         {(getRevenuesState.isError ||
            getRevenuesByYearsState.isError ||
            getRevenuesByTenantsState.isError) && (
            <Card>
               <CardContent className="pt-6">
                  <div className="flex flex-col items-center justify-center py-8">
                     <p className="text-destructive text-sm font-medium">
                        Failed to load revenue data
                     </p>
                     <p className="text-muted-foreground text-xs mt-2">
                        Please try again later
                     </p>
                  </div>
               </CardContent>
            </Card>
         )}

         {/* Metrics Cards */}
         {!getRevenuesState.isError && (
            <div className="mt-4">
               <MetricsCards orders={orders} />
            </div>
         )}

         <div className="mt-6">
            <RevenueOrdersChart />

            {isSuperAdmin && <MultiTenantChart />}

            {isSuperAdmin && <YearOverYearChart />}
         </div>
      </div>
   );
};

export default RevenueAnalytics;
