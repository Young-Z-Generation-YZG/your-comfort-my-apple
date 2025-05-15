'use client';
import DateRangePicker from '@components/date-range-picker';
import { InputField } from '@components/input-field';
import { LoadingOverlay } from '@components/loading-overlay';
import { SelectField } from '@components/select-field';
import { useEffect, useState } from 'react';
import { FormProvider, useForm } from 'react-hook-form';
import {
   promotionEventResolver,
   PromotionEventSchemaType,
} from '~/src/domain/schemas/discount.schema';
import { useToast } from '~/src/hooks/use-toast';
import { useCreatePromotionEventMutation } from '~/src/infrastructure/services/promotion.service';
import isServerErrorResponse from '~/src/infrastructure/utils/is-server-error';
import { toast as sonnerToast } from 'sonner';
import { Button } from '@components/ui/button';
import {
   MultipleSelectField,
   OptionType,
} from '@components/multiple-select-field';

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

const storagesList: OptionType[] = [
   { value: 'objectid22', label: 'iPhone 16' },
   { value: 'objectid21', label: 'iPhone 15' },
   { value: 'objectid20', label: 'iPhone 14' },
   { value: 'objectid19', label: 'iPhone 13' },
   { value: 'objectid18', label: 'iPhone 12' },
   { value: 'objectid17', label: 'Apple Watch Series 10' },
   { value: 'objectid16', label: 'Apple Watch Series 9' },
   { value: 'objectid15', label: 'Apple Watch Ultra 2' },
   { value: 'objectid14', label: 'Apple Watch SE 2' },
   { value: 'objectid13', label: 'AirPods' },
   { value: 'objectid12', label: 'EarPods' },
   { value: 'objectid11', label: 'Apple Pencil' },
   { value: 'objectid10', label: 'iPad' },
   { value: 'objectid9', label: 'iPad Pro' },
   { value: 'objectid8', label: 'iPad mini' },
   { value: 'objectid7', label: 'Display' },
   { value: 'objectid6', label: 'Mac Pro' },
   { value: 'objectid5', label: 'Mac Studio' },
   { value: 'objectid4', label: 'Mac mimi' },
   { value: 'objectid3', label: 'iMac' },
   { value: 'objectid2', label: 'MacBook Air' },
   { value: 'objectid1', label: 'MacBook Pro' },
];

const PromotionEventForm = () => {
   const [isLoading, setIsLoading] = useState(false);

   const { toast } = useToast();

   const form = useForm({
      resolver: promotionEventResolver,
      defaultValues: {
         event_title: '',
         event_description: '',
      },
   });

   const [
      createEventAsync,
      { isLoading: isCreating, isSuccess, isError, error },
   ] = useCreatePromotionEventMutation();

   const handleSubmit = (data: PromotionEventSchemaType) => {
      console.log('Form data:', data);
   };

   useEffect(() => {
      if (isSuccess) {
         sonnerToast.success('Event Created', {
            style: {
               backgroundColor: '#4CAF50',
               color: '#FFFFFF',
            },
         });
      }

      if (isError && isServerErrorResponse(error)) {
         if (error?.data?.error?.message) {
            toast({
               variant: 'destructive',
               title: `${error.data.error.message ?? 'Server busy, please try again later'}`,
               description: `Wrong email or password`,
            });
         } else {
            toast({
               variant: 'destructive',
               title: `Server busy, please try again later`,
            });
         }
      }
   }, [isSuccess, isError, error]);

   useEffect(() => {
      setIsLoading(isCreating);
   }, [isCreating]);

   return (
      <div>
         <LoadingOverlay isLoading={isLoading} fullScreen={true} />
         <FormProvider {...form}>
            <form onSubmit={form.handleSubmit(handleSubmit)}>
               <div className="grid grid-cols-2 gap-4">
                  <InputField
                     form={form}
                     name="event_title"
                     label="Event Title"
                     description="Enter general product model each product item"
                  />

                  <SelectField
                     form={form}
                     name="event_state"
                     label="Event Status"
                     description="Set the current status of this promotion."
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
                  description="Select the start and end date for this promotion event."
                  className="w-full"
                  onValueChange={(value) => {
                     if (value?.from && value?.to) {
                        form.setValue('event_valid_from', value.from);
                        form.setValue('event_valid_to', value.to);
                     }
                  }}
                  value={undefined}
               />

               <MultipleSelectField
                  form={form}
                  name="promotion_categories"
                  label="Promotion Categories"
                  optionsData={storagesList}
                  onValueChange={(value) => {
                     // setSelectedStorages(value);
                  }}
                  placeholder="Select storages..."
               />

               <div className="flex justify-between mt-5">
                  <Button
                     type="button"
                     variant="outline"
                     onClick={() => {
                        form.reset();
                        form.clearErrors();
                     }}
                  >
                     Reset
                  </Button>
                  <Button type="submit">Create Promotion</Button>
               </div>
            </form>
         </FormProvider>
      </div>
   );
};

export default PromotionEventForm;
