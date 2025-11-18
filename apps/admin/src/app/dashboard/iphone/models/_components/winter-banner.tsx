import { Sparkles } from 'lucide-react';

const WinterBanner = () => {
   return (
      <div className="border-l border-mute py-2 px-5 w-full ml-5 basis-[30%]">
         <div className="relative">
            <div className="absolute -top-1 -right-1 w-3 h-3 bg-rose-500 rounded-full animate-pulse"></div>
            <div className="text-lg font-semibold text-rose-600">Promotion</div>
         </div>
         <div className="mt-4 relative overflow-hidden">
            <div className="absolute -right-12 -top-4 w-24 h-24 bg-gradient-to-br from-amber-300 to-rose-500 rotate-45 opacity-20"></div>
            <div className="relative z-10 p-3 bg-gradient-to-r from-amber-50 to-rose-50 rounded-md border border-amber-200">
               <div className="flex items-center gap-2">
                  <Sparkles className="h-5 w-5 text-amber-500" />
                  <span className="font-medium text-amber-800">
                     Summer Sale 2024
                  </span>
               </div>
               <div className="mt-1 flex items-center gap-2">
                  <div className="px-2 py-0.5 bg-rose-100 rounded text-sm font-bold text-rose-700">
                     10% OFF
                  </div>
                  <span className="text-xs text-gray-500">
                     Ends{' '}
                     {new Date('2025-04-30T00:00:00Z').toLocaleDateString()}
                  </span>
               </div>
            </div>
         </div>
      </div>
   );
};

export default WinterBanner;
