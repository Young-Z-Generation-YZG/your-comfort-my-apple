"use client";

import { cn } from "~/lib/utils";
import CountUp from "react-countup";

export type AppCardProps = {
  label: string;
  amount: string;
  description: string;
  icon: string;
};

const CardWrapper = (props: AppCardProps) => {
  return (
    <CardContent className="border-none shadow-[0_15px_30px_-15px_rgba(0,0,0,0.5)]">
      <section>
        <p className="text-sm font-medium">{props.label}</p>
      </section>

      <section className="flex flex-col">
        <h2 className="text-2xl font-bold text-blue-400">
          <CountUp end={parseInt(props.amount)} duration={2} delay={1} />
        </h2>
        <p className="text-xs text-gray-500 font-semibold mt-2">
          {props.description}
        </p>
      </section>
    </CardContent>
  );
};

export const CardContent = (props: React.HTMLAttributes<HTMLDivElement>) => {
  return (
    <div
      {...props}
      className={cn(
        "flex w-full flex-col gap-3 rounded-xl border p-5 shadow bg-muted/50",
        props.className
      )}
    />
  );
};

export default CardWrapper;
