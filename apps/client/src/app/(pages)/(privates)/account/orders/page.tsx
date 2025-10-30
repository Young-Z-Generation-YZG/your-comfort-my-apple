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
// import {
//    Pagination,
//    PaginationContent,
//    PaginationItem,
//    PaginationLink,
//    PaginationNext,
//    PaginationPrevious,
// } from '@components/ui/pagination';
import { Badge } from '@components/ui/badge';
import {
   Search,
   ChevronRight,
   ChevronsRight,
   ChevronLeft,
   ChevronsLeft,
   Ellipsis,
} from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { Button } from '@components/ui/button';
import { useRouter } from 'next/navigation';
// import { useOrders } from './_hooks/useOrders';
import { useOrders } from './_hooks/';
import { cn } from '~/infrastructure/lib/utils';
import usePagination from '@components/hooks/use-pagination';
import { EOrderStatus } from '~/domain/enums/order-status.enum';
import useOrderingService from '@components/hooks/api/use-ordering-service';
import { useEffect } from 'react';

const fakeOrders = {
   total_records: 3,
   total_pages: 1,
   page_size: 5,
   current_page: 1,
   items: [
      {
         tenant_id: null,
         branch_id: null,
         order_id: '073b0b68-f7fb-46a0-8851-1e472092f29c',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#912039',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Foo Bar',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284890',
            contact_address_line: '123 Street',
            contact_district: 'Thu Duc',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_id: '073b0b68-f7fb-46a0-8851-1e472092f29c',
               order_item_id: '23e54d5b-02e5-4194-87dd-585007a22c4b',
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '128GB',
               unit_price: 1000,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               promotion: null,
               is_reviewed: false,
               created_at: '2025-10-27T17:47:57.662349Z',
               updated_at: '2025-10-27T17:47:57.662349Z',
               updated_by: null,
               is_deleted: false,
               deleted_at: null,
               deleted_by: null,
            },
         ],
         promotion_id: null,
         promotion_type: null,
         discount_type: null,
         discount_value: null,
         discount_amount: null,
         total_amount: 1000,
         created_at: '2025-10-27T17:47:57.661369Z',
         updated_at: '2025-10-27T17:47:57.66137Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: '5b7404da-3b34-48f8-ab46-fc858b0698d7',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#268356',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Foo Bar',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284890',
            contact_address_line: '123 Street',
            contact_district: 'Thu Duc',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_id: '5b7404da-3b34-48f8-ab46-fc858b0698d7',
               order_item_id: '77cd2f63-5890-4fc5-8f85-ea86b0f4aef1',
               sku_id: null,
               model_id: '68e403d5617b27ad030bf28f',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '128GB',
               unit_price: 1000,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: 'iphone-15',
               quantity: 1,
               promotion: {
                  promotion_id: '550e8400-e29b-41d4-a716-446655440000',
                  promotion_type: 'COUPON',
                  discount_type: 'PERCENTAGE',
                  discount_value: 0.1,
                  discount_amount: 100,
                  final_price: 900,
               },
               is_reviewed: false,
               created_at: '2025-10-27T18:01:42.770772Z',
               updated_at: '2025-10-27T18:01:42.770772Z',
               updated_by: null,
               is_deleted: false,
               deleted_at: null,
               deleted_by: null,
            },
         ],
         promotion_id: '550e8400-e29b-41d4-a716-446655440000',
         promotion_type: 'COUPON',
         discount_type: 'PERCENTAGE',
         discount_value: 0.1,
         discount_amount: 100,
         total_amount: 900,
         created_at: '2025-10-27T18:01:42.769227Z',
         updated_at: '2025-10-27T18:01:42.769227Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         tenant_id: null,
         branch_id: null,
         order_id: 'a6c9fbce-ab37-4472-b8d6-a3684b9d8241',
         customer_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         customer_email: 'user@gmail.com',
         order_code: '#750653',
         status: 'PAID',
         payment_method: 'VNPAY',
         shipping_address: {
            contact_name: 'Foo Bar',
            contact_email: 'user@gmail.com',
            contact_phone_number: '0333284890',
            contact_address_line: '123 Street',
            contact_district: 'Thu Duc',
            contact_province: 'Ho Chi Minh',
            contact_country: 'Vietnam',
         },
         order_items: [
            {
               order_id: 'a6c9fbce-ab37-4472-b8d6-a3684b9d8241',
               order_item_id: '0c9d7078-2949-43e7-a4af-6acf3dd8453e',
               sku_id: null,
               model_id: 'ModelId',
               model_name: 'IPHONE_15',
               color_name: 'BLUE',
               storage_name: '256GB',
               unit_price: 1100,
               display_image_url:
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
               model_slug: '',
               quantity: 1,
               promotion: {
                  promotion_id: '99a356c8-c026-4137-8820-394763f30521',
                  promotion_type: 'EVENT',
                  discount_type: 'PERCENTAGE',
                  discount_value: 0.1,
                  discount_amount: 110,
                  final_price: 990,
               },
               is_reviewed: false,
               created_at: '2025-10-27T18:07:03.60679Z',
               updated_at: '2025-10-27T18:07:03.60679Z',
               updated_by: null,
               is_deleted: false,
               deleted_at: null,
               deleted_by: null,
            },
         ],
         promotion_id: '99a356c8-c026-4137-8820-394763f30521',
         promotion_type: 'EVENT',
         discount_type: 'PERCENTAGE',
         discount_value: 0.1,
         discount_amount: 110,
         total_amount: 990,
         created_at: '2025-10-27T18:07:03.606782Z',
         updated_at: '2025-10-27T18:07:03.606782Z',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
   ],
   links: {
      first: '?_page=1&_limit=5',
      prev: null,
      next: null,
      last: '?_page=1&_limit=5',
   },
};

type TOrderItem = (typeof fakeOrders.items)[number];

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

const OrderPage = () => {
   const router = useRouter();

   const { getOrdersAsync, getOrdersState, isLoading } = useOrderingService();

   useEffect(() => {
      const fetchOrders = async () => {
         const result = await getOrdersAsync();
         if (result.isSuccess) {
            console.log(result.data);
         }
      };
      fetchOrders();
   }, [getOrdersAsync]);

   const {
      currentPage,
      totalPages,
      pageSize,
      totalRecords,
      isLastPage,
      isFirstPage,
      isNextPage,
      isPrevPage,
      paginationItems,
      getPageNumbers,
   } = usePagination(
      getOrdersState.data && getOrdersState.data.items.length > 0
         ? getOrdersState.data
         : fakeOrders,
   );

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
                  {totalRecords > 0 && (
                     <div className="text-sm text-gray-500">
                        {totalRecords} {totalRecords === 1 ? 'order' : 'orders'}{' '}
                        found
                     </div>
                  )}
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
               ) : paginationItems.length > 0 ? (
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
                              {paginationItems.map(
                                 (order: TOrderItem, index: number) => {
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
               {totalPages >= 1 && (
                  <div className="flex items-center gap-2 justify-end mr-5 py-5">
                     {/* First Page */}
                     <Button
                        variant="outline"
                        size="icon"
                        className="h-9 w-9"
                        onClick={() => {}}
                        disabled={isFirstPage}
                     >
                        <ChevronsLeft className="h-4 w-4" />
                     </Button>

                     {/* Previous Page */}
                     <Button
                        variant="outline"
                        size="icon"
                        className="h-9 w-9"
                        onClick={() => {}}
                        disabled={!isPrevPage}
                     >
                        <ChevronLeft className="h-4 w-4" />
                     </Button>

                     {/* Page Numbers */}
                     <div className="flex items-center gap-1">
                        {getPageNumbers().map((page, index) => {
                           if (page === '...') {
                              return (
                                 <span
                                    key={`ellipsis-${index}`}
                                    className="px-2 text-gray-400"
                                 >
                                    <Ellipsis className="h-4 w-4" />
                                 </span>
                              );
                           }

                           return (
                              <Button
                                 key={index}
                                 variant={
                                    currentPage === page ? 'default' : 'outline'
                                 }
                                 size="icon"
                                 className={cn(
                                    'h-9 w-9',
                                    currentPage === page &&
                                       'bg-black text-white hover:bg-black/90',
                                 )}
                              >
                                 {page as number}
                              </Button>
                           );
                        })}
                     </div>

                     {/* Next Page */}
                     <Button
                        variant="outline"
                        size="icon"
                        className="h-9 w-9"
                        onClick={() => {}}
                        disabled={!isNextPage}
                     >
                        <ChevronRight className="h-4 w-4" />
                     </Button>

                     {/* Last Page */}
                     <Button
                        variant="outline"
                        size="icon"
                        className="h-9 w-9"
                        onClick={() => {}}
                        disabled={isLastPage}
                     >
                        <ChevronsRight className="h-4 w-4" />
                     </Button>
                  </div>
               )}
            </motion.div>
         </main>
      </CardContext>
   );
};

export default OrderPage;
