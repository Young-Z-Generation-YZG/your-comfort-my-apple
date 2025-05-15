'use client';

import { CardWrapper, CardContent } from '@components/card-wrapper';
import { Badge } from '@components/ui/badge';
import { Button } from '@components/ui/button';
import { CardDescription, CardHeader, CardTitle } from '@components/ui/card';
import { AreaChartInteractive } from '@components/ui/chart-ui/area-chart-interactive';
import { BarChartLabel } from '@components/ui/chart-ui/bar-chart-label';
import { LineChartMultiple } from '@components/ui/chart-ui/line-chart-multiple';
import { PieChartInteractive } from '@components/ui/chart-ui/pie-chart-interactive';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '@components/ui/table';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@components/ui/tabs';

const topProducts = [
   {
      id: 1,
      name: 'Wireless Earbuds Pro',
      price: 129.99,
      sales: 1245,
      revenue: 161837.55,
      growth: 12.4,
   },
   {
      id: 2,
      name: 'Smart Watch Series 5',
      price: 249.99,
      sales: 876,
      revenue: 218991.24,
      growth: 8.7,
   },
   {
      id: 3,
      name: 'Ultra HD 4K Monitor',
      price: 349.99,
      sales: 654,
      revenue: 228893.46,
      growth: -2.3,
   },
   {
      id: 4,
      name: 'Ergonomic Office Chair',
      price: 189.99,
      sales: 587,
      revenue: 111524.13,
      growth: 5.6,
   },
   {
      id: 5,
      name: 'Noise Cancelling Headphones',
      price: 199.99,
      sales: 542,
      revenue: 108394.58,
      growth: 15.2,
   },
];

const recentOrders = [
   {
      id: '#ORD-7245',
      customer: 'Emma Johnson',
      date: '2025-05-13',
      status: 'Completed',
      total: 324.99,
   },
   {
      id: '#ORD-7244',
      customer: 'Liam Smith',
      date: '2025-05-13',
      status: 'Processing',
      total: 189.5,
   },
   {
      id: '#ORD-7243',
      customer: 'Olivia Brown',
      date: '2025-05-12',
      status: 'Completed',
      total: 432.75,
   },
   {
      id: '#ORD-7242',
      customer: 'Noah Wilson',
      date: '2025-05-12',
      status: 'Shipped',
      total: 267.25,
   },
   {
      id: '#ORD-7241',
      customer: 'Sophia Davis',
      date: '2025-05-11',
      status: 'Completed',
      total: 156.8,
   },
];

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

         <CardWrapper className="flex flex-col gap-4">
            <div className="flex items-center">
               <p className="font-medium text-sm mr-2">Overview by</p>
               <Tabs defaultValue="daily" className="w-[400px]">
                  <TabsList>
                     <TabsTrigger value="daily">Daily</TabsTrigger>
                     <TabsTrigger value="weekly">Weekly</TabsTrigger>
                     <TabsTrigger value="monthly">Monthly</TabsTrigger>
                  </TabsList>
               </Tabs>
            </div>
            <div className="grid grid-cols-4 gap-4">
               <CardContent
                  title="Total Revenue"
                  value={267950}
                  compareValue={12.5}
                  compareValueType="percentage"
                  type="amount"
                  compareText="from last month"
                  icon="up"
                  variantColor="green"
               />

               <CardContent
                  title="Average Order Value"
                  value={89.32}
                  compareValue={12.5}
                  type="decimal"
                  compareValueType="percentage"
                  compareText="from last month"
                  icon="up"
                  variantColor="green"
               />

               <CardContent
                  title="Conversion Rate"
                  value={2.5}
                  valueSuffix="%"
                  compareValue={12.5}
                  type="percentage"
                  compareValueType="percentage"
                  compareText="from last month"
                  icon="up"
                  variantColor="green"
               />

               <CardContent
                  title="Total Orders"
                  value={3281}
                  compareValue={12.5}
                  compareValueType="percentage"
                  compareText="from last month"
                  type="text"
                  icon="up"
                  variantColor="green"
               />
            </div>
         </CardWrapper>

         <CardWrapper className="flex flex-col gap-4">
            <div className="flex items-center">
               <Tabs defaultValue="revenue" className="w-full">
                  <TabsList>
                     <TabsTrigger value="revenue">Revenue</TabsTrigger>
                     <TabsTrigger value="orders">Orders</TabsTrigger>
                     <TabsTrigger value="profit">Profit</TabsTrigger>
                  </TabsList>

                  <TabsContent value="revenue">
                     <AreaChartInteractive />
                  </TabsContent>
                  <TabsContent value="orders" className="w-full">
                     <BarChartLabel />
                  </TabsContent>
                  <TabsContent value="profit">
                     <LineChartMultiple />
                  </TabsContent>
               </Tabs>
            </div>
         </CardWrapper>

         <div className="grid grid-cols-2 gap-4">
            <CardWrapper className="flex flex-col gap-4">
               <PieChartInteractive />
            </CardWrapper>

            <CardWrapper className="flex flex-col gap-4 bg-muted/10">
               <CardHeader>
                  <CardTitle>Top Products</CardTitle>
                  <CardDescription>
                     Best performing products by revenue
                  </CardDescription>
               </CardHeader>
               <Table>
                  <TableHeader>
                     <TableRow>
                        <TableHead>Product</TableHead>
                        <TableHead className="text-right">Price</TableHead>
                        <TableHead className="text-right">Sales</TableHead>
                        <TableHead className="text-right">Revenue</TableHead>
                     </TableRow>
                  </TableHeader>
                  <TableBody>
                     {topProducts.map((product) => (
                        <TableRow key={product.id}>
                           <TableCell className="font-medium">
                              {product.name}
                           </TableCell>
                           <TableCell className="text-right">
                              ${product.price}
                           </TableCell>
                           <TableCell className="text-right">
                              {product.sales}
                           </TableCell>
                           <TableCell className="text-right">
                              ${product.revenue.toLocaleString()}
                              <span
                                 className={`ml-2 text-xs ${product.growth > 0 ? 'text-green-600' : 'text-red-600'}`}
                              >
                                 {product.growth > 0 ? '+' : ''}
                                 {product.growth}%
                              </span>
                           </TableCell>
                        </TableRow>
                     ))}
                  </TableBody>
               </Table>
            </CardWrapper>
         </div>

         <div>
            <CardWrapper className="flex flex-col gap-4 bg-muted/10">
               <CardHeader className="flex flex-row items-center justify-between">
                  <div>
                     <CardTitle>Recent Orders</CardTitle>
                     <CardDescription>
                        Latest transactions from your store
                     </CardDescription>
                  </div>
                  <Button variant="outline" size="sm">
                     View All
                  </Button>
               </CardHeader>
               <Table>
                  <TableHeader>
                     <TableRow>
                        <TableHead>Order ID</TableHead>
                        <TableHead>Customer</TableHead>
                        <TableHead>Date</TableHead>
                        <TableHead>Status</TableHead>
                        <TableHead className="text-right">Amount</TableHead>
                     </TableRow>
                  </TableHeader>
                  <TableBody>
                     {recentOrders.map((order) => (
                        <TableRow key={order.id}>
                           <TableCell className="font-medium">
                              {order.id}
                           </TableCell>
                           <TableCell>{order.customer}</TableCell>
                           <TableCell>{order.date}</TableCell>
                           <TableCell>
                              <Badge
                                 variant={
                                    order.status === 'Completed'
                                       ? 'outline'
                                       : order.status === 'Processing'
                                         ? 'default'
                                         : 'secondary'
                                 }
                              >
                                 {order.status}
                              </Badge>
                           </TableCell>
                           <TableCell className="text-right">
                              ${order.total.toFixed(2)}
                           </TableCell>
                        </TableRow>
                     ))}
                  </TableBody>
               </Table>
            </CardWrapper>
         </div>
      </div>
   );
};

export default RevenueAnalytics;
