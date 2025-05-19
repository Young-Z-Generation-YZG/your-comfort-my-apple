import { FormDescription, FormLabel } from '@components/ui/form';
import {
   Sortable,
   SortableDragHandle,
   SortableItem,
} from '@components/dnd-ui/sortable';
import {
   type UseFormReturn,
   type FieldValues,
   type Path,
   useFieldArray,
   ArrayPath,
   FieldArray,
} from 'react-hook-form';
import { Button } from '@components/ui/button';
import { InputField } from '@components/input-field';
import { GradientPicker } from '@components/color-picker';
import { cn } from '../infrastructure/lib/utils';
import { GripVertical, Trash } from 'lucide-react';

type ColorArrayFieldProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   name: ArrayPath<T>;
   className?: string;
};

function ColorArrayField<T extends FieldValues>({
   form,
   name,
   className = '',
}: ColorArrayFieldProps<T>) {
   const { fields, append, move, remove } = useFieldArray({
      name: name,
      control: form.control,
   });

   const handleMove = (event: { activeIndex: number; overIndex: number }) => {
      const { activeIndex, overIndex } = event;
      // Move the item in the fields array
      move(activeIndex, overIndex);

      // Update model_order for all fields based on their new indices
      const updatedFields = form.getValues(name as Path<T>);
      updatedFields.forEach((field: any, index: number) => {
         form.setValue(
            `${name}.${index}.color_order` as Path<T>,
            index as any,
            {
               shouldDirty: true,
            },
         );
      });
   };

   const rootErrorMessage = (form.formState.errors[name] as any)?.root?.message;

   return (
      <div className={cn('mb-5', className)}>
         <FormLabel>Choose Colors</FormLabel>
         <FormDescription>
            Choose color options for the product model
         </FormDescription>

         <div className="border rounded-xl mt-2 p-3 flex flex-col gap-3">
            <Sortable
               value={fields}
               onMove={handleMove}
               overlay={
                  <div className="flex flex-col gap-1 h-full p-3 bg-primary/10 rounded-md">
                     <div className="w-20 h-8 rounded-sm bg-primary/20" />
                     <div className="w-full h-8 rounded-sm bg-primary/20 mb-2" />
                     <div className="w-full h-[80px] rounded-sm bg-primary/20" />
                  </div>
               }
            >
               {fields.map((field, index) => {
                  const colorHexErrorMessage =
                     (form.formState?.errors[name] as any)?.[index]?.color_hex
                        ?.message ?? '';

                  // default value
                  form.setValue(
                     `${name}.${index}.color_image` as Path<T>,
                     'empty' as unknown as any,
                  );

                  return (
                     <SortableItem key={field.id} value={field.id} asChild>
                        <div className="flex flex-row items-center bg-muted p-2 rounded-md justify-between gap-5">
                           <div className="w-full">
                              <InputField
                                 form={form}
                                 name={
                                    `${name}.${index}.color_name` as ArrayPath<T>
                                 }
                                 label="Color name"
                                 description="Enter product color description"
                              />

                              <div
                                 className=" preview flex flex-col justify-center p-5 items-start rounded !bg-cover !bg-center transition-all"
                                 style={{
                                    background:
                                       form.watch(
                                          `${name}.${index}.color_hex` as Path<T>,
                                       ) || '',
                                 }}
                              >
                                 <GradientPicker
                                    background={
                                       form.watch(
                                          `${name}.${index}.color_hex` as Path<T>,
                                       ) || ''
                                    }
                                    setBackground={(color) => {
                                       form.setValue(
                                          `${name}.${index}.color_hex` as Path<T>,
                                          color as any,
                                          {
                                             shouldValidate: true,
                                          },
                                       );
                                    }}
                                 />
                              </div>
                              <div className="mt-2">
                                 <FormDescription>
                                    Choose a color
                                 </FormDescription>

                                 {colorHexErrorMessage && (
                                    <p className="text-destructive text-xs font-medium mt-1">
                                       {colorHexErrorMessage}
                                    </p>
                                 )}
                              </div>
                           </div>
                           <div className="">
                              <div className="flex gap-3">
                                 <Button
                                    type="button"
                                    variant="outline"
                                    size="icon"
                                    className="size-8 translate-y-[100%]"
                                    onClick={() => remove(index)}
                                 >
                                    <Trash className="size-4 text-destructive" />
                                    <span className="sr-only">Remove</span>
                                 </Button>

                                 <SortableDragHandle
                                    type="button"
                                    variant="outline"
                                    size="icon"
                                    className="size-8 translate-y-[100%]"
                                 >
                                    <GripVertical
                                       className="size-4"
                                       aria-hidden="true"
                                    />
                                 </SortableDragHandle>
                              </div>
                           </div>
                        </div>
                     </SortableItem>
                  );
               })}
               <div className="flex justify-center mt-2 border-2 border-dashed rounded-md cursor-pointer text-primary font-semibold">
                  <button
                     type="button"
                     className="text-primary-500 hover:text-primary-700 text-sm p-3 w-full"
                     onClick={() => {
                        append({
                           color_name: '',
                           color_hex: '',
                           color_image: '',
                           color_order: fields.length,
                        } as unknown as FieldArray<T, ArrayPath<T>>);
                     }}
                  >
                     + Add More
                  </button>
               </div>
            </Sortable>
         </div>

         {rootErrorMessage && (
            <p className="text-destructive text-[12.8px] font-medium mt-1">
               {rootErrorMessage}
            </p>
         )}
      </div>
   );
}

export default ColorArrayField;
