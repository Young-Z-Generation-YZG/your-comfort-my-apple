import { Fragment, useEffect, useRef, useState } from 'react';
import { FormControl, FormField, FormItem } from './ui/form';

import { type UseFormReturn, type FieldValues, Path } from 'react-hook-form';
import { Button } from './ui/button';
import {
   Sheet,
   SheetClose,
   SheetContent,
   SheetDescription,
   SheetFooter,
   SheetHeader,
   SheetTitle,
   SheetTrigger,
} from './ui/sheet';
import { CldImage } from 'next-cloudinary';
import { ScrollArea } from './ui/scroll-area';
import { ICloudinaryImage } from '~/src/domain/interfaces/common/ICloudinaryImage';
import { Checkbox } from './ui/checkbox';
import UploadImageDialog from './upload-image-dialog';
import { LoadingOverlay } from '@components/loading-overlay';
import useUploadService from '~/src/hooks/api/use-upload-service';

type ChooseImageFieldProps<T extends FieldValues> = {
   form: UseFormReturn<T>;
   name: Path<T>;
   index: number;
};

function ChooseImageField<T extends FieldValues>({
   form,
   name,
   index,
}: ChooseImageFieldProps<T>) {
   const [selectedItem, setSelectedItem] = useState<string | null>(null);
   const [images, setImages] = useState<ICloudinaryImage[]>([]);
   const uploadBtnRef = useRef<HTMLButtonElement>(
      null,
   ) as React.RefObject<HTMLButtonElement>;

   const handleCheckItem = (imageId: string, imageUrl: string) => {
      if (selectedItem === imageId) {
         setSelectedItem(null);

         form.setValue(`${name}.${index}.image_id` as Path<T>, null as any);
         form.setValue(`${name}.${index}.image_url` as Path<T>, null as any);

         return;
      }

      setSelectedItem(imageId);

      form.setValue(`${name}.${index}.image_id` as Path<T>, imageId as any);
      form.setValue(`${name}.${index}.image_url` as Path<T>, imageUrl as any);
   };

   // const rootErrorMessage = (form.formState.errors[name] as any)?.root?.message;

   const { getImagesAsync, getImagesState, isLoading } = useUploadService();

   useEffect(() => {
      getImagesAsync();
   }, [getImagesAsync]);

   useEffect(() => {
      if (getImagesState.data) {
         setImages(getImagesState.data);
      }
   }, [getImagesState.data]);

   return (
      <Fragment>
         <LoadingOverlay isLoading={isLoading} fullScreen={true} />
         <Sheet>
            <SheetTrigger asChild>
               <Button className={''}>Choose image</Button>
            </SheetTrigger>
            <SheetContent className="w-[70%] sm:max-w-2xl">
               <SheetHeader>
                  <SheetTitle>Choose image</SheetTitle>
                  <SheetDescription>
                     Select an item to display in the sidebar.
                  </SheetDescription>
               </SheetHeader>
               <div className="py-4 max-h-[82vh]">
                  <FormField
                     control={form.control}
                     name={name}
                     render={() => (
                        <ScrollArea className="h-[80vh] w-full rounded-md border">
                           <FormItem>
                              <div className="grid grid-cols-2 p-3 gap-2">
                                 <div className=" ml-3 border-2 border-dashed rounded-md">
                                    <button
                                       className="w-full h-full flex justify-center items-center"
                                       onClick={() => {
                                          uploadBtnRef.current?.click();
                                       }}
                                    >
                                       {/* <BsCloudPlus className="w-28 h-28" /> */}
                                    </button>

                                    {uploadBtnRef && (
                                       <UploadImageDialog
                                          triggerBtnRef={uploadBtnRef}
                                       />
                                    )}
                                 </div>
                                 {images.map((image) => (
                                    <FormItem
                                       key={image.public_id}
                                       className="relative flex flex-row items-start space-x-3 space-y-0"
                                    >
                                       <FormControl>
                                          <Checkbox
                                             className="absolute right-2 top-2"
                                             checked={
                                                selectedItem === image.public_id
                                             }
                                             onCheckedChange={() => {
                                                handleCheckItem(
                                                   image.public_id,
                                                   image.secure_url,
                                                );
                                             }}
                                          />
                                       </FormControl>
                                       <div className="p-2 bg-slate-100 rounded-md flex h-full">
                                          <CldImage
                                             src={image.secure_url}
                                             alt="Image"
                                             width={1000}
                                             height={1000}
                                             className=""
                                          />
                                       </div>
                                    </FormItem>
                                 ))}
                              </div>
                           </FormItem>
                        </ScrollArea>
                     )}
                  />
               </div>
               <SheetFooter>
                  <SheetClose asChild>
                     <Button type="button" className="mt-3">
                        Close
                     </Button>
                  </SheetClose>
               </SheetFooter>
            </SheetContent>
         </Sheet>
      </Fragment>
   );
}

export default ChooseImageField;
