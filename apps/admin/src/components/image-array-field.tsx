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
import { cn } from '../infrastructure/lib/utils';
import { CldImage } from 'next-cloudinary';
import ChooseImageField from '@components/choose-image-field';
import { GripVertical, Trash } from 'lucide-react';

type ImageArrayFieldProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   name: ArrayPath<T>;
   className?: string;
};

function ImageArrayField<T extends FieldValues>({
   form,
   name,
   className = '',
}: ImageArrayFieldProps<T>) {
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
            `${name}.${index}.image_order` as Path<T>,
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
         <FormLabel>Choose Images</FormLabel>
         <FormDescription>Choose images for the product model</FormDescription>

         <div className="border rounded-xl mt-2 p-3 flex flex-col gap-3">
            <Sortable
               value={fields}
               onMove={handleMove}
               overlay={
                  <div className="flex flex-row h-full p-3 bg-primary/10 rounded-md">
                     <div className="flex flex-col gap-2 justify-between basis-2/3">
                        <div className="w-full h-full rounded-sm bg-primary/20" />
                        <div className="w-full h-8 rounded-sm bg-primary/20" />
                     </div>
                     <div className="flex">
                        <div></div>
                     </div>
                  </div>
               }
            >
               {fields.map((field, index) => {
                  // const colorHexErrorMessage =
                  //    (form.formState?.errors[name] as any)?.[index]?.color_hex
                  //       ?.message ?? '';

                  const imageUrl = form.watch(
                     `${name}.${index}.image_url` as unknown as Path<T>,
                  );

                  // default value
                  // form.setValue(
                  //     `${name}.${index}.color_image` as Path<T>,
                  //     "empty" as unknown as any,
                  // );

                  return (
                     <SortableItem key={field.id} value={field.id} asChild>
                        <div className="flex flex-row items-center bg-muted p-2 rounded-md justify-between gap-5">
                           <div className="basis-2/3">
                              <div className="flex flex-col gap-2">
                                 {imageUrl ? (
                                    <CldImage
                                       src={imageUrl}
                                       alt="Image"
                                       width={500}
                                       height={500}
                                       className="w-full min-h-[250px] max-h-[300px]"
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
                                          className="w-[50%] min-h-[250px] h-full text-center text-slate-800 cursor-pointer"
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

                                 <ChooseImageField
                                    form={form}
                                    name={`${name}` as Path<T>}
                                    index={index}
                                 />
                              </div>
                           </div>
                           <div className="self-start">
                              <InputField
                                 form={form}
                                 name={`${name}.${index}.image_id` as Path<T>}
                                 label="ID"
                                 disabled={true}
                              />

                              <div className="flex gap-3 justify-end">
                                 <Button
                                    type="button"
                                    variant="outline"
                                    size="icon"
                                    className="size-8"
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
                                    className="size-8"
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
                           image_id: '',
                           image_url: '',
                           image_name: 'name',
                           image_description: 'desc',
                           image_width: 0,
                           image_height: 0,
                           image_bytes: 0,
                           image_order: fields.length,
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

export default ImageArrayField;
