import { cn } from "~/lib/utils";

export type ModelProps = {
  model: string;
  amount: string;
  description: string;
};

export const ModelContent = (props: ModelProps) => {
  return (
    <ModelWrapper className="flex justify-between w-[328px]">
      <div className="">
        <p className="text-xl font-medium">{props.model}</p>
        <p className="text-xs">{props.description}</p>

        <div className="mt-5">
          <p className="text-xs">Apple Intelligence footnote ⁸</p>
        </div>
      </div>

      <div className="text-sm font-medium">${props.amount}</div>
    </ModelWrapper>
  );
};

export const ModelWrapper = (props: React.HTMLAttributes<HTMLDivElement>) => {
  return (
    <div
      {...props}
      className={cn(
        "flex gap-3 rounded-xl border p-5 shadow bg-muted/50",
        props.className
      )}
    />
  );
};
