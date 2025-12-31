import { TEvent } from '~/domain/types/catalog.type';
import envConfig from '~/infrastructure/config/env.config';
import https from 'https';

const EVENT_ID = '611db6eb-3d64-474e-9e23-3517ad0df6ec';
// Full API endpoint as provided - includes the base URL
const API_ENDPOINT = envConfig.API_ENDPOINT;

/**
 * Check if URL is localhost with HTTPS (likely self-signed certificate)
 */
function isLocalhostHttps(url: string): boolean {
   try {
      const urlObj = new URL(url);
      return (
         urlObj.protocol === 'https:' &&
         (urlObj.hostname === 'localhost' || urlObj.hostname === '127.0.0.1')
      );
   } catch {
      return false;
   }
}

/**
 * Custom fetch function that handles self-signed certificates for localhost HTTPS
 * This is needed because Node.js's native fetch (undici) doesn't support custom agents
 */
async function fetchWithSelfSignedCert(
   url: string,
   options: RequestInit = {},
): Promise<Response> {
   // Only use custom https for localhost HTTPS in development
   if (isLocalhostHttps(url) && process.env.NODE_ENV === 'development') {
      return new Promise((resolve, reject) => {
         const urlObj = new URL(url);
         const requestOptions = {
            hostname: urlObj.hostname,
            port: urlObj.port || 443,
            path: urlObj.pathname + urlObj.search,
            method: options.method || 'GET',
            headers: {
               ...(options.headers as Record<string, string>),
            },
            rejectUnauthorized: false, // Accept self-signed certificates
         };

         const req = https.request(requestOptions, (res) => {
            const chunks: Buffer[] = [];

            res.on('data', (chunk) => {
               chunks.push(chunk);
            });

            res.on('end', () => {
               const body = Buffer.concat(chunks);
               // Convert Node.js response to Fetch API Response
               const response = new Response(body, {
                  status: res.statusCode || 200,
                  statusText: res.statusMessage || 'OK',
                  headers: res.headers as HeadersInit,
               });
               resolve(response);
            });
         });

         req.on('error', (error) => {
            reject(error);
         });

         // Handle AbortController signal
         if (options.signal) {
            options.signal.addEventListener('abort', () => {
               req.destroy();
               reject(
                  new DOMException('The operation was aborted.', 'AbortError'),
               );
            });
         }

         if (options.body) {
            req.write(options.body);
         }

         req.end();
      });
   }

   // Use native fetch for all other cases
   return fetch(url, options);
}

/**
 * Server-side function to fetch event data
 * This is used for SSR and SEO optimization
 * Returns the event data directly (not wrapped)
 *
 * Note: For localhost HTTPS with self-signed certificates in development,
 * you need to set NODE_TLS_REJECT_UNAUTHORIZED=0 in your environment
 * or use HTTP instead of HTTPS for local development.
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

         const response = await fetchWithSelfSignedCert(attemptUrl, {
            method: 'GET',
            headers: {
               'Content-Type': 'application/json',
               'ngrok-skip-browser-warning': 'true',
               Accept: 'application/json',
            },
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
