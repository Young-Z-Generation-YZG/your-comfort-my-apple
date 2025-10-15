import { useCallback, useEffect, useState } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';

export const usePromoCode = () => {
   const router = useRouter();
   const searchParams = useSearchParams();

   // Get coupon code from URL params
   const urlCouponCode = searchParams.get('_couponCode') || '';

   // State for promo code input
   const [promoCode, setPromoCode] = useState(urlCouponCode);

   // Sync promo code state with URL changes
   useEffect(() => {
      const currentCouponCode = searchParams.get('_couponCode') || '';
      setPromoCode(currentCouponCode);
   }, [searchParams]);

   // Handle promo code apply
   const handleApplyPromoCode = useCallback(() => {
      if (!promoCode.trim()) return;

      const params = new URLSearchParams(searchParams.toString());
      params.set('_couponCode', promoCode.trim());
      router.push(`?${params.toString()}`, { scroll: false });
   }, [promoCode, searchParams, router]);

   // Handle promo code input change
   const handlePromoCodeChange = useCallback(
      (e: React.ChangeEvent<HTMLInputElement>) => {
         setPromoCode(e.target.value);
      },
      [],
   );

   // Handle promo code removal
   const handleRemovePromoCode = useCallback(() => {
      const params = new URLSearchParams(searchParams.toString());
      params.delete('_couponCode');
      router.push(`?${params.toString()}`, { scroll: false });
   }, [searchParams, router]);

   return {
      promoCode,
      urlCouponCode,
      handleApplyPromoCode,
      handlePromoCodeChange,
      handleRemovePromoCode,
   };
};
