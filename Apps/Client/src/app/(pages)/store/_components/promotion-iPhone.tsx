'use client';

import { ShoppingBag } from 'lucide-react';
import { useEffect, useState } from 'react';
import { Button } from '@components/ui/button';
import CartWrapper from './card-wrapper';
import Link from 'next/link';
import { ProductPromotion } from '../page';
import { useStoreBasketAsyncMutation } from '~/infrastructure/services/basket.service';
import {
   StoreBasketFormType,
   StoreBasketResolver,
} from '~/domain/schemas/basket.schema';
import { useDispatch } from 'react-redux';
import { useAppSelector } from '~/infrastructure/redux/store';
import { addCartItem } from '~/infrastructure/redux/features/cart.slice';
import { useLoading } from '@components/contexts/loading.context';
import { cn } from '~/infrastructure/lib/utils';
import { CldImage } from 'next-cloudinary';
import { useForm } from 'react-hook-form';
import { TCartItem } from '~/infrastructure/redux/types/cart.type';
import { useRouter } from 'next/navigation';
import { toast as sonnerToast } from 'sonner';

interface PromotionIPhoneProps {
   index: number;
   item: ProductPromotion;
}

const PromotionIPhone = ({ item, index }: PromotionIPhoneProps) => {
   const router = useRouter();
   const [selectedVariant, setSelectedVariant] = useState<string | null>(null);
   const { showLoading, hideLoading } = useLoading();

   const dispatch = useDispatch();
   const { items, currentOrder } = useAppSelector((state) => state.cart.value);

   const {
      handleSubmit,
      formState: { errors },
      setValue,
   } = useForm<StoreBasketFormType>({
      resolver: StoreBasketResolver,
      defaultValues: {
         product_id: '',
         model_id: item.product_model_id,
         product_name: item.promotion_product_name,
         product_color_name: '',
         product_unit_price: item.promotion_product_unit_price,
         product_name_tag: item.product_name_tag,
         product_image: '',
         product_slug: item.promotion_product_slug,
         category_id: item.category_id,
         quantity: 1,
         promotion: {
            promotion_id_or_code: item.promotion_id,
            promotion_event_type: item.promotion_event_type,
            promotion_title: item.promotion_title,
            promotion_discount_type: item.promotion_discount_type,
            promotion_discount_value: item.promotion_discount_value,
            promotion_discount_unit_price: item.promotion_product_unit_price,
            promotion_final_price: item.promotion_final_price,
         },
         order: currentOrder,
      },
   });

   console.log('errors', errors);

   const onBuySubmit = async (data: StoreBasketFormType) => {
      console.log('data', data);

      var basketItem: TCartItem = {
         product_id: data.product_id,
         model_id: data.model_id,
         product_name: data.product_name,
         product_color_name: data.product_color_name,
         product_unit_price: data.product_unit_price,
         product_name_tag: data.product_name_tag,
         product_image: data.product_image,
         product_slug: data.product_slug,
         category_id: data.category_id,
         quantity: data.quantity,
         promotion: {
            promotion_id_or_code: item.promotion_id,
            promotion_event_type: item.promotion_event_type,
            promotion_title: item.promotion_title,
            promotion_discount_type: item.promotion_discount_type,
            promotion_discount_value: item.promotion_discount_value,
            promotion_discount_unit_price: item.promotion_product_unit_price,
            promotion_final_price: item.promotion_final_price,
         },
         order: currentOrder,
      };

      console.log('basketItem', basketItem);

      dispatch(addCartItem(basketItem));

      router.push('/cart');
   };

   const onAddToCartSubmit = async (data: StoreBasketFormType) => {
      console.log('data', data);

      var basketItem: TCartItem = {
         product_id: data.product_id,
         model_id: data.model_id,
         product_name: data.product_name,
         product_color_name: data.product_color_name,
         product_unit_price: data.product_unit_price,
         product_name_tag: data.product_name_tag,
         product_image: data.product_image,
         product_slug: data.product_slug,
         category_id: data.category_id,
         quantity: data.quantity,
         promotion: {
            promotion_id_or_code: item.promotion_id,
            promotion_event_type: item.promotion_event_type,
            promotion_title: item.promotion_title,
            promotion_discount_type: item.promotion_discount_type,
            promotion_discount_value: item.promotion_discount_value,
            promotion_discount_unit_price: item.promotion_product_unit_price,
            promotion_final_price: item.promotion_final_price,
         },
         order: currentOrder,
      };

      console.log('basketItem', basketItem);

      dispatch(addCartItem(basketItem));

      sonnerToast.success('Added To Cart', {
         action: {
            label: 'View Cart',
            onClick: () => {
               router.push('/cart');
            },
         },
      });
   };

   // useEffect(() => {
   //    if (isLoadingStoreBasket) {
   //       showLoading();
   //    } else {
   //       setTimeout(() => {
   //          hideLoading();
   //       }, 500);
   //    }
   // }, [isLoadingStoreBasket]);

   const getRandomIndex = (length: number) => {
      return Math.floor(Math.random() * length);
   };

   return (
      <div className="col-span-1" key={index + 1}>
         <CartWrapper>
            <div className="relative cursor-pointer">
               <span className="absolute top-3 right-3 z-50 rounded-full bg-red-600 px-2 py-1 text-xs font-bold text-white">
                  Save{' '}
                  {item.promotion_discount_type.toUpperCase() === 'PERCENTAGE'
                     ? `${item.promotion_discount_value * 100}%`
                     : `$${item.promotion_discount_value}`}
               </span>
               <div className="flex items-center justify-center bg-[#f5f5f7] transition-all hover:shadow-md">
                  <Link href="#">
                     <CldImage
                        width={500}
                        height={500}
                        className="object-none h-[320px]"
                        src={`${item.product_variants.find((v) => v.color_name === selectedVariant)?.product_color_image ?? item.product_variants[getRandomIndex(item.product_variants.length)].product_color_image}`}
                        alt="iPhone Model"
                     />
                  </Link>
               </div>
            </div>

            {/* Variant Color Selectors */}
            <div className="mt-3 flex items-center justify-center gap-3">
               {item.product_variants.map((variant, idx) => (
                  <div
                     key={idx}
                     className={cn(
                        'h-[30px] w-[30px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out',
                        selectedVariant === variant.color_name
                           ? 'ring-2 ring-[#2563EB] ring-offset-2 ring-offset-white'
                           : 'hover:ring-2 hover:ring-[#2563EB] hover:ring-offset-2 hover:ring-offset-white',
                     )}
                     style={{ backgroundColor: variant.color_hex }}
                     onClick={() => {
                        if (variant.color_name !== selectedVariant) {
                           setSelectedVariant(variant.color_name);
                           setValue('product_id', variant.product_id);
                           setValue(
                              'product_image',
                              variant.product_color_image,
                           );
                           setValue('product_color_name', variant.color_name);
                        } else {
                           setSelectedVariant(null);
                           setValue('product_id', '');
                           setValue('product_image', '');
                           setValue('product_color_name', '');
                        }
                     }}
                  />
               ))}
            </div>

            <div className="">
               {!!selectedVariant ||
                  (errors.product_id && (
                     <p className="text-red-500 text-sm font-semibold text-center mt-2">
                        {errors.product_color_name?.message}
                     </p>
                  ))}
            </div>

            {/* Product Details */}
            <div className="p-6">
               <h3 className="mb-4 text-2xl font-semibold">
                  {item.promotion_product_name}
               </h3>

               <div className="mb-4">
                  <div className="flex items-end gap-2">
                     <span className="text-2xl font-medium text-red-600 font-SFProText">
                        ${item.promotion_final_price.toFixed(2)}
                     </span>
                     <span className="text-base text-gray-500 line-through font-SFProText">
                        ${item.promotion_product_unit_price.toFixed(2)}
                     </span>
                  </div>
                  <p className="mt-1 text-sm text-gray-500 font-SFProText">
                     Save $
                     {(
                        (item.promotion_product_unit_price ?? 0) -
                        (item.promotion_final_price ?? 0)
                     ).toFixed(2)}{' '}
                     for a limited time
                  </p>
               </div>

               {/* Buttons */}
               <div className="flex gap-2">
                  <Button
                     className="w-full cursor-pointer rounded-lg bg-blue-600 py-2 text-white font-SFProText font-medium hover:bg-blue-700"
                     onClick={() => {
                        alert('Buy Now');
                        handleSubmit(onBuySubmit)();
                     }}
                  >
                     Buy
                  </Button>
                  <Button
                     className="rounded-full py-3 px-2 border bg-white text-black hover:bg-slate-300/50 active:bg-slate-100/50 transition-all duration-200 ease-linear"
                     onClick={() => {
                        handleSubmit(onAddToCartSubmit)();
                     }}
                  >
                     <ShoppingBag className="h-5 w-5" />
                     <span className="sr-only">Add to bag</span>
                  </Button>
               </div>
            </div>
         </CartWrapper>
      </div>
   );
};

export default PromotionIPhone;
