import { useForm } from 'react-hook-form';
import {
   ProfileFormType,
   ProfileResolver,
} from '~/domain/schemas/profile.schema';

import { useState } from 'react';
import { Button } from '@components/ui/button';
import { Input } from '@components/ui/input';
import { Label } from '@components/ui/label';
import { motion, AnimatePresence } from 'framer-motion';
import { FiEdit3 } from 'react-icons/fi';
import { cn } from '~/infrastructure/lib/utils';
import {
   Form,
   FormControl,
   FormDescription,
   FormField,
   FormItem,
   FormLabel,
   FormMessage,
} from '@components/ui/form';
import { AlertCircle, CalendarIcon, Save } from 'lucide-react';
import FieldInputSecond from '@components/client/forms/field-input-second';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { IProfilePayload } from '~/domain/interfaces/identity/profile';
import {
   Popover,
   PopoverContent,
   PopoverTrigger,
} from '@components/ui/popover';
import { Calendar } from '@components/ui/calendar';
import { format } from 'date-fns';

type ProfileFormProps = {
   profile: {
      firstName?: string;
      lastName?: string;
      email?: string;
      phoneNumber?: string;
      birthDate?: string;
      imageId?: string;
      imageUrl?: string;
   };
};

const ProfileForm = ({
   profile: {
      firstName,
      lastName,
      email,
      phoneNumber,
      birthDate,
      imageId,
      imageUrl,
   },
}: ProfileFormProps) => {
   const [isEditing, setIsEditing] = useState(false);
   const [profile, setProfile] = useState<IProfilePayload | null>({
      first_name: firstName || '',
      last_name: lastName || '',
      email: email || '',
      phone_number: phoneNumber || '',
      birth_date: birthDate || '2003-08-16',
   });

   const form = useForm<ProfileFormType>({
      resolver: ProfileResolver,
      defaultValues: {
         email: profile?.email || '',
         first_name: profile?.first_name || '',
         last_name: profile?.last_name || '',
         phone_number: profile?.phone_number || '',
         birth_date: profile?.birth_date || '2003-08-16',
      },
   });

   console.log('Form values:', form.getValues());
   console.log('Form errors:', form.formState.errors);

   const handleSubmit = async (data: ProfileFormType) => {
      console.log('Form submitted:', data);

      setIsEditing(false);
   };

   const handleSelectChange = (name: string, value: string) => {};

   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         variants={{
            hidden: { opacity: 0 },
            visible: {
               opacity: 1,
               transition: {
                  staggerChildren: 0.05,
               },
            },
         }}
         initial="hidden"
         animate="visible"
      >
         <div className="px-6 py-4 border-b border-gray-200 flex justify-between items-center">
            <h2 className="text-lg font-medium text-gray-900">
               Personal Information
            </h2>
            {!isEditing && (
               <motion.div
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
               >
                  <Button
                     variant="outline"
                     className="select-none rounded-full h-[22px] px-2 py-1 font-SFProText text-sm font-medium"
                     onClick={() => setIsEditing(true)}
                  >
                     <span>edit</span>
                     <span>
                        <FiEdit3 />
                     </span>
                  </Button>
               </motion.div>
            )}
         </div>

         <form
            onSubmit={form.handleSubmit(handleSubmit)}
            className="p-6 space-y-6"
         >
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6 gap-y-3">
               <FieldInputSecond
                  form={form}
                  name="first_name"
                  label="First Name"
                  disabled={!isEditing}
               />

               <FieldInputSecond
                  form={form}
                  name="last_name"
                  label="Last Name"
                  disabled={!isEditing}
               />

               <FieldInputSecond
                  form={form}
                  name="email"
                  label="Email"
                  disabled={true}
               />

               <FieldInputSecond
                  form={form}
                  type="number"
                  name="phone_number"
                  label="Phone Number"
                  disabled={!isEditing}
               />

               <motion.div
                  className="space-y-2"
                  variants={{
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
                  }}
               >
                  <Form {...form}>
                     <FormField
                        control={form.control}
                        name="birth_date"
                        render={({ field }) => (
                           <FormItem className="flex flex-col">
                              <Label
                                 htmlFor="birth_date"
                                 className="mt-1 mb-[8px]"
                              >
                                 Birth Date
                              </Label>
                              <Popover>
                                 <PopoverTrigger asChild>
                                    <FormControl>
                                       <Button
                                          variant={'outline'}
                                          disabled={!isEditing}
                                          className={cn(
                                             'w-full pl-3 text-left font-normal',
                                             !field.value &&
                                                'text-muted-foreground',
                                          )}
                                       >
                                          {field.value ? (
                                             format(
                                                new Date(field.value),
                                                'PPP',
                                             )
                                          ) : (
                                             <span>Pick a date</span>
                                          )}
                                          <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                                       </Button>
                                    </FormControl>
                                 </PopoverTrigger>
                                 <PopoverContent
                                    className="w-auto p-0"
                                    align="start"
                                 >
                                    <Calendar
                                       mode="single"
                                       selected={
                                          field.value
                                             ? new Date(field.value)
                                             : undefined
                                       }
                                       defaultMonth={
                                          field.value
                                             ? new Date(field.value)
                                             : new Date()
                                       }
                                       onSelect={field.onChange}
                                       disabled={(date) =>
                                          date > new Date() ||
                                          date < new Date('1900-01-01')
                                       }
                                       initialFocus
                                    />
                                 </PopoverContent>
                              </Popover>
                              <FormMessage />
                           </FormItem>
                        )}
                     />
                  </Form>
               </motion.div>

               <motion.div
                  className="space-y-2"
                  variants={{
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
                  }}
               >
                  <Label htmlFor="language">Preferred Language</Label>
                  <Select
                     value={'English'}
                     onValueChange={(value) =>
                        handleSelectChange('language', value)
                     }
                     disabled={true}
                  >
                     <SelectTrigger className="transition-all duration-200">
                        <SelectValue placeholder="Select language" />
                     </SelectTrigger>
                     <SelectContent>
                        <SelectItem value="English">English</SelectItem>
                        <SelectItem value="Spanish">Spanish</SelectItem>
                        <SelectItem value="French">French</SelectItem>
                        <SelectItem value="German">German</SelectItem>
                        <SelectItem value="Japanese">Japanese</SelectItem>
                        <SelectItem value="Chinese">Chinese</SelectItem>
                        <SelectItem value="Vietnamese">Vietnamese</SelectItem>
                     </SelectContent>
                  </Select>
               </motion.div>
            </div>
         </form>

         {isEditing && (
            <div className="flex gap-3 justify-end px-6 py-4">
               <Button
                  variant="outline"
                  onClick={() => {
                     setIsEditing(false);
                     form.reset({
                        first_name: profile?.first_name || '',
                        last_name: profile?.last_name || '',
                        email: profile?.email || '',
                        phone_number: profile?.phone_number || '',
                        birth_date: profile?.birth_date || '',
                     });
                     form.clearErrors();
                  }}
               >
                  Cancel
               </Button>

               <Button
                  onClick={() => {
                     form.handleSubmit(handleSubmit)();
                  }}
               >
                  {false ? (
                     <span className="flex items-center">
                        <svg
                           className="animate-spin -ml-1 mr-2 text-white"
                           xmlns="http://www.w3.org/2000/svg"
                           fill="none"
                           viewBox="0 0 24 24"
                        >
                           <circle
                              className="opacity-25"
                              cx="12"
                              cy="12"
                              r="10"
                              stroke="currentColor"
                              strokeWidth="4"
                           ></circle>
                           <path
                              className="opacity-75"
                              fill="currentColor"
                              d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                           ></path>
                        </svg>
                        Saving...
                     </span>
                  ) : (
                     <span className="flex items-center">
                        <Save className="mr-2" />
                        Save Changes
                     </span>
                  )}
               </Button>
            </div>
         )}
      </motion.div>
   );
};

export default ProfileForm;
