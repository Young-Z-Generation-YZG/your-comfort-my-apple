'use client';

import { useState, useEffect } from 'react';
import Link from 'next/link';
import { ArrowRight, X } from 'lucide-react';
import { Button } from '~/components/ui/button';
import { cn } from '~/infrastructure/lib/utils';

export default function PromotionBanner() {
   const [countdown, setCountdown] = useState({
      days: 0,
      hours: 0,
      minutes: 0,
      seconds: 0,
   });

   const [dismissed, setDismissed] = useState(false);

   // Set end date to Black Friday (example: November 24, 2023)
   useEffect(() => {
      const targetDate = new Date('November 24, 2023 23:59:59').getTime();

      const updateCountdown = () => {
         const now = new Date().getTime();
         const distance = targetDate - now;

         setCountdown({
            days: Math.floor(distance / (1000 * 60 * 60 * 24)),
            hours: Math.floor(
               (distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60),
            ),
            minutes: Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60)),
            seconds: Math.floor((distance % (1000 * 60)) / 1000),
         });
      };

      // Initial update
      updateCountdown();

      // Update every second
      const interval = setInterval(updateCountdown, 1000);

      return () => clearInterval(interval);
   }, []);

   if (dismissed) return null;

   return (
      <div className="relative bg-black text-white overflow-hidden">
         {/* Decorative elements */}
         <div className="absolute inset-0 overflow-hidden">
            <div className="absolute -left-10 top-1/2 transform -translate-y-1/2 w-40 h-40 bg-red-500 rounded-full opacity-20 blur-xl"></div>
            <div className="absolute right-1/4 top-0 w-20 h-20 bg-blue-500 rounded-full opacity-20 blur-xl"></div>
            <div className="absolute right-10 bottom-0 w-32 h-32 bg-purple-500 rounded-full opacity-20 blur-xl"></div>
         </div>

         {/* Close button */}
         <button
            onClick={() => setDismissed(true)}
            className="absolute top-3 right-3 md:top-4 md:right-4 text-gray-400 hover:text-white z-10"
            aria-label="Dismiss banner"
         >
            <X className="h-5 w-5" />
         </button>

         <div className="container max-w-6xl mx-auto px-4 py-8 md:py-12 relative z-0">
            <div className="flex flex-col items-center text-center">
               {/* Main heading */}
               <h2 className="text-3xl md:text-5xl font-bold mb-2 tracking-tight">
                  <span className="text-red-500">BLACK</span>{' '}
                  <span className="text-white">FRIDAY</span>
               </h2>

               <p className="text-xl md:text-2xl font-medium mb-6 max-w-2xl">
                  Our biggest sale of the year is here. Save up to{' '}
                  <span className="text-red-500 font-bold">25%</span> on select
                  Apple products.
               </p>

               {/* Countdown timer */}
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

               {/* Featured deals */}
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

               {/* CTA button */}
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
