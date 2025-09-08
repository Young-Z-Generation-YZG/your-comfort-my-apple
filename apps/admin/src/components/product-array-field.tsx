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
import { CldImage } from 'next-cloudinary';

type ProductOptionType = {
   value: string;
   label: string;
   image: string;
};

type ProductArrayFieldProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   name: ArrayPath<T>;
   productData: ProductOptionType[];
   index: number;
   className?: string;
};

function ProductArrayField<T extends FieldValues>({
   form,
   name,
   productData = [],
   index: categoryIndex,
   className = '',
}: ProductArrayFieldProps<T>) {
   const { fields, append, move, remove } = useFieldArray({
      name: name,
      control: form.control,
   });

   const rootErrorMessage = (form.formState.errors[name] as any)?.root?.message;

   // Watch the form values to get the latest selected categories
   const selectedData = form.watch(name as Path<T>) || [];

   const isSelectedCategory = form.getValues(
      `promotion_categories.${categoryIndex}.category_id` as Path<T>,
   );

   return (
      <div className={cn('mb-5', className)}>
         <FormLabel>Apply promotion for Products</FormLabel>
         <FormDescription>
            Choose products for the promotion. You can add multiple products to
            the promotion. Each product can have its own discount value and
            discount type.
         </FormDescription>

         <div className="border rounded-xl mt-2 p-3 flex flex-col gap-3">
            {fields.map((field, index) => {
               // Filter options for this specific SelectField, excluding selections from other fields
               const optionsData = productData.filter((option) => {
                  return !selectedData.some(
                     (item: any, i: number) =>
                        i !== index && item.category_id === option.value,
                  );
               });

               const image = form.getValues(
                  `${name}.${index}.product_image` as Path<T>,
               );

               return (
                  <div
                     className="flex gap-3 w-[inherit] bg-muted p-2 rounded-md"
                     key={field.id} // Use field.id for stable key
                  >
                     <div className="flex-1 flex flex-row gap-2">
                        <div className="flex-1">
                           <SelectField
                              form={form}
                              name={
                                 `${name}.${index}.product_slug` as ArrayPath<T>
                              }
                              label="Promotion Product In Category"
                              description="Choose product for the promotion in this category"
                              optionsData={optionsData}
                              onChange={(value: string) => {
                                 const productDetail = productData.find(
                                    (item) => item.value === value,
                                 );

                                 if (productDetail) {
                                    form.setValue(
                                       `${name}.${index}.product_image` as Path<T>,
                                       productDetail?.image as any,
                                    );
                                 }
                              }}
                           />
                           <div className="w-full">
                              {image ? (
                                 <CldImage
                                    width={300}
                                    height={200}
                                    className=""
                                    src={image || ''}
                                    alt="iPhone Model"
                                 />
                              ) : (
                                 <div className="flex justify-center max-h-[300px]">
                                    <svg
                                       xmlns="http://www.w3.org/2000/svg"
                                       width="24"
                                       height="24"
                                       viewBox="0 0 24 24"
                                       fill="none"
                                       stroke="currentColor"
                                       strokeWidth="2"
                                       strokeLinecap="round"
                                       strokeLinejoin="round"
                                       className="min-h-[200px] h-full text-center text-slate-800"
                                    >
                                       <rect
                                          width="18"
                                          height="18"
                                          x="3"
                                          y="3"
                                          rx="2"
                                          ry="2"
                                       />
                                       <circle cx="9" cy="9" r="2" />
                                       <path d="m21 15-3.086-3.086a2 2 0 0 0-2.828 0L6 21" />
                                    </svg>
                                 </div>
                              )}
                           </div>
                        </div>

                        <div className="basis-1/4">
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

                        <div className="basis-1/5">
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
                           />
                        </div>
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
            {isSelectedCategory && (
               <div className="flex justify-center mt-2 border-2 border-dashed rounded-md cursor-pointer text-primary font-semibold">
                  <button
                     type="button"
                     className="text-primary-500 hover:text-primary-700 text-sm p-3 w-full"
                     onClick={() => {
                        if (
                           form.getValues('promotion_categories' as Path<T>)
                              .length === 0
                        ) {
                        }

                        append({
                           product_slug: '',
                           product_image: '',
                           discount_value: 0,
                           discount_type: 'PERCENTAGE',
                        } as unknown as FieldArray<T, ArrayPath<T>>);
                     }}
                  >
                     + Add More
                  </button>
               </div>
            )}
         </div>

         {rootErrorMessage && (
            <p className="text-destructive text-[12.8px] font-medium mt-1">
               {rootErrorMessage}
            </p>
         )}
      </div>
   );
}

export default ProductArrayField;
