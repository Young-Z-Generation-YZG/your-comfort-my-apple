"use client";

import { UseFormReturn } from "react-hook-form";

import {
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "~/components/ui/form";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "~/components/ui/select";

export type CISelectProps = {
  form: UseFormReturn;
  name: string;
  items: string[];
  label?: string;
  description?: string;
  disabled?: boolean;
  className?: string;
  labelClassName?: string;
  descriptionClassName?: string;
  defaultValue?: string;
  value?: string;
  placeholder?: string;
};

const CISelect = (props: CISelectProps) => {
  return (
    <FormField
      control={props.form.control}
      name={props.name}
      render={({ field }) => (
        <FormItem className={props.className}>
          <FormLabel className={props.labelClassName}>{props.label}</FormLabel>
          <Select onValueChange={field.onChange} defaultValue={field.value}>
            <FormControl>
              <SelectTrigger>
                <SelectValue
                  placeholder={
                    props.placeholder ?? "Select a verified email to display"
                  }
                />
              </SelectTrigger>
            </FormControl>
            <SelectContent>
              {props.items.length ? (
                props.items.map((item, index) => {
                  return (
                    <SelectItem key={index} value={item}>
                      {item}
                    </SelectItem>
                  );
                })
              ) : (
                <p className="ml-3 text-sm">Empty item</p>
              )}
            </SelectContent>
          </Select>
          <FormDescription className={props.descriptionClassName}>
            {props.description}
          </FormDescription>
          <FormMessage />
        </FormItem>
      )}
    />
  );
};

export default CISelect;
