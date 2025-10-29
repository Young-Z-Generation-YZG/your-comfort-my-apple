'use client';

import { Button } from '@components/ui/button';
import ContentWrapper from '@components/ui/content-wrapper';
import { Input } from '@components/ui/input';
import {
   Search,
   Package,
   MapPin,
   Building2,
   CheckCircle,
   AlertCircle,
   Calendar,
   User,
   CreditCard,
} from 'lucide-react';
import { Fragment, useState } from 'react';
import { Badge } from '@components/ui/badge';
import { Card, CardContent } from '@components/ui/card';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { cn } from '~/src/infrastructure/lib/utils';

// Mock data for unassigned orders
const mockUnassignedOrders = [
   {
      order_id: '1',
      order_code: 'ORD-2024-001',
      order_customer_email: 'customer1@example.com',
      order_status: 'PENDING',
      order_payment_method: 'CREDIT_CARD',
      order_shipping_address: {
         contact_name: 'Nguyen Van A',
         contact_email: 'customer1@example.com',
         contact_phone_number: '+84 901 234 567',
         contact_address_line: '123 Nguyen Hue Street',
         contact_district: 'District 1',
         contact_province: 'Ho Chi Minh City',
         contact_country: 'Vietnam',
      },
      order_items_count: 3,
      order_total_amount: 2500000,
      order_created_at: '2024-10-29T08:30:00Z',
      assigned_tenant: null,
   },
   {
      order_id: '2',
      order_code: 'ORD-2024-002',
      order_customer_email: 'customer2@example.com',
      order_status: 'PENDING',
      order_payment_method: 'COD',
      order_shipping_address: {
         contact_name: 'Tran Thi B',
         contact_email: 'customer2@example.com',
         contact_phone_number: '+84 902 345 678',
         contact_address_line: '456 Le Loi Street',
         contact_district: 'Hoan Kiem',
         contact_province: 'Ha Noi',
         contact_country: 'Vietnam',
      },
      order_items_count: 2,
      order_total_amount: 1800000,
      order_created_at: '2024-10-29T09:15:00Z',
      assigned_tenant: null,
   },
   {
      order_id: '3',
      order_code: 'ORD-2024-003',
      order_customer_email: 'customer3@example.com',
      order_status: 'PENDING',
      order_payment_method: 'BANK_TRANSFER',
      order_shipping_address: {
         contact_name: 'Le Van C',
         contact_email: 'customer3@example.com',
         contact_phone_number: '+84 903 456 789',
         contact_address_line: '789 Tran Phu Street',
         contact_district: 'Son Tra',
         contact_province: 'Da Nang',
         contact_country: 'Vietnam',
      },
      order_items_count: 1,
      order_total_amount: 3200000,
      order_created_at: '2024-10-29T10:00:00Z',
      assigned_tenant: null,
   },
];

// Mock tenant data
const mockTenants = [
   {
      tenant_id: '664355f845e56534956be32b',
      code: 'WARE_HOUSE',
      name: 'Main Warehouse',
      tenant_type: 'WARE_HOUSE',
   },
   {
      tenant_id: '664355f845e56534956be32c',
      code: 'HCM_BRANCH_01',
      name: 'Ho Chi Minh Branch 01',
      tenant_type: 'BRANCH',
   },
   {
      tenant_id: '664355f845e56534956be32d',
      code: 'HN_BRANCH_01',
      name: 'Ha Noi Branch 01',
      tenant_type: 'BRANCH',
   },
   {
      tenant_id: '664355f845e56534956be32e',
      code: 'DN_BRANCH_01',
      name: 'Da Nang Branch 01',
      tenant_type: 'BRANCH',
   },
];

const TenantAssignmentPage = () => {
   const [orders] = useState(mockUnassignedOrders);
   const [searchQuery, setSearchQuery] = useState('');
   const [selectedOrders, setSelectedOrders] = useState<string[]>([]);

   // Filter orders based on search
   const filteredOrders = orders.filter(
      (order) =>
         order.order_code.toLowerCase().includes(searchQuery.toLowerCase()) ||
         order.order_shipping_address.contact_name
            .toLowerCase()
            .includes(searchQuery.toLowerCase()) ||
         order.order_shipping_address.contact_province
            .toLowerCase()
            .includes(searchQuery.toLowerCase()),
   );

   const handleSelectOrder = (orderId: string) => {
      setSelectedOrders((prev) =>
         prev.includes(orderId)
            ? prev.filter((id) => id !== orderId)
            : [...prev, orderId],
      );
   };

   const handleSelectAll = () => {
      if (selectedOrders.length === filteredOrders.length) {
         setSelectedOrders([]);
      } else {
         setSelectedOrders(filteredOrders.map((order) => order.order_id));
      }
   };

   const handleAssignTenant = (orderId: string, tenantId: string) => {
      // Handle tenant assignment logic here
      console.log(`Assigning order ${orderId} to tenant ${tenantId}`);
   };

   const formatCurrency = (amount: number) => {
      return new Intl.NumberFormat('vi-VN', {
         style: 'currency',
         currency: 'VND',
      }).format(amount);
   };

   return (
      <Fragment>
         <div className="p-4">
            <ContentWrapper>
               <div className="flex flex-col gap-6 p-6 bg-gray-50">
                  <div className="flex items-center justify-between">
                     <div>
                        <h1 className="text-3xl font-bold tracking-tight">
                           Tenant Assignment
                        </h1>
                        <p className="text-muted-foreground">
                           Assign incoming orders to warehouses or branches
                        </p>
                     </div>
                     <div className="flex items-center gap-3">
                        <Badge
                           variant="outline"
                           className="text-lg py-2 px-4 bg-amber-50 text-amber-700 border-amber-200"
                        >
                           <AlertCircle className="h-4 w-4 mr-2" />
                           {filteredOrders.length} Pending
                        </Badge>
                     </div>
                  </div>

                  <div className="p-8">
                     <div className="mb-6 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
                        <div className="relative w-full max-w-md">
                           <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                           <Input
                              placeholder="Search by order code, customer, or location..."
                              className="pl-10 bg-white dark:bg-slate-800 border-slate-200 dark:border-slate-700 h-10"
                              value={searchQuery}
                              onChange={(e) => setSearchQuery(e.target.value)}
                           />
                        </div>
                        <div className="flex items-center gap-2">
                           <Button
                              variant="outline"
                              size="sm"
                              onClick={handleSelectAll}
                              className="h-10"
                           >
                              {selectedOrders.length === filteredOrders.length
                                 ? 'Deselect All'
                                 : 'Select All'}
                           </Button>
                           <Button
                              size="sm"
                              className="h-10"
                              disabled={selectedOrders.length === 0}
                           >
                              Bulk Assign ({selectedOrders.length})
                           </Button>
                        </div>
                     </div>

                     {/* Orders Cards */}
                     {filteredOrders.length > 0 ? (
                        <div className="space-y-4">
                           {filteredOrders.map((order) => (
                              <Card
                                 key={order.order_id}
                                 className={cn(
                                    'transition-all duration-200 border-2',
                                    selectedOrders.includes(order.order_id)
                                       ? 'border-blue-500 bg-blue-50/50'
                                       : 'border-slate-200 dark:border-slate-700',
                                 )}
                              >
                                 <CardContent className="p-6">
                                    <div className="flex flex-col lg:flex-row gap-6">
                                       {/* Order Info Section */}
                                       <div className="flex-1 space-y-4">
                                          <div className="flex items-start justify-between">
                                             <div>
                                                <div className="flex items-center gap-3 mb-2">
                                                   <input
                                                      type="checkbox"
                                                      checked={selectedOrders.includes(
                                                         order.order_id,
                                                      )}
                                                      onChange={() =>
                                                         handleSelectOrder(
                                                            order.order_id,
                                                         )
                                                      }
                                                      className="h-5 w-5 rounded border-gray-300 cursor-pointer"
                                                   />
                                                   <h3 className="text-xl font-bold">
                                                      {order.order_code}
                                                   </h3>
                                                   <Badge className="bg-amber-100 text-amber-700 border-amber-200">
                                                      <AlertCircle className="h-3 w-3 mr-1" />
                                                      Unassigned
                                                   </Badge>
                                                </div>
                                                <div className="flex items-center gap-4 text-sm text-muted-foreground ml-8">
                                                   <span className="flex items-center gap-1">
                                                      <Calendar className="h-4 w-4" />
                                                      {new Date(
                                                         order.order_created_at,
                                                      ).toLocaleString(
                                                         'en-US',
                                                         {
                                                            month: 'short',
                                                            day: 'numeric',
                                                            year: 'numeric',
                                                            hour: '2-digit',
                                                            minute: '2-digit',
                                                         },
                                                      )}
                                                   </span>
                                                   <span className="flex items-center gap-1">
                                                      <Package className="h-4 w-4" />
                                                      {order.order_items_count}{' '}
                                                      items
                                                   </span>
                                                   <span className="flex items-center gap-1">
                                                      <CreditCard className="h-4 w-4" />
                                                      {
                                                         order.order_payment_method
                                                      }
                                                   </span>
                                                </div>
                                             </div>
                                             <div className="text-right">
                                                <div className="text-2xl font-bold text-blue-600">
                                                   {formatCurrency(
                                                      order.order_total_amount,
                                                   )}
                                                </div>
                                             </div>
                                          </div>

                                          {/* Shipping Address */}
                                          <div className="bg-slate-50 dark:bg-slate-800 rounded-lg p-4">
                                             <div className="flex items-start gap-3">
                                                <MapPin className="h-5 w-5 text-muted-foreground mt-0.5" />
                                                <div className="flex-1">
                                                   <p className="font-medium mb-1 flex items-center gap-2">
                                                      <User className="h-4 w-4" />
                                                      {
                                                         order
                                                            .order_shipping_address
                                                            .contact_name
                                                      }
                                                   </p>
                                                   <p className="text-sm text-muted-foreground">
                                                      {
                                                         order
                                                            .order_shipping_address
                                                            .contact_address_line
                                                      }
                                                   </p>
                                                   <p className="text-sm text-muted-foreground">
                                                      {
                                                         order
                                                            .order_shipping_address
                                                            .contact_district
                                                      }
                                                      ,{' '}
                                                      {
                                                         order
                                                            .order_shipping_address
                                                            .contact_province
                                                      }
                                                   </p>
                                                   <p className="text-sm text-muted-foreground">
                                                      {
                                                         order
                                                            .order_shipping_address
                                                            .contact_phone_number
                                                      }
                                                   </p>
                                                </div>
                                             </div>
                                          </div>
                                       </div>

                                       {/* Tenant Assignment Section */}
                                       <div className="lg:w-80 flex flex-col gap-3">
                                          <label className="text-sm font-medium">
                                             Assign to Tenant
                                          </label>
                                          <Select
                                             onValueChange={(value) =>
                                                handleAssignTenant(
                                                   order.order_id,
                                                   value,
                                                )
                                             }
                                          >
                                             <SelectTrigger className="w-full">
                                                <SelectValue placeholder="Select tenant..." />
                                             </SelectTrigger>
                                             <SelectContent>
                                                {mockTenants.map((tenant) => (
                                                   <SelectItem
                                                      key={tenant.tenant_id}
                                                      value={tenant.tenant_id}
                                                   >
                                                      <div className="flex items-center gap-2">
                                                         {tenant.tenant_type ===
                                                         'WARE_HOUSE' ? (
                                                            <Building2 className="h-4 w-4 text-purple-600" />
                                                         ) : (
                                                            <MapPin className="h-4 w-4 text-blue-600" />
                                                         )}
                                                         <span>
                                                            {tenant.name}
                                                         </span>
                                                      </div>
                                                   </SelectItem>
                                                ))}
                                             </SelectContent>
                                          </Select>
                                          <Button
                                             className="w-full"
                                             size="lg"
                                             disabled={!order.assigned_tenant}
                                          >
                                             <CheckCircle className="h-4 w-4 mr-2" />
                                             Confirm Assignment
                                          </Button>
                                       </div>
                                    </div>
                                 </CardContent>
                              </Card>
                           ))}
                        </div>
                     ) : (
                        <div className="flex flex-col items-center justify-center py-16 bg-white dark:bg-slate-800 rounded-xl border border-slate-200 dark:border-slate-700">
                           <Package className="h-16 w-16 text-muted-foreground/50 mb-4" />
                           <p className="font-medium text-lg mb-1">
                              No pending orders
                           </p>
                           <p className="text-sm text-muted-foreground">
                              All orders have been assigned to tenants
                           </p>
                        </div>
                     )}
                  </div>
               </div>
            </ContentWrapper>
         </div>
      </Fragment>
   );
};

export default TenantAssignmentPage;
