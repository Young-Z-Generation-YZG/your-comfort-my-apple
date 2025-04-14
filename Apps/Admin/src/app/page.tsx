import { Button } from '@components/ui/button';
import Image from 'next/image';

export default function Home() {
   return (
      <div className="p-5">
         <Button className="">Test</Button>

         <div>
            <Image
               src={
                  'https://res.cloudinary.com/delkyrtji/image/upload/v1744120615/pngimg.com_-_iphone16_PNG37_meffth.png'
               }
               alt="iphone"
               width={500}
               height={500}
            />
         </div>
      </div>
   );
}
