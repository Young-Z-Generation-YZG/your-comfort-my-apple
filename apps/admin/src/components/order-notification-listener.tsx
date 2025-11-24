'use client';

import { useEffect, useMemo, useRef } from 'react';
import {
   HttpTransportType,
   HubConnection,
   HubConnectionBuilder,
   LogLevel,
} from '@microsoft/signalr';
import { toast } from 'sonner';

import envConfig from '~/src/infrastructure/config/env.config';
import { useAppSelector } from '~/src/infrastructure/redux/store';

type OrderNotificationPayload = {
   orderId: string;
   userId: string;
   status: string;
};

const ORDER_HUB_URL =
   envConfig.ORDERING_NOTIFICATION_HUB ??
   'https://7b1bec6f44c4.ngrok-free.app/ordering-services/orderHub';

const OrderNotificationListener = () => {
   const { currentUser } = useAppSelector((state) => state.auth);

   const accessToken = currentUser?.accessToken ?? undefined;

   const connectionRef = useRef<HubConnection | null>(null);

   useEffect(() => {
      if (!accessToken) {
         if (connectionRef.current) {
            connectionRef.current.stop().catch(() => undefined);
            connectionRef.current = null;
         }
         return;
      }

      const connection = new HubConnectionBuilder()
         .withUrl(ORDER_HUB_URL, {
            accessTokenFactory: () => accessToken,
            skipNegotiation: false, // Allow transport negotiation
            transport:
               HttpTransportType.WebSockets |
               HttpTransportType.ServerSentEvents |
               HttpTransportType.LongPolling, // Explicit fallbacks
         })
         .withAutomaticReconnect()
         .configureLogging(LogLevel.Information) // Bump for debug
         .build();

      connection.on(
         'OrderStatusUpdated',
         (notification: OrderNotificationPayload) => {
            toast.info('Order status updated', {
               description: `Order ${notification.orderId} is now ${notification.status}.`,
            });
         },
      );

      connection
         .start()
         .catch((error) =>
            console.error('[OrderHub] Failed to establish connection', error),
         );

      connectionRef.current = connection;

      return () => {
         connection.off('OrderStatusUpdated');
         connection
            .stop()
            .catch(() => undefined)
            .finally(() => {
               connectionRef.current = null;
            });
      };
   }, [accessToken]);

   return null;
};

export default OrderNotificationListener;
