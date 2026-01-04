'use client';

import { useEffect } from 'react';
import usePromotionService from '~/hooks/api/use-promotion-service';
import EventContent from './_components/event-content';
import { ProductGridSkeleton } from './_components/product-card-skeleton';
import { TEvent, TEventItem } from '~/domain/types/catalog.type';

const EVENT_ID = '611db6eb-3d64-474e-9e23-3517ad0df6ec';

/**
 * Client Component for Black Friday Event Page
 * Fetches data on the client using promotion service hook
 */
export default function BlackFridayPage() {
   const { isLoading, getEventDetailsState, getEventDetailsAsync } =
      usePromotionService();

   const { data: eventData } = getEventDetailsState;

   useEffect(() => {
      // Fetch event data on mount
      getEventDetailsAsync(EVENT_ID);
   }, [getEventDetailsAsync]);

   // Fallback data if API fails
   const fallbackEvent: TEvent = {
      id: EVENT_ID,
      title: 'Black Friday Special',
      description: 'Limited-time savings on your favorite Apple products.',
      start_date: new Date().toISOString(),
      end_date: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString(),
      event_items: [],
      created_at: new Date().toISOString(),
      updated_at: new Date().toISOString(),
      updated_by: null,
      is_deleted: false,
      deleted_at: null,
      deleted_by: null,
   };

   // Show loading state
   if (isLoading) {
      return (
         <div className="bg-[#f5f5f7]">
            <div className="bg-white text-red-500 font-bold">
               {/* Banner placeholder - you can add a loading banner here if needed */}
            </div>

            <div className="row w-full max-w-[1200px] mx-auto px-4 sm:px-6 lg:px-0 pt-[64px] md:pt-[80px] pb-[48px] md:pb-[64px]">
               <p className="text-3xl sm:text-4xl lg:text-5xl w-full max-w-[700px] text-wrap font-semibold text-slate-900/60 leading-9 sm:leading-[48px] lg:leading-[60px]">
                  <span className="inline text-black">Store.</span>
                  The best way to buy the products you love.
               </p>
            </div>

            <div className="w-full max-w-[1200px] mx-auto px-4 sm:px-6 lg:px-0 pt-[64px] md:pt-[80px] pb-[48px] md:pb-[64px]">
               <div className="text-3xl sm:text-4xl lg:text-5xl w-full max-w-[700px] text-wrap font-semibold text-slate-900/60 leading-9 sm:leading-[48px] lg:leading-[60px]">
                  <h1 className="text-black">
                     Loading Black Friday Special...
                  </h1>
               </div>
               <ProductGridSkeleton count={6} className="mt-5" />
            </div>
         </div>
      );
   }

   // Use data from RTK Query state, fallback to default
   const event = eventData || fallbackEvent;
   const eventItems = event.event_items || [];

   return (
      <>
         {/* Main Content */}
         <EventContent eventData={event} eventItems={eventItems} />
      </>
   );
}
