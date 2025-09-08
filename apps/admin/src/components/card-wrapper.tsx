'use client';

import { ArrowUp, ArrowDown } from 'lucide-react';
import { Fragment } from 'react';
import CountUp from 'react-countup';
import { cn } from '~/src/infrastructure/lib/utils';

type CardWrapperProps = {
   children: React.ReactNode;
   className?: string;
};

export const CardWrapper = ({ children, className }: CardWrapperProps) => {
   return (
      <Fragment>
         <div
            className={cn(
               'rounded-xl border p-5 shadow bg-muted/50',
               className,
            )}
         >
            {children}
         </div>
      </Fragment>
   );
};

type CardContentProps = {
   title: string;
   value: number;
   valueSuffix?: string;
   compareValue: number;
   compareText: string;
   type?: 'amount' | 'percentage' | 'count' | 'text' | 'decimal';
   compareValueType: 'text' | 'decimal' | 'percentage' | 'amount';
   icon: 'up' | 'down';
   variantColor: 'green' | 'red';
   className?: string;
};

export const CardContent = ({
   title,
   value,
   valueSuffix,
   compareValue,
   compareText,
   type,
   compareValueType,
   icon,
   variantColor,
   className,
}: CardContentProps) => {
   const renderIndicator = () => {
      switch (type) {
         case 'text':
            return (
               <Fragment>
                  <CountUp end={value} duration={2} delay={1} />
                  {valueSuffix}
               </Fragment>
            );
         case 'decimal':
            return (
               <Fragment>
                  <CountUp end={value} decimals={2} duration={2} delay={1} />
                  {valueSuffix}
               </Fragment>
            );
         case 'percentage':
            return (
               <Fragment>
                  <CountUp end={value} decimals={2} duration={2} delay={1} />
                  {valueSuffix}
               </Fragment>
            );
         case 'amount':
            return (
               <Fragment>
                  <CountUp end={value} duration={2} delay={1} />
                  {valueSuffix}
               </Fragment>
            );
         default:
            return <ArrowUp className="mr-1 h-4 w-4" />;
      }
   };

   const renderCompareValue = () => {
      switch (compareValueType) {
         case 'text':
            return (
               <Fragment>
                  {compareValue}
                  {valueSuffix} {compareText}
               </Fragment>
            );
         case 'decimal':
            return (
               <Fragment>
                  {compareValue}
                  {compareText}
               </Fragment>
            );
         case 'percentage':
            return (
               <Fragment>
                  {compareValue}% {compareText}
               </Fragment>
            );
         case 'amount':
            return (
               <Fragment>
                  {compareValue}
                  {compareText}
               </Fragment>
            );
         default:
            return <CountUp end={compareValue} duration={2} delay={1} />;
      }
   };

   return (
      <div
         className={cn(
            'p-5 rounded-md order-none shadow-[0_15px_30px_-15px_rgba(0,0,0,0.5)]',
            className,
         )}
      >
         <div className="flex flex-col gap-2">
            <p className="text-sm font-medium">{title}</p>

            <h2 className="text-2xl font-bold text-primary">
               {renderIndicator()}

               {/* {type === 'amount' ? (
                  <Fragment>
                     ${' '}
                     <CountUp end={value} decimals={2} duration={2} delay={1} />
                  </Fragment>
               ) : type === 'percentage' ? (
                  <Fragment>
                     <CountUp
                        end={value}
                        decimals={2}
                        duration={2}
                        delay={1}
                        formattingFn={(value) => {
                           return value.toFixed(2);
                        }}
                     />
                     %{' '}
                  </Fragment>
               ) : type === 'count' ? (
                  <CountUp end={value} duration={2} delay={1} />
               ) : type === 'text' ? (
                  <Fragment>
                     <CountUp end={value} decimals={2} duration={2} delay={1} />
                     {text}
                  </Fragment>
               ) : (
                  <CountUp
                     end={value}
                     decimals={2}
                     duration={2}
                     delay={1}
                     formattingFn={(value) => {
                        return value.toFixed(2);
                     }}
                  />
               )} */}
            </h2>
            <div className="text-xs text-gray-500 font-semibold mt-2">
               <div
                  className={cn('flex items-center text-sm', {
                     'text-green-600': variantColor === 'green',
                     'text-red-600': variantColor === 'red',
                  })}
               >
                  {icon === 'up' ? (
                     <ArrowUp className="mr-1 h-4 w-4" />
                  ) : icon === 'down' ? (
                     <ArrowDown className="mr-1 h-4 w-4" />
                  ) : (
                     <ArrowUp className="mr-1 h-4 w-4" />
                  )}

                  {renderCompareValue()}
                  {/* {percentage && percentageText && (
                     <Fragment>
                        <span>{percentage.toFixed(2)}%</span>
                        <p className="ml-1 text-muted-foreground">
                           {percentageText}
                        </p>
                     </Fragment>
                  )} */}
               </div>
            </div>
         </div>
      </div>
   );
};
