import { FormDescription, FormLabel } from './ui/form';
import {
   type UseFormReturn,
   type FieldValues,
   useFieldArray,
   ArrayPath,
   FieldArray,
   Path,
} from 'react-hook-form';
import { Button } from './ui/button';
import { InputField } from './input-field';
import { cn } from '../infrastructure/lib/utils';
import { Trash } from 'lucide-react';
import { SelectField } from './select-field';
import { OptionType } from './multiple-select-field';
import ContentWrapper from './content-wrapper';
import ProductArrayField from './product-array-field';

const mockData = [
   {
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      category_name: 'iPhone 16',
      parent_id: '',
      category_slug: 'iphone-16',
   },
   {
      category_id: '92dc470aa9ee0a5e6fbafdbd',
      category_name: 'iPhone 15',
      parent_id: '',
      category_slug: 'iphone-15',
   },
   {
      category_id: '93dc470aa9ee0a5e6fbafdbe',
      category_name: 'iPhone 14',
      parent_id: '',
      category_slug: 'iphone-14',
   },
   {
      category_id: '94dc470aa9ee0a5e6fbafdbf',
      category_name: 'iPhone 13',
      parent_id: '',
      category_slug: 'iphone-13',
   },
];

const promotionCategories: OptionType[] = [
   { value: '91dc470aa9ee0a5e6fbafdbc', label: 'iPhone 16' },
   { value: '92dc470aa9ee0a5e6fbafdbd', label: 'iPhone 15' },
   { value: '93dc470aa9ee0a5e6fbafdbe', label: 'iPhone 14' },
   { value: '94dc470aa9ee0a5e6fbafdbf', label: 'iPhone 13' },
   { value: '67dc5378a9ee0a5e6fbafdb8', label: 'Apple Watch Series 10' },
   { value: '67dc5379a9ee0a5e6fbafdb9', label: 'Apple Watch Series 9' },
   { value: '67dc537ba9ee0a5e6fbafdba', label: 'Apple Watch Ultra 2' },
   { value: '67dc537ca9ee0a5e6fbafdbb', label: 'Apple Watch SE 2' },
   { value: '67dc5397a9ee0a5e6fbafdbc', label: 'AirPods' },
   { value: '67dc539aa9ee0a5e6fbafdbd', label: 'EarPods' },
   { value: '67dc533ca9ee0a5e6fbafdb6', label: 'Keyboards' },
   { value: '67dc533aa9ee0a5e6fbafdb5', label: 'Apple Pencil' },
   { value: '67dc5336a9ee0a5e6fbafdb3', label: 'iPad' },
   { value: '82dc4708a9ee0a5e6fbafdac', label: 'iPad Air' },
   { value: '81c4708a9ee0a5e6fbafdabd', label: 'iPad Pro' },
   { value: '67dc5338a9ee0a5e6fbafdb4', label: 'iPad mini' },
   { value: '73dc43ee9b19c6773e9cec47', label: 'Display' },
   { value: '72dc43ee9b19c6773e9cec46', label: 'Mac Pro' },
   { value: '71dc43ee9b19c6773e9cec45', label: 'Mac Studio' },
   { value: '70dc43ee9b19c6773e9cec44', label: 'Mac mimi' },
   { value: '69dc43ee9b19c6773e9cec43', label: 'iMac' },
   { value: '67dc43ee9b19c6773e9cec41', label: 'MacBook Air' },
   { value: '68dc43ee9b19c6773e9cec42', label: 'MacBook Pro' },
];

type ProductOptionType = {
   value: string;
   category_id: string;
   label: string;
   image: string;
};

const mockProductsData: ProductOptionType[] = [
   {
      value: 'iphone-16-128gb',
      label: 'iphone-16-128gb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp',
   },
   {
      value: 'iphone-16-256gb',
      label: 'iphone-16-256gb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp',
   },
   {
      value: 'iphone-16-512gb',
      label: 'iphone-16-512gb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp',
   },
   {
      value: 'iphone-16-1tb',
      label: 'iphone-16-1tb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp',
   },
   {
      value: 'iphone-16-plus-128gb',
      label: 'iphone-16-plus-128gb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp',
   },
   {
      value: 'iphone-16-plus-256gb',
      label: 'iphone-16-plus-256gb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp',
   },
   {
      value: 'iphone-16-plus-512gb',
      label: 'iphone-16-plus-512gb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp',
   },
   {
      value: 'iphone-16-plus-1tb',
      label: 'iphone-16-plus-1tb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744811359/iphone-16-finish-select-202409-6-1inch-ultramarine_g6af08.webp',
   },
   {
      value: 'iphone-16e-128gb',
      label: 'iphone-16e-128gb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744701741/iphone-16e-finish-select-202502-white_g1coja.webp',
   },
   {
      value: 'iphone-16e-256gb',
      label: 'iphone-16e-256gb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744701741/iphone-16e-finish-select-202502-white_g1coja.webp',
   },
   {
      value: 'iphone-16e-512gb',
      label: 'iphone-16e-512gb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744701741/iphone-16e-finish-select-202502-white_g1coja.webp',
   },
   {
      value: 'iphone-16e-1tb',
      label: 'iphone-16e-1tb',
      category_id: '91dc470aa9ee0a5e6fbafdbc',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744701741/iphone-16e-finish-select-202502-white_g1coja.webp',
   },
   {
      value: 'iphone-15-128gb',
      label: 'iphone-15-128gb',
      category_id: '92dc470aa9ee0a5e6fbafdbd',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
   },
   {
      value: 'iphone-15-256gb',
      label: 'iphone-15-256gb',
      category_id: '92dc470aa9ee0a5e6fbafdbd',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
   },
   {
      value: 'iphone-15-512gb',
      label: 'iphone-15-512gb',
      category_id: '92dc470aa9ee0a5e6fbafdbd',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
   },
   {
      value: 'iphone-15-1tb',
      label: 'iphone-15-1tb',
      category_id: '92dc470aa9ee0a5e6fbafdbd',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
   },
   {
      value: 'iphone-15-plus-128gb',
      label: 'iphone-15-plus-128gb',
      category_id: '92dc470aa9ee0a5e6fbafdbd',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
   },
   {
      value: 'iphone-15-plus-256gb',
      label: 'iphone-15-plus-256gb',
      category_id: '92dc470aa9ee0a5e6fbafdbd',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
   },
   {
      value: 'iphone-15-plus-512gb',
      label: 'iphone-15-plus-512gb',
      category_id: '92dc470aa9ee0a5e6fbafdbd',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
   },
   {
      value: 'iphone-15-plus-1tb',
      label: 'iphone-15-plus-1tb',
      category_id: '92dc470aa9ee0a5e6fbafdbd',
      image: 'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp',
   },
];

type CategoryArrayFieldProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   name: ArrayPath<T>;
   className?: string;
};

function CategoryArrayField<T extends FieldValues>({
   form,
   name,
   className = '',
}: CategoryArrayFieldProps<T>) {
   const { fields, append, move, remove } = useFieldArray({
      name: name,
      control: form.control,
   });

   const rootErrorMessage = (form.formState.errors[name] as any)?.root?.message;

   // Watch the form values to get the latest selected categories
   const selectedData = form.watch(name as Path<T>) || [];

   return (
      <div className={cn('mb-5', className)}>
         <FormLabel>Apply promotion for categories</FormLabel>
         <FormDescription>
            Choose categories for the promotion. You can add multiple
            categories.
         </FormDescription>

         <div className="border rounded-xl mt-2 p-3 flex flex-col gap-3">
            {fields.map((field, index) => {
               // Filter options for this specific SelectField, excluding selections from other fields
               const optionsData = promotionCategories.filter((option) => {
                  return !selectedData.some(
                     (item: any, i: number) =>
                        i !== index && item.category_id === option.value,
                  );
               });

               const productData = mockProductsData.filter(
                  (option) =>
                     option.category_id ===
                     form.getValues(`${name}.${index}.category_id` as Path<T>),
               );

               return (
                  <div
                     className="flex gap-3 w-[inherit] bg-muted p-2 rounded-md"
                     key={field.id} // Use field.id for stable key
                  >
                     <div className="flex-1 grid grid-cols-4 gap-2">
                        <div className="col-span-2">
                           <SelectField
                              form={form}
                              name={
                                 `${name}.${index}.category_id` as ArrayPath<T>
                              }
                              label="Category"
                              description="Choose category for the promotion"
                              optionsData={optionsData}
                              onChange={(value: string) => {
                                 const category = mockData.find(
                                    (item) => item.category_id === value,
                                 );

                                 if (category) {
                                    form.setValue(
                                       `${name}.${index}.category_id` as Path<T>,
                                       category?.category_id as any,
                                    );

                                    form.setValue(
                                       `${name}.${index}.category_name` as Path<T>,
                                       category?.category_name as any,
                                    );

                                    form.setValue(
                                       `${name}.${index}.category_slug` as Path<T>,
                                       category?.category_slug as any,
                                    );
                                 }
                              }}
                           />
                        </div>

                        <div className="col-span-1">
                           <InputField
                              form={form}
                              type="percentage"
                              name={
                                 `${name}.${index}.discount_value` as ArrayPath<T>
                              }
                              label="Discount value %"
                              onChange={(value: string | number) => {
                                 form.setValue(
                                    `${name}.${index}.discount_value` as Path<T>,
                                    value as any,
                                 );
                              }}
                           />
                        </div>

                        <div className="col-span-1">
                           <SelectField
                              form={form}
                              name={
                                 `${name}.${index}.discount_type` as ArrayPath<T> // Fixed: Use discount_type instead of category_id
                              }
                              label="Discount type"
                              description="Choose discount type"
                              optionsData={[
                                 { value: 'PERCENTAGE', label: 'Percentage' },
                                 { value: 'FIXED', label: 'Fixed' },
                              ]}
                              defaultValue="PERCENTAGE"
                              disabled
                           />
                        </div>

                        <ContentWrapper className="col-span-4 bg-white">
                           <ProductArrayField
                              form={form}
                              index={index}
                              productData={productData}
                              name={
                                 `${name}.${index}.promotion_products` as ArrayPath<T>
                              }
                           />
                        </ContentWrapper>
                     </div>

                     <div className="flex gap-3">
                        <Button
                           type="button"
                           variant="outline"
                           size="icon"
                           className="size-8 shrink-0 translate-y-[100%]"
                           onClick={() => remove(index)}
                        >
                           <Trash
                              className="size-4 text-destructive"
                              aria-hidden="true"
                           />
                           <span className="sr-only">Remove</span>
                        </Button>
                     </div>
                  </div>
               );
            })}
            <div className="flex justify-center mt-2 border-2 border-dashed rounded-md cursor-pointer text-primary font-semibold">
               <button
                  type="button"
                  className="text-primary-500 hover:text-primary-700 text-sm p-3 w-full"
                  onClick={() => {
                     append({
                        category_id: '',
                        category_name: '',
                        discount_value: 0,
                        discount_type: 'PERCENTAGE',
                     } as unknown as FieldArray<T, ArrayPath<T>>);
                  }}
               >
                  + Add More
               </button>
            </div>
         </div>

         {rootErrorMessage && (
            <p className="text-destructive text-[12.8px] font-medium mt-1">
               {rootErrorMessage}
            </p>
         )}
      </div>
   );
}

export default CategoryArrayField;
