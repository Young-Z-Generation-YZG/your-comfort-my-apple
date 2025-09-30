import { Skeleton } from '@components/ui/skeleton';
import { cn } from '~/infrastructure/lib/utils';

export function ProductCardSkeleton() {
   return (
      <div className="bg-white rounded-2xl overflow-hidden shadow-sm">
         <div className="relative">
            <div className="absolute top-3 right-3 z-10">
               <Skeleton className="h-5 w-16 rounded-full" />
            </div>
            <Skeleton className="h-[300px] w-full" />
         </div>

         <div className="p-6">
            <div className="mb-4">
               <Skeleton className="h-7 w-3/4 mb-2" />
               <Skeleton className="h-4 w-1/2" />
            </div>

            <div className="mb-4">
               <div className="flex items-center gap-2">
                  <Skeleton className="h-6 w-20" />
                  <Skeleton className="h-4 w-16" />
               </div>
               <Skeleton className="h-4 w-40 mt-1" />
            </div>

            <div className="flex gap-2 mt-6">
               <Skeleton className="h-10 flex-1 rounded-full" />
               <Skeleton className="h-10 w-10 rounded-full" />
            </div>
         </div>
      </div>
   );
}

export function ProductGridSkeleton({
   count = 6,
   className = '',
}: {
   count?: number;
   className?: string;
}) {
   return (
      <div className={cn('', className)}>
         {Array(count)
            .fill(0)
            .map((_, index) => (
               <ProductCardSkeleton key={index} />
            ))}
      </div>
   );
}
