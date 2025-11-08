'use client';

import { Avatar, AvatarImage, AvatarFallback } from '@components/ui/avatar';
import RatingStar from '@components/ui/rating-star';
import { Separator } from '@components/ui/separator';
import { TReviewItem } from '~/infrastructure/services/review.service';

type ReviewItemProps = {
   review: TReviewItem;
};

const ReviewItem = ({ review }: ReviewItemProps) => {
   const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString('en-US', {
         year: 'numeric',
         month: 'long',
         day: 'numeric',
      });
   };

   const getInitials = (name: string) => {
      if (!name) return 'U';
      const parts = name.trim().split(' ');
      if (parts.length >= 2) {
         return `${parts[0][0]}${parts[1][0]}`.toUpperCase();
      }
      return name[0].toUpperCase();
   };

   const customerName = review.customer_review_info?.name || 'Anonymous User';
   const avatarUrl = review.customer_review_info?.avatar_image_url;

   return (
      <div className="bg-white dark:bg-gray-900 border border-gray-200 dark:border-gray-800 rounded-lg p-6 shadow-sm hover:shadow-md transition-shadow">
         <div className="flex gap-4">
            {/* Avatar */}
            <Avatar className="h-12 w-12 flex-shrink-0">
               {avatarUrl ? (
                  <AvatarImage src={avatarUrl} alt={customerName} />
               ) : null}
               <AvatarFallback className="bg-gray-200 dark:bg-gray-700 text-gray-600 dark:text-gray-300">
                  {getInitials(customerName)}
               </AvatarFallback>
            </Avatar>

            {/* Content */}
            <div className="flex-1 min-w-0">
               {/* Header */}
               <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-2 mb-3">
                  <div className="flex flex-col gap-1">
                     <h3 className="font-semibold text-gray-900 dark:text-gray-100">
                        {customerName}
                     </h3>
                     <div className="flex items-center gap-2">
                        <RatingStar rating={review.rating} size="sm" />
                        <span className="text-sm text-gray-600 dark:text-gray-400">
                           {review.rating}.0
                        </span>
                     </div>
                  </div>
                  <span className="text-sm text-gray-500 dark:text-gray-500">
                     {formatDate(review.created_at)}
                  </span>
               </div>

               <Separator className="mb-3" />

               {/* Review Content */}
               <p className="text-gray-700 dark:text-gray-300 leading-relaxed whitespace-pre-wrap">
                  {review.content}
               </p>
            </div>
         </div>
      </div>
   );
};

export default ReviewItem;
