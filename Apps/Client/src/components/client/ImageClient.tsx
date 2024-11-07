import Image from "next/image";
import { cn } from "~/lib/utils";
interface ImageProps{
     src: string;
     className: string;
     alt: string;
}

const ImageClient = ({src, alt, className}: ImageProps) => {
     return (
          <div className={cn("relative", className)}>
               <Image 
               src={src} 
               alt={alt} 
               fill={true} 
               objectFit="cover"
               quality={100}
               />
          </div>
     )
}

export default ImageClient;   