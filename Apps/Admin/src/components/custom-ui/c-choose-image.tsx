/* eslint-disable @typescript-eslint/no-explicit-any */
"use client";

import { useState } from "react";
import { UseFormReturn } from "react-hook-form";
import { Button } from "~/components/ui/button";
import { Checkbox } from "~/components/ui/checkbox";
import { FormControl, FormField, FormItem } from "~/components/ui/form";
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "~/components/ui/sheet";
import Image from "next/image";

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
] as const;

export type CChooseImageProps = {
  form: UseFormReturn<any>;
  name: string;
  index: number;
  btnClassName?: string;
};

const CChooseImage = (props: CChooseImageProps) => {
  const [selectedItem, setSelectedItem] = useState<string | null>(null);

  const handleItemChange = (imageId: string, imageUrl: string) => {
    setSelectedItem(imageId);
    props.form.setValue(`${props.name}.${props.index}.imageUrl`, imageUrl);
    props.form.setValue(`${props.name}.${props.index}.imageId`, imageId);
  };

  return (
    <Sheet>
      <SheetTrigger asChild>
        <Button className={props.btnClassName}>Choose image</Button>
      </SheetTrigger>
      <SheetContent className="w-[55%]">
        <SheetHeader>
          <SheetTitle>Choose image</SheetTitle>
          <SheetDescription>
            Select an item to display in the sidebar.
          </SheetDescription>
        </SheetHeader>
        <div className="py-4">
          <FormField
            control={props.form.control}
            name={props.name}
            render={() => (
              <FormItem>
                <div className="grid grid-cols-3 p-5 border rounded-xl mt-2 gap-3">
                  {imagesList.map((image) => (
                    <FormItem
                      key={image.imageId}
                      className="relative flex flex-row items-start space-x-3 space-y-0"
                    >
                      <FormControl>
                        <Checkbox
                          className="absolute right-2 top-2"
                          checked={selectedItem === image.imageId}
                          onCheckedChange={() =>
                            handleItemChange(image.imageId, image.imageUrl)
                          }
                        />
                      </FormControl>
                      <div>
                        <Image
                          className="rounded-md object-contain"
                          src={image.imageUrl}
                          alt="iphone-12-pro-max"
                          width={1000}
                          height={1000}
                          // data-imageId={props.item.public_id}
                        />
                      </div>
                    </FormItem>
                  ))}
                </div>
              </FormItem>
            )}
          />
        </div>
        <SheetFooter>
          <SheetClose asChild>
            <Button type="button">Close</Button>
          </SheetClose>
        </SheetFooter>
      </SheetContent>
    </Sheet>
  );
};

export default CChooseImage;
