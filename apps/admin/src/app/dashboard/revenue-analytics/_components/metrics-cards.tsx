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

interface MetricsCardsProps {
   orders: TOrder[];
}

const MetricsCards = ({ orders }: MetricsCardsProps) => {
   // Calculate metrics from orders
   const metrics = useMemo(() => {
      // Filter paid/delivered orders
      const paidOrders = orders.filter(
         (order) => order.status === 'PAID' || order.status === 'DELIVERED',
      );

      // Calculate total revenue
      const totalRevenue = paidOrders.reduce(
         (sum, order) => sum + order.total_amount,
         0,
      );

      // Calculate total orders
      const totalOrders = paidOrders.length;

      // Calculate unique customers
      const uniqueCustomers = new Set(
         paidOrders.map((order) => order.customer_id),
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

      const last30DaysOrders = paidOrders.filter((order) => {
         const orderDate = new Date(order.created_at);
         return orderDate >= last30Days && orderDate <= now;
      });

      const previous30DaysOrders = paidOrders.filter((order) => {
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
            : 0;

      const ordersGrowthRate =
         previous30DaysOrders.length > 0
            ? ((last30DaysOrders.length - previous30DaysOrders.length) /
                 previous30DaysOrders.length) *
              100
            : 0;

      const customersGrowthRate =
         previous30DaysOrders.length > 0
            ? ((new Set(last30DaysOrders.map((o) => o.customer_id)).size -
                 new Set(previous30DaysOrders.map((o) => o.customer_id)).size) /
                 new Set(previous30DaysOrders.map((o) => o.customer_id)).size) *
              100
            : 0;

      const aovGrowthRate =
         previous30DaysOrders.length > 0
            ? ((last30DaysRevenue / last30DaysOrders.length -
                 previous30DaysRevenue / previous30DaysOrders.length) /
                 (previous30DaysRevenue / previous30DaysOrders.length)) *
              100
            : 0;

      return {
         totalRevenue,
         totalOrders,
         uniqueCustomers,
         averageOrderValue,
         revenueGrowthRate: Math.round(revenueGrowthRate * 10) / 10,
         ordersGrowthRate: Math.round(ordersGrowthRate * 10) / 10,
         customersGrowthRate: Math.round(customersGrowthRate * 10) / 10,
         aovGrowthRate: Math.round(aovGrowthRate * 10) / 10,
      };
   }, [orders]);

   const metricCards = [
      {
         title: 'Total Revenue',
         value: `$${metrics.totalRevenue.toLocaleString('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
         })}`,
         description: 'From paid and delivered orders',
         growthRate: metrics.revenueGrowthRate,
         icon: DollarSign,
         trendText:
            metrics.revenueGrowthRate >= 0
               ? 'Trending up this period'
               : 'Trending down this period',
      },
      {
         title: 'Total Orders',
         value: metrics.totalOrders.toLocaleString(),
         description: 'Paid and delivered orders',
         growthRate: metrics.ordersGrowthRate,
         icon: ShoppingCart,
         trendText:
            metrics.ordersGrowthRate >= 0
               ? 'More orders this period'
               : 'Fewer orders this period',
      },
      {
         title: 'Unique Customers',
         value: metrics.uniqueCustomers.toLocaleString(),
         description: 'Customers with paid orders',
         growthRate: metrics.customersGrowthRate,
         icon: Users,
         trendText:
            metrics.customersGrowthRate >= 0
               ? 'More customers this period'
               : 'Fewer customers this period',
      },
      {
         title: 'Average Order Value',
         value: `$${metrics.averageOrderValue.toLocaleString('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
         })}`,
         description: 'Average revenue per order',
         growthRate: metrics.aovGrowthRate,
         icon: TrendingUp,
         trendText:
            metrics.aovGrowthRate >= 0
               ? 'Higher AOV this period'
               : 'Lower AOV this period',
      },
   ];

   return (
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
         {metricCards.map((metric) => {
            const Icon = metric.icon;
            const isPositive = metric.growthRate >= 0;

            return (
               <Card key={metric.title}>
                  <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                     <CardTitle className="text-sm font-medium">
                        {metric.title}
                     </CardTitle>
                     <Badge
                        variant={isPositive ? 'secondary' : 'destructive'}
                        className="gap-1"
                     >
                        {isPositive ? (
                           <ArrowUpRight className="h-3 w-3" />
                        ) : (
                           <ArrowDownRight className="h-3 w-3" />
                        )}{' '}
                        {isPositive ? '+' : ''}
                        {metric.growthRate.toFixed(1)}%
                     </Badge>
                  </CardHeader>
                  <CardContent>
                     <div className="text-2xl font-bold">{metric.value}</div>
                     <p className="text-xs text-muted-foreground mt-3 flex items-center gap-2">
                        {metric.trendText}
                        {isPositive ? (
                           <TrendingUp className="h-3.5 w-3.5" />
                        ) : (
                           <TrendingUp className="h-3.5 w-3.5 rotate-180" />
                        )}
                     </p>
                     <p className="text-xs text-muted-foreground">
                        {metric.description}
                     </p>
                  </CardContent>
               </Card>
            );
         })}
      </div>
   );
};

export default MetricsCards;

