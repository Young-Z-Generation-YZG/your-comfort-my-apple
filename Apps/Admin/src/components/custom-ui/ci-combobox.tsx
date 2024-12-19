/* eslint-disable @typescript-eslint/no-unused-vars */
"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { Check, ChevronsUpDown } from "lucide-react";
import { useForm, UseFormReturn } from "react-hook-form";
import { z } from "zod";

import { cn } from "~/lib/utils";
import { Button } from "~/components/ui/button";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from "~/components/ui/command";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "~/components/ui/form";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "~/components/ui/popover";
import { toast } from "~/hooks/use-toast";

const languages = [
  { label: "English", value: "en" },
  { label: "French", value: "fr" },
  { label: "German", value: "de" },
  { label: "Spanish", value: "es" },
  { label: "Portuguese", value: "pt" },
  { label: "Russian", value: "ru" },
  { label: "Japanese", value: "ja" },
  { label: "Korean", value: "ko" },
  { label: "Chinese", value: "zh" },
] as const;

export type CIComboboxProps = {
  form: UseFormReturn;
  name: string;
  items: {
    label: string;
    value: string;
  }[];
  label?: string;
  description?: string;
  disabled?: boolean;
  className?: string;
  labelClassName?: string;
  descriptionClassName?: string;
  defaultValue?: string;
  value?: string;
  placeholder?: string;
  onSelected?: (value: string) => void;
};

const CICombobox = (props: CIComboboxProps) => {
  return (
    <FormField
      control={props.form.control}
      name={props.name}
      render={({ field }) => (
        <FormItem className={cn("flex flex-col", props.className)}>
          <FormLabel className={props.labelClassName}>{props.label}</FormLabel>
          <Popover>
            <PopoverTrigger asChild>
              <FormControl>
                <Button
                  variant="outline"
                  role="combobox"
                  className={cn(
                    "w-full justify-between",
                    !field.value && "text-muted-foreground"
                  )}
                >
                  {field.value
                    ? props.items.find((item) => item.value === field.value)
                        ?.label
                    : `Select ${props.name}`}
                  <ChevronsUpDown className="opacity-50" />
                </Button>
              </FormControl>
            </PopoverTrigger>
            <PopoverContent className="p-0" align="end">
              <Command>
                <CommandInput
                  placeholder="Search framework..."
                  className="h-9"
                />
                <CommandList>
                  <CommandEmpty>No framework found.</CommandEmpty>
                  <CommandGroup>
                    {props.items.length ? (
                      props.items.map((item) => (
                        <CommandItem
                          value={item.label}
                          key={item.value}
                          onSelect={() => {
                            props.form.setValue(props.name, item.value);

                            if (props.onSelected) {
                              props.onSelected(item.value);
                            }
                          }}
                        >
                          {item.label}
                          <Check
                            className={cn(
                              "ml-auto",
                              item.value === field.value
                                ? "opacity-100"
                                : "opacity-0"
                            )}
                          />
                        </CommandItem>
                      ))
                    ) : (
                      <CommandEmpty>No item found.</CommandEmpty>
                    )}
                  </CommandGroup>
                </CommandList>
              </Command>
            </PopoverContent>
          </Popover>
          <FormDescription className={props.descriptionClassName}>
            {props.description}
          </FormDescription>
          <FormMessage />
        </FormItem>
      )}
    />
  );
};

export default CICombobox;
