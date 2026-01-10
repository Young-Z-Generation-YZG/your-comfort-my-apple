'use client';

import { useParams } from 'next/navigation';
import { useEffect, useState } from 'react';
import useInventoryService from '~/src/hooks/api/use-inventory-service';
import { LoadingOverlay } from '@components/loading-overlay';
import { Badge } from '@components/ui/badge';
import { Button } from '@components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@components/ui/card';
import { ChevronLeft, Package, Clock, User, Box, ArrowRightLeft, ShoppingCart } from 'lucide-react';
import Link from 'next/link';
import Image from 'next/image';
import { cn } from '~/src/infrastructure/lib/utils';
import { format } from 'date-fns';
import { RequestSkuModal } from '../components/request-sku-modal';

const SkuDetailPage = () => {
   const params = useParams();
   const id = params.id as string;
   const { getSkuByIdWithImageAsync, getSkuByIdWithImageState, isLoading } = useInventoryService();

   useEffect(() => {
      if (id) {
         getSkuByIdWithImageAsync(id);
      }
   }, [id, getSkuByIdWithImageAsync]);

   const sku = getSkuByIdWithImageState.data;

   if (getSkuByIdWithImageState.isError) {
      return (
         <div className="flex flex-col items-center justify-center min-h-[400px] space-y-4">
            <p className="text-red-500 font-medium">Failed to load SKU details.</p>
            <Button asChild variant="outline">
               <Link href="/dashboard/warehouses">
                  <ChevronLeft className="mr-2 h-4 w-4" />
                  Back to Warehouse
               </Link>
            </Button>
         </div>
      );
   }

   const getStateStyle = (state: string) => {
      switch (state) {
         case 'ACTIVE':
            return 'bg-green-100 text-green-800 border-green-300';
         case 'INACTIVE':
            return 'bg-gray-100 text-gray-800 border-gray-300';
         case 'OUT_OF_STOCK':
            return 'bg-red-100 text-red-800 border-red-300';
         default:
            return 'bg-gray-100 text-gray-800 border-gray-300';
      }
   };

   return (
      <div className="p-6 max-w-7xl mx-auto space-y-6">
         <div className="flex items-center justify-between">
            <div className="flex items-center gap-4">
               <Button asChild variant="ghost" size="icon">
                  <Link href="/dashboard/warehouses">
                     <ChevronLeft className="h-5 w-5" />
                  </Link>
               </Button>
               <div>
                  <h1 className="text-2xl font-bold tracking-tight">SKU Details</h1>
                  <p className="text-muted-foreground">
                     Detailed information for SKU: {sku?.code || '...'}
                  </p>
               </div>
            </div>
            <div className="flex gap-2">
               {sku && <RequestSkuModal sku={sku} />}
            </div>
         </div>

         <LoadingOverlay isLoading={isLoading}>
            {sku && (
               <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                  {/* Left Column: Image and Summary */}
                  <Card className="lg:col-span-1 border-0 shadow-lg bg-white/50 backdrop-blur-sm">
                     <CardHeader>
                        <CardTitle className="text-lg">Product Information</CardTitle>
                     </CardHeader>
                     <CardContent className="space-y-6">
                        <div className="aspect-square relative rounded-xl overflow-hidden bg-gray-100 border border-gray-200">
                           {sku.display_image_url ? (
                              <Image
                                 src={sku.display_image_url}
                                 alt={sku.code}
                                 fill
                                 className="object-contain transition-transform hover:scale-150 scale-125"
                              />
                           ) : (
                              <div className="flex items-center justify-center h-full">
                                 <Package className="h-16 w-16 text-gray-300" />
                              </div>
                           )}
                        </div>

                        <div className="space-y-4">
                           <div>
                              <label className="text-xs font-semibold text-gray-500 uppercase tracking-wider">Status</label>
                              <div className="mt-1">
                                 <Badge variant="outline" className={cn("px-3 py-1 text-sm font-medium", getStateStyle(sku.state))}>
                                    {sku.state.replace(/_/g, ' ')}
                                 </Badge>
                              </div>
                           </div>

                           <div className="grid grid-cols-2 gap-4">
                              <div className="bg-blue-50/50 p-3 rounded-xl border border-blue-100">
                                 <label className="text-[10px] font-bold text-blue-600 uppercase">Available</label>
                                 <p className="text-2xl font-bold text-blue-900">{sku.available_in_stock}</p>
                              </div>
                              <div className="bg-green-50/50 p-3 rounded-xl border border-green-100">
                                 <label className="text-[10px] font-bold text-green-600 uppercase">Sold</label>
                                 <p className="text-2xl font-bold text-green-900">{sku.total_sold}</p>
                              </div>
                           </div>
                        </div>
                     </CardContent>
                  </Card>

                  {/* Right Column: Details and Logs */}
                  <div className="lg:col-span-2 space-y-6">
                     {/* Details Card */}
                     <Card className="border-0 shadow-lg bg-white/50 backdrop-blur-sm">
                        <CardHeader>
                           <CardTitle className="text-lg flex items-center gap-2">
                              <Box className="h-5 w-5 text-indigo-500" />
                              Specifications
                           </CardTitle>
                        </CardHeader>
                        <CardContent>
                           <div className="grid grid-cols-1 md:grid-cols-2 gap-x-8 gap-y-6">
                              <div className="space-y-1">
                                 <p className="text-sm text-gray-500 font-medium">Model</p>
                                 <p className="text-base font-semibold text-gray-900">{sku.model.name}</p>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm text-gray-500 font-medium">Color</p>
                                 <div className="flex items-center gap-2">
                                    <div 
                                       className="h-4 w-4 rounded-full border border-gray-300 shadow-sm" 
                                       style={{ backgroundColor: sku.color.hex_code }}
                                    />
                                    <p className="text-base font-semibold text-gray-900">{sku.color.name}</p>
                                 </div>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm text-gray-500 font-medium">Storage</p>
                                 <p className="text-base font-semibold text-gray-900">{sku.storage.name}</p>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm text-gray-500 font-medium">Unit Price</p>
                                 <p className="text-xl font-bold text-indigo-600">
                                    {new Intl.NumberFormat('en-US', {
                                       style: 'currency',
                                       currency: 'USD',
                                    }).format(sku.unit_price)}
                                 </p>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm text-gray-500 font-medium">SKU Code</p>
                                 <p className="text-base font-mono font-bold bg-gray-50 px-2 py-1 rounded inline-block">{sku.code}</p>
                              </div>
                              <div className="space-y-1">
                                 <p className="text-sm text-gray-500 font-medium">Product Classification</p>
                                 <p className="text-base font-semibold text-gray-900">{sku.product_classification}</p>
                              </div>
                           </div>
                        </CardContent>
                     </Card>

                     {/* Reservations and History */}
                     <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        {/* Event Reservation */}
                        <Card className="border-0 shadow-lg bg-white/50 backdrop-blur-sm">
                           <CardHeader className="pb-2">
                              <CardTitle className="text-sm font-bold text-gray-500 uppercase flex items-center gap-2">
                                 <ShoppingCart className="h-4 w-4" />
                                 Event Reservation
                              </CardTitle>
                           </CardHeader>
                           <CardContent>
                              {sku.reserved_for_event ? (
                                 <div className="space-y-2 pt-2">
                                    <div className="flex justify-between items-center">
                                       <span className="text-sm text-gray-600">Event:</span>
                                       <span className="text-sm font-bold text-gray-900">{sku.reserved_for_event.event_name}</span>
                                    </div>
                                    <div className="flex justify-between items-center">
                                       <span className="text-sm text-gray-600">Quantity:</span>
                                       <Badge className="bg-orange-100 text-orange-700 hover:bg-orange-100 border-orange-200">
                                          {sku.reserved_for_event.reserved_quantity}
                                       </Badge>
                                    </div>
                                 </div>
                              ) : (
                                 <div className="py-4 text-center text-sm text-gray-400 italic">No event reservation</div>
                              )}
                           </CardContent>
                        </Card>

                        {/* Internal Requests */}
                        <Card className="border-0 shadow-lg bg-white/50 backdrop-blur-sm">
                           <CardHeader className="pb-2">
                              <CardTitle className="text-sm font-bold text-gray-500 uppercase flex items-center gap-2">
                                 <ArrowRightLeft className="h-4 w-4" />
                                 Internal Requests
                              </CardTitle>
                           </CardHeader>
                           <CardContent>
                              {sku.reserved_for_sku_requests && sku.reserved_for_sku_requests.length > 0 ? (
                                 <div className="space-y-3 pt-2">
                                    {sku.reserved_for_sku_requests.map((req, idx) => (
                                       <div key={idx} className="flex justify-between items-center text-sm border-b border-gray-100 last:border-0 pb-2 last:pb-0">
                                          <div className="flex flex-col">
                                             <span className="text-gray-500 text-[10px] font-bold">TO BRANCH</span>
                                             <span className="font-medium">{req.to_branch_name}</span>
                                          </div>
                                          <Badge variant="secondary" className="font-bold">
                                             {req.reserved_quantity}
                                          </Badge>
                                       </div>
                                    ))}
                                 </div>
                              ) : (
                                 <div className="py-4 text-center text-sm text-gray-400 italic">No internal requests</div>
                              )}
                           </CardContent>
                        </Card>
                     </div>

                     {/* Audit Card */}
                     <Card className="border-0 shadow-lg bg-white/50 backdrop-blur-sm">
                        <CardHeader>
                           <CardTitle className="text-lg flex items-center gap-2">
                              <Clock className="h-5 w-5 text-gray-400" />
                              Audit Info
                           </CardTitle>
                        </CardHeader>
                        <CardContent>
                           <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                              <div className="flex items-start gap-3">
                                 <div className="mt-1 p-2 bg-gray-100 rounded-lg">
                                    <Clock className="h-4 w-4 text-gray-500" />
                                 </div>
                                 <div>
                                    <p className="text-xs font-bold text-gray-400 uppercase">Created At</p>
                                    <p className="text-sm font-medium text-gray-900">{format(new Date(sku.created_at), 'PPPp')}</p>
                                 </div>
                              </div>
                              <div className="flex items-start gap-3">
                                 <div className="mt-1 p-2 bg-gray-100 rounded-lg">
                                    <User className="h-4 w-4 text-gray-500" />
                                 </div>
                                 <div>
                                    <p className="text-xs font-bold text-gray-400 uppercase">Updated By</p>
                                    {/* <p className="text-sm font-medium text-gray-900">{sku.updated_by || 'System'}</p> */}
                                 </div>
                              </div>
                           </div>
                        </CardContent>
                     </Card>
                  </div>
               </div>
            )}
         </LoadingOverlay>
      </div>
   );
};

export default SkuDetailPage;