'use client';

import {
   ArrowDown,
   ArrowUp,
   BarChart3,
   ChevronDown,
   Download,
   FileText,
   Filter,
   Package,
   ShoppingBag,
   ShoppingCart,
   Tag,
   Users,
   Smartphone,
   Monitor,
   Tablet,
   AlertTriangle,
   Globe,
   Clock,
   Zap,
} from 'lucide-react';
import Link from 'next/link';
import { addDays } from 'date-fns';

import { Badge } from '@components/ui/badge';
import { Button } from '@components/ui/button';
import {
   Card,
   CardContent,
   CardDescription,
   CardHeader,
   CardTitle,
   CardFooter,
} from '@components/ui/card';
import {
   DropdownMenu,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { Separator } from '@components/ui/separator';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@components/ui/tabs';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '@components/ui/table';
import type { DateRange } from 'react-day-picker';
// import { DateRangePicker } from '@components/ui/date-range-picker';
import {
   AreaChart,
   BarChart,
   PieChart,
   ResponsiveContainer,
   Area,
   Bar,
   Pie,
   XAxis,
   YAxis,
   CartesianGrid,
   Tooltip,
   Cell,
   Legend,
   Treemap,
} from 'recharts';
import { Progress } from '@components/ui/progress';

import {
   CardContent as CustomCardContent,
   CardWrapper,
} from '@components/card-wrapper';
import { useState } from 'react';
// import { useState } from 'react';

// Sample data for charts
const browserData = [
   { name: 'Chrome', value: 62.5, users: 8031 },
   { name: 'Safari', value: 19.3, users: 2480 },
   { name: 'Firefox', value: 7.2, users: 925 },
   { name: 'Edge', value: 5.8, users: 746 },
   { name: 'Opera', value: 2.1, users: 270 },
   { name: 'Samsung Internet', value: 1.8, users: 231 },
   { name: 'Other', value: 1.3, users: 167 },
];

const osData = [
   { name: 'Windows', value: 38.2, users: 4909 },
   { name: 'Android', value: 32.5, users: 4176 },
   { name: 'iOS', value: 16.8, users: 2159 },
   { name: 'macOS', value: 9.7, users: 1246 },
   { name: 'Linux', value: 2.1, users: 270 },
   { name: 'Other', value: 0.7, users: 90 },
];

const deviceData = [
   { name: 'Mobile', value: 52.4, users: 6733 },
   { name: 'Desktop', value: 41.3, users: 5307 },
   { name: 'Tablet', value: 6.3, users: 810 },
];

const screenSizeData = [
   { name: '360x640', value: 18.2, users: 2339 },
   { name: '375x667', value: 12.5, users: 1606 },
   { name: '1366x768', value: 11.8, users: 1516 },
   { name: '1920x1080', value: 10.4, users: 1336 },
   { name: '414x896', value: 8.7, users: 1118 },
   { name: '1440x900', value: 5.3, users: 681 },
   { name: '768x1024', value: 4.8, users: 617 },
   { name: 'Other', value: 28.3, users: 3637 },
];

const trendData = [
   {
      month: 'Jan',
      Chrome: 60.2,
      Safari: 18.5,
      Firefox: 8.1,
      Edge: 5.2,
      Other: 8.0,
   },
   {
      month: 'Feb',
      Chrome: 60.5,
      Safari: 18.7,
      Firefox: 8.0,
      Edge: 5.3,
      Other: 7.5,
   },
   {
      month: 'Mar',
      Chrome: 61.0,
      Safari: 18.9,
      Firefox: 7.8,
      Edge: 5.4,
      Other: 6.9,
   },
   {
      month: 'Apr',
      Chrome: 61.3,
      Safari: 19.0,
      Firefox: 7.6,
      Edge: 5.5,
      Other: 6.6,
   },
   {
      month: 'May',
      Chrome: 61.7,
      Safari: 19.1,
      Firefox: 7.5,
      Edge: 5.6,
      Other: 6.1,
   },
   {
      month: 'Jun',
      Chrome: 62.0,
      Safari: 19.2,
      Firefox: 7.4,
      Edge: 5.7,
      Other: 5.7,
   },
   {
      month: 'Jul',
      Chrome: 62.5,
      Safari: 19.3,
      Firefox: 7.2,
      Edge: 5.8,
      Other: 5.2,
   },
];

const performanceData = [
   { name: 'Chrome', loadTime: 1.8, bounceRate: 28, conversionRate: 4.2 },
   { name: 'Safari', loadTime: 2.1, bounceRate: 32, conversionRate: 3.8 },
   { name: 'Firefox', loadTime: 1.9, bounceRate: 30, conversionRate: 4.0 },
   { name: 'Edge', loadTime: 2.0, bounceRate: 31, conversionRate: 3.9 },
   { name: 'Opera', loadTime: 2.2, bounceRate: 33, conversionRate: 3.7 },
   {
      name: 'Samsung Internet',
      loadTime: 2.4,
      bounceRate: 35,
      conversionRate: 3.5,
   },
];

const compatibilityIssues = [
   {
      id: 1,
      browser: 'Internet Explorer 11',
      issue: 'Checkout process fails at payment step',
      affectedUsers: 42,
      severity: 'High',
      status: 'Open',
   },
   {
      id: 2,
      browser: 'Safari (iOS 13)',
      issue: 'Product images not loading correctly',
      affectedUsers: 156,
      severity: 'Medium',
      status: 'In Progress',
   },
   {
      id: 3,
      browser: 'Firefox (older versions)',
      issue: 'Form validation errors not displaying',
      affectedUsers: 89,
      severity: 'Medium',
      status: 'Open',
   },
   {
      id: 4,
      browser: 'Chrome (Android)',
      issue: 'Menu dropdown not working on some devices',
      affectedUsers: 215,
      severity: 'Low',
      status: 'Resolved',
   },
   {
      id: 5,
      browser: 'Edge (Legacy)',
      issue: 'CSS animations causing performance issues',
      affectedUsers: 67,
      severity: 'Low',
      status: 'Open',
   },
];

const COLORS = [
   '#0088FE',
   '#00C49F',
   '#FFBB28',
   '#FF8042',
   '#8884D8',
   '#82ca9d',
   '#ffc658',
];

// Data for treemap
const deviceTreemapData = [
   {
      name: 'Mobile',
      children: [
         { name: 'Android', size: 4176 },
         { name: 'iOS', size: 2159 },
         { name: 'Other Mobile', size: 398 },
      ],
   },
   {
      name: 'Desktop',
      children: [
         { name: 'Windows', size: 4909 },
         { name: 'macOS', size: 1246 },
         { name: 'Linux', size: 270 },
      ],
   },
   {
      name: 'Tablet',
      children: [
         { name: 'iPad', size: 620 },
         { name: 'Android Tablet', size: 170 },
         { name: 'Other Tablet', size: 20 },
      ],
   },
];

// Flatten the treemap data for Recharts
const flattenedTreemapData = deviceTreemapData.reduce(
   (acc, category) => {
      return [
         ...acc,
         ...category.children.map((child) => ({
            name: `${category.name} - ${child.name}`,
            size: child.size,
            category: category.name,
         })),
      ];
   },
   [] as { name: string; size: number; category: string }[],
);

const UserAgentReports = () => {
   const [dateRange, setDateRange] = useState<DateRange | undefined>({
      from: addDays(new Date(), -30),
      to: new Date(),
   });

   return (
      <div className="flex flex-col flex-1 gap-4 p-4">
         <h1 className="text-3xl font-bold tracking-tight">
            User Agent Reports
         </h1>

         <p className="text-muted-foreground">
            View your user agent reports and compare them with the previous
            month.
         </p>

         <CardWrapper className="flex flex-col gap-4">
            <div className="grid grid-cols-4 gap-4">
               <CustomCardContent
                  title="Total Devoices"
                  value={12850}
                  compareValue={8.5}
                  type="amount"
                  compareValueType="percentage"
                  compareText="from last month"
                  icon="up"
                  variantColor="green"
               />

               <CustomCardContent
                  title="Mobile traffic"
                  value={52.4}
                  valueSuffix="%"
                  compareValue={3.2}
                  type="percentage"
                  compareValueType="percentage"
                  compareText="from last month"
                  icon="up"
                  variantColor="green"
               />

               <CustomCardContent
                  title="Avg. Page Load Time"
                  value={2.1}
                  valueSuffix="s"
                  compareValue={0.3}
                  compareValueType="text"
                  compareText="from last month"
                  type="decimal"
                  icon="down"
                  variantColor="green"
               />

               <CustomCardContent
                  title="Compatibility Issues"
                  value={5}
                  compareValue={2}
                  compareText="from last month"
                  type="text"
                  compareValueType="text"
                  icon="up"
                  variantColor="red"
               />
            </div>
         </CardWrapper>

         <CardWrapper>
            {/* Device Distribution */}
            <div className="grid gap-6 md:grid-cols-2">
               <Card className="col-span-1 md:col-span-2">
                  <CardHeader>
                     <CardTitle>Device Distribution</CardTitle>
                     <CardDescription>
                        Hierarchical view of devices, operating systems, and
                        browsers
                     </CardDescription>
                  </CardHeader>
                  <CardContent>
                     <div className="h-[400px]">
                        <ResponsiveContainer width="100%" height="100%">
                           <Treemap
                              data={flattenedTreemapData}
                              dataKey="size"
                              nameKey="name"
                              stroke="#fff"
                              fill="#8884d8"
                           >
                              <Tooltip
                                 content={({ active, payload }) => {
                                    if (active && payload && payload.length) {
                                       const data = payload[0].payload;
                                       return (
                                          <div className="rounded-md border bg-background p-2 shadow-sm">
                                             <p className="font-medium">
                                                {data.name}
                                             </p>
                                             <p className="text-sm text-muted-foreground">
                                                Users:{' '}
                                                {data.size.toLocaleString()}
                                             </p>
                                             <p className="text-sm text-muted-foreground">
                                                {(
                                                   (data.size / 12850) *
                                                   100
                                                ).toFixed(1)}
                                                % of total
                                             </p>
                                          </div>
                                       );
                                    }
                                    return null;
                                 }}
                              />
                           </Treemap>
                        </ResponsiveContainer>
                     </div>
                  </CardContent>
               </Card>
            </div>
         </CardWrapper>

         <CardWrapper>
            {/* Browser, OS, Device Distribution */}
            <div className="grid gap-6 md:grid-cols-3">
               <Card>
                  <CardHeader>
                     <CardTitle>Browser Distribution</CardTitle>
                     <CardDescription>
                        Browser usage across your site
                     </CardDescription>
                  </CardHeader>
                  <CardContent>
                     <div className="h-[300px]">
                        <ResponsiveContainer width="100%" height="100%">
                           <PieChart>
                              <Pie
                                 data={browserData}
                                 cx="50%"
                                 cy="50%"
                                 labelLine={false}
                                 outerRadius={80}
                                 fill="#8884d8"
                                 dataKey="value"
                                 label={({ name, percent }) =>
                                    `${name} ${(percent * 100).toFixed(0)}%`
                                 }
                              >
                                 {browserData.map((entry, index) => (
                                    <Cell
                                       key={`cell-${index}`}
                                       fill={COLORS[index % COLORS.length]}
                                    />
                                 ))}
                              </Pie>
                              <Tooltip />
                           </PieChart>
                        </ResponsiveContainer>
                     </div>
                     <div className="mt-4 space-y-2">
                        {browserData.slice(0, 3).map((browser, index) => (
                           <div
                              key={index}
                              className="flex items-center justify-between text-sm"
                           >
                              <div className="flex items-center">
                                 <div
                                    className="mr-2 h-3 w-3 rounded-full"
                                    style={{
                                       backgroundColor:
                                          COLORS[index % COLORS.length],
                                    }}
                                 />
                                 <span>{browser.name}</span>
                              </div>
                              <div className="font-medium">
                                 {browser.value}%
                              </div>
                           </div>
                        ))}
                     </div>
                  </CardContent>
               </Card>

               <Card>
                  <CardHeader>
                     <CardTitle>Operating System</CardTitle>
                     <CardDescription>
                        OS distribution across your users
                     </CardDescription>
                  </CardHeader>
                  <CardContent>
                     <div className="h-[300px]">
                        <ResponsiveContainer width="100%" height="100%">
                           <PieChart>
                              <Pie
                                 data={osData}
                                 cx="50%"
                                 cy="50%"
                                 labelLine={false}
                                 outerRadius={80}
                                 fill="#8884d8"
                                 dataKey="value"
                                 label={({ name, percent }) =>
                                    `${name} ${(percent * 100).toFixed(0)}%`
                                 }
                              >
                                 {osData.map((entry, index) => (
                                    <Cell
                                       key={`cell-${index}`}
                                       fill={COLORS[index % COLORS.length]}
                                    />
                                 ))}
                              </Pie>
                              <Tooltip />
                           </PieChart>
                        </ResponsiveContainer>
                     </div>
                     <div className="mt-4 space-y-2">
                        {osData.slice(0, 3).map((os, index) => (
                           <div
                              key={index}
                              className="flex items-center justify-between text-sm"
                           >
                              <div className="flex items-center">
                                 <div
                                    className="mr-2 h-3 w-3 rounded-full"
                                    style={{
                                       backgroundColor:
                                          COLORS[index % COLORS.length],
                                    }}
                                 />
                                 <span>{os.name}</span>
                              </div>
                              <div className="font-medium">{os.value}%</div>
                           </div>
                        ))}
                     </div>
                  </CardContent>
               </Card>

               <Card>
                  <CardHeader>
                     <CardTitle>Device Type</CardTitle>
                     <CardDescription>
                        Distribution by device category
                     </CardDescription>
                  </CardHeader>
                  <CardContent>
                     <div className="h-[300px]">
                        <ResponsiveContainer width="100%" height="100%">
                           <PieChart>
                              <Pie
                                 data={deviceData}
                                 cx="50%"
                                 cy="50%"
                                 labelLine={false}
                                 outerRadius={80}
                                 fill="#8884d8"
                                 dataKey="value"
                                 label={({ name, percent }) =>
                                    `${name} ${(percent * 100).toFixed(0)}%`
                                 }
                              >
                                 {deviceData.map((entry, index) => (
                                    <Cell
                                       key={`cell-${index}`}
                                       fill={COLORS[index % COLORS.length]}
                                    />
                                 ))}
                              </Pie>
                              <Tooltip />
                           </PieChart>
                        </ResponsiveContainer>
                     </div>
                     <div className="mt-4 space-y-2">
                        {deviceData.map((device, index) => (
                           <div
                              key={index}
                              className="flex items-center justify-between text-sm"
                           >
                              <div className="flex items-center">
                                 <div
                                    className="mr-2 h-3 w-3 rounded-full"
                                    style={{
                                       backgroundColor:
                                          COLORS[index % COLORS.length],
                                    }}
                                 />
                                 <span>{device.name}</span>
                              </div>
                              <div className="font-medium">{device.value}%</div>
                           </div>
                        ))}
                     </div>
                  </CardContent>
               </Card>
            </div>
         </CardWrapper>

         <CardWrapper>
            {/* Device-specific Metrics */}
            <div className="grid gap-6 md:grid-cols-3">
               <Card>
                  <CardHeader className="pb-2">
                     <div className="flex items-center justify-between">
                        <CardTitle className="text-sm font-medium">
                           Mobile Metrics
                        </CardTitle>
                        <Smartphone className="h-4 w-4 text-muted-foreground" />
                     </div>
                  </CardHeader>
                  <CardContent className="space-y-4">
                     <div>
                        <div className="flex items-center justify-between text-sm">
                           <span className="text-muted-foreground">
                              Avg. Load Time
                           </span>
                           <span className="font-medium">2.4s</span>
                        </div>
                        <Progress value={80} className="h-1 w-full" />
                     </div>
                     <div>
                        <div className="flex items-center justify-between text-sm">
                           <span className="text-muted-foreground">
                              Bounce Rate
                           </span>
                           <span className="font-medium">38.2%</span>
                        </div>
                        <Progress value={38.2} className="h-1 w-full" />
                     </div>
                     <div>
                        <div className="flex items-center justify-between text-sm">
                           <span className="text-muted-foreground">
                              Conversion Rate
                           </span>
                           <span className="font-medium">3.1%</span>
                        </div>
                        <Progress value={31} className="h-1 w-full" />
                     </div>
                  </CardContent>
                  <CardFooter className="border-t px-6 py-4">
                     <div className="flex items-center justify-between w-full text-sm">
                        <span className="text-muted-foreground">
                           Most Common
                        </span>
                        <span className="font-medium">iPhone (iOS 16)</span>
                     </div>
                  </CardFooter>
               </Card>

               <Card>
                  <CardHeader className="pb-2">
                     <div className="flex items-center justify-between">
                        <CardTitle className="text-sm font-medium">
                           Desktop Metrics
                        </CardTitle>
                        <Monitor className="h-4 w-4 text-muted-foreground" />
                     </div>
                  </CardHeader>
                  <CardContent className="space-y-4">
                     <div>
                        <div className="flex items-center justify-between text-sm">
                           <span className="text-muted-foreground">
                              Avg. Load Time
                           </span>
                           <span className="font-medium">1.8s</span>
                        </div>
                        <Progress value={60} className="h-1 w-full" />
                     </div>
                     <div>
                        <div className="flex items-center justify-between text-sm">
                           <span className="text-muted-foreground">
                              Bounce Rate
                           </span>
                           <span className="font-medium">28.5%</span>
                        </div>
                        <Progress value={28.5} className="h-1 w-full" />
                     </div>
                     <div>
                        <div className="flex items-center justify-between text-sm">
                           <span className="text-muted-foreground">
                              Conversion Rate
                           </span>
                           <span className="font-medium">5.2%</span>
                        </div>
                        <Progress value={52} className="h-1 w-full" />
                     </div>
                  </CardContent>
                  <CardFooter className="border-t px-6 py-4">
                     <div className="flex items-center justify-between w-full text-sm">
                        <span className="text-muted-foreground">
                           Most Common
                        </span>
                        <span className="font-medium">Windows 10 (Chrome)</span>
                     </div>
                  </CardFooter>
               </Card>

               <Card>
                  <CardHeader className="pb-2">
                     <div className="flex items-center justify-between">
                        <CardTitle className="text-sm font-medium">
                           Tablet Metrics
                        </CardTitle>
                        <Tablet className="h-4 w-4 text-muted-foreground" />
                     </div>
                  </CardHeader>
                  <CardContent className="space-y-4">
                     <div>
                        <div className="flex items-center justify-between text-sm">
                           <span className="text-muted-foreground">
                              Avg. Load Time
                           </span>
                           <span className="font-medium">2.1s</span>
                        </div>
                        <Progress value={70} className="h-1 w-full" />
                     </div>
                     <div>
                        <div className="flex items-center justify-between text-sm">
                           <span className="text-muted-foreground">
                              Bounce Rate
                           </span>
                           <span className="font-medium">32.7%</span>
                        </div>
                        <Progress value={32.7} className="h-1 w-full" />
                     </div>
                     <div>
                        <div className="flex items-center justify-between text-sm">
                           <span className="text-muted-foreground">
                              Conversion Rate
                           </span>
                           <span className="font-medium">4.5%</span>
                        </div>
                        <Progress value={45} className="h-1 w-full" />
                     </div>
                  </CardContent>
                  <CardFooter className="border-t px-6 py-4">
                     <div className="flex items-center justify-between w-full text-sm">
                        <span className="text-muted-foreground">
                           Most Common
                        </span>
                        <span className="font-medium">iPad (Safari)</span>
                     </div>
                  </CardFooter>
               </Card>
            </div>
         </CardWrapper>

         <CardWrapper>
            {/* Technical Recommendations */}
            <Card>
               <CardHeader>
                  <CardTitle>Technical Recommendations</CardTitle>
                  <CardDescription>
                     Suggestions to improve cross-browser and cross-device
                     compatibility
                  </CardDescription>
               </CardHeader>
               <CardContent>
                  <div className="space-y-4">
                     <div className="flex items-start gap-4 rounded-lg border p-4">
                        <div className="flex h-8 w-8 items-center justify-center rounded-full bg-yellow-100 text-yellow-600">
                           <AlertTriangle className="h-4 w-4" />
                        </div>
                        <div>
                           <h4 className="font-medium">
                              Optimize for Mobile Safari
                           </h4>
                           <p className="text-sm text-muted-foreground">
                              19.3% of your users are on Safari mobile. Consider
                              optimizing checkout flow for iOS Safari to reduce
                              cart abandonment.
                           </p>
                        </div>
                     </div>
                     <div className="flex items-start gap-4 rounded-lg border p-4">
                        <div className="flex h-8 w-8 items-center justify-center rounded-full bg-green-100 text-green-600">
                           <Globe className="h-4 w-4" />
                        </div>
                        <div>
                           <h4 className="font-medium">Progressive Web App</h4>
                           <p className="text-sm text-muted-foreground">
                              With 52.4% mobile traffic, implementing a PWA
                              could improve engagement and reduce bounce rates
                              on mobile devices.
                           </p>
                        </div>
                     </div>
                     <div className="flex items-start gap-4 rounded-lg border p-4">
                        <div className="flex h-8 w-8 items-center justify-center rounded-full bg-blue-100 text-blue-600">
                           <Clock className="h-4 w-4" />
                        </div>
                        <div>
                           <h4 className="font-medium">Lazy Loading</h4>
                           <p className="text-sm text-muted-foreground">
                              Implement lazy loading for images to improve load
                              times on slower mobile connections, especially for
                              Android users (32.5% of traffic).
                           </p>
                        </div>
                     </div>
                     <div className="flex items-start gap-4 rounded-lg border p-4">
                        <div className="flex h-8 w-8 items-center justify-center rounded-full bg-purple-100 text-purple-600">
                           <Zap className="h-4 w-4" />
                        </div>
                        <div>
                           <h4 className="font-medium">Responsive Images</h4>
                           <p className="text-sm text-muted-foreground">
                              Implement responsive images with srcset to
                              optimize for the wide variety of screen
                              resolutions detected in your user base.
                           </p>
                        </div>
                     </div>
                  </div>
               </CardContent>
            </Card>
         </CardWrapper>
      </div>
   );
};

export default UserAgentReports;
