'use client';

import { useState } from 'react';
import Image from 'next/image';
import { Skeleton } from '@components/ui/skeleton';

interface ImageWithSkeletonProps {
   src: string;
   alt: string;
   width: number;
   height: number;
   className?: string;
}

export default function ImageWithSkeleton({
   src,
   alt,
   width,
   height,
   className = '',
}: ImageWithSkeletonProps) {
   const [isLoading, setIsLoading] = useState(true);

   return (
      <div className="relative">
         {isLoading && <Skeleton className={`absolute inset-0 ${className}`} />}
         <Image
            src={src || '/placeholder.svg'}
            alt={alt}
            width={width}
            height={height}
            className={`${className} ${isLoading ? 'opacity-0' : 'opacity-100 transition-opacity duration-500'}`}
            onLoad={() => setIsLoading(false)}
         />
      </div>
   );
}
