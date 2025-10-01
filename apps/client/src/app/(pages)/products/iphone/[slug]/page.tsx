import { LoadingOverlay } from '~/components/client/loading-overlay';
import { cn } from '~/infrastructure/lib/utils';
import Carrier from './_components/carrier';
import IphoneDetails from './_components/iphone-details';
import NextImage from 'next/image';

const IphoneDetailPage = () => {
   return (
      <div>
         {/* CARRIER */}
         <Carrier />

         <div className="w-full p-28">
            {/* Product Detail */}
            <IphoneDetails />
         </div>
      </div>
   );
};

export default IphoneDetailPage;
