'use client';

import { CardContext } from '../_components/card-content';
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
import { Badge } from '@components/ui/badge';
import { Search, ChevronRight, Ellipsis } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { Button } from '@components/ui/button';
import { useRouter } from 'next/navigation';
import { cn } from '~/infrastructure/lib/utils';
import { EOrderStatus } from '~/domain/enums/order-status.enum';
import useOrderingService from '@components/hooks/api/use-ordering-service';
import { useCallback, useEffect } from 'react';
import { TOrder } from '~/infrastructure/services/ordering.service';
import useFilters from '~/app/(pages)/shop/_hooks/use-filter';

const getHoverStatusColor = (status: string) => {
   switch (status) {
      case 'PENDING':
         return 'bg-yellow-100 text-yellow-800 hover:bg-yellow-200 hover:text-yellow-900';
      case 'PENDING_ASSIGNMENT':
         return 'bg-yellow-100 text-yellow-800 hover:bg-yellow-200 hover:text-yellow-900';
      case 'CONFIRMED':
         return 'bg-green-100 text-green-800 hover:bg-green-200 hover:text-green-900';
      case 'PAID':
         return 'bg-blue-100 text-blue-800 hover:bg-blue-200 hover:text-blue-900';
      case 'DELIVERED':
         return 'bg-green-100 text-green-800 hover:bg-green-200 hover:text-green-900';
      case 'CANCELLED':
         return 'bg-red-100 text-red-800 hover:bg-red-200 hover:text-red-900';
      default:
         return 'bg-gray-100 text-gray-800 hover:bg-gray-200 hover:text-gray-900';
   }
};

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

// Helper function for status badge color with hover
const getStatusColor = (status: string) => {
   switch (status) {
      case 'PENDING':
         return 'bg-yellow-100 text-yellow-800';
      case 'PENDING_ASSIGNMENT':
         return 'bg-yellow-100 text-yellow-800';
      case 'CONFIRMED':
         return 'bg-green-100 text-green-600';
      case 'PAID':
         return 'bg-blue-100 text-blue-800';
      case 'DELIVERED':
         return 'bg-green-100 text-green-800';
      case 'CANCELLED':
         return 'bg-red-100 text-red-800';
      default:
         return 'bg-gray-100 text-gray-800';
   }
};

export type TOrderFilter = {
   _page?: number | null;
   _limit?: number | null;
};

const MAX_VISIBLE_ITEM_LEFT = 2; // Number of pages to show on the LEFT side of "..."
const MAX_VISIBLE_ITEM_RIGHT = 2; // Number of pages to show on the RIGHT side of "..."

const OrderPage = () => {
   const router = useRouter();

   const { getOrdersAsync, getOrdersState, isLoading } = useOrderingService();

   const { filters, setFilters } = useFilters<TOrderFilter>({
      _page: 'number',
      _limit: 'number',
   });

   useEffect(() => {
      getOrdersAsync({
         _page: filters._page ?? undefined,
         _limit: filters._limit ?? undefined,
      });
   }, [filters, getOrdersAsync]);

   const getPaginationItems = useCallback(() => {
      const items: Array<{
         type: 'nav' | 'page' | 'ellipsis';
         label: string;
         value: number | null;
         disabled?: boolean;
      }> = [];

      const totalPages = getOrdersState.data?.total_pages || 0;
      const currentPage = getOrdersState.data?.current_page || 1;

      if (totalPages === 0) return items;

      const isFirstPage = currentPage === 1;
      const isLastPage = currentPage === totalPages;

      // First page button
      items.push({
         type: 'nav',
         label: '<<',
         value: 1,
         disabled: isFirstPage,
      });

      // Previous page button
      items.push({
         type: 'nav',
         label: '<',
         value: currentPage > 1 ? currentPage - 1 : null,
         disabled: isFirstPage,
      });

      // Page numbers - custom logic to group left cluster, current cluster, and trailing cluster
      const addedPages = new Set<number>();
      const frontPages: number[] = [];
      const pushFrontPage = (page: number) => {
         if (page < 1 || page > totalPages) return;
         if (page >= currentPage) return;
         if (frontPages.length >= MAX_VISIBLE_ITEM_LEFT) return;
         if (addedPages.has(page)) return;
         frontPages.push(page);
         addedPages.add(page);
      };

      const rawSegmentIndex =
         Math.floor((currentPage - 1) / MAX_VISIBLE_ITEM_LEFT) - 1;
      const segmentIndex = Math.max(0, rawSegmentIndex);
      const leftStartPage = segmentIndex * MAX_VISIBLE_ITEM_LEFT + 1;

      for (let page = leftStartPage; page < currentPage; page++) {
         pushFrontPage(page);
      }

      frontPages.forEach((page) => {
         items.push({
            type: 'page',
            label: `${page}`,
            value: page,
         });
      });

      const centerEndPage = Math.min(
         totalPages,
         currentPage + MAX_VISIBLE_ITEM_LEFT,
      );
      for (let page = currentPage; page <= centerEndPage; page++) {
         if (addedPages.has(page)) continue;
         items.push({
            type: 'page',
            label: `${page}`,
            value: page,
         });
         addedPages.add(page);
      }

      const lastFrontPage =
         addedPages.size > 0 ? Math.max(...Array.from(addedPages)) : 0;

      // Show last MAX_VISIBLE_ITEM_RIGHT + 1 pages (including last page, so if RIGHT=2, show 33,34,35 for totalPages=35)
      const rightStartPage = Math.max(1, totalPages - MAX_VISIBLE_ITEM_RIGHT);
      if (rightStartPage > lastFrontPage + 1) {
         items.push({
            type: 'ellipsis',
            label: '...',
            value: null,
         });
      }
      for (let page = rightStartPage; page <= totalPages; page++) {
         if (addedPages.has(page)) continue;
         items.push({
            type: 'page',
            label: `${page}`,
            value: page,
         });
         addedPages.add(page);
      }

      // Next page button
      items.push({
         type: 'nav',
         label: '>',
         value: currentPage < totalPages ? currentPage + 1 : null,
         disabled: isLastPage,
      });

      // Last page button
      items.push({
         type: 'nav',
         label: '>>',
         value: totalPages,
         disabled: isLastPage,
      });

      return items;
   }, [getOrdersState.data]);

   return (
      <CardContext className="px-0 py-0">
         <main className="mx-auto max-w-7xl px-4 py-8">
            <motion.div
               initial={{ opacity: 0, y: 20 }}
               animate={{ opacity: 1, y: 0 }}
               transition={{ duration: 0.3 }}
            >
               <div className="flex justify-between items-center">
                  <div>
                     <h1 className="text-3xl font-medium text-gray-900">
                        Orders
                     </h1>
                     <p className="mt-1 text-sm text-gray-500">
                        Manage your orders and track their status in one place.
                     </p>
                  </div>
                  {/* {totalRecords > 0 && (
                     <div className="text-sm text-gray-500">
                        {totalRecords} {totalRecords === 1 ? 'order' : 'orders'}{' '}
                        found
                     </div>
                  )} */}
               </div>
            </motion.div>

            <motion.div
               className="bg-white rounded-lg border border-gray-200 overflow-hidden mt-5"
               variants={containerVariants}
               initial="hidden"
               animate="visible"
            >
               <motion.div
                  className="px-6 py-3 bg-gray-50 border-b border-gray-200 flex flex-col sm:flex-row gap-3"
                  variants={itemVariants}
               >
                  <div className="relative flex-1">
                     <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                     <Input
                        placeholder="Search by order number..."
                        // value={searchQuery}
                        // onChange={(e) => handleSearch(e.target.value)}
                        className="pl-9 h-9"
                     />
                  </div>
                  <div className="flex gap-3">
                     <Select value={''} onValueChange={() => {}}>
                        <SelectTrigger className="w-[180px] h-9">
                           <SelectValue placeholder="Filter by status" />
                        </SelectTrigger>
                        <SelectContent>
                           <SelectItem value="all">All Statuses</SelectItem>
                           <SelectItem value="PENDING_ASSIGNMENT">
                              Pending
                           </SelectItem>
                           <SelectItem value="PROCESSING">
                              Processing
                           </SelectItem>
                           <SelectItem value="SHIPPED">Shipped</SelectItem>
                           <SelectItem value="DELIVERED">Delivered</SelectItem>
                           <SelectItem value="CANCELED">Canceled</SelectItem>
                        </SelectContent>
                     </Select>
                     <Select value={''} onValueChange={() => {}}>
                        <SelectTrigger className="w-[140px] h-9">
                           <SelectValue placeholder="Sort by" />
                        </SelectTrigger>
                        <SelectContent>
                           <SelectItem value="date-desc">
                              Newest First
                           </SelectItem>
                           <SelectItem value="date-asc">
                              Oldest First
                           </SelectItem>
                           <SelectItem value="total-desc">
                              Highest Amount
                           </SelectItem>
                           <SelectItem value="total-asc">
                              Lowest Amount
                           </SelectItem>
                        </SelectContent>
                     </Select>
                  </div>
               </motion.div>

               {isLoading ? (
                  <div className="px-6 py-8 text-center">
                     <div className="inline-block h-8 w-8 animate-spin rounded-full border-4 border-solid border-blue-600 border-r-transparent"></div>
                     <p className="mt-4 text-sm text-gray-500">
                        Loading your orders...
                     </p>
                  </div>
               ) : getOrdersState.data?.items &&
                 getOrdersState.data?.items.length > 0 ? (
                  <div className="overflow-x-auto">
                     <Table>
                        <TableHeader>
                           <TableRow>
                              <TableHead className="w-[150px]">
                                 Order Number
                              </TableHead>
                              <TableHead className="w-[120px]">Date</TableHead>
                              <TableHead className="w-[120px]">
                                 Status
                              </TableHead>
                              <TableHead className="w-[100px]">Items</TableHead>
                              <TableHead className="text-right w-[120px]">
                                 Total
                              </TableHead>
                              <TableHead className="text-right w-[100px]">
                                 Actions
                              </TableHead>
                           </TableRow>
                        </TableHeader>
                        <TableBody>
                           <AnimatePresence>
                              {getOrdersState.data?.items.map(
                                 (order: TOrder, index: number) => {
                                    const status =
                                       order.status ==
                                       EOrderStatus.PENDING_ASSIGNMENT
                                          ? EOrderStatus.PENDING
                                          : order.status;

                                    const statusStyle = getStatusColor(status);
                                    const hoverStatusStyle =
                                       getHoverStatusColor(status);

                                    return (
                                       <motion.tr
                                          key={order.order_id}
                                          className="hover:bg-gray-50"
                                          variants={itemVariants}
                                          initial="hidden"
                                          animate="visible"
                                          custom={index}
                                          transition={{ duration: 0.2 }}
                                       >
                                          <TableCell className="font-medium">
                                             {order.order_code}
                                          </TableCell>
                                          <TableCell>
                                             {new Date(
                                                order.created_at,
                                             ).toLocaleDateString('en-US', {
                                                year: 'numeric',
                                                month: 'short',
                                                day: 'numeric',
                                             })}
                                          </TableCell>
                                          <TableCell>
                                             <Badge
                                                className={cn(
                                                   'select-none',
                                                   statusStyle,
                                                   hoverStatusStyle,
                                                )}
                                             >
                                                {status.replace('_', ' ')}
                                             </Badge>
                                          </TableCell>
                                          <TableCell>
                                             {order.order_items.reduce(
                                                (acc, item) =>
                                                   acc + item.quantity,
                                                0,
                                             )}
                                          </TableCell>
                                          <TableCell className="text-right">
                                             ${order.total_amount.toFixed(2)}
                                          </TableCell>
                                          <TableCell className="text-right">
                                             <Button
                                                variant="ghost"
                                                size="sm"
                                                className="text-blue-600 hover:text-blue-800"
                                                onClick={() => {
                                                   router.push(
                                                      `/account/orders/${order.order_id}`,
                                                   );
                                                }}
                                             >
                                                Details
                                                <ChevronRight className="ml-1 h-4 w-4" />
                                             </Button>
                                          </TableCell>
                                       </motion.tr>
                                    );
                                 },
                              )}
                           </AnimatePresence>
                        </TableBody>
                     </Table>
                  </div>
               ) : (
                  <motion.div
                     className="px-6 py-8 text-center"
                     variants={itemVariants}
                  >
                     <p className="text-sm text-gray-500">
                        No orders found matching your criteria.
                     </p>
                     {/* {(searchQuery || statusFilter !== 'all') && (
                        <Button
                           variant="link"
                           className="mt-2 text-sm font-medium text-blue-600"
                           onClick={clearFilters}
                        >
                           Clear filters
                        </Button>
                     )} */}
                  </motion.div>
               )}

               {/* Pagination Controls */}
               {getOrdersState.data && getOrdersState.data.total_pages > 0 && (
                  <div className="flex items-center gap-2 justify-end mr-5 py-5">
                     {getPaginationItems().map((item, index) => {
                        if (item.type === 'ellipsis') {
                           return (
                              <span
                                 key={`ellipsis-${index}`}
                                 className="px-2 text-gray-400 flex items-center"
                              >
                                 <Ellipsis className="h-4 w-4" />
                              </span>
                           );
                        }

                        const isCurrentPage =
                           item.type === 'page' &&
                           item.value ===
                              (getOrdersState.data?.current_page || 1);

                        return (
                           <Button
                              key={`${item.type}-${item.label}-${index}`}
                              variant={isCurrentPage ? 'default' : 'outline'}
                              size="sm"
                              disabled={item.disabled || item.value === null}
                              onClick={() => {
                                 if (item.value !== null && !item.disabled) {
                                    setFilters((prev) => ({
                                       ...prev,
                                       _page: item.value!,
                                    }));
                                 }
                              }}
                              className={cn(
                                 isCurrentPage &&
                                    'bg-black text-white hover:bg-black/90',
                              )}
                           >
                              {item.label}
                           </Button>
                        );
                     })}
                  </div>
               )}
            </motion.div>
         </main>
      </CardContext>
   );
};

export default OrderPage;
