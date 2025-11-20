'use client';

import { ChevronDown, TrendingUp } from 'lucide-react';
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
import useTenantService from '~/src/hooks/api/use-tenant-service';

const MultiTenantChart = () => {
   const { getTenantsAsync, getTenantsState } = useTenantService();

   const listTenantIds = useMemo(() => {
      return getTenantsState.data?.map((tenant) => tenant.id) || [];
   }, [getTenantsState.data]);

   // Filter states for Chart 3 - Multi-select tenants
   const [selectedTenants, setSelectedTenants] = useState<
      Record<string, boolean>
   >({});

   const { getRevenuesByTenantsAsync, getRevenuesByTenantsState } =
      useOrderingService();

   // Get available tenants from API
   const tenants = useMemo(() => {
      return getTenantsState.data || [];
   }, [getTenantsState.data]);

   useEffect(() => {
      setSelectedTenants(
         Object.fromEntries(listTenantIds.map((id) => [id, true])) as Record<
            string,
            boolean
         >,
      );
   }, [listTenantIds]);

   const toggleTenant = (tenantId: string) => {
      setSelectedTenants((prev) => ({
         ...prev,
         [tenantId]: !prev[tenantId],
      }));
   };

   const getTenantFilterLabel = () => {
      const selectedCount =
         Object.values(selectedTenants).filter(Boolean).length;
      if (selectedCount === 0) return 'Select Tenants';
      if (selectedCount === tenants.length) return 'All Tenants';
      return `${selectedCount} Tenant${selectedCount > 1 ? 's' : ''}`;
   };

   // Process tenant comparison data for Chart 3
   const tenantComparisonData = useMemo(() => {
      const revenuesByTenants = getRevenuesByTenantsState.data?.groups || {};
      const monthMap = new Map<string, Record<string, number>>();

      // Get selected tenant IDs
      const selectedTenantIds = Object.entries(selectedTenants)
         .filter(([, selected]) => selected)
         .map(([tenantId]) => tenantId);

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

      // Initialize all months with 0 revenue for all selected tenants
      monthOrder.forEach((month) => {
         const initialData: Record<string, number> = {};
         selectedTenantIds.forEach((tenantId) => {
            initialData[tenantId] = 0;
         });
         monthMap.set(month, initialData);
      });

      // Process each selected tenant and update revenue values
      selectedTenantIds.forEach((tenantId) => {
         if (revenuesByTenants[tenantId]) {
            revenuesByTenants[tenantId].forEach((order) => {
               if (order.status === 'DELIVERED') {
                  const date = new Date(order.created_at);
                  const monthIndex = date.getMonth(); // 0-11

                  // Get month key from monthOrder array using month index
                  const monthKey = monthOrder[monthIndex];

                  const current = monthMap.get(monthKey);
                  if (current && current[tenantId] !== undefined) {
                     current[tenantId] += order.total_amount;
                  }
               }
            });
         }
      });

      // Convert to array and sort by month order
      // Ensure all selected tenant IDs are present in every month's data
      const chartData = Array.from(monthMap.entries())
         .map(([month, data]) => {
            const monthData: Record<string, number | string> = { month };
            // Ensure all selected tenants have a value (default to 0 if missing)
            selectedTenantIds.forEach((tenantId) => {
               monthData[tenantId] = data[tenantId] ?? 0;
            });
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

      return chartData;
   }, [getRevenuesByTenantsState.data, selectedTenants]);

   useEffect(() => {
      // Fetch tenants list
      getTenantsAsync();
   }, [getTenantsAsync]);

   useEffect(() => {
      // Fetch revenues by tenants for Chart 3
      const tenantsToFetch = Object.entries(selectedTenants)
         .filter(
            ([tenantId, selected]) =>
               selected && tenantId && tenantId !== 'undefined',
         )
         .map(([tenantId]) => tenantId)
         .filter((id): id is string => Boolean(id));
      if (tenantsToFetch.length > 0) {
         getRevenuesByTenantsAsync(tenantsToFetch);
      }
   }, [getRevenuesByTenantsAsync, selectedTenants]);

   return (
      <div className="mt-10">
         <div className="flex items-center gap-4">
            <DropdownMenu>
               <DropdownMenuTrigger asChild>
                  <Button variant="outline">
                     Filter by: {getTenantFilterLabel()}
                     <div className="flex items-center gap-2">
                        <ChevronDown />
                     </div>
                  </Button>
               </DropdownMenuTrigger>
               <DropdownMenuContent align="start" side="bottom" sideOffset={4}>
                  {tenants.map((tenant, index) => (
                     <DropdownMenuCheckboxItem
                        key={index}
                        checked={selectedTenants[tenant.id] || false}
                        onCheckedChange={() => toggleTenant(tenant.id)}
                     >
                        {tenant.name || tenant.id}
                     </DropdownMenuCheckboxItem>
                  ))}
               </DropdownMenuContent>
            </DropdownMenu>
         </div>

         {/* Chart 3 - Multi-Tenant Comparison */}
         <Card className="mt-4">
            <CardHeader>
               <CardTitle>
                  {Object.values(selectedTenants).filter(Boolean).length > 1
                     ? 'Multi-Tenant Revenue Comparison'
                     : Object.values(selectedTenants).filter(Boolean).length ===
                         1
                       ? 'Single Tenant Revenue Analytics'
                       : 'Tenant Revenue Analytics'}
               </CardTitle>
               <CardDescription>
                  {Object.values(selectedTenants).filter(Boolean).length > 1
                     ? 'Monthly revenue comparison across selected tenants'
                     : Object.values(selectedTenants).filter(Boolean).length ===
                         1
                       ? 'Monthly revenue for selected tenant'
                       : 'Select tenants to display'}
               </CardDescription>
               <div className="flex items-center gap-4 flex-wrap">
                  {tenants
                     .filter((tenant) => selectedTenants[tenant.id] === true)
                     .map((tenant, index) => {
                        const colors = [
                           '#e11d48',
                           '#3b82f6',
                           '#10b981',
                           '#f59e0b',
                           '#8b5cf6',
                           '#ec4899',
                        ];
                        const color = colors[index % colors.length];
                        return (
                           <div key={index} className="flex items-center gap-2">
                              <span
                                 className="w-5 h-2 rounded-full"
                                 style={{ backgroundColor: color }}
                              ></span>
                              <p className="text-sm font-medium">
                                 {tenant.name || tenant.id}
                              </p>
                           </div>
                        );
                     })}
               </div>
            </CardHeader>
            <CardContent>
               <ChartContainer
                  config={Object.fromEntries(
                     tenants
                        .filter((tenant) => selectedTenants[tenant.id] === true)
                        .map((tenant, index) => {
                           const colors = [
                              '#e11d48',
                              '#3b82f6',
                              '#10b981',
                              '#f59e0b',
                              '#8b5cf6',
                              '#ec4899',
                           ];
                           const color = colors[index % colors.length];
                           return [
                              tenant.id,
                              {
                                 label: tenant.name || tenant.id,
                                 color: color,
                              },
                           ];
                        }),
                  )}
               >
                  <LineChart
                     accessibilityLayer
                     data={tenantComparisonData}
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
                     {tenants
                        .filter(
                           (tenant) =>
                              tenant?.id && selectedTenants[tenant.id] === true,
                        )
                        .map((tenant, index) => {
                           const colors = [
                              '#e11d48',
                              '#3b82f6',
                              '#10b981',
                              '#f59e0b',
                              '#8b5cf6',
                              '#ec4899',
                           ];
                           const color = colors[index % colors.length];
                           return (
                              <Line
                                 key={`tenant-${tenant.id}-${index}`}
                                 dataKey={tenant.id}
                                 type="monotone"
                                 stroke={color}
                                 strokeWidth={2}
                                 dot={{
                                    fill: color,
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
                           );
                        })}
                  </LineChart>
               </ChartContainer>
            </CardContent>
            <CardFooter className="flex-col items-start gap-2 text-sm">
               <div className="flex gap-2 leading-none font-medium">
                  {Object.values(selectedTenants).filter(Boolean).length > 1 &&
                     'Comparing monthly revenue across selected tenants'}
                  {Object.values(selectedTenants).filter(Boolean).length ===
                     1 && 'Showing revenue data for selected tenant'}
                  {Object.values(selectedTenants).filter(Boolean).length ===
                     0 && 'No tenants selected'}{' '}
                  <TrendingUp className="h-4 w-4" />
               </div>
               <div className="text-muted-foreground leading-none">
                  {Object.values(selectedTenants).filter(Boolean).length > 1
                     ? 'Comparing monthly revenue trends across multiple tenants'
                     : Object.values(selectedTenants).filter(Boolean).length ===
                         1
                       ? 'Displaying monthly revenue trend for selected tenant'
                       : 'Select at least one tenant to display data'}
               </div>
            </CardFooter>
         </Card>
      </div>
   );
};

export default MultiTenantChart;
