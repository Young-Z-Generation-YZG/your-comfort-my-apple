import { LoadingOverlay } from '@components/client/loading-overlay';
import { cn } from '~/infrastructure/lib/utils';
import Carrier from './carrier';

const ProductDetailsPage = () => {
   return (
      <div
         className={cn(
            'w-full flex flex-col items-start justify-center bg-[#fff]',
         )}
      >
         <LoadingOverlay isLoading={false} fullScreen />

         {/* CARRIER */}
         <Carrier />

         <div className="w-full h-[100px] bg-red-500 mx-20 text-center">
            <div className="max-auto">
               <h1>test</h1>
            </div>
         </div>
         {/* Product Detail */}
      </div>
   );
};

export default ProductDetailsPage;
