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
import { Textarea } from "~/components/ui/textarea";
import { cn } from "~/lib/utils";

export type CInputProps = {
  form: UseFormReturn;
  name: string;
  type: "text" | "email" | "password" | "number" | "color" | "url" | "textarea";
  children?: React.ReactNode;
  label?: string;
  description?: string;
  disabled?: boolean;
  className?: string;
  inputWrapperClassName?: string;
  inputClassname?: string;
  labelClassName?: string;
  descriptionClassName?: string;
  defaultValue?: string;
  value?: string;
  placeholder?: string;
  draggable?: boolean;
};

const CInput = (props: CInputProps) => {
  return (
    <FormField
      control={props.form.control}
      name={props.name}
      render={({ field }) => (
        <FormItem className={props.className}>
          <FormLabel className={props.labelClassName}>
            {props.label || ""}
          </FormLabel>
          <FormControl>
            {props.type !== "textarea" ? (
              <div className={props.inputWrapperClassName}>
                <Input
                  {...field}
                  type={props.type}
                  disabled={props.disabled}
                  className={props.inputClassname}
                  defaultValue={props.defaultValue}
                  value={props.value || field.value}
                  onBlur={(e) => {
                    if (e.currentTarget.value.length) {
                      e.currentTarget.className += " text-slate-500";

                      e.currentTarget.value = e.currentTarget.value.trim();
                    }
                  }}
                  onFocus={(e) => {
                    e.currentTarget.className =
                      e.currentTarget.className.replace(" text-slate-500", "");
                  }}
                />

                {props.children}
              </div>
            ) : (
              <Textarea
                {...field}
                disabled={props.disabled}
                className={props.inputClassname}
                defaultValue={props.defaultValue}
                value={props.value || field.value}
                placeholder={props.placeholder}
              />
            )}
          </FormControl>

          <FormDescription className={cn(props.descriptionClassName)}>
            {props.description || ""}
          </FormDescription>

          <FormMessage />
        </FormItem>
      )}
    />
  );
};

export default CInput;
