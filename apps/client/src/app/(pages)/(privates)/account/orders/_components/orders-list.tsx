'use client';

import { useState, useEffect } from 'react';
import { Input } from '@components/ui/input';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '@components/ui/table';
import {
   Pagination,
   PaginationContent,
   PaginationItem,
   PaginationLink,
   PaginationNext,
   PaginationPrevious,
} from '@components/ui/pagination';
import { Badge } from '@components/ui/badge';
import { Search, ChevronRight, ArrowUpDown } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { Button } from '@components/ui/button';

// Order type definition
type Order = {
   id: string;
   orderNumber: string;
   date: string;
   status: 'processing' | 'shipped' | 'delivered' | 'canceled';
   total: string;
   items: number;
};

// Sample order data
const sampleOrders: Order[] = [
   {
      id: '1',
      orderNumber: 'W12345678',
      date: 'Apr 12, 2023',
      status: 'delivered',
      total: '$2,399.00',
      items: 2,
   },
   {
      id: '2',
      orderNumber: 'W12345679',
      date: 'Mar 28, 2023',
      status: 'shipped',
      total: '$129.00',
      items: 1,
   },
   {
      id: '3',
      orderNumber: 'W12345680',
      date: 'Feb 15, 2023',
      status: 'delivered',
      total: '$1,299.00',
      items: 1,
   },
   {
      id: '4',
      orderNumber: 'W12345681',
      date: 'Jan 7, 2023',
      status: 'delivered',
      total: '$59.00',
      items: 3,
   },
   {
      id: '5',
      orderNumber: 'W12345682',
      date: 'Dec 24, 2022',
      status: 'delivered',
      total: '$249.00',
      items: 2,
   },
   {
      id: '6',
      orderNumber: 'W12345683',
      date: 'Nov 18, 2022',
      status: 'delivered',
      total: '$19.99',
      items: 1,
   },
   {
      id: '7',
      orderNumber: 'W12345684',
      date: 'Oct 5, 2022',
      status: 'delivered',
      total: '$549.00',
      items: 1,
   },
   {
      id: '8',
      orderNumber: 'W12345685',
      date: 'Apr 15, 2023',
      status: 'processing',
      total: '$1,599.00',
      items: 1,
   },
   {
      id: '9',
      orderNumber: 'W12345686',
      date: 'Apr 10, 2023',
      status: 'canceled',
      total: '$299.00',
      items: 2,
   },
];

type OrdersListProps = {
   onSelectOrder: (orderId: string) => void;
};

export function OrdersList({ onSelectOrder }: OrdersListProps) {
   const [orders, setOrders] = useState<Order[]>([]);
   const [searchQuery, setSearchQuery] = useState('');
   const [statusFilter, setStatusFilter] = useState<string>('all');
   const [sortBy, setSortBy] = useState<string>('date-desc');
   const [currentPage, setCurrentPage] = useState(1);
   const [isLoading, setIsLoading] = useState(true);

   const ordersPerPage = 5;

   // Simulate loading data
   useEffect(() => {
      const timer = setTimeout(() => {
         setOrders(sampleOrders);
         setIsLoading(false);
      }, 800);

      return () => clearTimeout(timer);
   }, []);

   // Filter orders based on search query and status
   const filteredOrders = orders.filter((order) => {
      const matchesSearch = order.orderNumber
         .toLowerCase()
         .includes(searchQuery.toLowerCase());
      const matchesStatus =
         statusFilter === 'all' || order.status === statusFilter;
      return matchesSearch && matchesStatus;
   });

   // Sort orders
   const sortedOrders = [...filteredOrders].sort((a, b) => {
      switch (sortBy) {
         case 'date-asc':
            return new Date(a.date).getTime() - new Date(b.date).getTime();
         case 'date-desc':
            return new Date(b.date).getTime() - new Date(a.date).getTime();
         case 'total-asc':
            return (
               Number.parseFloat(a.total.replace('$', '').replace(',', '')) -
               Number.parseFloat(b.total.replace('$', '').replace(',', ''))
            );
         case 'total-desc':
            return (
               Number.parseFloat(b.total.replace('$', '').replace(',', '')) -
               Number.parseFloat(a.total.replace('$', '').replace(',', ''))
            );
         default:
            return new Date(b.date).getTime() - new Date(a.date).getTime();
      }
   });

   // Paginate orders
   const indexOfLastOrder = currentPage * ordersPerPage;
   const indexOfFirstOrder = indexOfLastOrder - ordersPerPage;
   const currentOrders = sortedOrders.slice(
      indexOfFirstOrder,
      indexOfLastOrder,
   );
   const totalPages = Math.ceil(sortedOrders.length / ordersPerPage);

   // Get status badge color
   const getStatusColor = (status: Order['status']) => {
      switch (status) {
         case 'processing':
            return 'bg-yellow-100 text-yellow-800';
         case 'shipped':
            return 'bg-blue-100 text-blue-800';
         case 'delivered':
            return 'bg-green-100 text-green-800';
         case 'canceled':
            return 'bg-red-100 text-red-800';
         default:
            return 'bg-gray-100 text-gray-800';
      }
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
   };

   return (
      <div></div>
      // <motion.div
      //    className="bg-white rounded-lg border border-gray-200 overflow-hidden"
      //    variants={containerVariants}
      //    initial="hidden"
      //    animate="visible"
      // >
      //    {/* <div className="flex items-center justify-between px-6 py-4 border-b border-gray-200">
      //       <motion.h2
      //          className="text-lg font-medium text-gray-900"
      //          variants={itemVariants}
      //       >
      //          Orders
      //       </motion.h2>
      //    </div> */}

      //    <motion.div
      //       className="px-6 py-3 bg-gray-50 border-b border-gray-200 flex flex-col sm:flex-row gap-3"
      //       variants={itemVariants}
      //    >
      //       <div className="relative flex-1">
      //          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
      //          <Input
      //             placeholder="Search by order number..."
      //             value={searchQuery}
      //             onChange={(e) => setSearchQuery(e.target.value)}
      //             className="pl-9 h-9"
      //          />
      //       </div>
      //       <div className="flex gap-3">
      //          <Select value={statusFilter} onValueChange={setStatusFilter}>
      //             <SelectTrigger className="w-[140px] h-9">
      //                <SelectValue placeholder="Filter by status" />
      //             </SelectTrigger>
      //             <SelectContent>
      //                <SelectItem value="all">All Statuses</SelectItem>
      //                <SelectItem value="processing">Processing</SelectItem>
      //                <SelectItem value="shipped">Shipped</SelectItem>
      //                <SelectItem value="delivered">Delivered</SelectItem>
      //                <SelectItem value="canceled">Canceled</SelectItem>
      //             </SelectContent>
      //          </Select>
      //          <Select value={sortBy} onValueChange={setSortBy}>
      //             <SelectTrigger className="w-[140px] h-9">
      //                <SelectValue placeholder="Sort by" />
      //             </SelectTrigger>
      //             <SelectContent>
      //                <SelectItem value="date-desc">Newest First</SelectItem>
      //                <SelectItem value="date-asc">Oldest First</SelectItem>
      //                <SelectItem value="total-desc">Highest Amount</SelectItem>
      //                <SelectItem value="total-asc">Lowest Amount</SelectItem>
      //             </SelectContent>
      //          </Select>
      //       </div>
      //    </motion.div>

      //    {isLoading ? (
      //       <div className="px-6 py-8 text-center">
      //          <div className="inline-block h-8 w-8 animate-spin rounded-full border-4 border-solid border-blue-600 border-r-transparent align-[-0.125em] motion-reduce:animate-[spin_1.5s_linear_infinite]"></div>
      //          <p className="mt-4 text-sm text-gray-500">
      //             Loading your orders...
      //          </p>
      //       </div>
      //    ) : currentOrders.length > 0 ? (
      //       <div className="overflow-x-auto">
      //          <Table>
      //             <TableHeader>
      //                <TableRow>
      //                   <TableHead className="w-[150px]">
      //                      Order Number
      //                   </TableHead>
      //                   <TableHead className="w-[120px]">
      //                      <div className="flex items-center">
      //                         Date
      //                         <ArrowUpDown className="ml-1 h-3 w-3" />
      //                      </div>
      //                   </TableHead>
      //                   <TableHead className="w-[120px]">Status</TableHead>
      //                   <TableHead className="w-[100px]">Items</TableHead>
      //                   <TableHead className="text-right w-[120px]">
      //                      Total
      //                   </TableHead>
      //                   <TableHead className="text-right w-[100px]">
      //                      Actions
      //                   </TableHead>
      //                </TableRow>
      //             </TableHeader>
      //             <TableBody>
      //                <AnimatePresence>
      //                   {currentOrders.map((order, index) => (
      //                      <motion.tr
      //                         key={order.id}
      //                         className="cursor-pointer hover:bg-gray-50"
      //                         onClick={() => onSelectOrder(order.id)}
      //                         variants={itemVariants}
      //                         initial="hidden"
      //                         animate="visible"
      //                         custom={index}
      //                         whileHover={{
      //                            backgroundColor: 'rgba(243, 244, 246, 0.7)',
      //                         }}
      //                         transition={{ duration: 0.2 }}
      //                      >
      //                         <TableCell className="font-medium">
      //                            {order.orderNumber}
      //                         </TableCell>
      //                         <TableCell>{order.date}</TableCell>
      //                         <TableCell>
      //                            <Badge
      //                               className={`${getStatusColor(order.status)} capitalize`}
      //                            >
      //                               {order.status}
      //                            </Badge>
      //                         </TableCell>
      //                         <TableCell>{order.items}</TableCell>
      //                         <TableCell className="text-right">
      //                            {order.total}
      //                         </TableCell>
      //                         <TableCell className="text-right">
      //                            <Button
      //                               variant="ghost"
      //                               size="sm"
      //                               className="text-blue-600 hover:text-blue-800"
      //                            >
      //                               Details
      //                               <ChevronRight className="ml-1 h-4 w-4" />
      //                            </Button>
      //                         </TableCell>
      //                      </motion.tr>
      //                   ))}
      //                </AnimatePresence>
      //             </TableBody>
      //          </Table>
      //       </div>
      //    ) : (
      //       <motion.div
      //          className="px-6 py-8 text-center"
      //          variants={itemVariants}
      //       >
      //          <p className="text-sm text-gray-500">
      //             No orders found matching your criteria.
      //          </p>
      //          {(searchQuery || statusFilter !== 'all') && (
      //             <Button
      //                variant="link"
      //                className="mt-2 text-sm font-medium text-blue-600"
      //                onClick={() => {
      //                   setSearchQuery('');
      //                   setStatusFilter('all');
      //                }}
      //             >
      //                Clear filters
      //             </Button>
      //          )}
      //       </motion.div>
      //    )}

      //    {!isLoading && sortedOrders.length > ordersPerPage && (
      //       <motion.div
      //          className="px-6 py-4 border-t border-gray-200"
      //          variants={itemVariants}
      //       >
      //          <Pagination>
      //             <PaginationContent>
      //                <PaginationItem>
      //                   <PaginationPrevious
      //                      href="#"
      //                      onClick={(e) => {
      //                         e.preventDefault();
      //                         if (currentPage > 1)
      //                            setCurrentPage(currentPage - 1);
      //                      }}
      //                      className={
      //                         currentPage === 1
      //                            ? 'pointer-events-none opacity-50'
      //                            : ''
      //                      }
      //                   />
      //                </PaginationItem>

      //                {Array.from({ length: totalPages }).map((_, index) => (
      //                   <PaginationItem key={index}>
      //                      <PaginationLink
      //                         href="#"
      //                         onClick={(e) => {
      //                            e.preventDefault();
      //                            setCurrentPage(index + 1);
      //                         }}
      //                         isActive={currentPage === index + 1}
      //                      >
      //                         {index + 1}
      //                      </PaginationLink>
      //                   </PaginationItem>
      //                ))}

      //                <PaginationItem>
      //                   <PaginationNext
      //                      href="#"
      //                      onClick={(e) => {
      //                         e.preventDefault();
      //                         if (currentPage < totalPages)
      //                            setCurrentPage(currentPage + 1);
      //                      }}
      //                      className={
      //                         currentPage === totalPages
      //                            ? 'pointer-events-none opacity-50'
      //                            : ''
      //                      }
      //                   />
      //                </PaginationItem>
      //             </PaginationContent>
      //          </Pagination>
      //       </motion.div>
      //    )}
      // </motion.div>
   );
}
