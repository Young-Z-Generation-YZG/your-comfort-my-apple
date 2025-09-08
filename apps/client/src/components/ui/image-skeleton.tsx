import { cn } from '~/infrastructure/lib/utils';

type ImageSkeletonProps = {
   className?: string;
};

const ImageSkeleton = ({ className }: ImageSkeletonProps) => {
   return (
      <div
         className={cn(
            'animate-pulse rounded-full bg-muted [w-50px] [h-50px] bg-slate-200',
            className,
         )}
      ></div>
   );
};

export default ImageSkeleton;
