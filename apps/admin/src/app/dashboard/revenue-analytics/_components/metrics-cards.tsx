'use client';

import {
   Card,
   CardContent,
   CardHeader,
   CardTitle,
} from '@components/ui/card';
import { Badge } from '@components/ui/badge';
import {
   DollarSign,
   ShoppingCart,
   Users,
   TrendingUp,
   ArrowUpRight,
   ArrowDownRight,
} from 'lucide-react';
import { useMemo } from 'react';
import { TOrder } from '~/src/domain/types/ordering.type';
import { cn } from '~/src/infrastructure/lib/utils';

interface MetricsCardsProps {
   orders: TOrder[];
}

const MetricsCards = ({ orders }: MetricsCardsProps) => {
   // Calculate metrics from orders
   const metrics = useMemo(() => {
      // Filter delivered orders (most accurate for revenue)
      const deliveredOrders = orders.filter(
         (order) => order.status === 'DELIVERED',
      );

      // Filter paid orders (for comparison)
      const paidOrders = orders.filter((order) => order.status === 'PAID');

      // Calculate total revenue from delivered orders
      const totalRevenue = deliveredOrders.reduce(
         (sum, order) => sum + order.total_amount,
         0,
      );

      // Calculate total orders (delivered)
      const totalOrders = deliveredOrders.length;

      // Calculate unique customers from delivered orders
      const uniqueCustomers = new Set(
         deliveredOrders.map((order) => order.customer_id),
      ).size;

      // Calculate average order value
      const averageOrderValue =
         totalOrders > 0 ? totalRevenue / totalOrders : 0;

      // Calculate growth rate (comparing last 30 days vs previous 30 days)
      const now = new Date();
      const last30Days = new Date(now.getTime() - 30 * 24 * 60 * 60 * 1000);
      const previous30Days = new Date(
         now.getTime() - 60 * 24 * 60 * 60 * 1000,
      );

      const last30DaysOrders = deliveredOrders.filter((order) => {
         const orderDate = new Date(order.created_at);
         return orderDate >= last30Days && orderDate <= now;
      });

      const previous30DaysOrders = deliveredOrders.filter((order) => {
         const orderDate = new Date(order.created_at);
         return orderDate >= previous30Days && orderDate < last30Days;
      });

      const last30DaysRevenue = last30DaysOrders.reduce(
         (sum, order) => sum + order.total_amount,
         0,
      );

      const previous30DaysRevenue = previous30DaysOrders.reduce(
         (sum, order) => sum + order.total_amount,
         0,
      );

      const revenueGrowthRate =
         previous30DaysRevenue > 0
            ? ((last30DaysRevenue - previous30DaysRevenue) /
                 previous30DaysRevenue) *
              100
            : previous30DaysRevenue === 0 && last30DaysRevenue > 0
              ? 100
              : 0;

      const ordersGrowthRate =
         previous30DaysOrders.length > 0
            ? ((last30DaysOrders.length - previous30DaysOrders.length) /
                 previous30DaysOrders.length) *
              100
            : previous30DaysOrders.length === 0 && last30DaysOrders.length > 0
              ? 100
              : 0;

      const previous30DaysUniqueCustomers = new Set(
         previous30DaysOrders.map((o) => o.customer_id),
      ).size;

      const last30DaysUniqueCustomers = new Set(
         last30DaysOrders.map((o) => o.customer_id),
      ).size;

      const customersGrowthRate =
         previous30DaysUniqueCustomers > 0
            ? ((last30DaysUniqueCustomers - previous30DaysUniqueCustomers) /
                 previous30DaysUniqueCustomers) *
              100
            : previous30DaysUniqueCustomers === 0 &&
                last30DaysUniqueCustomers > 0
              ? 100
              : 0;

      const previousAOV =
         previous30DaysOrders.length > 0
            ? previous30DaysRevenue / previous30DaysOrders.length
            : 0;

      const lastAOV =
         last30DaysOrders.length > 0
            ? last30DaysRevenue / last30DaysOrders.length
            : 0;

      const aovGrowthRate =
         previousAOV > 0 ? ((lastAOV - previousAOV) / previousAOV) * 100 : 0;

      return {
         totalRevenue,
         totalOrders,
         uniqueCustomers,
         averageOrderValue,
         revenueGrowthRate: Math.round(revenueGrowthRate * 10) / 10,
         ordersGrowthRate: Math.round(ordersGrowthRate * 10) / 10,
         customersGrowthRate: Math.round(customersGrowthRate * 10) / 10,
         aovGrowthRate: Math.round(aovGrowthRate * 10) / 10,
         pendingOrders: paidOrders.length,
      };
   }, [orders]);

   const metricCards = [
      {
         title: 'Total Revenue',
         value: `$${metrics.totalRevenue.toLocaleString('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
         })}`,
         description: `From ${metrics.totalOrders.toLocaleString()} delivered orders`,
         growthRate: metrics.revenueGrowthRate,
         icon: DollarSign,
         iconColor: 'text-green-600 dark:text-green-400',
         iconBg: 'bg-green-100 dark:bg-green-900/30',
         trendText:
            metrics.revenueGrowthRate >= 0
               ? 'vs previous 30 days'
               : 'vs previous 30 days',
      },
      {
         title: 'Total Orders',
         value: metrics.totalOrders.toLocaleString(),
         description: `${metrics.pendingOrders} pending orders`,
         growthRate: metrics.ordersGrowthRate,
         icon: ShoppingCart,
         iconColor: 'text-blue-600 dark:text-blue-400',
         iconBg: 'bg-blue-100 dark:bg-blue-900/30',
         trendText:
            metrics.ordersGrowthRate >= 0
               ? 'vs previous 30 days'
               : 'vs previous 30 days',
      },
      {
         title: 'Unique Customers',
         value: metrics.uniqueCustomers.toLocaleString(),
         description: 'Active customers with orders',
         growthRate: metrics.customersGrowthRate,
         icon: Users,
         iconColor: 'text-purple-600 dark:text-purple-400',
         iconBg: 'bg-purple-100 dark:bg-purple-900/30',
         trendText:
            metrics.customersGrowthRate >= 0
               ? 'vs previous 30 days'
               : 'vs previous 30 days',
      },
      {
         title: 'Average Order Value',
         value: `$${metrics.averageOrderValue.toLocaleString('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
         })}`,
         description: 'Revenue per delivered order',
         growthRate: metrics.aovGrowthRate,
         icon: TrendingUp,
         iconColor: 'text-orange-600 dark:text-orange-400',
         iconBg: 'bg-orange-100 dark:bg-orange-900/30',
         trendText:
            metrics.aovGrowthRate >= 0
               ? 'vs previous 30 days'
               : 'vs previous 30 days',
      },
   ];

   return (
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
         {metricCards.map((metric) => {
            const Icon = metric.icon;
            const isPositive = metric.growthRate >= 0;
            const hasGrowth = Math.abs(metric.growthRate) > 0;

            return (
               <Card
                  key={metric.title}
                  className="relative overflow-hidden transition-all hover:shadow-md"
               >
                  <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                     <CardTitle className="text-sm font-medium text-muted-foreground">
                        {metric.title}
                     </CardTitle>
                     <div
                        className={cn(
                           'flex h-10 w-10 items-center justify-center rounded-lg',
                           metric.iconBg,
                        )}
                     >
                        <Icon className={cn('h-5 w-5', metric.iconColor)} />
                     </div>
                  </CardHeader>
                  <CardContent>
                     <div className="text-3xl font-bold tracking-tight">
                        {metric.value}
                     </div>
                     <div className="mt-4 flex items-center justify-between">
                        <p className="text-xs text-muted-foreground">
                           {metric.description}
                        </p>
                        {hasGrowth && (
                           <Badge
                              variant={isPositive ? 'secondary' : 'destructive'}
                              className="ml-auto gap-1 text-xs"
                           >
                              {isPositive ? (
                                 <ArrowUpRight className="h-3 w-3" />
                              ) : (
                                 <ArrowDownRight className="h-3 w-3" />
                              )}
                              {isPositive ? '+' : ''}
                              {metric.growthRate.toFixed(1)}%
                           </Badge>
                        )}
                     </div>
                     {hasGrowth && (
                        <p className="mt-2 text-xs text-muted-foreground">
                           {metric.trendText}
                        </p>
                     )}
                  </CardContent>
               </Card>
            );
         })}
      </div>
   );
};

export default MetricsCards;

