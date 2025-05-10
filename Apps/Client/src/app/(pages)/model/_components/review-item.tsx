/* eslint-disable react/react-in-jsx-scope */
'use client';
import { cn } from '~/infrastructure/lib/utils';
import { SFDisplayFont } from '@assets/fonts/font.config';
import { FaUserAlt } from 'react-icons/fa';
import { IReviewResponse } from '~/domain/interfaces/catalogs/review.interface';
import { Star } from 'lucide-react';

type ItemReviewProps = {
   review: IReviewResponse;
};

const ItemReview = ({ review }: ItemReviewProps) => {
   const starsRating = (rating: number, size: number) => {
      return [1, 2, 3, 4, 5].map((star) => (
         <Star
            className={`h-${size} w-${size} ${
               star <= rating
                  ? 'text-yellow-400 fill-yellow-400'
                  : 'text-gray-300'
            }`}
         />
      ));
   };

   const getReviewTitle = (rating: number) => {
      switch (rating) {
         case 1:
            return 'Bad';
         case 2:
            return 'Not good';
         case 3:
            return 'Average';
         case 4:
            return 'Good';
         case 5:
            return 'Excellent';
         default:
            return 'Unknown rating';
      }
   };

   return (
      <div
         className={cn(
            SFDisplayFont.variable,
            'font-SFProDisplay w-full h-fit pt-[10px] pb-[20px] flex flex-row text-[13px]',
         )}
      >
         <div
            className={cn(
               'basis-[25%] h-[120px] border-r border-[#ccc] flex flex-row',
            )}
         >
            <FaUserAlt className={cn('h-16 w-16 mr-2')} />
            <div className={cn('flex-1 pl-2')}>
               <div className={cn('flex flex-row mb-1')}>
                  {starsRating(review.rating, 4)}
               </div>
               <p className={cn('font-medium')}>
                  {review.customer_name?.length > 0
                     ? review.customer_name
                     : 'Ellenvs'}
               </p>
               <p className={cn('font-thin')}>
                  {new Date(review.created_at).toLocaleDateString('en-US', {
                     year: 'numeric',
                     month: 'long',
                     day: 'numeric',
                  })}
               </p>
            </div>
         </div>
         <div className={cn('basis-4/5 h-[120px] flex flex-col')}>
            <div className={cn('pl-2 text-xl font-semibold')}>
               {getReviewTitle(review.rating)}
            </div>
            <div className={cn('pl-2 font-normal text-sm font-SFProText mt-2')}>
               {review.content}
            </div>
         </div>

         <div className={cn('h-full flex flex-row gap-3')}>
            <div className={cn('h-full flex flex-col justify-between')}>
               <p>Ellenvs</p>
               <p>4 hours ago</p>
            </div>
         </div>
         <div className={cn('h-full flex flex-col gap-3')}>
            <p>Snel, scherp, 1 hand te bedienen</p>
         </div>
      </div>
   );
};

export default ItemReview;
