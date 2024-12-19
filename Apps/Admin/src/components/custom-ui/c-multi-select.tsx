/* eslint-disable @typescript-eslint/no-explicit-any */
"use client";

import { FormDescription, FormLabel } from "~/components/ui/form";
import { MultiSelect } from "~/components/ui/multi-select";

export type optionType = {
  value: string;
  label: string;
  icon: any;
};

export type CMultiSelectProps = {
  options: optionType[];
  onValueChange: (value: string[]) => void;
  defaultValue?: string[];
  label?: string;
  description?: string;
};

const CMultiSelect = (props: CMultiSelectProps) => {
  return (
    <div>
      <FormLabel className="block pb-2">{props.label || ""}</FormLabel>
      <MultiSelect
        options={props.options}
        onValueChange={props.onValueChange}
        defaultValue={props.defaultValue || []}
        placeholder="Select frameworks"
        variant="inverted"
        animation={2}
        maxCount={5}
      />
      <FormDescription className="block pt-2">
        {props.description || ""}
      </FormDescription>
    </div>
  );
};

export default CMultiSelect;
