'use client';

import { useState, useMemo } from 'react';
import { Avatar, AvatarImage, AvatarFallback } from '~/components/ui/avatar';
import RatingStar from '~/components/ui/rating-star';
import { Separator } from '~/components/ui/separator';
import { Button } from '~/components/ui/button';
import { Textarea } from '~/components/ui/textarea';
import { TReviewItem } from '~/domain/types/catalog.type';
import { useAppSelector } from '~/infrastructure/redux/store';
import useReviewService from '~/hooks/api/use-review-service';
import { Star, Edit2, X, Check, Loader2 } from 'lucide-react';
import { cn } from '~/infrastructure/lib/utils';
import { toast } from 'sonner';

type ReviewItemProps = {
   review: TReviewItem;
   onReviewUpdated?: () => void;
};

const ReviewItem = ({ review, onReviewUpdated }: ReviewItemProps) => {
   const [isEditing, setIsEditing] = useState(false);
   const [editedRating, setEditedRating] = useState(review.rating);
   const [editedContent, setEditedContent] = useState(review.content);
   const [hoverRating, setHoverRating] = useState(0);
   const [isSubmitting, setIsSubmitting] = useState(false);

   const authState = useAppSelector((state) => state.auth);
   const { updateReviewAsync, isLoading } = useReviewService();

   // Check if review belongs to current user
   const isOwner = useMemo(() => {
      if (!authState.isAuthenticated || !authState.userId) {
         return false;
      }
      // Primary check: user_id in customer_review_info matches current userId
      const matchesUserId =
         review.customer_review_info?.user_id === authState.userId;
      // Fallback checks: updated_by matches or customer name matches username
      const matchesUpdatedBy = review.updated_by === authState.userId;
      const matchesUsername =
         review.customer_review_info?.name?.toLowerCase() ===
         authState.username?.toLowerCase();
      return matchesUserId || matchesUpdatedBy || matchesUsername;
   }, [
      authState.isAuthenticated,
      authState.userId,
      authState.username,
      review.updated_by,
      review.customer_review_info?.user_id,
      review.customer_review_info?.name,
   ]);

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

   const handleEdit = () => {
      setIsEditing(true);
      setEditedRating(review.rating);
      setEditedContent(review.content);
   };

   const handleCancel = () => {
      setIsEditing(false);
      setEditedRating(review.rating);
      setEditedContent(review.content);
      setHoverRating(0);
   };

   const handleSave = async () => {
      if (!editedContent.trim()) {
         toast.error('Review content cannot be empty');
         return;
      }

      if (editedRating < 1 || editedRating > 5) {
         toast.error('Please select a rating');
         return;
      }

      setIsSubmitting(true);
      try {
         const result = await updateReviewAsync(review.id, {
            rating: editedRating,
            content: editedContent.trim(),
         });

         if (result.isSuccess) {
            setIsEditing(false);
            toast.success('Review updated successfully');
            onReviewUpdated?.();
         }
      } catch (error) {
         console.error('Failed to update review:', error);
         toast.error('Failed to update review');
      } finally {
         setIsSubmitting(false);
      }
   };

   const hasChanges =
      editedRating !== review.rating ||
      editedContent.trim() !== review.content.trim();

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
                     <div className="flex items-center gap-2">
                        <h3 className="font-semibold text-gray-900 dark:text-gray-100">
                           {customerName}
                        </h3>
                     </div>
                     {!isEditing ? (
                        <div className="flex items-center gap-2">
                           <RatingStar rating={review.rating} size="sm" />
                           <span className="text-sm text-gray-600 dark:text-gray-400">
                              {review.rating}.0
                           </span>
                        </div>
                     ) : (
                        <div className="flex items-center gap-1 mt-1">
                           {[1, 2, 3, 4, 5].map((star) => (
                              <button
                                 key={star}
                                 type="button"
                                 className="p-0.5 focus:outline-none transition-transform hover:scale-110"
                                 onMouseEnter={() => setHoverRating(star)}
                                 onMouseLeave={() => setHoverRating(0)}
                                 onClick={() => setEditedRating(star)}
                                 disabled={isSubmitting}
                              >
                                 <Star
                                    className={cn(
                                       'h-5 w-5 transition-colors',
                                       star <= (hoverRating || editedRating)
                                          ? 'text-yellow-400 fill-yellow-400'
                                          : 'text-gray-300 fill-gray-100 dark:fill-gray-700',
                                    )}
                                 />
                              </button>
                           ))}
                           <span className="ml-2 text-sm text-gray-600 dark:text-gray-400">
                              {editedRating}.0
                           </span>
                        </div>
                     )}
                  </div>
                  <span className="text-sm text-gray-500 dark:text-gray-500">
                     {formatDate(review.created_at)}
                  </span>
               </div>

               <Separator className="mb-3" />

               {/* Review Content */}
               {!isEditing ? (
                  <div className="space-y-3">
                     <p className="text-gray-700 dark:text-gray-300 leading-relaxed whitespace-pre-wrap">
                        {review.content}
                     </p>
                     {isOwner && (
                        <div className="flex justify-end pt-2">
                           <Button
                              variant="ghost"
                              size="sm"
                              onClick={handleEdit}
                              className="h-8 px-3 text-xs"
                           >
                              <Edit2 className="h-3.5 w-3.5 mr-1.5" />
                              Edit
                           </Button>
                        </div>
                     )}
                  </div>
               ) : (
                  <div className="space-y-3">
                     <Textarea
                        value={editedContent}
                        onChange={(e) => setEditedContent(e.target.value)}
                        placeholder="Share your experience with this product..."
                        rows={4}
                        className="resize-none"
                        disabled={isSubmitting}
                     />
                     <div className="flex justify-end gap-2 pt-2">
                        <Button
                           variant="outline"
                           size="sm"
                           onClick={handleCancel}
                           disabled={isSubmitting}
                           className="h-8 px-3"
                        >
                           <X className="h-3.5 w-3.5 mr-1.5" />
                           Cancel
                        </Button>
                        <Button
                           size="sm"
                           onClick={handleSave}
                           disabled={isSubmitting || !hasChanges}
                           className="h-8 px-3"
                        >
                           {isSubmitting ? (
                              <>
                                 <Loader2 className="h-3.5 w-3.5 mr-1.5 animate-spin" />
                                 Saving...
                              </>
                           ) : (
                              <>
                                 <Check className="h-3.5 w-3.5 mr-1.5" />
                                 Save
                              </>
                           )}
                        </Button>
                     </div>
                  </div>
               )}
            </div>
         </div>
      </div>
   );
};

export default ReviewItem;
