'use client';

import { Fragment, useState } from 'react';
import {
   Table,
   TableBody,
   TableCell,
   TableHead,
   TableHeader,
   TableRow,
} from '@components/ui/table';
import { Badge } from '@components/ui/badge';
import { Button } from '@components/ui/button';
import {
   DropdownMenu,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuLabel,
   DropdownMenuSeparator,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import { Input } from '@components/ui/input';
import {
   Calendar,
   Edit,
   MoreHorizontal,
   Plus,
   Search,
   Tag,
   Ticket,
   Trash2,
} from 'lucide-react';
import Link from 'next/link';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { motion, AnimatePresence } from 'framer-motion';
import ContentWrapper from '@components/ui/content-wrapper';
// Removed unused import for PageProps as it is not exported by 'next'

// Mock data for demonstration
const mockPromotions = [
   {
      id: 'prom_1',
      name: 'Summer Sale 2024',
      type: 'event',
      status: 'active',
      startDate: '2024-06-01',
      endDate: '2024-06-30',
      discount: '20%',
      code: null,
   },
   {
      id: 'prom_2',
      name: 'iPhone 15 Launch Discount',
      type: 'product',
      status: 'scheduled',
      startDate: '2024-07-15',
      endDate: '2024-08-15',
      discount: '10%',
      code: null,
   },
   {
      id: 'prom_3',
      name: 'New Customer Welcome',
      type: 'coupon',
      status: 'active',
      startDate: '2024-01-01',
      endDate: '2024-12-31',
      discount: '$25',
      code: 'WELCOME25',
   },
   {
      id: 'prom_4',
      name: 'Black Friday',
      type: 'event',
      status: 'draft',
      startDate: '2024-11-29',
      endDate: '2024-12-01',
      discount: '30%',
      code: null,
   },
   {
      id: 'prom_5',
      name: 'Accessories Bundle',
      type: 'product',
      status: 'active',
      startDate: '2024-05-01',
      endDate: '2024-07-31',
      discount: '15%',
      code: null,
   },
   {
      id: 'prom_6',
      name: 'Loyalty Reward',
      type: 'coupon',
      status: 'active',
      startDate: '2024-01-01',
      endDate: '2024-12-31',
      discount: '$50',
      code: 'LOYALTY50',
   },
];

const mockData = [
   {
      promotion_event_id: 'f55f322f-6406-4dfa-b2ea-2777f7813e70',
      promotion_event_title: 'Black Friday',
      promotion_event_description: 'Sale all item in shop with special price',
      promotion_event_type: 'PROMOTION_EVENT',
      promotion_event_state: 'ACTIVE',
      promotion_event_valid_from: '2025-05-01T00:00:00Z',
      promotion_event_valid_to: '2025-05-30T00:00:00Z',
   },
];

// Define PromotionListProps
type PromotionListProps = {
   filter: 'all' | 'event' | 'product' | 'coupon';
};

// Combine with PageProps
type Props = PromotionListProps;

const PromotionEventPage = () => {
   const [searchQuery, setSearchQuery] = useState('');
   const [statusFilter, setStatusFilter] = useState('all');

   // Filter promotions based on type, search query, and status
   const filteredPromotions = mockPromotions.filter((promotion) => {
      // const matchesType = filter === 'all' || promotion.type === filter;
      // const matchesSearch = promotion.name
      //    .toLowerCase()
      //    .includes(searchQuery.toLowerCase());
      // const matchesStatus =
      //    statusFilter === 'all' || promotion.status === statusFilter;
      return mockPromotions;
   });

   const getTypeIcon = (type: string) => {
      switch (type) {
         case 'event':
            return <Calendar className="mr-2 h-4 w-4" />;
         case 'product':
            return <Tag className="mr-2 h-4 w-4" />;
         case 'coupon':
            return <Ticket className="mr-2 h-4 w-4" />;
         default:
            return null;
      }
   };

   const getStatusBadge = (status: string) => {
      switch (status) {
         case 'ACTIVE':
            return <Badge className="bg-green-500">Active</Badge>;
         case 'scheduled':
            return <Badge className="bg-yellow-500">Scheduled</Badge>;
         case 'draft':
            return <Badge variant="outline">Draft</Badge>;
         case 'expired':
            return <Badge variant="secondary">Expired</Badge>;
         default:
            return null;
      }
   };

   return (
      <Fragment>
         <div className="p-4">
            <ContentWrapper>
               <div className="flex flex-col gap-6 p-6 bg-gray-50">
                  <div className="flex items-center justify-between">
                     <div>
                        <h1 className="text-3xl font-bold tracking-tight">
                           Manage Event Promotion
                        </h1>
                        <p className="text-muted-foreground">
                           Manage your promotional campaigns and events here.
                        </p>
                     </div>

                     <Button>
                        <Link href="/dashboards/promotions/events/create">
                           <Plus className="mr-2 h-4 w-4" />
                           New Event
                        </Link>
                     </Button>
                  </div>

                  <motion.div
                     className="space-y-4"
                     initial={{ opacity: 0 }}
                     animate={{ opacity: 1 }}
                     transition={{ duration: 0.3 }}
                  >
                     <motion.div
                        className="flex items-center justify-between"
                        initial={{ opacity: 0, y: 10 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.3, delay: 0.1 }}
                     >
                        <div className="flex items-center gap-2">
                           <div className="relative">
                              <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
                              <Input
                                 type="search"
                                 placeholder="Search promotions..."
                                 className="w-[250px] pl-8"
                                 value={searchQuery}
                                 onChange={(e) =>
                                    setSearchQuery(e.target.value)
                                 }
                              />
                           </div>
                           <Select
                              value={statusFilter}
                              onValueChange={setStatusFilter}
                           >
                              <SelectTrigger className="w-[180px]">
                                 <SelectValue placeholder="Filter by status" />
                              </SelectTrigger>
                              <SelectContent>
                                 <SelectItem value="all">
                                    All Statuses
                                 </SelectItem>
                                 <SelectItem value="active">Active</SelectItem>
                                 <SelectItem value="scheduled">
                                    Scheduled
                                 </SelectItem>
                                 <SelectItem value="draft">Draft</SelectItem>
                                 <SelectItem value="expired">
                                    Expired
                                 </SelectItem>
                              </SelectContent>
                           </Select>
                        </div>
                     </motion.div>

                     <motion.div
                        className="rounded-md border"
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.4, delay: 0.2 }}
                     >
                        <Table>
                           <TableHeader>
                              <TableRow>
                                 <TableHead className="min-w-[200px]">
                                    Name
                                 </TableHead>
                                 <TableHead>Type</TableHead>
                                 <TableHead>Status</TableHead>
                                 <TableHead>Duration</TableHead>
                                 <TableHead>Categories</TableHead>
                                 <TableHead>Products</TableHead>
                                 <TableHead className="text-right">
                                    Actions
                                 </TableHead>
                              </TableRow>
                           </TableHeader>
                           <TableBody>
                              <AnimatePresence>
                                 {mockData.length === 0 ? (
                                    <TableRow>
                                       <TableCell
                                          colSpan={7}
                                          className="h-24 text-center"
                                       >
                                          No promotions found.
                                       </TableCell>
                                    </TableRow>
                                 ) : (
                                    mockData.map((event, index) => (
                                       <motion.tr
                                          key={event.promotion_event_id}
                                          initial={{ opacity: 0, y: 20 }}
                                          animate={{ opacity: 1, y: 0 }}
                                          exit={{ opacity: 0, y: -20 }}
                                          transition={{
                                             duration: 0.3,
                                             delay: index * 0.05,
                                          }}
                                          className="border-b transition-colors hover:bg-muted/50 data-[state=selected]:bg-muted"
                                       >
                                          <TableCell className="font-medium">
                                             {event.promotion_event_title}
                                          </TableCell>
                                          <TableCell>
                                             <div className="flex items-center">
                                                <Calendar className="mr-2 h-4 w-4" />
                                                <span className="capitalize">
                                                   {event.promotion_event_type}
                                                </span>
                                             </div>
                                          </TableCell>
                                          <TableCell>
                                             {getStatusBadge(
                                                event.promotion_event_state,
                                             )}
                                          </TableCell>
                                          <TableCell>
                                             {new Date(
                                                event.promotion_event_valid_from,
                                             ).toLocaleDateString()}{' '}
                                             -{' '}
                                             {new Date(
                                                event.promotion_event_valid_to,
                                             ).toLocaleDateString()}
                                          </TableCell>
                                          <TableCell>
                                             <p className="text-xs font-medium p-1 bg-slate-100 rounded-md">
                                                {'2 categories'}
                                             </p>
                                          </TableCell>
                                          <TableCell>
                                             <p className="text-xs font-medium p-1 bg-slate-100 rounded-md">
                                                {'3 products'}
                                             </p>
                                          </TableCell>
                                          <TableCell className="text-right">
                                             <DropdownMenu>
                                                <DropdownMenuTrigger asChild>
                                                   <Button
                                                      variant="ghost"
                                                      className="h-8 w-8 p-0"
                                                   >
                                                      <span className="sr-only">
                                                         Open menu
                                                      </span>
                                                      <MoreHorizontal className="h-4 w-4" />
                                                   </Button>
                                                </DropdownMenuTrigger>
                                                <DropdownMenuContent align="end">
                                                   <DropdownMenuLabel>
                                                      Actions
                                                   </DropdownMenuLabel>
                                                   {/* <DropdownMenuItem asChild>
                                                      <Link
                                                         href={`/dashboards/promotions/events/${event.promotion_event_id}`}
                                                      >
                                                         View details
                                                      </Link>
                                                   </DropdownMenuItem> */}
                                                   <DropdownMenuSeparator />
                                                   <DropdownMenuItem asChild>
                                                      <Link
                                                         href={`/dashboards/promotions/events/${event.promotion_event_id}/edit`}
                                                      >
                                                         <Edit className="mr-2 h-4 w-4" />
                                                         Edit
                                                      </Link>
                                                   </DropdownMenuItem>
                                                   {/* <DropdownMenuItem className="text-destructive">
                                                      <Trash2 className="mr-2 h-4 w-4" />
                                                      Delete
                                                   </DropdownMenuItem> */}
                                                </DropdownMenuContent>
                                             </DropdownMenu>
                                          </TableCell>
                                       </motion.tr>
                                    ))
                                 )}
                              </AnimatePresence>
                           </TableBody>
                        </Table>
                     </motion.div>
                  </motion.div>
               </div>
            </ContentWrapper>
         </div>
      </Fragment>
   );
};

export default PromotionEventPage;
