'use client';

import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { X } from 'lucide-react';

interface RedirectCountdownProps {
   seconds: number;
   onComplete: () => void;
   onCancel: () => void;
}

export function RedirectCountdown({
   seconds,
   onComplete,
   onCancel,
}: RedirectCountdownProps) {
   const [timeLeft, setTimeLeft] = useState(seconds);
   const [isCancelled, setIsCancelled] = useState(false);

   useEffect(() => {
      if (isCancelled) return;

      if (timeLeft <= 0) {
         onComplete();
         return;
      }

      const timer = setTimeout(() => {
         setTimeLeft(timeLeft - 1);
      }, 1000);

      return () => clearTimeout(timer);
   }, [timeLeft, isCancelled, onComplete]);

   const handleCancel = () => {
      setIsCancelled(true);
      onCancel();
   };

   if (isCancelled) return null;

   return (
      <motion.div
         className="fixed bottom-6 right-6 bg-white rounded-lg shadow-lg border border-gray-200 p-4 flex items-center space-x-4 z-50"
         initial={{ opacity: 0, y: 50, scale: 0.9 }}
         animate={{ opacity: 1, y: 0, scale: 1 }}
         exit={{ opacity: 0, y: 20, scale: 0.9 }}
         transition={{ type: 'spring', stiffness: 300, damping: 25 }}
      >
         <div className="flex-1">
            <p className="text-sm font-medium">
               Redirecting in <span className="text-gray-900">{timeLeft}</span>{' '}
               seconds
            </p>
            <div className="w-full bg-gray-200 rounded-full h-1.5 mt-2">
               <motion.div
                  className="h-1.5 rounded-full bg-gray-900"
                  initial={{ width: '100%' }}
                  animate={{ width: `${(timeLeft / seconds) * 100}%` }}
                  transition={{ ease: 'linear' }}
               />
            </div>
         </div>
         <button
            onClick={handleCancel}
            className="p-1 rounded-full hover:bg-gray-100 transition-colors"
            aria-label="Cancel redirect"
         >
            <X className="h-4 w-4 text-gray-500" />
         </button>
      </motion.div>
   );
}
