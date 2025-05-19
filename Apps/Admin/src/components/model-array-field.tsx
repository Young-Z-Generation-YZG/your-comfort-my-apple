import { FormDescription, FormLabel } from './ui/form';
import {
   Sortable,
   SortableDragHandle,
   SortableItem,
} from '~/src/components/dnd-ui/sortable';
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
import { GripVertical, Trash } from 'lucide-react';

type ModelArrayFieldProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   name: ArrayPath<T>;
   className?: string;
};

function ModelArrayField<T extends FieldValues>({
   form,
   name,
   className = '',
}: ModelArrayFieldProps<T>) {
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
            `${name}.${index}.model_order` as Path<T>,
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
         <FormLabel>Choose Models</FormLabel>
         <FormDescription>
            Choose storage options for the product model
         </FormDescription>

         <div className="border rounded-xl mt-2 p-3 flex flex-col gap-3">
            <Sortable
               value={fields}
               onMove={handleMove}
               overlay={
                  <div className="flex flex-col gap-1 h-full p-3 bg-primary/10 rounded-md">
                     <div className="w-20 h-8 rounded-sm bg-primary/20" />
                     <div className="w-full h-8 rounded-sm bg-primary/20" />
                  </div>
               }
            >
               {fields.map((field, index) => {
                  return (
                     <SortableItem key={field.id} value={field.id} asChild>
                        <div className="flex gap-3 w-[inherit] bg-muted p-2 rounded-md">
                           <div className="flex-1">
                              <InputField
                                 form={form}
                                 name={
                                    `${name}.${index}.model_name` as ArrayPath<T>
                                 }
                                 label="Model name"
                                 description="Enter product model description"
                              />
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
                                 {/* <FaRegTrashAlt
                                                    className="size-4 text-destructive"
                                                    aria-hidden="true"
                                                /> */}
                                 <span className="sr-only">Remove</span>
                              </Button>

                              <SortableDragHandle
                                 type="button"
                                 variant="outline"
                                 size="icon"
                                 className="size-8 shrink-0 translate-y-[100%]"
                              >
                                 <GripVertical
                                    className="size-4"
                                    aria-hidden="true"
                                 />
                                 {/* <RxDragHandleDots2
                                    className="size-4"
                                    aria-hidden="true"
                                 /> */}
                              </SortableDragHandle>
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
                           model_name: '',
                           model_order: fields.length,
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

export default ModelArrayField;
