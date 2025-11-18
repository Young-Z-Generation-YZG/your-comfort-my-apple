import { TEvent } from '../lib/get-event-data';

interface BannerProps {
   eventData?: TEvent;
}

const Banner = ({ eventData }: BannerProps) => {
   return (
      <div>
         <h1>{eventData?.title || 'Banner'}</h1>
      </div>
   );
};

export default Banner;
