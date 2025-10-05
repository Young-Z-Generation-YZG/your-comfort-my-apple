import Carrier from './_components/carrier';
import IphoneDetails from './_components/iphone-details';
import Coverage from './_components/coverage';
import ProductInfo from './_components/product-info';
import CompareIPhoneSection from '~/components/client/compare-iphone-section';

const IphoneDetailPage = () => {
   return (
      <div>
         {/* CARRIER */}
         <Carrier />

         <div className="w-full p-28">
            {/* Product Detail */}
            <IphoneDetails />

            {/* Coverage */}
            <div className="mt-[200px]">
               <Coverage />
            </div>

            {/* Product Info */}
            <ProductInfo />

            {/* Compare iPhone Section */}
            <div className="mx-auto mt-20 mb-24">
               <CompareIPhoneSection />
            </div>
         </div>
      </div>
   );
};

export default IphoneDetailPage;
