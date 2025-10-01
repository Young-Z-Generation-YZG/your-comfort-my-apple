import images from '@components/client/images';
import { StaticImageData } from 'next/image';

interface AppleService {
   icon: StaticImageData | string;
   title: string;
   description: string;
}

export const appleServices: AppleService[] = [
   {
      icon: images.serviceTV,
      title: 'Apple TV+',
      description: '3 free months of original films and series.°°°',
   },
   {
      icon: images.serviceFitness,
      title: 'Apple Fitness+',
      description: '3 free months of workouts, from HIIT to Meditation.°°°',
   },
   {
      icon: images.serviceArcade,
      title: 'Apple Arcade',
      description:
         '3 free months of incredibly fun, uninterrupted gameplay.°°°',
   },
   {
      icon: images.serviceNews,
      title: 'Apple News+',
      description: '3 free months of top stories from leading publications.°°°',
   },
];
