import { AppleIcon } from '~/components/icon';

interface AppleCareCardProps {
   title: string;
   price: string;
   monthlyPrice: string;
   features?: string[];
}

const AppleCareCard = ({
   title,
   price,
   monthlyPrice,
   features,
}: AppleCareCardProps) => {
   const isNoCoverage = title.startsWith('No ');

   return (
      <div className="border-[0.8px] border-[#86868b] rounded-[10px] p-[14px] flex flex-col items-start">
         <div className="item-title text-[18px] font-semibold leading-[21px] tracking-[0.6px] flex flex-row items-start justify-start">
            {!isNoCoverage ? (
               <span className="flex flex-row items-start justify-start gap-1">
                  <AppleIcon size={21} color="red" />
                  <span>{title}</span>
               </span>
            ) : (
               <span>{title}</span>
            )}
         </div>

         {!isNoCoverage && (
            <>
               <div className="item-price w-full pt-1 pb-[21px] border-b-[0.8px] border-[#86868b] text-[14px] font-light leading-[21px] tracking-[0.5px]">
                  ${price} or ${monthlyPrice}/mo.
               </div>
               {features && features.length > 0 && (
                  <div className="item-sub-title w-full pt-[18px] text-[12px] font-light leading-[16px] tracking-[0.8px]">
                     <ul className="list-disc ml-4">
                        {features.map((feature, index) => (
                           <li
                              key={index}
                              className="pr-3 pb-[15px] tracking-[0.7px]"
                           >
                              {feature}
                           </li>
                        ))}
                     </ul>
                  </div>
               )}
            </>
         )}
      </div>
   );
};

export default AppleCareCard;
