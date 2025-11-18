'use client';

import {
   Card,
   CardContent,
   CardDescription,
   CardFooter,
   CardHeader,
   CardTitle,
} from '@components/ui/card';

import { isWithinInterval, startOfMonth } from 'date-fns';
import { useState, useMemo, useEffect } from 'react';

import { type DateRange } from 'react-day-picker';

import useOrderingService from '~/src/hooks/api/use-ordering-service';
import { LoadingOverlay } from '@components/loading-overlay';
import YearOverYearChart from './_components/year-over-year-chart';
import MultiTenantChart from './_components/multi-tenant-chart';
import RevenueOrdersChart from './_components/revenue-orders-chart';

export const description = 'A line chart with a label';

type FilterMetric = 'revenue' | 'orders' | 'both';
type GroupBy = 'date' | 'month';

const RevenueAnalytics = () => {
   // Filter states for Chart 1
   const [range, setRange] = useState<DateRange | undefined>(undefined);
   const [filterMetric, setFilterMetric] = useState<FilterMetric>('both');
   const [groupBy, setGroupBy] = useState<GroupBy>('date');
   const [isFiltered, setIsFiltered] = useState(false);

   const {
      getRevenuesAsync,
      getRevenuesState,
      getRevenuesByYearsState,
      getRevenuesByTenantsState,
      isLoading,
   } = useOrderingService();

   // Get orders from revenues API response (Chart 1)
   const orders = useMemo(() => {
      return getRevenuesState.data || [];
   }, [getRevenuesState.data]);

   // Calculate metrics from real data
   //    const metrics = useMemo(() => {
   //       const paidOrders = orders.filter((order) => order.status === 'PAID');
   //       const totalRevenue = paidOrders.reduce(
   //          (sum, order) => sum + order.total_amount,
   //          0,
   //       );
   //       const totalOrders = paidOrders.length;
   //       const uniqueCustomers = new Set(
   //          paidOrders.map((order) => order.customer_id),
   //       ).size;

   //       // Calculate growth rate (comparing last 30 days vs previous 30 days)
   //       const now = new Date();
   //       const last30Days = new Date(now.getTime() - 30 * 24 * 60 * 60 * 1000);
   //       const previous30Days = new Date(now.getTime() - 60 * 24 * 60 * 60 * 1000);

   //       const last30DaysRevenue = paidOrders
   //          .filter((order) => {
   //             const orderDate = new Date(order.created_at);
   //             return orderDate >= last30Days && orderDate <= now;
   //          })
   //          .reduce((sum, order) => sum + order.total_amount, 0);

   //       const previous30DaysRevenue = paidOrders
   //          .filter((order) => {
   //             const orderDate = new Date(order.created_at);
   //             return orderDate >= previous30Days && orderDate < last30Days;
   //          })
   //          .reduce((sum, order) => sum + order.total_amount, 0);

   //       const growthRate =
   //          previous30DaysRevenue > 0
   //             ? ((last30DaysRevenue - previous30DaysRevenue) /
   //                  previous30DaysRevenue) *
   //               100
   //             : 0;

   //       return {
   //          totalRevenue,
   //          totalOrders,
   //          uniqueCustomers,
   //          growthRate: Math.round(growthRate * 10) / 10,
   //       };
   //    }, [orders]);

   // Process and filter data
   const chartData = useMemo(() => {
      const dateMap = new Map<
         string,
         { revenue: number; orders: number; timestamp: number }
      >();

      orders.forEach((order) => {
         if (order.status === 'PAID') {
            const date = new Date(order.created_at);

            // Apply date range filter if set and filter is active
            if (isFiltered && range?.from && range?.to) {
               const isInRange = isWithinInterval(date, {
                  start: range.from,
                  end: range.to,
               });
               if (!isInRange) return;
            }

            // Group by date or month
            let dateKey: string;
            let timestamp: number;

            if (groupBy === 'month') {
               dateKey = date.toLocaleDateString('en-US', {
                  month: 'short',
                  year: 'numeric',
               });
               timestamp = startOfMonth(date).getTime();
            } else {
               dateKey = date.toLocaleDateString('en-US', {
                  month: 'short',
                  day: 'numeric',
               });
               timestamp = date.getTime();
            }

            if (!dateMap.has(dateKey)) {
               dateMap.set(dateKey, {
                  revenue: 0,
                  orders: 0,
                  timestamp,
               });
            }

            const current = dateMap.get(dateKey)!;
            current.revenue += order.total_amount;
            current.orders += 1;
         }
      });

      // Sort by timestamp
      return Array.from(dateMap.entries())
         .sort(([, a], [, b]) => a.timestamp - b.timestamp)
         .map(([date, data]) => ({
            date,
            revenue: data.revenue,
            orders: data.orders,
         }));
   }, [orders, range, groupBy, isFiltered]);

   // Handle filter submission
   const handleSubmitFilter = () => {
      setIsFiltered(true);
   };

   // Reset filter
   const handleResetFilter = () => {
      setRange(undefined);
      setFilterMetric('both');
      setGroupBy('date');
      setIsFiltered(false);
   };

   useEffect(() => {
      // Fetch revenues for Chart 1
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

         {/* Cards */}
         {/* {!getOrdersByAdminState.isError && (
            <div>
               <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                  <Card>
                     <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <CardTitle className="text-sm font-medium">
                           Total Revenue
                        </CardTitle>
                        <Badge
                           variant={
                              metrics.growthRate >= 0
                                 ? 'secondary'
                                 : 'destructive'
                           }
                           className="gap-1"
                        >
                           {metrics.growthRate >= 0 ? (
                              <ArrowUpRight className="h-3 w-3" />
                           ) : (
                              <ArrowDownRight className="h-3 w-3" />
                           )}{' '}
                           {metrics.growthRate >= 0 ? '+' : ''}
                           {metrics.growthRate.toFixed(1)}%
                        </Badge>
                     </CardHeader>
                     <CardContent>
                        <div className="text-2xl font-bold">
                           $
                           {metrics.totalRevenue.toLocaleString('en-US', {
                              minimumFractionDigits: 2,
                              maximumFractionDigits: 2,
                           })}
                        </div>
                        <p className="text-xs text-muted-foreground mt-3 flex items-center gap-2">
                           {metrics.growthRate >= 0
                              ? 'Trending up this period'
                              : 'Trending down this period'}
                           {metrics.growthRate >= 0 ? (
                              <TrendingUp className="h-3.5 w-3.5" />
                           ) : (
                              <TrendingUp className="h-3.5 w-3.5 rotate-180" />
                           )}
                        </p>
                        <p className="text-xs text-muted-foreground">
                           From {metrics.totalOrders} paid orders
                        </p>
                     </CardContent>
                  </Card>

                  <Card>
                     <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <CardTitle className="text-sm font-medium">
                           Total Orders
                        </CardTitle>
                        <Badge variant="secondary" className="gap-1">
                           <ArrowUpRight className="h-3 w-3" /> Paid
                        </Badge>
                     </CardHeader>
                     <CardContent>
                        <div className="text-2xl font-bold">
                           {metrics.totalOrders.toLocaleString()}
                        </div>
                        <p className="text-xs text-muted-foreground mt-3 flex items-center gap-2">
                           Total paid orders
                           <TrendingUp className="h-3.5 w-3.5" />
                        </p>
                        <p className="text-xs text-muted-foreground">
                           {orders.length - metrics.totalOrders} other status
                        </p>
                     </CardContent>
                  </Card>

                  <Card>
                     <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <CardTitle className="text-sm font-medium">
                           Unique Customers
                        </CardTitle>
                        <Badge variant="secondary" className="gap-1">
                           <ArrowUpRight className="h-3 w-3" /> Active
                        </Badge>
                     </CardHeader>
                     <CardContent>
                        <div className="text-2xl font-bold">
                           {metrics.uniqueCustomers.toLocaleString()}
                        </div>
                        <p className="text-xs text-muted-foreground mt-3 flex items-center gap-2">
                           Customers with paid orders
                           <TrendingUp className="h-3.5 w-3.5" />
                        </p>
                        <p className="text-xs text-muted-foreground">
                           Average order value: $
                           {metrics.totalOrders > 0
                              ? (
                                   metrics.totalRevenue / metrics.totalOrders
                                ).toLocaleString('en-US', {
                                   minimumFractionDigits: 2,
                                   maximumFractionDigits: 2,
                                })
                              : '0.00'}
                        </p>
                     </CardContent>
                  </Card>

                  <Card>
                     <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <CardTitle className="text-sm font-medium">
                           Growth Rate
                        </CardTitle>
                        <Badge
                           variant={
                              metrics.growthRate >= 0
                                 ? 'secondary'
                                 : 'destructive'
                           }
                           className="gap-1"
                        >
                           {metrics.growthRate >= 0 ? (
                              <ArrowUpRight className="h-3 w-3" />
                           ) : (
                              <ArrowDownRight className="h-3 w-3" />
                           )}{' '}
                           {metrics.growthRate >= 0 ? '+' : ''}
                           {metrics.growthRate.toFixed(1)}%
                        </Badge>
                     </CardHeader>
                     <CardContent>
                        <div className="text-2xl font-bold">
                           {metrics.growthRate >= 0 ? '+' : ''}
                           {metrics.growthRate.toFixed(1)}%
                        </div>
                        <p className="text-xs text-muted-foreground mt-3 flex items-center gap-2">
                           {metrics.growthRate >= 0
                              ? 'Steady performance increase'
                              : 'Performance decrease'}
                           {metrics.growthRate >= 0 ? (
                              <TrendingUp className="h-3.5 w-3.5" />
                           ) : (
                              <TrendingUp className="h-3.5 w-3.5 rotate-180" />
                           )}
                        </p>
                        <p className="text-xs text-muted-foreground">
                           Last 30 days vs previous 30 days
                        </p>
                     </CardContent>
                  </Card>
               </div>
            </div>
         )} */}

         {/* Filter section */}
         {/* <div>
               <div className="flex items-center gap-4">
                  <DropdownMenu>
                     <DropdownMenuTrigger asChild>
                        <Button variant="outline">
                           Filter by:{' '}
                           {filterMetric === 'both'
                              ? 'All'
                              : filterMetric === 'revenue'
                                ? 'Revenue'
                                : 'Orders'}
                           <div className="flex items-center gap-2">
                              <ChevronDown />
                           </div>
                        </Button>
                     </DropdownMenuTrigger>
                     <DropdownMenuContent
                        align="start"
                        side="bottom"
                        sideOffset={4}
                     >
                        <DropdownMenuCheckboxItem
                           checked={filterMetric === 'both'}
                           onCheckedChange={() => setFilterMetric('both')}
                        >
                           All
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           checked={filterMetric === 'revenue'}
                           onCheckedChange={() => setFilterMetric('revenue')}
                        >
                           Revenue
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           checked={filterMetric === 'orders'}
                           onCheckedChange={() => setFilterMetric('orders')}
                        >
                           Quantity of orders
                        </DropdownMenuCheckboxItem>
                     </DropdownMenuContent>
                  </DropdownMenu>

                  <DropdownMenu>
                     <DropdownMenuTrigger asChild>
                        <Button variant="outline">
                           Group by: {groupBy === 'date' ? 'Date' : 'Month'}
                           <div className="flex items-center gap-2">
                              <ChevronDown />
                           </div>
                        </Button>
                     </DropdownMenuTrigger>
                     <DropdownMenuContent
                        align="start"
                        side="bottom"
                        sideOffset={4}
                     >
                        <DropdownMenuCheckboxItem
                           checked={groupBy === 'date'}
                           onCheckedChange={() => setGroupBy('date')}
                        >
                           Date
                        </DropdownMenuCheckboxItem>
                        <DropdownMenuCheckboxItem
                           checked={groupBy === 'month'}
                           onCheckedChange={() => setGroupBy('month')}
                        >
                           Month
                        </DropdownMenuCheckboxItem>
                     </DropdownMenuContent>
                  </DropdownMenu>

                  {/* Date range picker */}
         {/* <div>
                     <Popover>
                        <PopoverTrigger asChild>
                           <Button
                              variant="outline"
                              id="dates"
                              className="w-56 justify-between font-normal"
                           >
                              {range?.from && range?.to
                                 ? `${range.from.toLocaleDateString()} - ${range.to.toLocaleDateString()}`
                                 : 'Select date'}
                              <ChevronDownIcon />
                           </Button>
                        </PopoverTrigger>
                        <PopoverContent
                           className="w-auto overflow-hidden p-0"
                           align="start"
                        >
                           <Calendar
                              mode="range"
                              selected={range}
                              captionLayout="dropdown"
                              onSelect={(range) => {
                                 setRange(range);
                              }}
                           />
                        </PopoverContent>
                     </Popover>
                  </div>
                  <Button
                     variant="ghost"
                     className="text-blue-600 hover:text-blue-600 hover:underline"
                     onClick={handleSubmitFilter}
                     disabled={!range?.from || !range?.to}
                  >
                     Apply Filter
                  </Button>
                  {isFiltered && (
                     <Button variant="outline" onClick={handleResetFilter}>
                        Reset
                     </Button>
                  )}
               </div> */}

         {/* Filter status */}
         {/* {isFiltered && (
                  <div className="flex items-center gap-2 text-sm text-muted-foreground">
                     <span>
                        Showing{' '}
                        <strong>
                           {filterMetric === 'both'
                              ? 'All Metrics'
                              : filterMetric === 'revenue'
                                ? 'Revenue'
                                : 'Orders'}
                        </strong>{' '}
                        grouped by{' '}
                        <strong>{groupBy === 'date' ? 'Date' : 'Month'}</strong>
                        {range?.from && range?.to && (
                           <>
                              {' '}
                              from{' '}
                              <strong>
                                 {range.from.toLocaleDateString()}
                              </strong>{' '}
                              to{' '}
                              <strong>{range.to.toLocaleDateString()}</strong>
                           </>
                        )}
                     </span>
                  </div>
               )} */}

         {/* Chart 1 */}
         {/* <Card className="mt-4">
                  <CardHeader className="flex flex-row items-start justify-between">
                     <div>
                        <CardTitle>Revenue & Orders Chart</CardTitle>
                        <CardDescription>
                           January - December 2025 - Order Performance
                        </CardDescription>
                     </div>

                     <ToggleGroup
                        type="single"
                        value={'30d'}
                        onValueChange={() => {}}
                        variant="outline"
                        className="flex"
                     >
                        <ToggleGroupItem value="90d">
                           Last 3 months
                        </ToggleGroupItem>
                        <ToggleGroupItem value="30d">
                           Last 30 days
                        </ToggleGroupItem>
                        <ToggleGroupItem value="7d">
                           Last 7 days
                        </ToggleGroupItem>
                     </ToggleGroup>
                  </CardHeader>
                  <CardContent>
                     <ChartContainer config={chartConfig}>
                        <LineChart
                           accessibilityLayer
                           data={chartData}
                           margin={{
                              top: 20,
                              left: 12,
                              right: 12,
                           }}
                        >
                           <CartesianGrid vertical={false} />
                           <XAxis
                              dataKey="date"
                              tickLine={false}
                              axisLine={false}
                              tickMargin={8}
                           />
                           <YAxis
                              tickLine={false}
                              axisLine={false}
                              tickMargin={8}
                              tickFormatter={(value) => `${value}`}
                           />
                           <ChartTooltip
                              cursor={false}
                              content={<ChartTooltipContent indicator="line" />}
                           />
                           {(filterMetric === 'revenue' ||
                              filterMetric === 'both') && (
                              <Line
                                 dataKey="revenue"
                                 type="natural"
                                 stroke="hsl(var(--color-revenue))"
                                 strokeWidth={2}
                                 dot={{
                                    fill: 'hsl(var(--color-revenue))',
                                 }}
                                 activeDot={{
                                    r: 6,
                                 }}
                              >
                                 <LabelList
                                    position="top"
                                    offset={12}
                                    className="fill-foreground"
                                    fontSize={12}
                                 />
                              </Line>
                           )}
                           {(filterMetric === 'orders' ||
                              filterMetric === 'both') && (
                              <Line
                                 dataKey="orders"
                                 type="natural"
                                 stroke="hsl(var(--color-orders))"
                                 strokeWidth={2}
                                 dot={{
                                    fill: 'hsl(var(--color-orders))',
                                 }}
                                 activeDot={{
                                    r: 6,
                                 }}
                              >
                                 <LabelList
                                    position="top"
                                    offset={12}
                                    className="fill-foreground"
                                    fontSize={12}
                                 />
                              </Line>
                           )}
                        </LineChart>
                     </ChartContainer>
                  </CardContent>
                  <CardFooter className="flex-col items-start gap-2 text-sm">
                     <div className="flex gap-2 leading-none font-medium">
                        {filterMetric === 'revenue' && 'Revenue Analytics'}
                        {filterMetric === 'orders' && 'Order Analytics'}
                        {filterMetric === 'both' &&
                           'Revenue & Order Analytics'}{' '}
                        <TrendingUp className="h-4 w-4" />
                     </div>
                     <div className="text-muted-foreground leading-none">
                        {isFiltered
                           ? `Showing filtered data ${groupBy === 'month' ? 'grouped by month' : 'by date'}`
                           : 'Showing all paid orders from January - December 2025'}
                     </div>
                  </CardFooter>
               </Card>
            </div>  */}

         <div>
            <RevenueOrdersChart />

            {/* <YearOverYearChart /> */}

            {/* <MultiTenantChart /> */}
         </div>
      </div>
   );
};

export default RevenueAnalytics;
