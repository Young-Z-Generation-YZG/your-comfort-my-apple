/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { Fragment, useEffect, useState } from "react";
import { useFieldArray, useForm } from "react-hook-form";
import { z } from "zod";
import CChooseImage from "~/components/custom-ui/c-choose-image";
import CInput from "~/components/custom-ui/c-input";
import CICombobox from "~/components/custom-ui/ci-combobox";
import { LoadingOverlay } from "~/components/loading-overlay";
import { Button } from "~/components/ui/button";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
} from "~/components/ui/form";
import { toast } from "~/hooks/use-toast";
import * as CatalogServices from "~/services/catalog.services";
import { IGetAllIphoneModelsResponse } from "~/types/api-types/catalog.type";
import Image from "next/image";
import CISelect from "~/components/custom-ui/ci-select";
import { GradientPicker } from "~/components/ui/color-picker";
import { Checkbox } from "~/components/ui/checkbox";

const imagesSchema = z.object({
  imageUrl: z.string().url("Invalid image URL"),
  imageId: z.string(),
  order: z.number().int().positive("Order must be a positive integer"),
});

const colorVariantSchema = z.object({
  name: z
    .string()
    .min(1, {
      message: "Color's name is required",
    })
    .max(14, {
      message: "Color name must be less than 14 characters",
    }),
  colorHash: z.string().min(1, "Color's hash is required"),
});

const CreateItemFormSchema = z.object({
  description: z.string().min(1, {
    message: "Description is required",
  }),
  productId: z.string().min(1, {
    message: "Product' model is required",
  }),
  model: z.string().min(1, {
    message: "Model is required",
  }),
  storage: z.string().min(1, {
    message: "Storage is required",
  }),
  color: colorVariantSchema,
  images: z
    .array(imagesSchema)
    .min(1, {
      message: "At least one image is required",
    })
    .refine(
      (images) => {
        const imageIds = new Set();
        for (const image of images) {
          if (imageIds.has(image.imageId)) {
            return false;
          }
          imageIds.add(image.imageId);
        }
        return true;
      },
      {
        message: "Image IDs must be unique",
      }
    ),
});

const ProductPage = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [selectedColor, setSelectedColor] = useState<string | null>(null);

  const [productModels, setProductModels] = useState<
    IGetAllIphoneModelsResponse[]
  >([]);

  const [selectedModel, setSelectedModel] =
    useState<IGetAllIphoneModelsResponse | null>(null);

  const defaultValues: Partial<z.infer<typeof CreateItemFormSchema>> = {
    description: "",
    productId: "",
    model: "",
    storage: "",
    color: { name: "", colorHash: "" },
    images: [{ imageUrl: "", imageId: "", order: 1 }],
    // images: [{ imageUrl: "", imageId: "", order: 1 }],
  };

  const form = useForm<z.infer<typeof CreateItemFormSchema>>({
    resolver: zodResolver(CreateItemFormSchema),
    defaultValues: defaultValues,
  });

  const {
    fields: images,
    append: appendImage,
    remove: removeImage,
  } = useFieldArray({
    name: "images",
    control: form.control,
  });

  const handleCreateItem = async (
    data: z.infer<typeof CreateItemFormSchema>
  ) => {
    toast({
      title: "You submitted the following values:",
      description: (
        <pre className="mt-2 w-[340px] rounded-md bg-slate-950 p-4">
          <code className="text-white">{JSON.stringify(data, null, 2)}</code>
        </pre>
      ),
    });
  };

  const handleChangeColor = (name: string, colorHash: string) => {
    setSelectedColor(colorHash);

    form.setValue("color.name", name);
    form.setValue("color.colorHash", colorHash);
  };

  useEffect(() => {
    const fetchData = async () => {
      const response = await CatalogServices.getAllProductAsync();

      await Promise.all(response);

      setProductModels(response);
    };

    setIsLoading(true);

    fetchData();

    setTimeout(() => {
      setIsLoading(false);
    }, 500);
  }, []);

  return (
    <Fragment>
      <LoadingOverlay isLoading={isLoading} />

      <div className="flex flex-row flex-1 gap-4 p-4 pt-0">
        <div className="basis-[70%] bg-muted/50 rounded-xl p-5">
          {/* <CICombobox /> */}

          {/* Form start */}
          <Form {...form}>
            <form onSubmit={form.handleSubmit(handleCreateItem)}>
              <CInput
                form={form as any}
                name="description"
                type="textarea"
                label="Description"
                description="Enter product model description"
              />

              {/* productId */}
              <div className="mt-5">
                <CICombobox
                  form={form as any}
                  name="productId"
                  items={productModels.map((model) => ({
                    label: model.name,
                    value: model.id,
                  }))}
                  className="flex-1"
                  label="General iPhone Models"
                  description="Select the model of the iPhone'item"
                  onSelected={(value) => {
                    setSelectedModel(
                      productModels.find((model) => model.id === value) || null
                    );
                  }}
                />
              </div>

              <div className="flex gap-4 mt-3">
                {/* Model */}
                <CISelect
                  form={form as any}
                  items={
                    selectedModel?.models?.map((model) => {
                      return model.name;
                    }) || []
                  }
                  name="model"
                  className="flex-1"
                  label="Model detail"
                  description="Select the model of the product"
                />

                {/* Storage */}
                <CISelect
                  form={form as any}
                  name="storage"
                  items={
                    selectedModel?.storages?.map((storage) => {
                      return storage.name;
                    }) || []
                  }
                  className="flex-1"
                  label="Storage"
                  description="Select the storage of the product"
                />
              </div>

              <div className="flex gap-4 mt-3">
                {/* Price */}
                {/* <CInput
                  form={form as any}
                  name="price"
                  label="Pricing"
                  type="number"
                  description="The price of the product"
                  className="flex-1"
                /> */}

                {/* Quantity in stock */}
                {/* <CInput
                  form={form as any}
                  name="quantityInStock"
                  label="Quantity in stock"
                  type="number"
                  description="The quantity of the product in stock"
                  className="flex-1"
                /> */}
              </div>

              {/* Color */}
              <div>
                <FormLabel>Choose Colors</FormLabel>
                <FormDescription>
                  Choose color options for the product model
                </FormDescription>

                <div className="w-full mt-3">
                  {selectedModel?.colors?.map((color, index) => {
                    return (
                      <div
                        key={index}
                        className="relative bg-slate-100 dark:bg-[#030712] p-2 rounded-md mt-2"
                      >
                        <FormField
                          control={form.control}
                          name="color"
                          render={({ field }) => {
                            return (
                              <FormItem className="absolute right-3 z-50 top-0 translate-y-[70%]">
                                <FormControl>
                                  <div className="flex justify-center items-center p-2 bg-slate-100 dark:bg-[#030712] rounded-md">
                                    <p className="text-sm min-w-24 font-medium first-letter:uppercase">
                                      {color.name}
                                    </p>
                                    <Checkbox
                                      checked={
                                        selectedColor === color.color_hash
                                      }
                                      onCheckedChange={() => {
                                        handleChangeColor(
                                          color.name,
                                          color.color_hash
                                        );
                                      }}
                                    />
                                  </div>
                                </FormControl>
                              </FormItem>
                            );
                          }}
                        />

                        <div
                          className={`preview h-[70px] cursor-pointer flex flex-col justify-center p-5 items-start rounded !bg-cover !bg-center transition-all hover:opacity-80`}
                          style={{ backgroundColor: `${color.color_hash}` }}
                          onClick={() => {
                            handleChangeColor(color.name, color.color_hash);
                          }}
                        >
                          <Button type="button" className="min-w-[100px]">
                            {color.color_hash}
                          </Button>
                        </div>
                      </div>
                    );
                  })}
                </div>
              </div>

              <div className="mt-5">
                <FormLabel>Choose Images</FormLabel>
                <FormDescription>
                  Choose images for the product model
                </FormDescription>

                {/* Images */}
                <div className="flex flex-col p-5 border rounded-xl mt-2 gap-10">
                  {images.map((image, index) => (
                    <div key={index} className="flex justify-between">
                      <div className="flex flex-col">
                        {form.watch(`images.${index}.imageUrl`) ? (
                          <Image
                            className="rounded-md w-[210px] object-contain"
                            src={form.watch(`images.${index}.imageUrl`)}
                            alt="iphone-12-pro-max"
                            width={1000}
                            height={1000}
                          />
                        ) : (
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            width="24"
                            height="24"
                            viewBox="0 0 24 24"
                            fill="none"
                            stroke="currentColor"
                            stroke-width="2"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                            className="w-52 h-52 text-slate-800 cursor-pointer"
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
                        )}

                        <CInput
                          form={form as any}
                          name={`images[${index}].imageUrl`}
                          type="text"
                          inputClassname="hidden"
                          disabled
                        />

                        <CChooseImage
                          form={form as any}
                          name="images"
                          index={index}
                          btnClassName="text-xs h-[30px] py-2 px-4"
                        />
                      </div>

                      <div className="">
                        <CInput
                          form={form as any}
                          name={`images[${index}].imageId`}
                          type="text"
                          label="Image id"
                          value={form.watch(`images.${index}.imageId`)}
                          labelClassName="text-xs"
                          disabled
                        />

                        <CInput
                          form={form as any}
                          name={`images.${index}.order`}
                          type="number"
                          label="Order number"
                          labelClassName="text-xs"
                          disabled
                        />

                        <Button
                          className="text-xs h-[32px] w-full"
                          onClick={() => removeImage(index)}
                        >
                          Remove
                        </Button>
                      </div>
                    </div>
                  ))}

                  <Button
                    type="button"
                    className="text-xs h-[32px] w-full"
                    onClick={() => {
                      appendImage({
                        imageUrl: "",
                        imageId: "",
                        order: images.length + 1,
                      });
                    }}
                  >
                    + Add image
                  </Button>
                </div>
              </div>

              <Button type="submit" className="mt-10 text-right">
                Create new model
              </Button>
            </form>
          </Form>
          {/* Form end */}
        </div>
        <div className="basis-[30%] bg-muted/50 rounded-xl"></div>
      </div>
    </Fragment>
  );
};

export default ProductPage;
