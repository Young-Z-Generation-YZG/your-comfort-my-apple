import { Sparkles } from 'lucide-react';
import BlackFridayBanner from './blackfriday-banner';
import { IModelPromotionResponse } from '~/src/domain/interfaces/catalogs/iPhone-model.interface';

const PromotionSection = ({
   promotion,
}: {
   promotion: IModelPromotionResponse;
}) => {
   return (
      <div className="py-2 px-5 w-full ml-5 basis-[30%] border-l border-mute">
         <BlackFridayBanner promotion={promotion} />
      </div>
   );
};

export default PromotionSection;
