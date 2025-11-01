'use client';

import { ChartLineLabel } from '@components/ui/chart-ui/chart-line-label';

const RevenueAnalytics = () => {
   return (
      <div className="flex flex-col flex-1 gap-4 p-4">
         <h1 className="text-3xl font-bold tracking-tight">
            Revenue Analytics
         </h1>

         <p className="text-muted-foreground">
            View your revenue analytics and compare them with the previous
            month.
         </p>

         <ChartLineLabel />
      </div>
   );
};

export default RevenueAnalytics;
