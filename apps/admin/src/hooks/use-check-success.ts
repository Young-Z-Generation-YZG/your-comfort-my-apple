import { useEffect, useRef } from 'react';
import { toast } from 'sonner';

const successStyleObj = {
   backgroundColor: '#DCFCE7',
   color: '#166534',
   border: '1px solid #86EFAC',
};

const cancelObj = {
   label: 'Close',
   onClick: () => {},
   actionButtonStyle: {
      backgroundColor: '#16A34A',
      color: '#FFFFFF',
   },
};

export const useCheckApiSuccess = (
   successes: {
      title: string;
      description?: string;
      isSuccess: boolean;
   }[],
) => {
   const previousSuccessStatesRef = useRef<Map<string, boolean>>(new Map());

   useEffect(() => {
      successes.forEach(({ title, description, isSuccess }) => {
         const successKey = `${title}-${description || ''}`;
         const previousSuccess =
            previousSuccessStatesRef.current.get(successKey);

         // Only show toast when success transitions from false to true
         if (isSuccess && previousSuccess !== true) {
            toast.success(title, {
               description: description,
               style: successStyleObj,
               cancel: cancelObj,
               duration: 2000,
            });
         }

         // Update the previous state
         previousSuccessStatesRef.current.set(successKey, isSuccess);
      });
   }, [successes]);

   useEffect(() => {
      // Clear ref when all successes are false
      const allFalse = successes.every(({ isSuccess }) => !isSuccess);
      if (allFalse) {
         previousSuccessStatesRef.current.clear();
      }
   }, [successes]);
};
