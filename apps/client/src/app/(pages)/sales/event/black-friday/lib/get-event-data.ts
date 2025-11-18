const EVENT_ID = '611db6eb-3d64-474e-9e23-3517ad0df6ec';
// Full API endpoint as provided - includes the base URL
const API_ENDPOINT = 'https://dc7a69b7406b.ngrok-free.app';

export type TEventItem = {
   id: string;
   event_id: string;
   sku_id: string;
   tenant_id: string;
   branch_id: string;
   model_name: string;
   normalized_model: string;
   color_name: string;
   normalized_color: string;
   color_hex_code: string;
   storage_name: string;
   normalized_storage: string;
   product_classification: string;
   image_url: string;
   discount_type: string;
   discount_value: number; // Percentage as number (e.g., 11 for 11%)
   discount_amount: number;
   original_price: number;
   final_price: number;
   stock: number;
   sold: number;
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

export type TEvent = {
   id: string;
   title: string;
   description: string;
   start_date: string;
   end_date: string;
   event_items: TEventItem[];
   created_at: string;
   updated_at: string;
   updated_by: string | null;
   is_deleted: boolean;
   deleted_at: string | null;
   deleted_by: string | null;
};

/**
 * Server-side function to fetch event data
 * This is used for SSR and SEO optimization
 * Returns the event data directly (not wrapped)
 */
export async function getEventData(): Promise<TEvent | null> {
   // Try direct URL first (as provided by user)
   const directUrl = `${API_ENDPOINT}/api/v1/promotions/events/${EVENT_ID}`;

   // Also try with catalog-services prefix (in case it's needed)
   const urlWithService = `${API_ENDPOINT}/catalog-services/api/v1/promotions/events/${EVENT_ID}`;

   let lastError: any = null;

   // Try both URLs
   for (const attemptUrl of [directUrl, urlWithService]) {
      try {
         console.log('[getEventData] Attempting fetch from:', attemptUrl);

         const controller = new AbortController();
         const timeoutId = setTimeout(() => controller.abort(), 10000); // 10 second timeout

         const response = await fetch(attemptUrl, {
            method: 'GET',
            headers: {
               'Content-Type': 'application/json',
               'ngrok-skip-browser-warning': 'true',
               Accept: 'application/json',
            },
            // Disable caching temporarily to debug
            cache: 'no-store',
            signal: controller.signal,
         });

         clearTimeout(timeoutId);

         console.log('[getEventData] Response received from:', attemptUrl);
         console.log('[getEventData] Response status:', response.status);
         console.log('[getEventData] Response ok:', response.ok);

         if (!response.ok) {
            const errorText = await response.text();
            console.warn('[getEventData] Failed response:', {
               status: response.status,
               statusText: response.statusText,
               error: errorText.substring(0, 200), // Limit error text length
               url: attemptUrl,
            });

            // If this is the first attempt, try the second URL
            if (attemptUrl === directUrl) {
               lastError = {
                  status: response.status,
                  statusText: response.statusText,
                  error: errorText,
               };
               continue;
            }

            // If both failed, return null
            return null;
         }

         const data = await response.json();
         console.log('[getEventData] ✅ Successfully fetched data:', {
            id: data?.id,
            title: data?.title,
            itemsCount: data?.event_items?.length || 0,
            url: attemptUrl,
         });

         return data as TEvent;
      } catch (error) {
         if (error instanceof Error && error.name === 'AbortError') {
            console.error(
               '[getEventData] Request timeout after 10 seconds for:',
               attemptUrl,
            );
            lastError = { type: 'timeout', url: attemptUrl };
            // Try next URL if available
            if (attemptUrl === directUrl) continue;
         } else {
            console.error('[getEventData] Error fetching from:', attemptUrl, {
               error: error instanceof Error ? error.message : String(error),
               errorType:
                  error instanceof Error
                     ? error.constructor.name
                     : typeof error,
            });
            lastError = error;
            // Try next URL if available
            if (attemptUrl === directUrl) continue;
         }
      }
   }

   // If we get here, both attempts failed
   console.error(
      '[getEventData] ❌ All fetch attempts failed. Last error:',
      lastError,
   );
   return null;
}
