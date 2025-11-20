'use client';

import ContentWrapper from '@components/content-wrapper';
import { InputField } from '@components/input-field';
import { LoadingOverlay } from '@components/loading-overlay';
import { Button } from '@components/ui/button';
import { Fragment, useEffect, useState } from 'react';
import { FormProvider, useForm } from 'react-hook-form';
import {
   CreateModelResolver,
   TCreateModelSchema,
} from '~/src/domain/schemas/catalog.schema';
import ModelArrayField from '@components/model-array-field';
import ColorArrayField from '@components/color-array-field';
import ImageArrayField from '@components/image-array-field';
import { Cylinder } from 'lucide-react';
import {
   MultipleSelectField,
   OptionType,
} from '@components/multiple-select-field';
import { CldImage } from 'next-cloudinary';
import { cn } from '~/src/infrastructure/lib/utils';

import {
   Carousel,
   CarouselContent,
   CarouselItem,
   CarouselNext,
   CarouselPrevious,
} from '@components/ui/carousel';
import { SelectField } from '@components/select-field';

const storagesList: OptionType[] = [
   { value: '64', label: '64GB', icon: Cylinder },
   { value: '128', label: '128GB', icon: Cylinder },
   { value: '256', label: '256GB', icon: Cylinder },
   { value: '512', label: '512GB', icon: Cylinder },
   { value: '1024', label: '1024GB', icon: Cylinder },
   { value: '2048', label: '2048GB', icon: Cylinder },
];

const mockColors = [
   {
      color_name: 'ultramarine',
      color_hex: '#9AADF6',
      color_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-ultramarine-202409',
      color_order: 0,
   },
   {
      color_name: 'teal',
      color_hex: '#B0D4D2',
      color_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-teal-202409',
      color_order: 1,
   },
   {
      color_name: 'pink',
      color_hex: '#F2ADDA',
      color_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-pink-202409',
      color_order: 2,
   },
   {
      color_name: 'white',
      color_hex: '#FAFAFA',
      color_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-white-202409',
      color_order: 3,
   },
   {
      color_name: 'black',
      color_hex: '#3C4042',
      color_image:
         'https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/finish-black-202409',
      color_order: 4,
   },
];

const mockStorages = [
   {
      storage_name: '128GB',
      storage_value: 128,
   },
   {
      storage_name: '256GB',
      storage_value: 256,
   },
   {
      storage_name: '512GB',
      storage_value: 512,
   },
   {
      storage_name: '1TB',
      storage_value: 1024,
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

const CreateModelPage = () => {
   const [selectedStorages, setSelectedStorages] = useState<number[]>([]);

   const form = useForm<TCreateModelSchema>({
      resolver: CreateModelResolver,
      defaultValues: {
         name: '',
         models: [
            {
               model_name: '',
               model_order: 0,
            },
         ],
         colors: [
            {
               color_name: '',
               color_hex: '',
               color_image: '',
               color_order: 0,
            },
         ],
         storages: [],
         description: '',
         description_images: [
            {
               image_id: '',
               image_url: '',
               image_name: 'webp',
               image_description: 'display first',
               image_width: 0,
               image_height: 0,
               image_bytes: 0,
               image_order: 0,
            },
         ],
      },
   });

   const onSubmit = async (data: TCreateModelSchema) => {};

   const renderModels = () => {
      const models = form.watch('models').sort((a, b) => {
         return a.model_order - b.model_order;
      });

      return (
         <div className="flex flex-col gap-1 mt-2">
            <p className="text-sm font-medium mb-1">Models:</p>
            {models.map((model, index) => {
               return (
                  <span className="text-sm" key={index}>
                     <p className="bg-secondary px-2 py-1 w-fit rounded-full">
                        {model.model_name}
                     </p>
                  </span>
               );
            })}
         </div>
      );
   };

   const renderStorages = () => {
      const storages = form.watch('storages').sort((a, b) => {
         return a - b;
      });

      return (
         <div className="flex flex-col gap-2 mt-2">
            <p className="text-sm font-medium mb-1">Storages:</p>
            <div className="flex flex-row flex-wrap gap-2">
               {storages.map((storage, index) => {
                  return (
                     <span
                        key={index}
                        className={cn(
                           'uppercase select-none text-xs font-medium min-w-[70px] py-1 border rounded-full text-center',
                        )}
                     >
                        {storage}GB
                     </span>
                  );
               })}
            </div>
         </div>
      );
   };

   const renderColors = () => {
      const colors = form.getValues('colors').sort((a, b) => {
         return a.color_order - b.color_order;
      });

      return (
         <div className="mt-5">
            <span className="flex flex-row gap-2">
               <p className="first-letter:uppercase text-sm font-medium">
                  colors:
               </p>
               <p className="first-letter:uppercase text-sm">ultramarine</p>
            </span>
            <div className="flex flex-row gap-2 mt-2">
               {colors.map((color, index) => {
                  return (
                     <div
                        key={index}
                        className={cn(
                           'h-[30px] w-[30px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out',
                        )}
                        style={{
                           backgroundColor: `${color.color_hex}`,
                        }}
                     />
                  );
               })}
            </div>
         </div>
      );
   };

   const renderImages = () => {
      const images = form.getValues('description_images').sort((a, b) => {
         return a.image_order - b.image_order;
      });

      return images.map((image, index) => {
         return (
            <Fragment key={index}>
               {image.image_url && (
                  <CarouselItem>
                     <CldImage
                        key={index}
                        className="rounded-lg object-cover w-full md:h-[350px]"
                        src={image.image_url || ''}
                        alt="Product Image"
                        width={1000}
                        height={1000}
                     />
                  </CarouselItem>
               )}

               {!image.image_url && (
                  <div className="w-full flex justify-center max-h-[300px]">
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
            </Fragment>
         );
      });
   };

   return (
      <Fragment>
         <LoadingOverlay fullScreen={true} isLoading={false} />
         <div className="flex flex-row flex-1 gap-4 p-4 pt-0">
            <ContentWrapper className="flex-1">
               <FormProvider {...form}>
                  <form
                     onSubmit={form.handleSubmit(onSubmit)}
                     onKeyDown={(e) => {
                        if (e.key === 'Enter' && !e.defaultPrevented) {
                           e.preventDefault();
                           form.handleSubmit(onSubmit)();
                        }
                     }}
                  >
                     <InputField
                        form={form}
                        name="name"
                        label="General product model title"
                        description="Enter general product model each product item"
                     />

                     <InputField
                        form={form}
                        type="textarea"
                        name="description"
                        label="Description"
                        description="Enter product model description"
                     />

                     <SelectField
                        form={form}
                        name="category_id"
                        label="Category"
                        description="Select the category of this product model"
                        optionsData={promotionCategories}
                     />

                     <ModelArrayField<TCreateModelSchema>
                        form={form}
                        name="models"
                     />

                     <MultipleSelectField
                        form={form}
                        name="storages"
                        label="Storages"
                        optionsData={storagesList}
                        onValueChange={(value) => {
                           const storages = value.map((v) =>
                              parseInt(v.toString()),
                           );
                           setSelectedStorages(storages);
                           form.setValue(
                              'storages',
                              storages.sort((a, b) => a - b),
                              {
                                 shouldDirty: true,
                                 shouldTouch: true,
                              },
                           );
                        }}
                        placeholder="Select storages..."
                     />

                     <ColorArrayField<TCreateModelSchema>
                        form={form}
                        name="colors"
                     />

                     <ImageArrayField<TCreateModelSchema>
                        form={form}
                        name="description_images"
                     />

                     <div className="grid grid-cols-2 gap-4"></div>
                  </form>
               </FormProvider>

               <div className="flex flex-row gap-2 justify-end mt-5">
                  <Button onClick={() => form.reset()} variant={'outline'}>
                     Reset
                  </Button>
                  <Button onClick={() => form.handleSubmit(onSubmit)()}>
                     Create New Model
                  </Button>
               </div>
            </ContentWrapper>

            <ContentWrapper className="w-1/3">
               <div>
                  <h2 className="text-lg font-semibold">Preview</h2>
                  <p className="text-sm text-muted-foreground">
                     Preview of the product model
                  </p>

                  <div>
                     <div className="flex flex-col gap-2 mt-4 overflow-hidden">
                        <Carousel>
                           <CarouselContent>{renderImages()}</CarouselContent>

                           <CarouselPrevious className="left-[1rem]" />
                           <CarouselNext className="right-[1rem]" />
                        </Carousel>
                     </div>

                     <p className="font-SFProText font-medium text-2xl mt-5">
                        {form.watch('name') || 'Product Model Name'}
                     </p>

                     {form.getValues('models').length > 0 && renderModels()}

                     {form.getValues('colors').length > 0 && renderColors()}

                     {form.getValues('storages').length > 0 && renderStorages()}
                  </div>
               </div>
            </ContentWrapper>
         </div>
      </Fragment>
   );
};

export default CreateModelPage;
