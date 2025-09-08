import { cn } from '~/infrastructure/lib/utils';
import { Skeleton } from './skeleton';

type TwoRowSkeletonProps = {
   className?: string;
};

const TwoRowSkeleton = ({ className }: TwoRowSkeletonProps) => {
   return (
      <div>
         <Skeleton className={cn('h-14 w-[400px] mb-2', className)} />
         <Skeleton className={cn('h-14 w-[400px] mb-2', className)} />
      </div>
   );
};

export default TwoRowSkeleton;
