import { Star } from 'lucide-react';
import { Fragment } from 'react';

type RatingStarProps = {
   rating: number;
   size?: number;
   className?: string;
};

const RatingStar = ({ rating, size = 16, className = '' }: RatingStarProps) => {
   return (
      <Fragment>
         {[1, 2, 3, 4, 5].map((star) => (
            <Star
               key={star}
               className={`h-[${size}px] w-[${size}px] ${
                  star <= rating
                     ? 'text-yellow-400 fill-yellow-400'
                     : 'text-gray-300'
               } ${className}`}
            />
         ))}
      </Fragment>
   );
};

export default RatingStar;
