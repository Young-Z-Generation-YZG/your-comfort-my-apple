'use client';

import ContentWrapper from '@components/ui/content-wrapper';
import { Fragment } from 'react';
import { EventPromotionForm } from '../../../_components/event-promotion-form';
import { Badge } from '@components/ui/badge';
import { Calendar, Clock } from 'lucide-react';
import { Separator } from '@components/ui/separator';

const EditEventPage = () => {
   return (
      <Fragment>
         <div className="p-4">
            <ContentWrapper>
               <div className="flex flex-col gap-6 p-6 bg-gray-50">
                  <div className="flex items-center justify-between">
                     <div>
                        <h1 className="text-3xl font-bold tracking-tight">
                           Event Promotions
                        </h1>
                        <p className="text-muted-foreground">
                           Manage your promotional campaigns and events here.
                        </p>
                     </div>
                  </div>
               </div>

               <div className="flex flex-row gap-5">
                  <div className="basis-2/3">
                     <div className="bg-white px-5 py-5 rounded-md">
                        <EventPromotionForm />
                     </div>
                  </div>

                  <div className="basis-1/3">
                     <div className="flex flex-col bg-white px-5 py-5 rounded-md">
                        <h2 className="font-semibold">Promotion Preview</h2>

                        <div className="space-y-4">
                           <div className="text-center mb-6">
                              <div className="h-16 w-16 bg-slate-100 dark:bg-slate-700 rounded-full mx-auto flex items-center justify-center mb-3">
                                 <Calendar className="h-8 w-8 text-slate-600" />
                              </div>
                              <h3 className="text-xl font-medium">
                                 {'New Promotion'}
                              </h3>
                              <Badge className="mt-2 bg-blue-50 text-blue-700 border-blue-200 border">
                                 <Clock className="h-3 w-3 mr-1" />
                                 Upcoming
                              </Badge>
                           </div>
                        </div>

                        <Separator />

                        <div className="space-y-3 pt-2">
                           <div className="flex items-center justify-between">
                              <span className="text-sm font-medium">
                                 Discount:
                              </span>
                              <span className="font-semibold">10% off</span>
                           </div>

                           <div className="flex items-center justify-between">
                              <span className="text-sm font-medium">
                                 Date Range:
                              </span>
                              <span className="text-sm">
                                 <>
                                    {new Date().toLocaleDateString()} -{' '}
                                    {new Date().toLocaleDateString()}
                                 </>
                              </span>
                           </div>

                           <div className="flex items-center justify-between">
                              <span className="text-sm font-medium">Type:</span>
                              <Badge variant="outline">{'Not set'}</Badge>
                           </div>
                        </div>
                     </div>
                  </div>
               </div>
            </ContentWrapper>
         </div>
      </Fragment>
   );
};

export default EditEventPage;
