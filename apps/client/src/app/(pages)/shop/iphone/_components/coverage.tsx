'use client';

import { cn } from '~/infrastructure/lib/utils';
import AppleCareCard from '~/app/(pages)/products/iphone/[slug]/_components/applecare-card';
import HelpItem from '../[slug]/_components/help-item';
import { appleCareOptions } from '~/app/(pages)/products/iphone/[slug]/_constants/applecare-data';

const Coverage = () => {
   return (
      <div className={cn('w-full bg-transparent flex flex-col mt-16 h-fit')}>
         <div className="coverage-title flex flex-row">
            <div className="basis-3/4">
               <div className="w-full text-[24px] font-semibold leading-[28px]">
                  <span className="text-[#1D1D1F] tracking-[0.3px]">
                     AppleCare+ coverage.{' '}
                  </span>
                  <span className="text-[#86868B] tracking-[0.3px]">
                     Protect your new iPhone.
                  </span>
               </div>
            </div>
            <div className="basis-1/4 min-w-[328px]"></div>
         </div>
         <div className="coverage-items flex flex-row mt-6">
            <div className="basis-3/4 list-items w-full grid grid-cols-3 gap-4 pr-12">
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
            <div className="basis-1/4 min-w-[328px]">
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
