import { cn } from "~/lib/utils";

export type ModelProps = {
  storage: string;
  amount: string;
};

export const StorageContent = (props: ModelProps) => {
  return (
    <StorageWrapper className="flex justify-between w-[328px]">
      <div className="">
        <p className="text-xl font-medium">{props.storage}</p>
      </div>

      <div className="text-sm font-medium">${props.amount}</div>
    </StorageWrapper>
  );
};

export const StorageWrapper = (props: React.HTMLAttributes<HTMLDivElement>) => {
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
