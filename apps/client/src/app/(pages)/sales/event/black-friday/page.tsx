import type { Metadata } from 'next';
import { getEventData, TEvent, TEventItem } from './lib/get-event-data';
import EventContent from './_components/event-content';

/**
 * Generate dynamic metadata for SEO
 * This runs on the server and provides metadata to search engines
 */
export async function generateMetadata(): Promise<Metadata> {
   const eventData = await getEventData();

   if (!eventData) {
      return {
         title: 'Black Friday Special - YB Store',
         description: 'Limited-time savings on your favorite Apple products.',
      };
   }

   const title = `${eventData.title} - YB Store`;
   const description =
      eventData.description ||
      'Limited-time savings on your favorite Apple products.';

   // Get banner image from first event item if available
   const bannerImage =
      eventData.event_items?.[0]?.image_url ||
      'https://via.placeholder.com/1200x630?text=Black+Friday+Sale';

   return {
      title,
      description,
      openGraph: {
         title,
         description,
         images: [
            {
               url: bannerImage,
               width: 1200,
               height: 630,
               alt: eventData.title,
            },
         ],
         type: 'website',
         siteName: 'YB Store',
      },
      twitter: {
         card: 'summary_large_image',
         title,
         description,
         images: [bannerImage],
      },
      alternates: {
         canonical: '/sales/event/black-friday',
      },
      keywords: [
         'black friday',
         'sale',
         'discount',
         'apple products',
         'iphone',
         'promotion',
         'limited time offer',
      ],
   };
}

/**
 * Server Component for Black Friday Event Page
 * Fetches data on the server for better SEO and performance
 */
export default async function BlackFridayPage() {
   const eventData = await getEventData();

   // Fallback data if API fails
   const fallbackEvent: TEvent = {
      id: '611db6eb-3d64-474e-9e23-3517ad0df6ec',
      title: 'Black Friday Special',
      description: 'Limited-time savings on your favorite Apple products.',
      start_date: new Date().toISOString(),
      end_date: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString(),
      event_items: [],
      created_at: new Date().toISOString(),
      updated_at: new Date().toISOString(),
      updated_by: null,
      is_deleted: false,
      deleted_at: null,
      deleted_by: null,
   };

   const event = eventData || fallbackEvent;
   const eventItems = event.event_items || [];

   // Generate structured data (JSON-LD) for SEO
   const structuredData = {
      '@context': 'https://schema.org',
      '@type': 'Event',
      name: event.title,
      description: event.description,
      startDate: event.start_date,
      endDate: event.end_date,
      eventStatus: 'https://schema.org/EventScheduled',
      eventAttendanceMode: 'https://schema.org/OfflineEventAttendanceMode',
      location: {
         '@type': 'Place',
         name: 'YB Store',
         address: {
            '@type': 'PostalAddress',
            addressCountry: 'US',
         },
      },
      offers: eventItems.map((item: TEventItem) => ({
         '@type': 'Offer',
         name: `${item.model_name} ${item.color_name} ${item.storage_name}`,
         price: item.final_price,
         priceCurrency: 'USD',
         availability:
            item.stock > 0
               ? 'https://schema.org/InStock'
               : 'https://schema.org/OutOfStock',
         url: `/products/${item.normalized_model.toLowerCase()}`,
         image: item.image_url,
      })),
   };

   return (
      <>
         {/* Structured Data for SEO */}
         <script
            type="application/ld+json"
            dangerouslySetInnerHTML={{
               __html: JSON.stringify(structuredData),
            }}
         />

         {/* Main Content */}
         <EventContent eventData={event} eventItems={eventItems} />
      </>
   );
}
