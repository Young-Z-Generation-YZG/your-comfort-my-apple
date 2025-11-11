'use client';

import { ChevronDown } from 'lucide-react';
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
import { useState, useMemo, useEffect } from 'react';

import { Button } from '~/src/components/ui/button';
import useOrderingService from '~/src/hooks/api/use-ordering-service';

const chartConfig = {
   year2024: {
      label: '2024',
      color: 'hsl(var(--chart-1))',
   },
   year2025: {
      label: '2025',
      color: 'hsl(var(--chart-2))',
   },
} satisfies ChartConfig;

const YearOverYearChart = () => {
   // Filter states for Chart 2 - Multi-select years
   const [selectedYears, setSelectedYears] = useState<{
      year2024: boolean;
      year2025: boolean;
   }>({
      year2024: true,
      year2025: true,
   });

   const { getRevenuesByYearsAsync, getRevenuesByYearsState } =
      useOrderingService();

   const toggleYear = (year: 'year2024' | 'year2025') => {
      setSelectedYears((prev) => ({
         ...prev,
         [year]: !prev[year],
      }));
   };

   const getYearFilterLabel = () => {
      if (selectedYears.year2024 && selectedYears.year2025) return 'Both Years';
      if (selectedYears.year2024) return '2024';
      if (selectedYears.year2025) return '2025';
      return 'Select Years';
   };

   // Process year-over-year data for Chart 2
   const yearOverYearData = useMemo(() => {
      const revenuesByYears = getRevenuesByYearsState.data?.groups || {};
      const monthMap = new Map<
         string,
         { year2024: number; year2025: number }
      >();

      // Month order for initialization
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

      // Initialize all months with 0 revenue for all selected years
      monthOrder.forEach((month) => {
         monthMap.set(month, { year2024: 0, year2025: 0 });
      });

      // Process 2024 orders
      if (selectedYears.year2024 && revenuesByYears['2024']) {
         revenuesByYears['2024'].forEach((order) => {
            if (order.status === 'DELIVERED') {
               const date = new Date(order.created_at);
               const monthIndex = date.getMonth(); // 0-11

               // Get month key from monthOrder array using month index
               const monthKey = monthOrder[monthIndex];

               const current = monthMap.get(monthKey);
               if (current) {
                  current.year2024 += order.total_amount;
               }
            }
         });
      }

      // Process 2025 orders
      if (selectedYears.year2025 && revenuesByYears['2025']) {
         revenuesByYears['2025'].forEach((order) => {
            if (order.status === 'DELIVERED') {
               const date = new Date(order.created_at);
               const monthIndex = date.getMonth(); // 0-11

               // Get month key from monthOrder array using month index
               const monthKey = monthOrder[monthIndex];

               const current = monthMap.get(monthKey);
               if (current) {
                  current.year2025 += order.total_amount;
               }
            }
         });
      }

      // Convert to array and sort by month order
      // Ensure all selected years are present in every month's data
      const chartData = Array.from(monthMap.entries())
         .map(([month, data]) => {
            const monthData: Record<string, number | string> = { month };
            // Ensure all selected years have a value (default to 0 if missing)
            if (selectedYears.year2024) {
               monthData.year2024 = data.year2024 ?? 0;
            }
            if (selectedYears.year2025) {
               monthData.year2025 = data.year2025 ?? 0;
            }
            return {
               ...monthData,
               order: monthOrder.indexOf(month),
            };
         })
         .sort((a, b) => (a.order as number) - (b.order as number))
         .map((item) => {
            // eslint-disable-next-line @typescript-eslint/no-unused-vars
            const { order, ...rest } = item;
            return rest;
         });

      console.log('year-over-year chartData', chartData);

      return chartData;
   }, [getRevenuesByYearsState.data, selectedYears]);

   useEffect(() => {
      // Fetch revenues by years for Chart 2
      const yearsToFetch: string[] = [];
      if (selectedYears.year2024) yearsToFetch.push('2024');
      if (selectedYears.year2025) yearsToFetch.push('2025');
      if (yearsToFetch.length > 0) {
         getRevenuesByYearsAsync(yearsToFetch);
      }
   }, [getRevenuesByYearsAsync, selectedYears]);

   return (
      <div className="mt-10">
         <div className="flex items-center gap-4">
            <DropdownMenu>
               <DropdownMenuTrigger asChild>
                  <Button variant="outline">
                     Filter by: {getYearFilterLabel()}
                     <div className="flex items-center gap-2">
                        <ChevronDown />
                     </div>
                  </Button>
               </DropdownMenuTrigger>
               <DropdownMenuContent align="start" side="bottom" sideOffset={4}>
                  <DropdownMenuCheckboxItem
                     checked={selectedYears.year2024}
                     onCheckedChange={() => toggleYear('year2024')}
                  >
                     2024
                  </DropdownMenuCheckboxItem>
                  <DropdownMenuCheckboxItem
                     checked={selectedYears.year2025}
                     onCheckedChange={() => toggleYear('year2025')}
                  >
                     2025
                  </DropdownMenuCheckboxItem>
               </DropdownMenuContent>
            </DropdownMenu>
         </div>

         {/* Chart 2 - Multi-Year Comparison */}
         <Card className="mt-4">
            <CardHeader>
               <CardTitle>
                  {selectedYears.year2024 && selectedYears.year2025
                     ? 'Year-over-Year Revenue Comparison'
                     : selectedYears.year2024
                       ? '2024 Revenue Analytics'
                       : selectedYears.year2025
                         ? '2025 Revenue Analytics'
                         : 'Revenue Analytics'}
               </CardTitle>
               <CardDescription>
                  {selectedYears.year2024 && selectedYears.year2025
                     ? 'Monthly revenue comparison between 2024 and 2025'
                     : selectedYears.year2024
                       ? 'Monthly revenue for 2024'
                       : selectedYears.year2025
                         ? 'Monthly revenue for 2025'
                         : 'Select years to display'}
               </CardDescription>
               <div className="flex items-center gap-4">
                  {[
                     { year: 2024, color: '#e11d48', key: 'year2024' },
                     { year: 2025, color: '#3b82f6', key: 'year2025' },
                  ].map((item) => (
                     <div key={item.key} className="flex items-center gap-2">
                        <span
                           className="w-5 h-2 rounded-full"
                           style={{ backgroundColor: item.color }}
                        ></span>
                        <p className="text-sm font-medium">{item.year}</p>
                     </div>
                  ))}
               </div>
            </CardHeader>
            <CardContent>
               <ChartContainer config={chartConfig}>
                  <LineChart
                     accessibilityLayer
                     data={yearOverYearData}
                     margin={{
                        top: 20,
                        left: 12,
                        right: 12,
                     }}
                  >
                     <CartesianGrid vertical={false} />
                     <XAxis
                        dataKey="month"
                        tickLine={false}
                        axisLine={false}
                        tickMargin={8}
                     />
                     <YAxis
                        tickLine={false}
                        axisLine={false}
                        tickMargin={8}
                        tickFormatter={(value) => `$${value}`}
                     />
                     <ChartTooltip
                        cursor={false}
                        content={<ChartTooltipContent indicator="line" />}
                     />
                     {selectedYears.year2024 && (
                        <Line
                           dataKey="year2024"
                           type="monotone"
                           stroke="#e11d48"
                           strokeWidth={2}
                           dot={{
                              fill: '#e11d48',
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
                     {selectedYears.year2025 && (
                        <Line
                           dataKey="year2025"
                           type="monotone"
                           stroke="#3b82f6"
                           strokeWidth={2}
                           dot={{
                              fill: '#3b82f6',
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
            {/* <CardFooter className="flex-col items-start gap-2 text-sm">
               <div className="flex gap-2 leading-none font-medium">
                  {selectedYears.year2024 &&
                     selectedYears.year2025 &&
                     '2025 shows 38% increase compared to 2024'}
                  {selectedYears.year2024 &&
                     !selectedYears.year2025 &&
                     'Showing 2024 revenue data'}
                  {selectedYears.year2025 &&
                     !selectedYears.year2024 &&
                     'Showing 2025 revenue data'}
                  {!selectedYears.year2024 &&
                     !selectedYears.year2025 &&
                     'No data selected'}{' '}
                  <TrendingUp className="h-4 w-4" />
               </div>
               <div className="text-muted-foreground leading-none">
                  {selectedYears.year2024 && selectedYears.year2025
                     ? 'Comparing monthly revenue across two years'
                     : selectedYears.year2024
                       ? 'Displaying 2024 monthly revenue trend'
                       : selectedYears.year2025
                         ? 'Displaying 2025 monthly revenue trend'
                         : 'Select at least one year to display data'}
               </div>
            </CardFooter> */}
         </Card>
      </div>
   );
};

export default YearOverYearChart;
