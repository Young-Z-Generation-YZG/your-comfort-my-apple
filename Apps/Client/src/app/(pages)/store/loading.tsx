import { Skeleton } from '~/components/ui/skeleton';
import { ProductGridSkeleton } from './_components/product-card-skeleton';

export default function Loading() {
   return (
      <div className="bg-[#f5f5f7] min-h-screen">
         <div className="h-16 bg-white"></div>

         <div className="w-[1200px] mx-auto pt-[80px] pb-[64px]">
            <Skeleton className="h-14 w-[500px] mb-4" />
            <Skeleton className="h-14 w-[600px]" />
         </div>

         <div className="w-full bg-white py-8">
            <div className="w-[1200px] mx-auto">
               <div className="flex gap-4 overflow-x-auto pb-4">
                  {Array(8)
                     .fill(0)
                     .map((_, i) => (
                        <Skeleton
                           key={i}
                           className="h-24 w-24 rounded-xl flex-shrink-0"
                        />
                     ))}
               </div>
            </div>
         </div>

         <div className="w-[1200px] mx-auto pt-[80px] pb-[64px]">
            <Skeleton className="h-14 w-[400px] mb-2" />
            <Skeleton className="h-5 w-[300px] mb-8" />

            <ProductGridSkeleton count={6} />
         </div>
      </div>
   );
}
