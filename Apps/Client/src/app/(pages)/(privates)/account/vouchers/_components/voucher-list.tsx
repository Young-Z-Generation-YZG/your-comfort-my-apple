'use client';

import { useState, useEffect } from 'react';
import { Button } from '@components/ui/button';
import { Input } from '@components/ui/input';
import {
   Dialog,
   DialogContent,
   DialogHeader,
   DialogTitle,
   DialogTrigger,
} from '@components/ui/dialog';
import { Tabs, TabsList, TabsTrigger } from '@components/ui/tabs';
import { Badge } from '@components/ui/badge';
import { motion, AnimatePresence } from 'framer-motion';
import { useToast } from '~/hooks/use-toast';
import {
   Ticket,
   Plus,
   Search,
   X,
   Clock,
   CheckCircle,
   XCircle,
   ChevronRight,
   Copy,
   Calendar,
   Tag,
} from 'lucide-react';
import { VoucherDetails } from './voucher-details';

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

export function VouchersList() {
   const { toast } = useToast();
   const [activeTab, setActiveTab] = useState<string>('active');
   const [searchQuery, setSearchQuery] = useState('');
   const [vouchers, setVouchers] = useState<Voucher[]>([]);
   const [isLoading, setIsLoading] = useState(true);
   const [addVoucherOpen, setAddVoucherOpen] = useState(false);
   const [newVoucherCode, setNewVoucherCode] = useState('');
   const [isAddingVoucher, setIsAddingVoucher] = useState(false);
   const [selectedVoucher, setSelectedVoucher] = useState<Voucher | null>(null);
   const [detailsOpen, setDetailsOpen] = useState(false);

   // Sample voucher data
   useEffect(() => {
      const sampleVouchers: Voucher[] = [
         {
            id: '1',
            code: 'WELCOME20',
            description: '20% off your first purchase',
            discount: {
               type: 'percentage',
               value: 20,
            },
            minPurchase: 50,
            expiryDate: '2025-06-30',
            status: 'active',
            restrictions: [
               'Not valid with other promotions',
               'One-time use only',
            ],
            categories: ['All Products'],
         },
         {
            id: '2',
            code: 'SUMMER2023',
            description: 'Summer sale discount',
            discount: {
               type: 'fixed',
               value: 15,
            },
            expiryDate: '2023-08-31',
            status: 'expired',
            restrictions: ['Excludes sale items'],
            categories: ['Apparel', 'Accessories'],
         },
         {
            id: '3',
            code: 'FREESHIP50',
            description: 'Free shipping on orders over $50',
            discount: {
               type: 'shipping',
               value: 0,
            },
            minPurchase: 50,
            expiryDate: '2025-12-31',
            status: 'active',
            categories: ['All Products'],
         },
         {
            id: '4',
            code: 'HOLIDAY25',
            description: 'Holiday season special discount',
            discount: {
               type: 'percentage',
               value: 25,
            },
            minPurchase: 100,
            expiryDate: '2024-01-15',
            status: 'used',
            usedDate: '2023-12-24',
            categories: ['Electronics', 'Home Goods'],
         },
         {
            id: '5',
            code: 'APPLE10',
            description: '$10 off Apple accessories',
            discount: {
               type: 'fixed',
               value: 10,
            },
            expiryDate: '2025-09-15',
            status: 'active',
            restrictions: ['Valid only for accessories'],
            categories: ['Accessories'],
         },
      ];

      // Simulate loading data
      setTimeout(() => {
         setVouchers(sampleVouchers);
         setIsLoading(false);
      }, 800);
   }, []);

   // Filter vouchers based on active tab and search query
   const filteredVouchers = vouchers.filter((voucher) => {
      const matchesTab = activeTab === 'all' || voucher.status === activeTab;
      const matchesSearch =
         voucher.code.toLowerCase().includes(searchQuery.toLowerCase()) ||
         voucher.description.toLowerCase().includes(searchQuery.toLowerCase());

      return matchesTab && matchesSearch;
   });

   const handleAddVoucher = () => {
      if (!newVoucherCode.trim()) {
         toast({
            title: 'Error',
            description: 'Please enter a voucher code',
            variant: 'destructive',
         });
         return;
      }

      setIsAddingVoucher(true);

      // Simulate API call to validate and add voucher
      setTimeout(() => {
         // Check if voucher already exists
         const voucherExists = vouchers.some(
            (v) => v.code === newVoucherCode.trim(),
         );

         if (voucherExists) {
            toast({
               title: 'Error',
               description:
                  'This voucher code has already been added to your account',
               variant: 'destructive',
            });
            setIsAddingVoucher(false);
            return;
         }

         // Simulate successful voucher addition
         const newVoucher: Voucher = {
            id: Math.random().toString(36).substring(2, 9),
            code: newVoucherCode.trim(),
            description: 'New voucher added',
            discount: {
               type: 'percentage',
               value: 15,
            },
            expiryDate: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000)
               .toISOString()
               .split('T')[0], // 30 days from now
            status: 'active',
            categories: ['All Products'],
         };

         setVouchers([newVoucher, ...vouchers]);
         setNewVoucherCode('');
         setAddVoucherOpen(false);
         setIsAddingVoucher(false);
         setActiveTab('active');

         toast({
            title: 'Voucher added',
            description:
               'Your voucher has been successfully added to your account',
         });
      }, 1500);
   };

   const handleCopyCode = (code: string) => {
      navigator.clipboard.writeText(code);
      toast({
         title: 'Code copied',
         description: 'Voucher code copied to clipboard',
      });
   };

   const handleViewDetails = (voucher: Voucher) => {
      setSelectedVoucher(voucher);
      setDetailsOpen(true);
   };

   // Animation variants
   const containerVariants = {
      hidden: { opacity: 0 },
      visible: {
         opacity: 1,
         transition: {
            staggerChildren: 0.05,
         },
      },
   };

   const itemVariants = {
      hidden: { opacity: 0, y: 20 },
      visible: {
         opacity: 1,
         y: 0,
         transition: {
            type: 'spring',
            stiffness: 300,
            damping: 24,
         },
      },
      exit: {
         opacity: 0,
         y: -20,
         transition: {
            duration: 0.2,
         },
      },
   };

   // Get status badge
   const getStatusBadge = (status: VoucherStatus) => {
      switch (status) {
         case 'active':
            return (
               <Badge className="bg-green-100 text-green-800 hover:bg-green-200">
                  Active
               </Badge>
            );
         case 'used':
            return (
               <Badge className="bg-blue-100 text-blue-800 hover:bg-blue-200">
                  Used
               </Badge>
            );
         case 'expired':
            return (
               <Badge className="bg-gray-100 text-gray-800 hover:bg-gray-200">
                  Expired
               </Badge>
            );
      }
   };

   // Get discount text
   const getDiscountText = (
      discount: { type: string; value: number },
      minPurchase?: number,
   ) => {
      switch (discount.type) {
         case 'percentage':
            return `${discount.value}% off${minPurchase ? ` on orders over $${minPurchase}` : ''}`;
         case 'fixed':
            return `$${discount.value} off${minPurchase ? ` on orders over $${minPurchase}` : ''}`;
         case 'shipping':
            return `Free shipping${minPurchase ? ` on orders over $${minPurchase}` : ''}`;
         default:
            return '';
      }
   };

   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         variants={containerVariants}
         initial="hidden"
         animate="visible"
      >
         <div className="flex items-center justify-between px-6 py-4 border-b border-gray-200">
            <h2 className="text-lg font-medium text-gray-900">
               Vouchers & Coupons
            </h2>
         </div>

         <div className="px-6 py-3 bg-gray-50 border-b border-gray-200">
            <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-3">
               <div className="relative flex-1 max-w-sm">
                  <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                  <Input
                     placeholder="Search vouchers..."
                     value={searchQuery}
                     onChange={(e) => setSearchQuery(e.target.value)}
                     className="pl-9 h-9"
                  />
                  <AnimatePresence>
                     {searchQuery && (
                        <motion.button
                           initial={{ opacity: 0, scale: 0.8 }}
                           animate={{ opacity: 1, scale: 1 }}
                           exit={{ opacity: 0, scale: 0.8 }}
                           onClick={() => setSearchQuery('')}
                           className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600"
                        >
                           <X className="h-4 w-4" />
                        </motion.button>
                     )}
                  </AnimatePresence>
               </div>
               <Tabs
                  value={activeTab}
                  onValueChange={setActiveTab}
                  className="w-full sm:w-auto"
               >
                  <TabsList className="grid grid-cols-3 w-full sm:w-auto">
                     <TabsTrigger value="active" className="text-xs sm:text-sm">
                        <CheckCircle className="h-3 w-3 mr-1" />
                        Active
                     </TabsTrigger>
                     <TabsTrigger value="used" className="text-xs sm:text-sm">
                        <Clock className="h-3 w-3 mr-1" />
                        Used
                     </TabsTrigger>
                     <TabsTrigger
                        value="expired"
                        className="text-xs sm:text-sm"
                     >
                        <XCircle className="h-3 w-3 mr-1" />
                        Expired
                     </TabsTrigger>
                  </TabsList>
               </Tabs>
            </div>
         </div>

         {isLoading ? (
            <div className="px-6 py-8 text-center">
               <div className="inline-block h-8 w-8 animate-spin rounded-full border-4 border-solid border-blue-600 border-r-transparent align-[-0.125em] motion-reduce:animate-[spin_1.5s_linear_infinite]"></div>
               <p className="mt-4 text-sm text-gray-500">
                  Loading your vouchers...
               </p>
            </div>
         ) : filteredVouchers.length > 0 ? (
            <div className="divide-y divide-gray-200">
               <AnimatePresence>
                  {filteredVouchers.map((voucher) => (
                     <motion.div
                        key={voucher.id}
                        variants={itemVariants}
                        initial="hidden"
                        animate="visible"
                        exit="exit"
                        layout
                        className="px-6 py-4 hover:bg-gray-50 transition-colors duration-200"
                     >
                        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
                           <div className="flex items-start space-x-3">
                              <div className="flex-shrink-0">
                                 <div className="h-10 w-10 rounded-full bg-blue-100 flex items-center justify-center">
                                    <Ticket className="h-5 w-5 text-blue-600" />
                                 </div>
                              </div>
                              <div className="flex-1 min-w-0">
                                 <div className="flex items-center">
                                    <h3 className="text-sm font-medium text-gray-900 truncate">
                                       {voucher.description}
                                    </h3>
                                    <span className="ml-2">
                                       {getStatusBadge(voucher.status)}
                                    </span>
                                 </div>
                                 <div className="mt-1 flex flex-col sm:flex-row sm:items-center text-xs text-gray-500 gap-1 sm:gap-3">
                                    <div className="flex items-center">
                                       <Tag className="h-3 w-3 mr-1" />
                                       <span>
                                          {getDiscountText(
                                             voucher.discount,
                                             voucher.minPurchase,
                                          )}
                                       </span>
                                    </div>
                                    <div className="flex items-center">
                                       <Calendar className="h-3 w-3 mr-1" />
                                       <span>
                                          {voucher.status === 'expired'
                                             ? `Expired on ${new Date(voucher.expiryDate).toLocaleDateString()}`
                                             : voucher.status === 'used'
                                               ? `Used on ${new Date(voucher.usedDate!).toLocaleDateString()}`
                                               : `Valid until ${new Date(voucher.expiryDate).toLocaleDateString()}`}
                                       </span>
                                    </div>
                                 </div>
                              </div>
                           </div>
                           <div className="flex items-center space-x-2 ml-auto">
                              <div className="flex items-center space-x-1 bg-gray-100 rounded-md px-2 py-1">
                                 <code className="text-sm font-mono text-gray-800">
                                    {voucher.code}
                                 </code>
                                 <motion.button
                                    whileHover={{ scale: 1.1 }}
                                    whileTap={{ scale: 0.9 }}
                                    onClick={() => handleCopyCode(voucher.code)}
                                    className="text-gray-500 hover:text-gray-700"
                                 >
                                    <Copy className="h-3 w-3" />
                                 </motion.button>
                              </div>
                              <motion.div
                                 whileHover={{ scale: 1.05 }}
                                 whileTap={{ scale: 0.95 }}
                              >
                                 <Button
                                    variant="ghost"
                                    size="sm"
                                    className="text-blue-600 hover:text-blue-800 hover:bg-blue-50"
                                    onClick={() => handleViewDetails(voucher)}
                                 >
                                    Details
                                    <ChevronRight className="ml-1 h-4 w-4" />
                                 </Button>
                              </motion.div>
                           </div>
                        </div>
                     </motion.div>
                  ))}
               </AnimatePresence>
            </div>
         ) : (
            <motion.div
               initial={{ opacity: 0 }}
               animate={{ opacity: 1 }}
               transition={{ delay: 0.3 }}
               className="px-6 py-8 text-center"
            >
               <div className="inline-flex h-12 w-12 items-center justify-center rounded-full bg-gray-100">
                  <Ticket className="h-6 w-6 text-gray-400" />
               </div>
               <h3 className="mt-2 text-sm font-medium text-gray-900">
                  No vouchers found
               </h3>
               <p className="mt-1 text-sm text-gray-500">
                  {searchQuery
                     ? 'No vouchers match your search criteria.'
                     : activeTab === 'active'
                       ? "You don't have any active vouchers."
                       : activeTab === 'used'
                         ? "You don't have any used vouchers."
                         : "You don't have any expired vouchers."}
               </p>
               {searchQuery ? (
                  <Button
                     variant="link"
                     className="mt-3 text-sm font-medium text-blue-600"
                     onClick={() => setSearchQuery('')}
                  >
                     Clear search
                  </Button>
               ) : (
                  <Dialog>
                     <DialogTrigger asChild>
                        <Button variant="outline" className="mt-4">
                           <Plus className="h-4 w-4 mr-1" />
                           Add Your First Voucher
                        </Button>
                     </DialogTrigger>
                     <DialogContent className="sm:max-w-[425px]">
                        <DialogHeader>
                           <DialogTitle>Add New Voucher</DialogTitle>
                        </DialogHeader>
                        <div className="space-y-4 mt-4">
                           <div className="space-y-2">
                              <label
                                 htmlFor="voucher-code"
                                 className="text-sm font-medium text-gray-700"
                              >
                                 Enter Voucher Code
                              </label>
                              <Input
                                 id="voucher-code"
                                 value={newVoucherCode}
                                 onChange={(e) =>
                                    setNewVoucherCode(e.target.value)
                                 }
                                 placeholder="e.g. WELCOME20"
                                 className="uppercase"
                              />
                           </div>
                           <p className="text-xs text-gray-500">
                              Enter the voucher code exactly as it appears,
                              including any hyphens or special characters.
                           </p>
                           <div className="flex justify-end space-x-2 pt-4">
                              <Button
                                 variant="outline"
                                 onClick={() => setAddVoucherOpen(false)}
                              >
                                 Cancel
                              </Button>
                              <Button
                                 onClick={handleAddVoucher}
                                 disabled={isAddingVoucher}
                              >
                                 {isAddingVoucher ? (
                                    <span className="flex items-center">
                                       <svg
                                          className="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
                                          xmlns="http://www.w3.org/2000/svg"
                                          fill="none"
                                          viewBox="0 0 24 24"
                                       >
                                          <circle
                                             className="opacity-25"
                                             cx="12"
                                             cy="12"
                                             r="10"
                                             stroke="currentColor"
                                             strokeWidth="4"
                                          ></circle>
                                          <path
                                             className="opacity-75"
                                             fill="currentColor"
                                             d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                                          ></path>
                                       </svg>
                                       Verifying...
                                    </span>
                                 ) : (
                                    'Add Voucher'
                                 )}
                              </Button>
                           </div>
                        </div>
                     </DialogContent>
                  </Dialog>
               )}
            </motion.div>
         )}

         {/* Voucher Details Dialog */}
         <Dialog open={detailsOpen} onOpenChange={setDetailsOpen}>
            <DialogContent className="sm:max-w-[500px]">
               <DialogHeader>
                  <DialogTitle>Voucher Details</DialogTitle>
               </DialogHeader>
               {selectedVoucher && (
                  <VoucherDetails
                     voucher={selectedVoucher}
                     onClose={() => setDetailsOpen(false)}
                  />
               )}
            </DialogContent>
         </Dialog>
      </motion.div>
   );
}
