'use client';

import { Button } from '@components/ui/button';
import { Separator } from '@components/ui/separator';
import { Badge } from '@components/ui/badge';
import { motion } from 'framer-motion';
import { useToast } from '@components/hooks/use-toast';
import {
   Ticket,
   Copy,
   Calendar,
   Tag,
   ShoppingBag,
   AlertCircle,
   CheckCircle,
   Clock,
   XCircle,
   Info,
} from 'lucide-react';

type VoucherStatus = 'active' | 'used' | 'expired';

type Voucher = {
   id: string;
   code: string;
   description: string;
   discount: {
      type: 'percentage' | 'fixed' | 'shipping';
      value: number;
   };
   minPurchase?: number;
   expiryDate: string;
   status: VoucherStatus;
   usedDate?: string;
   restrictions?: string[];
   categories?: string[];
};

type VoucherDetailsProps = {
   voucher: Voucher;
   onClose: () => void;
};

export function VoucherDetails({ voucher, onClose }: VoucherDetailsProps) {
   const { toast } = useToast();

   const handleCopyCode = () => {
      navigator.clipboard.writeText(voucher.code);
      toast({
         title: 'Code copied',
         description: 'Voucher code copied to clipboard',
      });
   };

   // Get status icon and color
   const getStatusInfo = (status: VoucherStatus) => {
      switch (status) {
         case 'active':
            return {
               icon: <CheckCircle className="h-5 w-5 text-green-500" />,
               label: 'Active',
               color: 'bg-green-100 text-green-800',
            };
         case 'used':
            return {
               icon: <Clock className="h-5 w-5 text-blue-500" />,
               label: 'Used',
               color: 'bg-blue-100 text-blue-800',
            };
         case 'expired':
            return {
               icon: <XCircle className="h-5 w-5 text-gray-500" />,
               label: 'Expired',
               color: 'bg-gray-100 text-gray-800',
            };
      }
   };

   // Get discount text
   const getDiscountText = (discount: { type: string; value: number }) => {
      switch (discount.type) {
         case 'percentage':
            return `${discount.value}% off`;
         case 'fixed':
            return `$${discount.value} off`;
         case 'shipping':
            return 'Free shipping';
         default:
            return '';
      }
   };

   const statusInfo = getStatusInfo(voucher.status);

   return (
      <div className="space-y-6 py-2">
         <div className="flex items-center justify-between">
            <div className="flex items-center space-x-3">
               <div className="h-10 w-10 rounded-full bg-blue-100 flex items-center justify-center">
                  <Ticket className="h-5 w-5 text-blue-600" />
               </div>
               <div>
                  <h3 className="text-lg font-medium text-gray-900">
                     {voucher.description}
                  </h3>
                  <Badge className={statusInfo.color}>{statusInfo.label}</Badge>
               </div>
            </div>
         </div>

         <div className="bg-gray-50 rounded-lg p-4 flex flex-col sm:flex-row sm:items-center justify-between">
            <div className="flex items-center">
               <code className="text-lg font-mono font-bold text-gray-800">
                  {voucher.code}
               </code>
            </div>
            <motion.div
               whileHover={{ scale: 1.05 }}
               whileTap={{ scale: 0.95 }}
               className="mt-2 sm:mt-0"
            >
               <Button variant="outline" size="sm" onClick={handleCopyCode}>
                  <Copy className="h-4 w-4 mr-1" />
                  Copy Code
               </Button>
            </motion.div>
         </div>

         <div className="space-y-4">
            <div className="flex items-center space-x-2">
               <Tag className="h-5 w-5 text-blue-600" />
               <span className="text-sm font-medium text-gray-700">
                  Discount
               </span>
            </div>
            <div className="pl-7">
               <p className="text-sm text-gray-600">
                  {getDiscountText(voucher.discount)}
               </p>
               {voucher.minPurchase && (
                  <p className="text-sm text-gray-600 mt-1">
                     Minimum purchase: ${voucher.minPurchase.toFixed(2)}
                  </p>
               )}
            </div>
         </div>

         <Separator />

         <div className="space-y-4">
            <div className="flex items-center space-x-2">
               <Calendar className="h-5 w-5 text-blue-600" />
               <span className="text-sm font-medium text-gray-700">
                  Validity
               </span>
            </div>
            <div className="pl-7">
               {voucher.status === 'active' ? (
                  <p className="text-sm text-gray-600">
                     Valid until{' '}
                     {new Date(voucher.expiryDate).toLocaleDateString()}
                  </p>
               ) : voucher.status === 'used' ? (
                  <p className="text-sm text-gray-600">
                     Used on {new Date(voucher.usedDate!).toLocaleDateString()}
                  </p>
               ) : (
                  <p className="text-sm text-gray-600">
                     Expired on{' '}
                     {new Date(voucher.expiryDate).toLocaleDateString()}
                  </p>
               )}
            </div>
         </div>

         {voucher.categories && voucher.categories.length > 0 && (
            <>
               <Separator />
               <div className="space-y-4">
                  <div className="flex items-center space-x-2">
                     <ShoppingBag className="h-5 w-5 text-blue-600" />
                     <span className="text-sm font-medium text-gray-700">
                        Applicable Categories
                     </span>
                  </div>
                  <div className="pl-7 flex flex-wrap gap-2">
                     {voucher.categories.map((category, index) => (
                        <Badge
                           key={index}
                           variant="outline"
                           className="bg-gray-50"
                        >
                           {category}
                        </Badge>
                     ))}
                  </div>
               </div>
            </>
         )}

         {voucher.restrictions && voucher.restrictions.length > 0 && (
            <>
               <Separator />
               <div className="space-y-4">
                  <div className="flex items-center space-x-2">
                     <Info className="h-5 w-5 text-blue-600" />
                     <span className="text-sm font-medium text-gray-700">
                        Restrictions
                     </span>
                  </div>
                  <div className="pl-7">
                     <ul className="list-disc list-inside space-y-1">
                        {voucher.restrictions.map((restriction, index) => (
                           <li key={index} className="text-sm text-gray-600">
                              {restriction}
                           </li>
                        ))}
                     </ul>
                  </div>
               </div>
            </>
         )}

         {voucher.status === 'active' && (
            <div className="bg-blue-50 border border-blue-100 rounded-lg p-4">
               <div className="flex items-start">
                  <AlertCircle className="h-5 w-5 text-blue-600 mt-0.5 mr-2" />
                  <div>
                     <h4 className="text-sm font-medium text-blue-800">
                        How to use this voucher
                     </h4>
                     <p className="mt-1 text-sm text-blue-700">
                        Enter the voucher code at checkout to apply the discount
                        to your order.
                     </p>
                  </div>
               </div>
            </div>
         )}

         <div className="flex justify-end pt-2">
            <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }}>
               <Button onClick={onClose}>Close</Button>
            </motion.div>
         </div>
      </div>
   );
}
