import { Sparkles, Zap } from 'lucide-react';
import { IModelPromotionResponse } from '~/src/domain/interfaces/catalogs/iPhone-model.interface';

const BlackFridayBanner = ({
   promotion,
}: {
   promotion: IModelPromotionResponse;
}) => {
   return (
      <div>
         <div className="relative">
            <div className="absolute -top-1 -right-1 w-3 h-3 bg-yellow-500 rounded-full animate-pulse"></div>
            <div className="text-lg font-bold text-yellow-500">Promotion</div>
         </div>
         <div className="mt-4 relative overflow-hidden">
            <div className="absolute -right-12 -top-4 w-24 h-24 bg-yellow-400 rotate-45 opacity-10"></div>
            <div className="relative z-10 p-3 bg-black rounded-md border border-yellow-500">
               <div className="flex items-center gap-2">
                  <Zap className="h-5 w-5 text-yellow-500" />
                  <span className="font-bold text-yellow-400">
                     {'Black Friday Sale!'}
                  </span>
               </div>
               <div className="mt-1 flex items-center gap-2">
                  <div className="px-2 py-0.5 bg-yellow-500 rounded text-sm font-bold text-black">
                     {'percentage' === 'percentage'
                        ? `${15}% OFF`
                        : `$${100} OFF`}
                  </div>
                  <span className="text-xs text-gray-400">
                     Ends {new Date('2024-06-30').toLocaleDateString()}
                  </span>
               </div>
               <div className="mt-2 w-full h-[1px] bg-gradient-to-r from-transparent via-yellow-500 to-transparent"></div>
            </div>
         </div>
      </div>
   );
};

export default BlackFridayBanner;
