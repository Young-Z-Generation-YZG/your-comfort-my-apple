'use client';

import { cn } from '~/infrastructure/lib/utils';
import AppleCareCard from './applecare-card';
import HelpItem from './help-item';
import { appleCareOptions } from '../_constants/applecare-data';

const Coverage = () => {
   return (
      <div className={cn('w-full bg-transparent flex flex-col mt-16 h-fit')}>
         {/* Title */}
         <div className="coverage-title flex flex-col md:flex-row md:items-center">
            <div className="md:basis-3/4">
               <div className="w-full text-[24px] font-semibold leading-[28px]">
                  <span className="text-[#1D1D1F] tracking-[0.3px]">
                     AppleCare+ coverage.{' '}
                  </span>
                  <span className="text-[#86868B] tracking-[0.3px]">
                     Protect your new iPhone.
                  </span>
               </div>
            </div>
            <div className="md:basis-1/4 min-w-[0] mt-4 md:mt-0">
               {/* Empty or reserved for spacing on desktop */}
            </div>
         </div>

         {/* Items */}
         <div className="coverage-items flex flex-col md:flex-row mt-6 gap-6">
            {/* AppleCare cards */}
            <div className="basis-full md:basis-3/4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 pr-0 md:pr-12">
               {appleCareOptions.map((option, index) => (
                  <AppleCareCard
                     key={index}
                     title={option.title}
                     price={option.price}
                     monthlyPrice={option.monthlyPrice}
                     features={option.features}
                  />
               ))}
            </div>

            {/* HelpItem */}
            <div className="basis-full md:basis-1/4 min-w-[0]">
               <HelpItem
                  title="What kind of protection do you need?"
                  subTitle="Compare the additional features and coverage of the two AppleCare+ plans."
               />
            </div>
         </div>
      </div>
   );
};

export default Coverage;
