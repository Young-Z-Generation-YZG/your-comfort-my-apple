'use client';

import { useCallback, useEffect, useMemo, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import {
   ArrowLeft,
   Mail,
   Phone,
   Calendar,
   User,
   Building2,
   CheckCircle2,
   XCircle,
   ShoppingBag,
   Package,
   Ellipsis,
   ChevronLeft,
   ChevronRight,
   ChevronsLeft,
   ChevronsRight,
} from 'lucide-react';
import Image from 'next/image';

import {
   Card,
   CardContent,
   CardDescription,
   CardHeader,
   CardTitle,
} from '@components/ui/card';
import { Button } from '@components/ui/button';
import { Badge } from '@components/ui/badge';
import { Separator } from '@components/ui/separator';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '@components/ui/table';
import {
   Select,
   SelectContent,
   SelectGroup,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { LoadingOverlay } from '@components/loading-overlay';
import useUserService from '~/src/hooks/api/use-user-service';
import useOrderingService from '~/src/hooks/api/use-ordering-service';
import { TUser } from '~/src/domain/types/identity.type';
import { TOrder } from '~/src/domain/types/ordering.type';
import { cn } from '~/src/infrastructure/lib/utils';
import usePaginationV2 from '~/src/hooks/use-pagination';
import {
   ColumnDef,
   flexRender,
   getCoreRowModel,
   useReactTable,
} from '@tanstack/react-table';

const dateFormatter = new Intl.DateTimeFormat('en-US', {
   dateStyle: 'medium',
   timeStyle: 'short',
});

const dateOnlyFormatter = new Intl.DateTimeFormat('en-US', {
   dateStyle: 'medium',
});

const currencyFormatter = new Intl.NumberFormat('en-US', {
   style: 'currency',
   currency: 'USD',
});

// Helper function to get gender badge style
const getGenderStyle = (gender: string) => {
   switch (gender?.toUpperCase()) {
      case 'MALE':
         return 'bg-blue-100 text-blue-800 border-blue-300';
      case 'FEMALE':
         return 'bg-pink-100 text-pink-800 border-pink-300';
      case 'OTHER':
         return 'bg-gray-100 text-gray-800 border-gray-300';
      default:
         return 'bg-gray-100 text-gray-800 border-gray-300';
   }
};

const getOrderStatusStyle = (status: string) => {
   switch (status?.toUpperCase()) {
      case 'PENDING':
         return 'bg-yellow-100 text-yellow-800 border-yellow-300';
      case 'CONFIRMED':
         return 'bg-blue-100 text-blue-800 border-blue-300';
      case 'PREPARING':
         return 'bg-purple-100 text-purple-800 border-purple-300';
      case 'DELIVERING':
         return 'bg-indigo-100 text-indigo-800 border-indigo-300';
      case 'DELIVERED':
      case 'PAID':
         return 'bg-green-100 text-green-800 border-green-300';
      case 'CANCELLED':
         return 'bg-red-100 text-red-800 border-red-300';
      default:
         return 'bg-gray-100 text-gray-800 border-gray-300';
   }
};

const PAGE_LIMIT_OPTIONS = [5, 10, 20];

const CustomerManagementDetailPage = () => {
   const params = useParams<{ id: string }>();
   const router = useRouter();

   const {
      getUserByUserIdAsync,
      getUserByUserIdQueryState,
      isLoading: userLoading,
   } = useUserService();

   const { getUserOrdersDetailsAsync, getUserOrdersDetailsState } =
      useOrderingService();

   const [orderPage, setOrderPage] = useState(1);
   const [orderLimit, setOrderLimit] = useState<number>(PAGE_LIMIT_OPTIONS[0]);

   // Use query state data directly for automatic refetching
   const user = getUserByUserIdQueryState.data as TUser | undefined;

   const userId = useMemo(() => {
      const id = params?.id;
      if (Array.isArray(id)) {
         return id[0];
      }
      return id ?? '';
   }, [params]);

   const fetchUserDetails = useCallback(async () => {
      if (!userId) {
         return;
      }

      // Trigger the query - RTK Query will handle caching and automatic refetching
      await getUserByUserIdAsync(userId);
   }, [userId, getUserByUserIdAsync]);

   useEffect(() => {
      fetchUserDetails();
   }, [fetchUserDetails]);

   const fetchUserOrders = useCallback(async () => {
      if (!userId) {
         return;
      }

      await getUserOrdersDetailsAsync(userId, {
         _page: orderPage,
         _limit: orderLimit,
      });
   }, [getUserOrdersDetailsAsync, userId, orderPage, orderLimit]);

   useEffect(() => {
      setOrderPage(1);
      setOrderLimit(PAGE_LIMIT_OPTIONS[0]);
   }, [userId]);

   useEffect(() => {
      fetchUserOrders();
   }, [fetchUserOrders]);

   const ordersLoading =
      getUserOrdersDetailsState.isLoading ||
      getUserOrdersDetailsState.isFetching;

   const combinedLoading = userLoading || ordersLoading;

   const ordersPaginationData = useMemo(() => {
      return (
         getUserOrdersDetailsState.data ?? {
            total_records: 0,
            total_pages: 0,
            page_size: orderLimit,
            current_page: orderPage,
            items: [],
            links: {
               first: null,
               last: null,
               prev: null,
               next: null,
            },
         }
      );
   }, [getUserOrdersDetailsState.data, orderLimit, orderPage]);

   const userOrders = ordersPaginationData.items ?? [];

   const {
      getPaginationItems: getOrderPaginationItems,
      currentPage: currentOrdersPage,
      totalPages: orderTotalPages,
      totalRecords: orderTotalRecords,
      firstItemIndex: orderFirstItemIndex,
      lastItemIndex: orderLastItemIndex,
      limitSelectValue: orderLimitSelectValue,
   } = usePaginationV2(ordersPaginationData, {
      pageSizeOverride: orderLimit,
      currentPageOverride: orderPage,
      fallbackPageSize: PAGE_LIMIT_OPTIONS[0],
   });

   const orderPaginationItems = useMemo(
      () => getOrderPaginationItems(),
      [getOrderPaginationItems],
   );

   const orderColumns = useMemo<ColumnDef<TOrder>[]>(
      () => [
         {
            id: 'order',
            header: 'Order',
            cell: ({ row }) => {
               const order = row.original;
               return (
                  <div>
                     <div className="font-semibold">{order.order_code}</div>
                     <p className="text-xs text-muted-foreground">
                        {order.created_at
                           ? dateFormatter.format(new Date(order.created_at))
                           : 'N/A'}
                     </p>
                  </div>
               );
            },
         },
         {
            id: 'status',
            header: 'Status',
            cell: ({ row }) => (
               <Badge
                  variant="outline"
                  className={cn(
                     'capitalize',
                     getOrderStatusStyle(row.original.status),
                  )}
               >
                  {row.original.status?.toLowerCase() ?? 'unknown'}
               </Badge>
            ),
         },
         {
            id: 'payment',
            header: 'Payment',
            cell: ({ row }) => (
               <div className="text-sm font-medium capitalize">
                  {row.original.payment_method
                     ?.replace(/_/g, ' ')
                     .toLowerCase() || 'n/a'}
               </div>
            ),
         },
         {
            id: 'total',
            header: 'Total',
            meta: { className: 'text-right' },
            cell: ({ row }) => (
               <span className="font-semibold">
                  {currencyFormatter.format(row.original.total_amount || 0)}
               </span>
            ),
         },
         {
            id: 'actions',
            header: 'Actions',
            meta: { className: 'text-right' },
            cell: ({ row }) => (
               <Button
                  variant="outline"
                  size="sm"
                  onClick={() =>
                     router.push(
                        `/dashboard/online/orders/${row.original.order_id}`,
                     )
                  }
               >
                  View
               </Button>
            ),
         },
      ],
      [router],
   );

   const ordersTable = useReactTable({
      data: userOrders,
      columns: orderColumns,
      getCoreRowModel: getCoreRowModel(),
   });

   if (!user && !userLoading && getUserByUserIdQueryState.isError) {
      return (
         <div className="p-5">
            <Button
               variant="ghost"
               onClick={() => router.back()}
               className="mb-4"
            >
               <ArrowLeft className="mr-2 h-4 w-4" />
               Back
            </Button>
            <Card>
               <CardHeader>
                  <CardTitle>Customer Not Found</CardTitle>
                  <CardDescription>
                     The customer you&apos;re looking for doesn&apos;t exist or
                     has been deleted.
                  </CardDescription>
               </CardHeader>
            </Card>
         </div>
      );
   }

   return (
      <div className="p-5">
         <div className="mb-6">
            <Button variant="ghost" onClick={() => router.back()}>
               <ArrowLeft className="mr-2 h-4 w-4" />
               Back
            </Button>
         </div>

         <LoadingOverlay isLoading={combinedLoading}>
            {user && (
               <div className="max-w-6xl mx-auto space-y-6">
                  {/* Profile Header - Different Layout */}
                  <Card className="overflow-hidden">
                     <div className="bg-gradient-to-r from-blue-50 to-indigo-50 p-6">
                        <div className="flex items-center gap-6">
                           {user.profile?.image_url ? (
                              <div className="relative h-32 w-32 rounded-full overflow-hidden border-4 border-white shadow-lg">
                                 <Image
                                    src={user.profile.image_url}
                                    alt={
                                       user.profile.full_name ||
                                       'Profile picture'
                                    }
                                    fill
                                    className="object-cover"
                                 />
                              </div>
                           ) : (
                              <div className="flex h-32 w-32 items-center justify-center rounded-full bg-white border-4 border-white shadow-lg">
                                 <User className="h-16 w-16 text-gray-400" />
                              </div>
                           )}
                           <div className="flex-1">
                              <div className="flex items-center gap-3 mb-2">
                                 <h1 className="text-3xl font-bold text-gray-900">
                                    {user.profile?.full_name || 'N/A'}
                                 </h1>
                                 {user.email_confirmed ? (
                                    <Badge
                                       variant="outline"
                                       className="bg-green-100 text-green-800 border-green-300"
                                    >
                                       <CheckCircle2 className="h-3 w-3 mr-1" />
                                       Verified
                                    </Badge>
                                 ) : (
                                    <Badge
                                       variant="outline"
                                       className="bg-red-100 text-red-800 border-red-300"
                                    >
                                       <XCircle className="h-3 w-3 mr-1" />
                                       Not Verified
                                    </Badge>
                                 )}
                              </div>
                              <p className="text-lg text-gray-600 mb-3">
                                 {user.email}
                              </p>
                              <div className="flex items-center gap-4">
                                 {user.profile?.gender && (
                                    <Badge
                                       variant="outline"
                                       className={cn(
                                          'capitalize',
                                          getGenderStyle(user.profile.gender),
                                       )}
                                    >
                                       {user.profile.gender.toLowerCase()}
                                    </Badge>
                                 )}
                                 {user.profile?.birth_day && (
                                    <div className="flex items-center gap-2 text-sm text-gray-600">
                                       <Calendar className="h-4 w-4" />
                                       <span>
                                          {dateOnlyFormatter.format(
                                             new Date(user.profile.birth_day),
                                          )}
                                       </span>
                                    </div>
                                 )}
                              </div>
                           </div>
                        </div>
                     </div>
                  </Card>

                  {/* Information Grid - Different Layout */}
                  <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                     {/* Personal Information */}
                     <Card className="lg:col-span-2">
                        <CardHeader>
                           <CardTitle className="flex items-center gap-2">
                              <User className="h-5 w-5" />
                              Personal Information
                           </CardTitle>
                        </CardHeader>
                        <CardContent>
                           <div className="grid grid-cols-2 gap-6">
                              <div className="space-y-1">
                                 <p className="text-sm font-medium text-muted-foreground">
                                    First Name
                                 </p>
                                 <p className="text-base font-semibold capitalize">
                                    {user.profile?.first_name || 'N/A'}
                                 </p>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm font-medium text-muted-foreground">
                                    Last Name
                                 </p>
                                 <p className="text-base font-semibold capitalize">
                                    {user.profile?.last_name || 'N/A'}
                                 </p>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm font-medium text-muted-foreground">
                                    Full Name
                                 </p>
                                 <p className="text-base font-semibold capitalize">
                                    {user.profile?.full_name || 'N/A'}
                                 </p>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm font-medium text-muted-foreground">
                                    Gender
                                 </p>
                                 {user.profile?.gender ? (
                                    <Badge
                                       variant="outline"
                                       className={cn(
                                          'capitalize',
                                          getGenderStyle(user.profile.gender),
                                       )}
                                    >
                                       {user.profile.gender.toLowerCase()}
                                    </Badge>
                                 ) : (
                                    <p className="text-base font-semibold">
                                       N/A
                                    </p>
                                 )}
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm font-medium text-muted-foreground">
                                    Birthday
                                 </p>
                                 <p className="text-base font-semibold">
                                    {user.profile?.birth_day
                                       ? dateOnlyFormatter.format(
                                            new Date(user.profile.birth_day),
                                         )
                                       : 'N/A'}
                                 </p>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm font-medium text-muted-foreground">
                                    Phone Number
                                 </p>
                                 <p className="text-base font-semibold">
                                    {user.profile?.phone_number ||
                                       user.phone_number ||
                                       'N/A'}
                                 </p>
                              </div>
                           </div>
                        </CardContent>
                     </Card>

                     {/* Account Details - Sidebar Style */}
                     <Card>
                        <CardHeader>
                           <CardTitle className="flex items-center gap-2">
                              <Building2 className="h-5 w-5" />
                              Account Details
                           </CardTitle>
                        </CardHeader>
                        <CardContent className="space-y-4">
                           <div className="space-y-1">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <Mail className="h-4 w-4" />
                                 <span>Email</span>
                              </div>
                              <p className="text-sm font-medium break-all">
                                 {user.email || 'N/A'}
                              </p>
                           </div>

                           <Separator />

                           <div className="space-y-1">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <User className="h-4 w-4" />
                                 <span>Username</span>
                              </div>
                              <p className="text-sm font-medium">
                                 {user.user_name || 'N/A'}
                              </p>
                           </div>

                           <Separator />

                           <div className="space-y-1">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <CheckCircle2 className="h-4 w-4" />
                                 <span>Email Status</span>
                              </div>
                              <Badge
                                 variant="outline"
                                 className={
                                    user.email_confirmed
                                       ? 'bg-green-100 text-green-800 border-green-300'
                                       : 'bg-red-100 text-red-800 border-red-300'
                                 }
                              >
                                 {user.email_confirmed
                                    ? 'Verified'
                                    : 'Not Verified'}
                              </Badge>
                           </div>

                           <Separator />

                           <div className="space-y-1">
                              <div className="flex items-center gap-2 text-sm text-muted-foreground">
                                 <Phone className="h-4 w-4" />
                                 <span>Phone</span>
                              </div>
                              <p className="text-sm font-medium">
                                 {user.phone_number || 'N/A'}
                              </p>
                           </div>
                        </CardContent>
                     </Card>
                  </div>

                  {/* System Information - Compact Layout */}
                  <Card>
                     <CardHeader>
                        <CardTitle>System Information</CardTitle>
                        <CardDescription>
                           Account metadata and timestamps
                        </CardDescription>
                     </CardHeader>
                     <CardContent>
                        <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
                           <div className="space-y-1">
                              <p className="text-xs font-medium text-muted-foreground uppercase tracking-wide">
                                 Tenant ID
                              </p>
                              <p className="text-sm font-mono font-medium">
                                 {user.tenant_id || 'N/A'}
                              </p>
                           </div>
                           <div className="space-y-1">
                              <p className="text-xs font-medium text-muted-foreground uppercase tracking-wide">
                                 Branch ID
                              </p>
                              <p className="text-sm font-mono font-medium">
                                 {user.branch_id || 'N/A'}
                              </p>
                           </div>
                           <div className="space-y-1">
                              <p className="text-xs font-medium text-muted-foreground uppercase tracking-wide">
                                 Created At
                              </p>
                              <p className="text-sm font-medium">
                                 {user.created_at
                                    ? dateFormatter.format(
                                         new Date(user.created_at),
                                      )
                                    : 'N/A'}
                              </p>
                           </div>
                           <div className="space-y-1">
                              <p className="text-xs font-medium text-muted-foreground uppercase tracking-wide">
                                 Updated At
                              </p>
                              <p className="text-sm font-medium">
                                 {user.updated_at
                                    ? dateFormatter.format(
                                         new Date(user.updated_at),
                                      )
                                    : 'N/A'}
                              </p>
                           </div>
                           {user.updated_by && (
                              <div className="space-y-1">
                                 <p className="text-xs font-medium text-muted-foreground uppercase tracking-wide">
                                    Updated By
                                 </p>
                                 <p className="text-sm font-mono font-medium">
                                    {user.updated_by}
                                 </p>
                              </div>
                           )}
                           {user.is_deleted && (
                              <>
                                 <div className="space-y-1">
                                    <p className="text-xs font-medium text-muted-foreground uppercase tracking-wide">
                                       Deleted At
                                    </p>
                                    <p className="text-sm font-medium">
                                       {user.deleted_at
                                          ? dateFormatter.format(
                                               new Date(user.deleted_at),
                                            )
                                          : 'N/A'}
                                    </p>
                                 </div>
                                 {user.deleted_by && (
                                    <div className="space-y-1">
                                       <p className="text-xs font-medium text-muted-foreground uppercase tracking-wide">
                                          Deleted By
                                       </p>
                                       <p className="text-sm font-mono font-medium">
                                          {user.deleted_by}
                                       </p>
                                    </div>
                                 )}
                              </>
                           )}
                        </div>
                        {user.is_deleted && (
                           <div className="mt-4 pt-4 border-t">
                              <Badge
                                 variant="outline"
                                 className="bg-red-100 text-red-800 border-red-300"
                              >
                                 Account Deleted
                              </Badge>
                           </div>
                        )}
                     </CardContent>
                  </Card>

                  {/* Recent Orders */}
                  <Card>
                     <CardHeader>
                        <CardTitle className="flex items-center gap-2">
                           <ShoppingBag className="h-5 w-5" />
                           Recent Orders
                        </CardTitle>
                        <CardDescription>
                           Latest 5 orders placed by this customer
                        </CardDescription>
                     </CardHeader>
                     <CardContent>
                        {ordersLoading ? (
                           <div className="py-6 text-center text-sm text-muted-foreground">
                              Loading recent orders...
                           </div>
                        ) : getUserOrdersDetailsState.isError ? (
                           <div className="rounded-md border border-destructive/30 bg-destructive/10 p-4 text-sm text-destructive">
                              Unable to load recent orders right now.
                           </div>
                        ) : userOrders.length > 0 ? (
                           <div className="space-y-4">
                              <div className="overflow-x-auto">
                                 <Table>
                                    <TableHeader>
                                       {ordersTable
                                          .getHeaderGroups()
                                          .map((headerGroup) => (
                                             <TableRow key={headerGroup.id}>
                                                {headerGroup.headers.map(
                                                   (header) => {
                                                      const meta = header.column
                                                         .columnDef.meta as
                                                         | {
                                                              className?: string;
                                                           }
                                                         | undefined;

                                                      return (
                                                         <TableHead
                                                            key={header.id}
                                                            className={cn(
                                                               meta?.className,
                                                            )}
                                                         >
                                                            {header.isPlaceholder
                                                               ? null
                                                               : flexRender(
                                                                    header
                                                                       .column
                                                                       .columnDef
                                                                       .header,
                                                                    header.getContext(),
                                                                 )}
                                                         </TableHead>
                                                      );
                                                   },
                                                )}
                                             </TableRow>
                                          ))}
                                    </TableHeader>
                                    <TableBody>
                                       {ordersTable
                                          .getRowModel()
                                          .rows.map((row) => (
                                             <TableRow key={row.id}>
                                                {row
                                                   .getVisibleCells()
                                                   .map((cell) => {
                                                      const meta = cell.column
                                                         .columnDef.meta as
                                                         | {
                                                              className?: string;
                                                           }
                                                         | undefined;

                                                      return (
                                                         <TableCell
                                                            key={cell.id}
                                                            className={cn(
                                                               meta?.className,
                                                            )}
                                                         >
                                                            {flexRender(
                                                               cell.column
                                                                  .columnDef
                                                                  .cell,
                                                               cell.getContext(),
                                                            )}
                                                         </TableCell>
                                                      );
                                                   })}
                                             </TableRow>
                                          ))}
                                    </TableBody>
                                 </Table>
                              </div>

                              {(orderTotalPages ?? 0) > 0 && (
                                 <div className="flex flex-col gap-4 border-t pt-4">
                                    <div className="flex flex-col gap-2 text-sm text-muted-foreground lg:flex-row lg:items-center lg:justify-between">
                                       <div className="flex items-center gap-2">
                                          <Select
                                             value={orderLimitSelectValue}
                                             onValueChange={(value) => {
                                                setOrderLimit(Number(value));
                                                setOrderPage(1);
                                             }}
                                          >
                                             <SelectTrigger className="w-auto h-9">
                                                <SelectValue />
                                             </SelectTrigger>
                                             <SelectContent>
                                                <SelectGroup>
                                                   {PAGE_LIMIT_OPTIONS.map(
                                                      (option) => (
                                                         <SelectItem
                                                            key={option}
                                                            value={String(
                                                               option,
                                                            )}
                                                         >
                                                            {option} / page
                                                         </SelectItem>
                                                      ),
                                                   )}
                                                </SelectGroup>
                                             </SelectContent>
                                          </Select>

                                          <span>
                                             Showing{' '}
                                             <span className="font-medium">
                                                {orderFirstItemIndex}
                                             </span>{' '}
                                             to{' '}
                                             <span className="font-medium">
                                                {orderLastItemIndex}
                                             </span>{' '}
                                             of{' '}
                                             <span className="font-medium">
                                                {orderTotalRecords}
                                             </span>{' '}
                                             orders
                                          </span>
                                       </div>
                                    </div>

                                    <div className="flex flex-wrap items-center justify-end gap-2">
                                       {orderPaginationItems.map(
                                          (item, index) => {
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
                                                   currentOrdersPage;

                                             return (
                                                <Button
                                                   key={`${item.type}-${item.label}-${index}`}
                                                   variant={
                                                      isCurrentPage
                                                         ? 'default'
                                                         : 'outline'
                                                   }
                                                   size="icon"
                                                   className={cn(
                                                      'h-9 w-9',
                                                      isCurrentPage &&
                                                         'bg-black text-white hover:bg-black/90',
                                                   )}
                                                   disabled={
                                                      item.disabled ||
                                                      item.value === null
                                                   }
                                                   onClick={() => {
                                                      if (
                                                         item.value !== null &&
                                                         !item.disabled
                                                      ) {
                                                         setOrderPage(
                                                            item.value,
                                                         );
                                                      }
                                                   }}
                                                >
                                                   {item.type === 'nav' ? (
                                                      item.label === '<<' ? (
                                                         <ChevronsLeft className="h-4 w-4" />
                                                      ) : item.label ===
                                                        '>>' ? (
                                                         <ChevronsRight className="h-4 w-4" />
                                                      ) : item.label === '<' ? (
                                                         <ChevronLeft className="h-4 w-4" />
                                                      ) : (
                                                         <ChevronRight className="h-4 w-4" />
                                                      )
                                                   ) : (
                                                      item.label
                                                   )}
                                                </Button>
                                             );
                                          },
                                       )}
                                    </div>
                                 </div>
                              )}
                           </div>
                        ) : (
                           <div className="flex flex-col items-center justify-center py-8 text-center text-muted-foreground">
                              <Package className="mb-3 h-10 w-10 text-muted-foreground" />
                              <p className="text-sm">
                                 This customer has not placed any orders yet.
                              </p>
                           </div>
                        )}
                     </CardContent>
                  </Card>
               </div>
            )}
         </LoadingOverlay>
      </div>
   );
};

export default CustomerManagementDetailPage;
