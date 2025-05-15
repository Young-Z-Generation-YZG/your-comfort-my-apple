'use client';

import ContentWrapper from '@components/content-wrapper';
import { InputField } from '@components/input-field';
import { LoadingOverlay } from '@components/loading-overlay';
import { Button } from '@components/ui/button';
import { Fragment, useState } from 'react';
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

const storagesList: OptionType[] = [
   { value: '64', label: '64GB', icon: Cylinder },
   { value: '128', label: '128GB', icon: Cylinder },
   { value: '256', label: '256GB', icon: Cylinder },
   { value: '512', label: '512GB', icon: Cylinder },
   { value: '1024', label: '1024GB', icon: Cylinder },
   { value: '2048', label: '2048GB', icon: Cylinder },
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

   console.log('Error: ', form.formState.errors);
   console.log('Value: ', form.getValues());

   const onSubmit = async (data: TCreateModelSchema) => {
      console.log('TCreateModelSchema', data);
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
                           setSelectedStorages(value);
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
               <div>right</div>
            </ContentWrapper>
         </div>
      </Fragment>
   );
};

export default CreateModelPage;
