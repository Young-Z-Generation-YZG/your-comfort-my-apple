/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
"use client";

import { Form, FormDescription, FormLabel } from "~/components/ui/form";
import { z } from "zod";
import { set, useFieldArray, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import CInput from "~/components/custom-ui/c-input";
import { Cylinder } from "lucide-react";
import { Fragment, useEffect, useRef, useState } from "react";
import CMultiSelect from "~/components/custom-ui/c-multi-select";
import { GradientPicker } from "~/components/ui/color-picker";
import { Button } from "~/components/ui/button";
import { toast } from "~/hooks/use-toast";
import Image from "next/image";
import CChooseImage from "~/components/custom-ui/c-choose-image";
import * as CatalogServices from "~/services/catalog.services";
import { ICreateNewIPhoneModelPayload } from "~/types/api-types/catalog.type";
import { LoadingOverlay } from "~/components/loading-overlay";

const colorVariantSchema = z.object({
  name: z
    .string()
    .min(3, {
      message: "Color name must be at least 3 characters",
    })
    .max(14, {
      message: "Color name must be less than 14 characters",
    }),
  colorHash: z.string().min(1, "Color hash is required"),
  order: z.number().int().positive("Order must be a positive integer"),
});

const imagesSchema = z.object({
  imageUrl: z.string().url("Invalid image URL"),
  imageId: z.string(),
  order: z.number().int().positive("Order must be a positive integer"),
});

const CreateModelFormSchema = z.object({
  name: z.string().min(6, {
    message: "Name must be at least 6 characters",
  }),
  description: z.string().min(1, {
    message: "Description is required",
  }),
  models: z
    .array(
      z.object({
        name: z.string().min(3, {
          message: "Model name must be at least 3 characters",
        }),
        order: z.number().int().positive("Order must be a positive integer"),
      })
    )
    .min(1, {
      message: "At least one model is required",
    }),
  storages: z
    .array(z.string())
    .min(1, {
      message: "At least one storage is required",
    })
    .refine(
      (val) => val.every((v) => storagesList.some((s) => s.value === v)),
      {
        message: "Invalid storage value",
      }
    ),
  colors: z
    .array(colorVariantSchema)
    .min(1, {
      message: "At least one color is required",
    })
    .refine(
      (colors) => {
        const names = new Set();
        const hashes = new Set();
        for (const color of colors) {
          if (names.has(color.name) || hashes.has(color.colorHash)) {
            return false;
          }
          names.add(color.name);
          hashes.add(color.colorHash);
        }
        return true;
      },
      {
        message: "Color names and color hashes must be unique",
      }
    ),
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

const storagesList = [
  { value: "64", label: "64GB", icon: Cylinder },
  { value: "128", label: "128GB", icon: Cylinder },
  { value: "256", label: "256GB", icon: Cylinder },
  { value: "512", label: "512GB", icon: Cylinder },
  { value: "1024", label: "1024GB", icon: Cylinder },
  { value: "2048", label: "2048GB", icon: Cylinder },
];

const imagesList = [
  {
    imageUrl:
      "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-1-202409_GEO_US?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127697",
    imageId: "imageId1",
  },
  {
    imageUrl:
      "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-2-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127558",
    imageId: "imageId2",
  },
  {
    imageUrl:
      "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-3-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127642",
    imageId: "imageId3",
  },
  {
    imageUrl:
      "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-4-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127808",
    imageId: "imageId4",
  },
  {
    imageUrl:
      "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-5-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127726",
    imageId: "imageId5",
  },
  {
    imageUrl:
      "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-1-202409_GEO_US?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127697",
    imageId: "imageId6",
  },
];

const IphoneModelPage = () => {
  const [selectedStorages, setSelectedStorages] = useState<string[]>([
    "64",
    "128",
  ]);

  const [isLoading, setIsLoading] = useState(false);

  // Default value
  const defaultValues: Partial<z.infer<typeof CreateModelFormSchema>> = {
    name: "",
    description: "",
    models: [{ name: "", order: 1 }],
    storages: selectedStorages,
    colors: [{ name: "", colorHash: "", order: 1 }],
    images: [{ imageUrl: "", imageId: "", order: 1 }],
  };

  const form = useForm<z.infer<typeof CreateModelFormSchema>>({
    resolver: zodResolver(CreateModelFormSchema),
    defaultValues: defaultValues,
  });

  const {
    fields: colors,
    append: appendColor,
    remove: removeColor,
  } = useFieldArray({
    name: "colors",
    control: form.control,
  });

  const {
    fields: models,
    append: appendModel,
    remove: removeModel,
  } = useFieldArray({
    name: "models",
    control: form.control,
  });

  const {
    fields: images,
    append: appendImage,
    remove: removeImage,
  } = useFieldArray({
    name: "images",
    control: form.control,
  });

  useEffect(() => {
    form.setValue("storages", selectedStorages, { shouldValidate: true });
  }, [selectedStorages, form]);

  // console.log("message", form.formState.errors.colors);
  // console.log("colors", form.watch(`colors.${1}.colorHash`) ?? "#0000cd");

  const handleCreateIphoneModel = async (
    data: z.infer<typeof CreateModelFormSchema>
  ) => {
    setIsLoading(true);

    toast({
      title: "You submitted the following values:",
      description: (
        <pre className="mt-2 w-[340px] rounded-md bg-slate-950 p-4">
          <code className="text-white">{JSON.stringify(data, null, 2)}</code>
        </pre>
      ),
    });

    // const examplePayload: ICreateNewIPhoneModelPayload = {
    //   name: "iPhone 16",
    //   description: "iPhone 16 description",
    //   models: [
    //     {
    //       name: "iPhone 16",
    //       order: 1,
    //     },
    //     {
    //       name: "iPhone 16 Plus",
    //       order: 2,
    //     },
    //   ],
    //   colors: [
    //     {
    //       name: "ultramarine",
    //       color_hash: "#a1aff5",
    //       order: 1,
    //     },
    //     {
    //       name: "teal",
    //       color_hash: "#bad7d6",
    //       order: 2,
    //     },
    //     {
    //       name: "pink",
    //       color_hash: "#e9aed6",
    //       order: 3,
    //     },
    //     {
    //       name: "white",
    //       color_hash: "#fafafa",
    //       order: 4,
    //     },
    //     {
    //       name: "black",
    //       color_hash: "#474a4d",
    //       order: 5,
    //     },
    //   ],
    //   storages: [128, 256, 512],
    //   images: [
    //     {
    //       url: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-1-202409_GEO_US?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127697",
    //       id: "image_id1",
    //       order: 1,
    //     },
    //     {
    //       url: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-2-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127558",
    //       id: "image_id2",
    //       order: 2,
    //     },
    //     {
    //       url: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-3-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127642",
    //       id: "image_id3",
    //       order: 3,
    //     },
    //     {
    //       url: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-4-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127808",
    //       id: "image_id4",
    //       order: 4,
    //     },
    //     {
    //       url: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-5-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127726",
    //       id: "image_id5",
    //       order: 5,
    //     },
    //     {
    //       url: "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-6-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127802",
    //       id: "image_id6",
    //       order: 6,
    //     },
    //   ],
    //   categoryId: "672cdaed4e67692dff64a47c",
    //   promotionId: "672cdaed4e67692dff64a47c",
    // };

    const payload: ICreateNewIPhoneModelPayload = {
      name: data.name,
      description: data.description,
      models: data.models,
      storages: data.storages.map((s) => parseInt(s)),
      colors: data.colors.map((color) => {
        return {
          name: color.name,
          color_hash: color.colorHash,
          order: color.order,
        };
      }),
      images: data.images.map((image) => {
        return {
          id: image.imageId,
          url: image.imageUrl,
          order: image.order,
        };
      }),
      categoryId: "672cdaed4e67692dff64a47c",
      promotionId: "672cdaed4e67692dff64a47c",
    };

    const response = await CatalogServices.createIphoneModelAsync(payload);

    if (response) {
      toast({
        title: "Success",
        description: "New model has been created",
        className: "bg-green-500 text-white",
      });
    } else {
      toast({
        variant: "destructive",
        title: "Uh oh! Something went wrong.",
        description: "There was a problem with your request.",
        className: "bg-red-500",
      });
    }

    setTimeout(() => {
      setIsLoading(false);
    }, 500);
  };

  return (
    <Fragment>
      <LoadingOverlay isLoading={isLoading} />
      <div className="flex flex-row flex-1 gap-4 p-4 pt-0">
        <div className="basis-[70%] bg-muted/50 rounded-xl p-3">
          <Form {...form}>
            <form onSubmit={form.handleSubmit(handleCreateIphoneModel)}>
              {/* name */}
              <CInput
                form={form as any}
                name="name"
                type="text"
                label="General product model title"
                description="Enter general product model each product item"
              />

              {/* Description */}
              <CInput
                form={form as any}
                name="description"
                type="textarea"
                label="Description"
                description="Enter product model description"
              />

              {/* Models */}
              <div className="mt-3">
                <FormLabel>Choose Models</FormLabel>
                <FormDescription>
                  Choose storage options for the product model
                </FormDescription>

                <div className="border rounded-xl mt-2 p-3">
                  <div className="flex flex-col">
                    {models.map((model, index) => (
                      <div
                        key={index}
                        className="flex items-center justify-between"
                      >
                        <CInput
                          form={form as any}
                          name={`models[${index}].name`}
                          type="text"
                          label="Model name"
                          labelClassName="text-xs"
                          inputClassname="min-w-[254px]"
                        />

                        <div className="flex gap-3 ">
                          <CInput
                            form={form as any}
                            name={`models.${index}.order`}
                            type="number"
                            label="Order number"
                            labelClassName="text-xs"
                            disabled
                          />
                          <Button
                            className="text-xs h-[35px] self-end mb-2"
                            onClick={() => {
                              removeModel(index);
                            }}
                          >
                            Remove
                          </Button>
                        </div>
                      </div>
                    ))}

                    <Button
                      type="button"
                      className="text-xs h-[32px] mt-5"
                      onClick={() =>
                        appendModel({
                          name: "",
                          order: models.length + 1,
                        })
                      }
                    >
                      + Add model
                    </Button>
                  </div>
                </div>
              </div>

              {/* Storages */}
              <div className="gap-4 py-5">
                <CMultiSelect
                  label="Select Storages"
                  description="Select storage options for the product"
                  options={storagesList}
                  onValueChange={(values) => {
                    setSelectedStorages(values);
                    form.setValue("storages", values, {
                      shouldValidate: true,
                    });
                  }}
                  defaultValue={selectedStorages}
                />
                {form.formState.errors.storages && (
                  <p className="text-destructive text-sm mt-1">
                    {form.formState.errors.storages.message}
                  </p>
                )}
              </div>

              {/* Colors picker */}
              <div>
                <FormLabel>Choose Colors</FormLabel>
                <FormDescription>
                  Choose color options for the product model
                </FormDescription>
                <div className="flex flex-col gap-5 p-5 border rounded-xl mt-2">
                  {colors.map((color, index) => (
                    <div
                      key={color.id}
                      className="flex flex-row items-center justify-between gap-10 mt-5"
                    >
                      <div className="w-full">
                        <div
                          className=" preview flex flex-col justify-center p-5 items-start rounded !bg-cover !bg-center transition-all"
                          style={{
                            background: form.watch(`colors.${index}.colorHash`),
                          }}
                        >
                          <GradientPicker
                            background={form.watch(`colors.${index}.colorHash`)}
                            setBackground={(color) => {
                              form.setValue(
                                `colors.${index}.colorHash`,
                                color,
                                {
                                  shouldValidate: true,
                                }
                              );
                            }}
                          />
                        </div>
                        <div className="mt-2">
                          <FormDescription>Choose a color</FormDescription>
                          {form.formState.errors.colors?.[index]?.colorHash
                            ?.message && (
                            <p className="text-destructive text-sm mt-1">
                              {
                                form.formState.errors.colors?.[index]?.colorHash
                                  ?.message
                              }
                            </p>
                          )}
                        </div>
                      </div>
                      <div className="flex flex-col basis-[30%]">
                        <CInput
                          form={form as any}
                          name={`colors[${index}].name`}
                          type="text"
                          label="Color name"
                          labelClassName="text-xs"
                        />
                        <CInput
                          form={form as any}
                          name={`colors.${index}.order`}
                          type="number"
                          label="Order number"
                          labelClassName="text-xs"
                          disabled
                        />
                        <Button
                          className="text-xs h-[32px] w-full"
                          onClick={() => removeColor(index)}
                        >
                          Remove
                        </Button>
                      </div>
                    </div>
                  ))}
                  {form.formState.errors.colors?.root && (
                    <p className="text-destructive text-sm mt-1">
                      {form.formState.errors.colors?.root.message}
                    </p>
                  )}
                  <FormDescription>Choose at least 1 color</FormDescription>
                  <Button
                    type="button"
                    className="text-xs h-[32px]"
                    onClick={() =>
                      appendColor({
                        name: "",
                        colorHash: "",
                        order: colors.length + 1,
                      })
                    }
                  >
                    + Add colors
                  </Button>
                </div>
              </div>

              {/* Images */}
              <div className="mt-5">
                <FormLabel>Choose Images</FormLabel>
                <FormDescription>
                  Choose images for the product model
                </FormDescription>

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

                {/* <div className="grid grid-cols-3 p-5 border rounded-xl mt-2 gap-3">
                  {imagesList.map((image, index) => {
                    return (
                      <CCheckbox
                        key={index}
                        form={form as any}
                        name="images"
                        items={imagesList.map((image) => ({
                          secure_url: image.imageUrl,
                          public_id: image.imageId,
                        }))}
                        item={{
                          secure_url: image.imageUrl,
                          public_id: image.imageId,
                        }}
                        type="text"
                      />
                    );
                  })}
                </div> */}
              </div>

              <Button type="submit" className="mt-10 text-right">
                Create new model
              </Button>
            </form>
          </Form>
        </div>
        <div className="basis-[30%] bg-muted/50 rounded-xl"></div>
      </div>
    </Fragment>
  );
};

export default IphoneModelPage;
