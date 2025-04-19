'use client';
import React, { useEffect, useState } from 'react';
import ItemReview from './review-item';
import { FaRegStar, FaStar } from 'react-icons/fa6';
import { IRatingStarResponse } from '~/domain/interfaces/catalogs/iPhone-model.inteface';
import { Star } from 'lucide-react';
import { useGetReviewByModelIdAsyncQuery } from '~/infrastructure/services/catalog.service';
import { IReviewResponse } from '~/domain/interfaces/catalogs/review.interface';

type ReviewSectionProps = {
   modelId: string;
   averageRating: number;
   totalReviews: number;
   ratingStars: IRatingStarResponse[];
};

const ReviewSection = ({
   modelId,
   averageRating,
   totalReviews,
   ratingStars,
}: ReviewSectionProps) => {
   const [indexStar, setIndexStar] = useState(-1);
   const [review, setReview] = useState<IReviewResponse[]>([]);

   const {
      data: reviewsDataAsync,
      isLoading: loadingReviews,
      isSuccess: successGetReviews,
      isError: errorReviews,
      error: errorMessageReviews,
   } = useGetReviewByModelIdAsyncQuery(modelId);

   useEffect(() => {
      if (successGetReviews) {
         setReview(reviewsDataAsync.items);
      }
   }, [reviewsDataAsync]);

   const starsRating = (rating: number, size: number) => {
      return [1, 2, 3, 4, 5].map((star) => (
         <Star
            className={`h-[${size}] w-[${size}] ${
               star <= rating
                  ? 'text-yellow-400 fill-yellow-400'
                  : 'text-gray-300'
            }`}
         />
      ));
   };

   const renderStarsReview = () => {
      return Array(5)
         .fill(0)
         .map((_, index) => {
            return indexStar < index ? (
               <div
                  key={index}
                  onClick={() => setIndexStar(index)}
                  className="w-fit h-fit border border-[#FFAA4E] hover:bg-[#FFAA4E] p-3 rounded-[5px] group ease-out duration-300 "
               >
                  <FaRegStar className="w-6 h-6 text-[#FFAA4E] group-hover:text-white" />
               </div>
            ) : (
               <div
                  key={index}
                  onClick={() => setIndexStar(index)}
                  className="w-fit h-fit border border-[#FFAA4E] bg-[#FFAA4E] p-3 rounded-[5px] ease-out duration-300"
               >
                  <FaStar className="w-6 h-6 text-white" />
               </div>
            );
         });
   };

   return (
      <div className="w-full h-max">
         <div className="w-[1076px] mx-auto h-full">
            <div className="w-full border-b border-black">
               <div className="w-full py-2 text-[30px] font-bold">Reviews</div>
               <div className="w-full grid grid-cols-3">
                  {/* Rating Snapshot */}
                  <div className="flex flex-col text-[13px] text-[#363636] mr-5 font-light tracking-[0.5px]">
                     <div className="p-2 pt-0">
                        Select a row below to filter reviews.
                     </div>
                     <div className="p-2 pt-0 flex flex-col">
                        <div className="flex flex-row items-center">
                           <div className="basis-[15%] text-sm">
                              {ratingStars[4]?.star} star
                           </div>
                           <div className="w-[220px] py-1 px-2 relative">
                              <div className="flex flex-row gap-[2px] items-center">
                                 {starsRating(ratingStars[4]?.star, 10)}
                              </div>
                           </div>
                           <div className="basis-[15%] text-sm font-medium">
                              ({ratingStars[4]?.count})
                           </div>
                        </div>

                        <div className="flex flex-row items-center">
                           <div className="basis-[15%] text-sm">
                              {ratingStars[3]?.star} star
                           </div>
                           <div className="w-[220px] py-1 px-2 relative">
                              <div className="flex flex-row gap-[2px] items-center">
                                 {starsRating(4, 10)}
                              </div>
                           </div>
                           <div className="basis-[15%] text-sm font-medium">
                              ({ratingStars[3]?.count})
                           </div>
                        </div>

                        <div className="flex flex-row items-center">
                           <div className="basis-[15%] text-sm">
                              {ratingStars[2]?.star} star
                           </div>
                           <div className="w-[220px] py-1 px-2 relative">
                              <div className="flex flex-row gap-[2px] items-center">
                                 {starsRating(3, 10)}
                              </div>
                           </div>
                           <div className="basis-[15%] text-sm font-medium">
                              ({ratingStars[2]?.count})
                           </div>
                        </div>

                        <div className="flex flex-row items-center">
                           <div className="basis-[15%] text-sm">
                              {ratingStars[1]?.star} star
                           </div>
                           <div className="w-[220px] py-1 px-2 relative">
                              <div className="flex flex-row gap-[2px] items-center">
                                 {starsRating(2, 10)}
                              </div>
                           </div>
                           <div className="basis-[15%] text-sm font-medium">
                              ({ratingStars[1]?.count})
                           </div>
                        </div>

                        <div className="flex flex-row items-center">
                           <div className="basis-[15%] text-sm">
                              {ratingStars[0]?.star} star
                           </div>
                           <div className="w-[220px] py-1 px-2 relative">
                              <div className="flex flex-row gap-[2px] items-center">
                                 {starsRating(1, 10)}
                              </div>
                           </div>
                           <div className="basis-[15%] text-sm font-medium">
                              ({ratingStars[0]?.count})
                           </div>
                        </div>
                     </div>
                  </div>

                  {/* Overall Rating */}
                  <div className="flex flex-col text-[14px] text-[#363636] font-light tracking-[0.5px]">
                     <div className="p-2 text-xl font-semibold">
                        Overall Rating
                     </div>
                     <div className="p-2 pt-0 flex flex-row">
                        <div className="text-5xl font-bold mr-3">
                           {averageRating}
                        </div>
                        <div className="flex flex-col justify-center">
                           <div className="flex flex-row gap-[2px] items-center">
                              {/* {starsRating(averageRating)} */}
                           </div>
                           <div className="text-[#0077C8] mt-1">
                              {totalReviews} Reviews
                           </div>
                        </div>
                     </div>
                  </div>

                  {/* Review this Product */}
                  <div className="flex flex-col text-[13px] text-[#363636] font-light tracking-[0.5px]">
                     <div className="p-2 text-right font-medium font-SFProText">
                        Review this Product
                     </div>
                     <div className="p-2 pt-0 flex flex-row justify-end gap-1">
                        {renderStarsReview()}
                     </div>
                     <div className="p-2 pt-3 text-right text-xs text-slate-500 font-SFProText">
                        Adding a review will require a valid email address
                     </div>
                  </div>
               </div>
            </div>
            <div className="w-full h-full mt-2">
               <div className="w-full py-2 text-xl font-bold">Experiments</div>
               <div className="w-full flex flex-col mt-2">
                  {review.map((item, index) => {
                     return (
                        <div key={index}>
                           <ItemReview review={item} />
                        </div>
                     );
                  })}
               </div>
            </div>
         </div>
      </div>
   );
};

export default ReviewSection;
