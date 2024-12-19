import { UseFormReturn } from "react-hook-form";
import { Checkbox } from "~/components/ui/checkbox";
import {
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
} from "~/components/ui/form";
import Image from "next/image";

export type image = {
  secure_url: string;
  public_id: string;
};

export type CCheckboxProps = {
  form: UseFormReturn;
  name: string;
  items: image[];
  item: image;
  type: "text" | "email" | "password" | "number" | "color" | "url";
  label?: string;
  description?: string;
  disabled?: boolean;
  inputClassname?: string;
  labelClassName?: string;
  descriptionClassName?: string;
};

const CCheckbox = (props: CCheckboxProps) => {
  return (
    <FormField
      control={props.form.control}
      name={props.name}
      render={({}) => (
        <FormItem>
          <div className="mb-4">
            <FormLabel className={props.labelClassName}>
              {props.label || ""}
            </FormLabel>
            <FormDescription className={props.descriptionClassName}>
              {props.description || ""}
            </FormDescription>

            <FormField
              key={props.item.public_id}
              control={props.form.control}
              name={props.name}
              render={({ field }) => (
                <FormItem className="relative" key={props.item.public_id}>
                  <FormControl>
                    <Checkbox
                      className="absolute right-2 top-2"
                      checked={field.value?.includes(props.item.secure_url)}
                      onCheckedChange={(checked) => {
                        // const imageArr = props.form.watch(props.name);

                        // const notEmptyArr = checked
                        //   ? props.item.secure_url
                        //   : "";

                        return checked;
                      }}
                    />
                  </FormControl>

                  <div>
                    <Image
                      className="rounded-md"
                      src={props.item.secure_url}
                      alt="iphone-12-pro-max"
                      width={1000}
                      height={1000}
                      data-imageId={props.item.public_id}
                    />
                  </div>
                </FormItem>
              )}
            />
          </div>
        </FormItem>
      )}
    />
  );
};

export default CCheckbox;
