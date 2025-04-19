'use client';

import { useEffect, useState } from 'react';
import { Button } from '@components/ui/button';
import { Textarea } from '@components/ui/textarea';
import { motion, AnimatePresence } from 'framer-motion';
import { useToast } from '~/hooks/use-toast';
import { Star, AlertCircle } from 'lucide-react';
import { useForm } from 'react-hook-form';
import {
   ReviewFormType,
   ReviewResolver,
} from '~/domain/schemas/catalog.schema';
import { useCreateReviewAsyncMutation } from '~/infrastructure/services/catalog.service';

type ReviewModalProps = {
   item: {
      product_id: string;
      model_id: string;
      order_item_id: string;
      name: string;
      image: string;
      options?: string;
   };
   onClose: () => void;
   onSubmit: () => void;
};

export function ReviewModal({ item, onClose, onSubmit }: ReviewModalProps) {
   const { toast } = useToast();
   const [rating, setRating] = useState(0);
   const [hoverRating, setHoverRating] = useState(0);
   const [comment, setComment] = useState('');
   const [isSubmitting, setIsSubmitting] = useState(false);

   const form = useForm<ReviewFormType>({
      resolver: ReviewResolver,
      defaultValues: {
         order_item_id: item.order_item_id,
         model_id: item.model_id,
         product_id: item.product_id,
         rating: 0,
         content: '',
      },
   });

   const {
      formState: { errors },
   } = form;

   const [
      createReviewAsync,
      { isLoading: isCreating, error: submitError, isError, reset },
   ] = useCreateReviewAsyncMutation();

   const handleSubmit = async (data: ReviewFormType) => {
      console.log('data', data);

      await createReviewAsync(data).unwrap();

      // Simulate API call
      setTimeout(() => {
         onSubmit();
         onClose();

         toast({
            title: 'Review submitted',
            description: 'Thank you for your feedback!',
            duration: 2000,
         });
      }, 1000);
   };

   useEffect(() => {
      if (isCreating) {
         setIsSubmitting(true);
      } else {
         setTimeout(() => {
            setIsSubmitting(false);
         }, 1000);
      }
   }, [isCreating]);

   return (
      <div className="space-y-6">
         <div className="flex items-start space-x-4">
            <div className="flex-shrink-0 w-20 h-20 bg-gray-100 rounded-md overflow-hidden">
               <img
                  src={item.image || '/placeholder.svg'}
                  alt={item.name}
                  className="w-full h-full object-center object-cover"
               />
            </div>
            <div>
               <h3 className="text-lg font-medium text-gray-900">
                  {item.name}
               </h3>
               {item.options && (
                  <p className="mt-1 text-sm text-gray-500">{item.options}</p>
               )}
            </div>
         </div>

         <div className="space-y-4">
            <div>
               <label className="block text-sm font-medium text-gray-700 mb-2">
                  Your Rating
               </label>
               <div className="flex items-center">
                  {[1, 2, 3, 4, 5].map((star) => (
                     <motion.button
                        key={star}
                        type="button"
                        whileHover={{ scale: 1.1 }}
                        whileTap={{ scale: 0.9 }}
                        className="p-1 focus:outline-none"
                        onMouseEnter={() => setHoverRating(star)}
                        onMouseLeave={() => setHoverRating(0)}
                        onClick={() => {
                           form.setValue('rating', star);
                           form.formState.errors.rating = undefined;
                           setRating(star);
                        }}
                     >
                        <Star
                           className={`h-8 w-8 ${
                              star <= (hoverRating || rating)
                                 ? 'text-yellow-400 fill-yellow-400'
                                 : 'text-gray-300'
                           }`}
                        />
                     </motion.button>
                  ))}
               </div>
               <AnimatePresence>
                  {errors?.rating && (
                     <motion.p
                        initial={{ opacity: 0, height: 0 }}
                        animate={{ opacity: 1, height: 'auto' }}
                        exit={{ opacity: 0, height: 0 }}
                        className="text-sm text-red-600 flex items-center mt-1"
                     >
                        <AlertCircle className="h-3 w-3 mr-1" />
                        {errors.rating?.message}
                     </motion.p>
                  )}
               </AnimatePresence>
            </div>

            <div>
               <label
                  htmlFor="comment"
                  className="block text-sm font-medium text-gray-700 mb-2"
               >
                  Your Review
               </label>
               <Textarea
                  id="comment"
                  value={comment}
                  onChange={(e) => {
                     setComment(e.target.value);
                     form.setValue('content', e.target.value);
                     if (comment.length > 0) {
                        form.formState.errors.content = undefined;
                     }
                  }}
                  placeholder="Share your experience with this product..."
                  rows={4}
                  className={`resize-none transition-all duration-200 ${
                     errors?.content ? 'border-red-300 focus:ring-red-500' : ''
                  }`}
               />
               <AnimatePresence>
                  {errors?.content && (
                     <motion.p
                        initial={{ opacity: 0, height: 0 }}
                        animate={{ opacity: 1, height: 'auto' }}
                        exit={{ opacity: 0, height: 0 }}
                        className="text-sm text-red-600 flex items-center mt-1"
                     >
                        <AlertCircle className="h-3 w-3 mr-1" />
                        {errors.content?.message}
                     </motion.p>
                  )}
               </AnimatePresence>
            </div>
         </div>

         <div className="flex justify-end space-x-3 pt-4 border-t border-gray-200">
            <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }}>
               <Button
                  type="button"
                  variant="outline"
                  onClick={onClose}
                  disabled={isSubmitting}
               >
                  Cancel
               </Button>
            </motion.div>
            <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }}>
               <Button
                  type="button"
                  onClick={() => {
                     form.handleSubmit(handleSubmit)();
                  }}
                  disabled={isSubmitting}
               >
                  {isSubmitting ? (
                     <span className="flex items-center">
                        <svg
                           className="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
                           xmlns="http://www.w3.org/2000/svg"
                           fill="none"
                           viewBox="0 0 24 24"
                        >
                           <circle
                              className="opacity-25"
                              cx="12"
                              cy="12"
                              r="10"
                              stroke="currentColor"
                              strokeWidth="4"
                           ></circle>
                           <path
                              className="opacity-75"
                              fill="currentColor"
                              d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                           ></path>
                        </svg>
                        Submitting...
                     </span>
                  ) : (
                     'Submit Review'
                  )}
               </Button>
            </motion.div>
         </div>
      </div>
   );
}
