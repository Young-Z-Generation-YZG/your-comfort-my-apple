'use client';

import ContentWrapper from '@components/ui/content-wrapper';
import { Fragment } from 'react';
import { EventPromotionForm } from '../../../_components/event-promotion-form';
import { Badge } from '@components/ui/badge';
import { ArrowLeft, Calendar, Clock } from 'lucide-react';
import { Separator } from '@components/ui/separator';
import { Button } from '@components/ui/button';
import Link from 'next/link';
import { useForm } from 'react-hook-form';
import {
   promotionEventResolver,
   PromotionEventSchemaType,
} from '~/src/domain/schemas/discount.schema';
import { Form } from '@components/ui/form';
import { InputField } from '@components/input-field';
import { SelectField } from '@components/select-field';
import DateRangePicker from '@components/date-range-picker';

const mockData = {
   promotion_event_id: 'f55f322f-6406-4dfa-b2ea-2777f7813e70',
   promotion_event_title: 'Black Friday',
   promotion_event_description: 'Sale all item in shop with special price',
   promotion_event_type: 'PROMOTION_EVENT',
   promotion_event_state: 'ACTIVE',
   promotion_event_valid_from: '2025-05-01T00:00:00Z',
   promotion_event_valid_to: '2025-05-30T00:00:00Z',
};

const stateOptions = [
   {
      value: 'ACTIVE',
      label: 'Active',
   },
   {
      value: 'INACTIVE',
      label: 'Inactive',
   },
];

const EditEventPage = () => {
   const form = useForm({
      resolver: promotionEventResolver,
      defaultValues: {
         event_title: mockData.promotion_event_title,
         event_description: mockData.promotion_event_description,
         event_state: 'ACTIVE' as const,
         event_valid_from: new Date(mockData.promotion_event_valid_from),
         event_valid_to: new Date(mockData.promotion_event_valid_to),
      },
   });

   const onUpdateEvent = async (data: PromotionEventSchemaType) => {};

   return (
      <Fragment>
         <div className="p-4">
            <ContentWrapper>
               <div className="flex flex-col gap-6 p-6 bg-gray-50">
                  <div className="flex items-center gap-2">
                     <Button variant="ghost" size="icon" asChild>
                        <Link
                           href={`/dashboards/promotions/events/${mockData.promotion_event_id}`}
                        >
                           <ArrowLeft className="h-4 w-4" />
                        </Link>
                     </Button>
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
                        {/* <EventPromotionForm /> */}
                        {/* <Form {...form}></Form> */}
                        <Form {...form}>
                           <form onSubmit={form.handleSubmit(onUpdateEvent)}>
                              <div className="grid grid-cols-1 gap-4">
                                 <div className="grid grid-cols-2 gap-4">
                                    <InputField
                                       form={form}
                                       name="event_title"
                                       label="Event Title"
                                       description="Enter a descriptive name for your promotion event"
                                    />

                                    <SelectField
                                       form={form}
                                       name="event_state"
                                       label="Event Status"
                                       description="Set the current status of this promotion"
                                       optionsData={stateOptions}
                                    />
                                 </div>

                                 <InputField
                                    form={form}
                                    type="textarea"
                                    name="event_description"
                                    label="Event Description"
                                    description="Provide details about this promotion event."
                                 />

                                 <DateRangePicker
                                    form={form}
                                    nameFrom="event_valid_from"
                                    nameTo="event_valid_to"
                                    label="Event Duration"
                                    description="Provide details about this promotion event"
                                    className="w-full"
                                    onValueChange={(value) => {
                                       if (value?.from && value?.to) {
                                          form.setValue(
                                             'event_valid_from',
                                             value.from,
                                          );
                                          form.setValue(
                                             'event_valid_to',
                                             value.to,
                                          );
                                       }
                                    }}
                                    value={undefined}
                                 />
                              </div>
                           </form>
                        </Form>
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
                                 {form.getValues('event_title')}
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
                                    {new Date(
                                       form.getValues('event_valid_from'),
                                    ).toLocaleDateString()}{' '}
                                    -{' '}
                                    {new Date(
                                       form.getValues('event_valid_to'),
                                    ).toLocaleDateString()}
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
