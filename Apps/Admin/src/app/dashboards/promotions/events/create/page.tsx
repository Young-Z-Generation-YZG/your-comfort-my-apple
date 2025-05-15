'use client';

import { Button } from '@components/ui/button';
import {
   Card,
   CardContent,
   CardDescription,
   CardFooter,
   CardHeader,
   CardTitle,
} from '@components/ui/card';
import { ArrowLeft } from 'lucide-react';
import Link from 'next/link';
import { motion } from 'framer-motion';
import PromotionEventForm from '~/src/app/dashboards/promotions/_components/_form/promotion-event-form';

const CreateEventPage = () => {
   return (
      <motion.div
         className="flex flex-col gap-6 p-6"
         initial={{ opacity: 0 }}
         animate={{ opacity: 1 }}
         transition={{ duration: 0.5 }}
      >
         <motion.div
            className="flex items-center gap-2"
            initial={{ opacity: 0, y: -20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.2 }}
         >
            <Button variant="ghost" size="icon" asChild>
               <Link href="/dashboards/promotions/events">
                  <ArrowLeft className="h-4 w-4" />
               </Link>
            </Button>
            <h1 className="text-3xl font-bold tracking-tight">
               New Event Promotion
            </h1>
         </motion.div>

         <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.3 }}
         >
            <Card>
               <CardHeader>
                  <CardTitle>Promotion Details</CardTitle>
                  <CardDescription>
                     Select the type of promotion you want to create and fill in
                     the details.
                  </CardDescription>
               </CardHeader>
               <CardContent>
                  <PromotionEventForm />
               </CardContent>
               <CardFooter className="flex justify-between"></CardFooter>
            </Card>
         </motion.div>
      </motion.div>
   );
};

export default CreateEventPage;
