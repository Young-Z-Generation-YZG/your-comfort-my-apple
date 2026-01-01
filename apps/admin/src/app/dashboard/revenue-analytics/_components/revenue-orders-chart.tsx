'use client';

import { ChevronDown, TrendingUp, ChevronDownIcon } from 'lucide-react';
import {
   CartesianGrid,
   LabelList,
   Line,
   LineChart,
   XAxis,
   YAxis,
} from 'recharts';

import {
   Card,
   CardContent,
   CardDescription,
   CardFooter,
   CardHeader,
   CardTitle,
} from '@components/ui/card';
import {
   ChartConfig,
   ChartContainer,
   ChartTooltip,
   ChartTooltipContent,
} from '@components/ui/chart';

import {
   DropdownMenu,
   DropdownMenuCheckboxItem,
   DropdownMenuContent,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import { isWithinInterval, startOfMonth } from 'date-fns';
import { useState, useMemo, useEffect } from 'react';

import { type DateRange } from 'react-day-picker';

import { Button } from '~/src/components/ui/button';
import { Calendar } from '~/src/components/ui/calendar';
import {
   Popover,
   PopoverContent,
   PopoverTrigger,
} from '~/src/components/ui/popover';
import { ToggleGroup, ToggleGroupItem } from '@components/ui/toggle-group';
import useOrderingService from '~/src/hooks/api/use-ordering-service';

const chartConfig = {
   revenue: {
      label: 'Revenue ($)',
      color: 'var(--chart-1)',
   },
   orders: {
      label: 'Orders',
      color: 'var(--chart-2)',
   },
} satisfies ChartConfig;

type FilterMetric = 'revenue' | 'orders' | 'both';
type GroupBy = 'date' | 'month';

const DEFAULT_RANGE: DateRange = {
   from: new Date(2025, 0, 1), // 1 Jan 2025
   to: new Date(2025, 11, 31), // 31 Dec 2025
};

const RevenueOrdersChart = () => {
   // Filter states for Chart 1
   const [range, setRange] = useState<DateRange | undefined>(DEFAULT_RANGE);
   const [filterMetric, setFilterMetric] = useState<FilterMetric>('both');
   const [groupBy, setGroupBy] = useState<GroupBy>('month');
   const [isFiltered, setIsFiltered] = useState(true);

   const { getRevenuesAsync, getRevenuesState } = useOrderingService();

   // Get orders from revenues API response (Chart 1)
   const orders = useMemo(() => {
      return getRevenuesState.data || [];
   }, [getRevenuesState.data]);

   // Process and filter data
   const chartData = useMemo(() => {
      const dateMap = new Map<
         string,
         { revenue: number; orders: number; timestamp: number }
      >();

      // Month order for initialization (when grouping by month)
      const monthOrder = [
         'Jan',
         'Feb',
         'Mar',
         'Apr',
         'May',
         'Jun',
         'Jul',
         'Aug',
         'Sep',
         'Oct',
         'Nov',
         'Dec',
      ];

      // Initialize all months with 0 revenue and orders when grouping by month
      if (groupBy === 'month') {
         // Use year from date range if available, otherwise use current year
         const targetYear =
            range?.from?.getFullYear() || new Date().getFullYear();

         monthOrder.forEach((month, index) => {
            const monthDate = new Date(targetYear, index, 1);
            const dateKey = monthDate.toLocaleDateString('en-US', {
               month: 'short',
               year: 'numeric',
            });
            dateMap.set(dateKey, {
               revenue: 0,
               orders: 0,
               timestamp: startOfMonth(monthDate).getTime(),
            });
         });
      }

      orders.forEach((order) => {
         if (order.status === 'DELIVERED') {
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
               const monthIndex = date.getMonth(); // 0-11
               const monthDate = new Date(date.getFullYear(), monthIndex, 1);
               dateKey = monthDate.toLocaleDateString('en-US', {
                  month: 'short',
                  year: 'numeric',
               });
               timestamp = startOfMonth(monthDate).getTime();
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
      setRange(DEFAULT_RANGE);
      setFilterMetric('both');
      setGroupBy('month');
      setIsFiltered(true);
   };

   useEffect(() => {
      // Fetch revenues for Chart 1
      getRevenuesAsync();
   }, [getRevenuesAsync]);

   return (
      <div>
         {/* Filter section */}
         <div>
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
               <div>
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
            </div>

            {/* Filter status */}
            {isFiltered && (
               <div className="flex items-center gap-2 text-sm text-muted-foreground mt-3">
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
                           </strong> to{' '}
                           <strong>{range.to.toLocaleDateString()}</strong>
                        </>
                     )}
                  </span>
               </div>
            )}

            {/* Chart 1 */}
            <Card className="mt-4">
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
                     <ToggleGroupItem value="30d">Last 30 days</ToggleGroupItem>
                     <ToggleGroupItem value="7d">Last 7 days</ToggleGroupItem>
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
                              type="monotone"
                              stroke="hsl(var(--color-revenue))"
                              strokeWidth={2}
                              dot={{
                                 fill: 'hsl(var(--color-revenue))',
                              }}
                              activeDot={{
                                 r: 6,
                              }}
                              connectNulls={false}
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
                              type="monotone"
                              stroke="hsl(var(--color-orders))"
                              strokeWidth={2}
                              dot={{
                                 fill: 'hsl(var(--color-orders))',
                              }}
                              activeDot={{
                                 r: 6,
                              }}
                              connectNulls={false}
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
                        : 'Showing all delivered orders from January - December 2025'}
                  </div>
               </CardFooter>
            </Card>
         </div>
      </div>
   );
};

export default RevenueOrdersChart;
