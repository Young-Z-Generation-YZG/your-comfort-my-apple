import ContentWrapper from '@components/ui/content-wrapper';
import { Fragment } from 'react';
import IphoneModelItem from './_components/model-item';

const iPhoneModelsPage = () => {
   return (
      <Fragment>
         <div className="p-4">
            <ContentWrapper className="flex flex-col gap-5">
               {Array(5)
                  .fill(0)
                  .map((_, index) => (
                     <IphoneModelItem key={index} />
                  ))}
            </ContentWrapper>
         </div>
      </Fragment>
   );
};

export default iPhoneModelsPage;
