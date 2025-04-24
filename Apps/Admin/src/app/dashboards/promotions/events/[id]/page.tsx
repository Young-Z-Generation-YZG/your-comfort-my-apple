'use client';

import { use } from 'react';
import { Button } from '@components/ui/button';
import {
   MotionCard,
   MotionCardContent,
   MotionCardDescription,
   MotionCardHeader,
   MotionCardTitle,
} from '../../_components/animated-card';
import { Badge } from '@components/ui/badge';
import { ArrowLeft, Calendar, Edit, Tag, Ticket } from 'lucide-react';
import Link from 'next/link';
import { motion } from 'framer-motion';

// Mock data for demonstration
const getPromotionById = (id: string) => {
   return {
      id: 'prom_1',
      name: 'Summer Sale 2024',
      type: 'event',
      status: 'active',
      startDate: '2024-06-01',
      endDate: '2024-06-30',
      discount: '20%',
      code: null,
      description:
         'Our biggest summer sale of the year with discounts across all product categories.',
      createdAt: '2024-05-15',
      updatedAt: '2024-05-20',
      createdBy: 'Admin User',
      stats: {
         totalUses: 245,
         totalRevenue: 12500,
         averageOrderValue: 51.02,
      },
   };
};

const EventDetailsPage = ({ params }: { params: Promise<{ id: string }> }) => {
   // Unwrap the params Promise using React.use()
   const resolvedParams = use(params);
   console.log('EventDetailsPage', resolvedParams.id);

   const promotion = getPromotionById(resolvedParams.id);

   const getTypeIcon = (type: string) => {
      switch (type) {
         case 'event':
            return <Calendar className="mr-2 h-5 w-5" />;
         case 'product':
            return <Tag className="mr-2 h-5 w-5" />;
         case 'coupon':
            return <Ticket className="mr-2 h-5 w-5" />;
         default:
            return null;
      }
   };

   const getStatusBadge = (status: string) => {
      switch (status) {
         case 'active':
            return <Badge className="bg-green-500">Active</Badge>;
         case 'scheduled':
            return <Badge className="bg-yellow-500">Scheduled</Badge>;
         case 'draft':
            return <Badge variant="outline">Draft</Badge>;
         case 'expired':
            return <Badge variant="secondary">Expired</Badge>;
         default:
            return null;
      }
   };

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
            <div className="flex-1">
               <h1 className="text-3xl font-bold tracking-tight">
                  {promotion.name}
               </h1>
               <div className="flex items-center gap-2 mt-1">
                  <div className="flex items-center">
                     {getTypeIcon(promotion.type)}
                     <span className="capitalize">
                        {promotion.type} Promotion
                     </span>
                  </div>
                  <span className="text-muted-foreground">•</span>
                  {getStatusBadge(promotion.status)}
               </div>
            </div>
            <Link href={`/dashboards/promotions/events/${promotion.id}/edit`}>
               <Edit className="mr-2 h-4 w-4" />
               Edit
            </Link>
         </motion.div>

         <div className="grid gap-6 md:grid-cols-2">
            <MotionCard delay={0.3}>
               <MotionCardHeader>
                  <MotionCardTitle>Promotion Details</MotionCardTitle>
                  <MotionCardDescription>
                     Basic information about this promotion
                  </MotionCardDescription>
               </MotionCardHeader>
               <MotionCardContent>
                  <dl className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                     <div>
                        <dt className="text-sm font-medium text-muted-foreground">
                           Promotion Type
                        </dt>
                        <dd className="mt-1 text-sm capitalize">
                           {promotion.type}
                        </dd>
                     </div>
                     <div>
                        <dt className="text-sm font-medium text-muted-foreground">
                           Status
                        </dt>
                        <dd className="mt-1 text-sm">
                           {getStatusBadge(promotion.status)}
                        </dd>
                     </div>
                     <div>
                        <dt className="text-sm font-medium text-muted-foreground">
                           Start Date
                        </dt>
                        <dd className="mt-1 text-sm">
                           {new Date(promotion.startDate).toLocaleDateString()}
                        </dd>
                     </div>
                     <div>
                        <dt className="text-sm font-medium text-muted-foreground">
                           End Date
                        </dt>
                        <dd className="mt-1 text-sm">
                           {new Date(promotion.endDate).toLocaleDateString()}
                        </dd>
                     </div>
                     <div>
                        <dt className="text-sm font-medium text-muted-foreground">
                           Discount
                        </dt>
                        <dd className="mt-1 text-sm">{promotion.discount}</dd>
                     </div>
                     <div>
                        <dt className="text-sm font-medium text-muted-foreground">
                           Coupon Code
                        </dt>
                        <dd className="mt-1 text-sm">
                           {promotion.code || 'N/A'}
                        </dd>
                     </div>
                     <div className="sm:col-span-2">
                        <dt className="text-sm font-medium text-muted-foreground">
                           Description
                        </dt>
                        <dd className="mt-1 text-sm">
                           {promotion.description}
                        </dd>
                     </div>
                  </dl>
               </MotionCardContent>
            </MotionCard>

            <MotionCard delay={0.4}>
               <MotionCardHeader>
                  <MotionCardTitle>Performance</MotionCardTitle>
                  <MotionCardDescription>
                     Usage statistics for this promotion
                  </MotionCardDescription>
               </MotionCardHeader>
               <MotionCardContent>
                  <dl className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                     <motion.div
                        initial={{ opacity: 0, scale: 0.9 }}
                        animate={{ opacity: 1, scale: 1 }}
                        transition={{ duration: 0.5, delay: 0.5 }}
                     >
                        <dt className="text-sm font-medium text-muted-foreground">
                           Total Uses
                        </dt>
                        <dd className="mt-1 text-2xl font-semibold">
                           {promotion.stats.totalUses}
                        </dd>
                     </motion.div>
                     <motion.div
                        initial={{ opacity: 0, scale: 0.9 }}
                        animate={{ opacity: 1, scale: 1 }}
                        transition={{ duration: 0.5, delay: 0.6 }}
                     >
                        <dt className="text-sm font-medium text-muted-foreground">
                           Total Revenue
                        </dt>
                        <dd className="mt-1 text-2xl font-semibold">
                           ${promotion.stats.totalRevenue.toLocaleString()}
                        </dd>
                     </motion.div>
                     <motion.div
                        initial={{ opacity: 0, scale: 0.9 }}
                        animate={{ opacity: 1, scale: 1 }}
                        transition={{ duration: 0.5, delay: 0.7 }}
                     >
                        <dt className="text-sm font-medium text-muted-foreground">
                           Average Order Value
                        </dt>
                        <dd className="mt-1 text-2xl font-semibold">
                           ${promotion.stats.averageOrderValue.toFixed(2)}
                        </dd>
                     </motion.div>
                  </dl>
               </MotionCardContent>
            </MotionCard>
         </div>

         <MotionCard delay={0.5}>
            <MotionCardHeader>
               <MotionCardTitle>Promotion History</MotionCardTitle>
               <MotionCardDescription>
                  Creation and modification history
               </MotionCardDescription>
            </MotionCardHeader>
            <MotionCardContent>
               <dl className="grid grid-cols-1 gap-4 sm:grid-cols-3">
                  <div>
                     <dt className="text-sm font-medium text-muted-foreground">
                        Created By
                     </dt>
                     <dd className="mt-1 text-sm">{promotion.createdBy}</dd>
                  </div>
                  <div>
                     <dt className="text-sm font-medium text-muted-foreground">
                        Created At
                     </dt>
                     <dd className="mt-1 text-sm">
                        {new Date(promotion.createdAt).toLocaleString()}
                     </dd>
                  </div>
                  <div>
                     <dt className="text-sm font-medium text-muted-foreground">
                        Last Updated
                     </dt>
                     <dd className="mt-1 text-sm">
                        {new Date(promotion.updatedAt).toLocaleString()}
                     </dd>
                  </div>
               </dl>
            </MotionCardContent>
         </MotionCard>
      </motion.div>
   );
};

export default EventDetailsPage;
