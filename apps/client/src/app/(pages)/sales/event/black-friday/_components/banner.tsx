'use client';

import { useMemo, useState, useEffect } from 'react';
import Link from 'next/link';
import { ArrowRight } from 'lucide-react';
import { Button } from '~/components/ui/button';
import { cn } from '~/infrastructure/lib/utils';

interface TimeLeft {
   days: number;
   hours: number;
   minutes: number;
   seconds: number;
}

const getBlackFridayDate = (year: number): Date => {
   const november1st = new Date(year, 10, 1); // Month is 0-indexed
   let dayOfWeek = november1st.getDay(); // 0 = Sunday, 1 = Monday, etc.
   let fourthFriday = 0;

   // Find the first Friday in November
   while (dayOfWeek !== 5) {
      // 5 is Friday
      november1st.setDate(november1st.getDate() + 1);
      dayOfWeek = november1st.getDay();
   }

   // Add three more weeks to get the fourth Friday
   fourthFriday = november1st.getDate() + 21;
   return new Date(year, 11, fourthFriday, 23, 59, 59);
};

const getNextBlackFriday = (): Date => {
   const now = new Date();
   const currentYear = now.getFullYear();
   let blackFridayThisYear = getBlackFridayDate(currentYear);

   if (now > blackFridayThisYear) {
      return getBlackFridayDate(currentYear + 1);
   }
   return blackFridayThisYear;
};

export const BlackFridayBanner = () => {
   const [countdown, setCountdown] = useState<TimeLeft>({
      days: 0,
      hours: 0,
      minutes: 0,
      seconds: 0,
   });

   const eventDate = useMemo(() => getNextBlackFriday(), []);

   useEffect(() => {
      const updateCountdown = () => {
         const now = new Date().getTime();
         const distance = Math.max(0, eventDate.getTime() - now);

         setCountdown({
            days: Math.floor(distance / (1000 * 60 * 60 * 24)),
            hours: Math.floor(
               (distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60),
            ),
            minutes: Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60)),
            seconds: Math.floor((distance % (1000 * 60)) / 1000),
         });
      };

      updateCountdown();
      const interval = setInterval(updateCountdown, 1000);
      return () => clearInterval(interval);
   }, [eventDate]);

   if (countdown.days <= 0 && countdown.hours <= 0 && countdown.minutes <= 0 && countdown.seconds <= 0) {
      return null; // Hide the banner if the event has passed
   }

   return (
      <div className="relative bg-black text-white overflow-hidden">
         <div className="absolute inset-0 overflow-hidden">
            <div className="absolute -left-10 top-1/2 transform -translate-y-1/2 w-40 h-40 bg-red-500 rounded-full opacity-20 blur-xl"></div>
            <div className="absolute right-1/4 top-0 w-20 h-20 bg-blue-500 rounded-full opacity-20 blur-xl"></div>
            <div className="absolute right-10 bottom-0 w-32 h-32 bg-purple-500 rounded-full opacity-20 blur-xl"></div>
         </div>

         <div className="container max-w-6xl mx-auto px-4 py-8 md:py-12 relative z-0">
            <div className="flex flex-col items-center text-center">
               <h2 className="text-3xl md:text-5xl font-bold mb-2 tracking-tight">
                  <span className="text-red-500">BLACK</span>{' '}
                  <span className="text-white">FRIDAY</span>
               </h2>

               <p className="text-xl md:text-2xl font-medium mb-6 max-w-2xl">
                  Our biggest sale of the year is here. Save up to{' '}
                  <span className="text-red-500 font-bold">25%</span> on select
                  Apple products.
               </p>

               <div className="grid grid-cols-4 gap-2 md:gap-4 mb-6 max-w-md w-full">
                  <div className="flex flex-col items-center">
                     <div className="bg-white/10 backdrop-blur-sm rounded-lg w-full py-2 md:py-3 px-1 md:px-2 mb-1">
                        <span className="text-xl md:text-3xl font-bold">
                           {countdown.days}
                        </span>
                     </div>
                     <span className="text-xs md:text-sm text-gray-300">
                        Days
                     </span>
                  </div>
                  <div className="flex flex-col items-center">
                     <div className="bg-white/10 backdrop-blur-sm rounded-lg w-full py-2 md:py-3 px-1 md:px-2 mb-1">
                        <span className="text-xl md:text-3xl font-bold">
                           {countdown.hours}
                        </span>
                     </div>
                     <span className="text-xs md:text-sm text-gray-300">
                        Hours
                     </span>
                  </div>
                  <div className="flex flex-col items-center">
                     <div className="bg-white/10 backdrop-blur-sm rounded-lg w-full py-2 md:py-3 px-1 md:px-2 mb-1">
                        <span className="text-xl md:text-3xl font-bold">
                           {countdown.minutes}
                        </span>
                     </div>
                     <span className="text-xs md:text-sm text-gray-300">
                        Minutes
                     </span>
                  </div>
                  <div className="flex flex-col items-center">
                     <div className="bg-white/10 backdrop-blur-sm rounded-lg w-full py-2 md:py-3 px-1 md:px-2 mb-1">
                        <span className="text-xl md:text-3xl font-bold">
                           {countdown.seconds}
                        </span>
                     </div>
                     <span className="text-xs md:text-sm text-gray-300">
                        Seconds
                     </span>
                  </div>
               </div>

               <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-8 w-full max-w-3xl">
                  <div className="bg-white/5 backdrop-blur-sm rounded-xl p-3 text-center">
                     <div className="font-bold text-lg md:text-xl">iPhone</div>
                     <div className="text-red-500 font-medium text-sm md:text-base">
                        Save $100
                     </div>
                  </div>
                  <div className="bg-white/5 backdrop-blur-sm rounded-xl p-3 text-center">
                     <div className="font-bold text-lg md:text-xl">MacBook</div>
                     <div className="text-red-500 font-medium text-sm md:text-base">
                        Save $150
                     </div>
                  </div>
                  <div className="bg-white/5 backdrop-blur-sm rounded-xl p-3 text-center">
                     <div className="font-bold text-lg md:text-xl">iPad</div>
                     <div className="text-red-500 font-medium text-sm md:text-base">
                        Save $100
                     </div>
                  </div>
                  <div className="bg-white/5 backdrop-blur-sm rounded-xl p-3 text-center">
                     <div className="font-bold text-lg md:text-xl">AirPods</div>
                     <div className="text-red-500 font-medium text-sm md:text-base">
                        Save $60
                     </div>
                  </div>
               </div>

               <Link href="/promotions">
                  <Button
                     className={cn(
                        'bg-red-600 hover:bg-red-700 text-white rounded-full px-8 py-6 text-lg',
                        'transition-all duration-300 hover:scale-105',
                     )}
                  >
                     Shop Black Friday Deals{' '}
                     <ArrowRight className="ml-2 h-5 w-5" />
                  </Button>
               </Link>
            </div>
         </div>
      </div>
   );
}
