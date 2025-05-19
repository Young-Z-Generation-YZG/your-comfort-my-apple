// import {
//    ColumnDef,
//    getCoreRowModel,
//    getPaginationRowModel,
//    useReactTable,
// } from '@tanstack/react-table';
// import {
//    Table,
//    TableBody,
//    TableCell,
//    TableHead,
//    TableHeader,
//    TableRow,
// } from '@components/ui/table';
// import { Badge } from '@components/ui/badge';
// import { cn } from '~/src/infrastructure/lib/utils';
// import {} from 'lucide-react';
// import {
//    CreditCard,
//    Package,
//    Truck,
//    PackageSearch,
//    X,
//    Loader,
// } from 'lucide-react';
// import { Button } from '@components/ui/button';
// import Link from 'next/link';
// import { OrderResponse } from '~/src/domain/interfaces/orders/order.interface';

// type DataTableProps<TData, TValue> = {
//    data: OrderResponse[];
//    columns: ColumnDef<TData, TValue>[];
// };

// // Status badge colors and icons
// const statusConfig = {
//    PENDING: {
//       color: 'bg-amber-50 text-amber-700 border-amber-200',
//       icon: <Loader className="h-4 w-4" />,
//    },
//    CONFIRMED: {
//       color: 'bg-green-50 text-green-700 border-green-200',
//       icon: <Package className="h-4 w-4" />,
//    },
//    PAID: {
//       color: 'bg-green-50 text-green-700 border-green-200',
//       icon: <CreditCard className="h-4 w-4" />,
//    },
//    PREPARING: {
//       color: 'bg-blue-50 text-blue-700 border-blue-200',
//       icon: <PackageSearch className="h-4 w-4" />,
//    },
//    DELIVERING: {
//       color: 'bg-indigo-50 text-indigo-700 border-indigo-200',
//       icon: <Truck className="h-4 w-4" />,
//    },
//    DELIVERED: {
//       color: 'bg-emerald-50 text-emerald-700 border-emerald-200',
//       icon: <Package className="h-4 w-4" />,
//    },
//    CANCELLED: {
//       color: 'bg-rose-50 text-rose-700 border-rose-200',
//       icon: <X className="h-4 w-4" />,
//    },
// };

// export function OrderTable<TData, TValue>({
//    columns,
//    data,
// }: DataTableProps<TData, TValue>) {
//    const table = useReactTable({
//       data,
//       columns,
//       getCoreRowModel: getCoreRowModel(),
//       getPaginationRowModel: getPaginationRowModel(),
//    });

//    return (
//       <div>
//          <Table>
//             <TableHeader>
//                <TableRow className="bg-slate-50 dark:bg-slate-800/50 hover:bg-slate-50 dark:hover:bg-slate-800/50">
//                   <TableHead className="font-medium">Order Code</TableHead>
//                   <TableHead className="font-medium">Customer</TableHead>
//                   <TableHead className="font-medium">Date</TableHead>
//                   <TableHead className="font-medium">Total</TableHead>
//                   <TableHead className="font-medium w-[150px]">
//                      Payment Type
//                   </TableHead>
//                   <TableHead className="font-medium">Status</TableHead>
//                   <TableHead className="text-right font-medium">
//                      Actions
//                   </TableHead>
//                </TableRow>
//             </TableHeader>
//             <TableBody>
//                {data.map((order) => (
//                   <TableRow
//                      key={order.order_code}
//                      className="transition-all hover:bg-slate-50 dark:hover:bg-slate-800/50 animate-fadeIn border-b border-slate-200 dark:border-slate-700"
//                   >
//                      <TableCell className="order-id font-medium">
//                         {order.order_code}
//                      </TableCell>
//                      <TableCell className="customer-name">
//                         {order.order_shipping_address.contact_name}
//                      </TableCell>
//                      <TableCell className="date">
//                         {new Date(order.order_created_at).toLocaleDateString()}
//                      </TableCell>
//                      <TableCell className="total-amount font-medium">
//                         ${order.order_total_amount.toFixed(2)}
//                      </TableCell>
//                      <TableCell>
//                         <Badge
//                            className={cn(
//                               'payment-type select-none flex items-center gap-1.5 font-medium py-1 px-2 mr-5 rounded-lg hover:bg-white',
//                               `${statusConfig[order.order_status as keyof typeof statusConfig]?.color}`,
//                            )}
//                         >
//                            <CreditCard className="h-3.5 w-3.5 text-slate-500" />
//                            {order.order_payment_method}
//                         </Badge>
//                      </TableCell>
//                      <TableCell>
//                         <Badge
//                            className={cn(
//                               'border select-none text-xs font-medium flex items-center gap-1.5 py-1 px-2 mr-5 rounded-lg uppercase hover:bg-white',
//                               `${statusConfig[order.order_status as keyof typeof statusConfig]?.color}`,
//                            )}
//                         >
//                            {
//                               statusConfig[
//                                  order.order_status as keyof typeof statusConfig
//                               ].icon
//                            }
//                            {order.order_status}
//                         </Badge>
//                      </TableCell>
//                      <TableCell className="text-right">
//                         <Link href={`/dashboards/orders/${order.order_id}`}>
//                            <Button
//                               variant="ghost"
//                               size="sm"
//                               className="h-8 px-3 text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-50"
//                            >
//                               View Details
//                            </Button>
//                         </Link>
//                      </TableCell>
//                   </TableRow>
//                ))}

//                {}
//             </TableBody>
//          </Table>
//       </div>
//    );
// }
