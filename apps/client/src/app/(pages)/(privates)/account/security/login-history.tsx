'use client';

import { Button } from '~/components/ui/button';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '~/components/ui/table';
import { motion } from 'framer-motion';
import { Laptop, Smartphone, AlertTriangle } from 'lucide-react';

type DeviceType = 'desktop' | 'mobile' | 'tablet';

type LoginEvent = {
   id: string;
   device: DeviceType;
   browser: string;
   location: string;
   ip: string;
   date: string;
   successful: boolean;
};

// Sample login history data
const sampleLoginHistory: LoginEvent[] = [
   {
      id: '1',
      device: 'desktop',
      browser: 'Safari 16.3',
      location: 'Cupertino, CA, USA',
      ip: '192.168.1.1',
      date: 'Apr 15, 2023, 10:23 AM',
      successful: true,
   },
   {
      id: '2',
      device: 'mobile',
      browser: 'Safari Mobile 16.3',
      location: 'San Francisco, CA, USA',
      ip: '192.168.1.2',
      date: 'Apr 14, 2023, 8:45 PM',
      successful: true,
   },
   {
      id: '3',
      device: 'desktop',
      browser: 'Chrome 112.0',
      location: 'New York, NY, USA',
      ip: '192.168.1.3',
      date: 'Apr 10, 2023, 3:12 PM',
      successful: false,
   },
   {
      id: '4',
      device: 'tablet',
      browser: 'Safari Mobile 16.2',
      location: 'Cupertino, CA, USA',
      ip: '192.168.1.4',
      date: 'Apr 5, 2023, 11:30 AM',
      successful: true,
   },
   {
      id: '5',
      device: 'desktop',
      browser: 'Firefox 112.0',
      location: 'Seattle, WA, USA',
      ip: '192.168.1.5',
      date: 'Mar 28, 2023, 9:15 AM',
      successful: true,
   },
];

const getDeviceIcon = (device: DeviceType): React.ReactNode => {
   switch (device) {
      case 'desktop':
         return <Laptop className="h-4 w-4" />;
      case 'mobile':
      case 'tablet':
         return <Smartphone className="h-4 w-4" />;
      default:
         return <Laptop className="h-4 w-4" />;
   }
};

type LoginHistoryProps = {
   isLoading?: boolean;
   loginHistory?: LoginEvent[];
};

export function LoginHistory({
   isLoading = false,
   loginHistory = sampleLoginHistory,
}: LoginHistoryProps) {
   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         initial={{ opacity: 0, y: 20 }}
         animate={{ opacity: 1, y: 0 }}
         transition={{ duration: 0.3, delay: 0.1 }}
      >
         <div className="px-6 py-4 border-b border-gray-200">
            <h2 className="text-lg font-medium text-gray-900">Login History</h2>
         </div>

         <div className="p-6">
            <p className="text-sm text-gray-500 mb-4">
               Review your recent account activity. If you notice any suspicious
               activity, please change your password immediately.
            </p>

            {isLoading ? (
               <div className="py-8 text-center">
                  <div className="inline-block h-8 w-8 animate-spin rounded-full border-4 border-solid border-blue-600 border-r-transparent align-[-0.125em] motion-reduce:animate-[spin_1.5s_linear_infinite]"></div>
                  <p className="mt-4 text-sm text-gray-500">
                     Loading login history...
                  </p>
               </div>
            ) : (
               <div className="overflow-x-auto">
                  <Table>
                     <TableHeader>
                        <TableRow>
                           <TableHead className="w-[120px]">
                              Date & Time
                           </TableHead>
                           <TableHead className="w-[100px]">Device</TableHead>
                           <TableHead className="w-[120px]">Browser</TableHead>
                           <TableHead className="w-[150px]">Location</TableHead>
                           <TableHead className="w-[120px]">
                              IP Address
                           </TableHead>
                           <TableHead className="w-[100px]">Status</TableHead>
                        </TableRow>
                     </TableHeader>
                     <TableBody>
                        {loginHistory.map((event) => (
                           <TableRow key={event.id}>
                              <TableCell className="font-medium">
                                 {event.date}
                              </TableCell>
                              <TableCell>
                                 <div className="flex items-center">
                                    <span className="mr-1.5 text-gray-500">
                                       {getDeviceIcon(event.device)}
                                    </span>
                                    <span className="capitalize">
                                       {event.device}
                                    </span>
                                 </div>
                              </TableCell>
                              <TableCell>{event.browser}</TableCell>
                              <TableCell>{event.location}</TableCell>
                              <TableCell>{event.ip}</TableCell>
                              <TableCell>
                                 {event.successful ? (
                                    <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
                                       Success
                                    </span>
                                 ) : (
                                    <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-red-100 text-red-800">
                                       <AlertTriangle className="h-3 w-3 mr-1" />
                                       Failed
                                    </span>
                                 )}
                              </TableCell>
                           </TableRow>
                        ))}
                     </TableBody>
                  </Table>
               </div>
            )}

            <div className="mt-6 flex justify-between items-center">
               <p className="text-xs text-gray-500">
                  This list shows login attempts from the past 30 days. Older
                  activity may not be shown.
               </p>
               <Button variant="outline" size="sm">
                  Download Full History
               </Button>
            </div>
         </div>
      </motion.div>
   );
}
