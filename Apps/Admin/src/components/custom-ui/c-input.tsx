import { UseFormReturn } from "react-hook-form";
import {
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "~/components/ui/form";
import { Input } from "~/components/ui/input";

export type CInputProps = {
  form: UseFormReturn;
  name: string;
  type: "text" | "email" | "password" | "number" | "color" | "url";
  label?: string;
  description?: string;
  disabled?: boolean;
  inputClassname?: string;
  labelClassName?: string;
  descriptionClassName?: string;
  defaultValue?: string;
  value?: string;
};

const CInput = (props: CInputProps) => {
  return (
    <FormField
      control={props.form.control}
      name={props.name}
      render={({ field }) => (
        <FormItem>
          <FormLabel className={props.labelClassName}>
            {props.label || ""}
          </FormLabel>
          <FormControl>
            <Input
              {...field}
              type={props.type}
              disabled={props.disabled}
              className={props.inputClassname}
              defaultValue={props.defaultValue}
              value={props.value || field.value}
            />
          </FormControl>

          <FormDescription className={props.descriptionClassName}>
            {props.description || ""}
          </FormDescription>

          <FormMessage />
        </FormItem>
      )}
    />
  );
};

export default CInput;
