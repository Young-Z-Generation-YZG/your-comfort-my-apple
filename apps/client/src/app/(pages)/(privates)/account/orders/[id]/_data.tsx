// Order type definition
type OrderItem = {
   id: string;
   name: string;
   image: string;
   price: string;
   quantity: number;
   options?: string;
};

type OrderDetails = {
   id: string;
   orderNumber: string;
   date: string;
   status: 'processing' | 'shipped' | 'delivered' | 'canceled';
   total: string;
   subtotal: string;
   tax: string;
   shipping: string;
   items: OrderItem[];
   shippingAddress: {
      name: string;
      street: string;
      city: string;
      state: string;
      zip: string;
      country: string;
   };
   paymentMethod: string;
   trackingNumber?: string;
   estimatedDelivery?: string;
};

// Sample order details
const sampleOrderDetails: Record<string, OrderDetails> = {
   '1': {
      id: '1',
      orderNumber: 'W12345678',
      date: 'Apr 12, 2023',
      status: 'delivered',
      total: '$2,399.00',
      subtotal: '$2,199.00',
      tax: '$175.92',
      shipping: '$24.08',
      items: [
         {
            id: '1',
            name: 'MacBook Air 13-inch',
            image: '/placeholder.svg?height=80&width=80',
            price: '$1,199.00',
            quantity: 1,
            options: 'M2 chip, 8GB RAM, 256GB SSD, Space Gray',
         },
         {
            id: '2',
            name: 'Apple AirPods Pro',
            image: '/placeholder.svg?height=80&width=80',
            price: '$249.00',
            quantity: 1,
         },
         {
            id: '3',
            name: 'AppleCare+ for MacBook Air',
            image: '/placeholder.svg?height=80&width=80',
            price: '$199.00',
            quantity: 1,
         },
      ],
      shippingAddress: {
         name: 'John Doe',
         street: '123 Apple Street',
         city: 'Cupertino',
         state: 'CA',
         zip: '95014',
         country: 'United States',
      },
      paymentMethod: 'Visa ending in 4242',
      trackingNumber: '1Z999AA10123456784',
      estimatedDelivery: 'Delivered on Apr 15, 2023',
   },
   '2': {
      id: '2',
      orderNumber: 'W12345679',
      date: 'Mar 28, 2023',
      status: 'shipped',
      total: '$129.00',
      subtotal: '$119.00',
      tax: '$9.52',
      shipping: '$0.48',
      items: [
         {
            id: '1',
            name: 'Apple Pencil (2nd Generation)',
            image: '/placeholder.svg?height=80&width=80',
            price: '$129.00',
            quantity: 1,
         },
      ],
      shippingAddress: {
         name: 'John Doe',
         street: '123 Apple Street',
         city: 'Cupertino',
         state: 'CA',
         zip: '95014',
         country: 'United States',
      },
      paymentMethod: 'Visa ending in 4242',
      trackingNumber: '1Z999AA10123456785',
      estimatedDelivery: 'Expected delivery on Apr 18, 2023',
   },
   '8': {
      id: '8',
      orderNumber: 'W12345685',
      date: 'Apr 15, 2023',
      status: 'processing',
      total: '$1,599.00',
      subtotal: '$1,499.00',
      tax: '$119.92',
      shipping: '$0.00',
      items: [
         {
            id: '1',
            name: 'iPhone 14 Pro',
            image: '/placeholder.svg?height=80&width=80',
            price: '$999.00',
            quantity: 1,
            options: '128GB, Deep Purple',
         },
         {
            id: '2',
            name: 'iPhone 14 Pro Leather Case',
            image: '/placeholder.svg?height=80&width=80',
            price: '$59.00',
            quantity: 1,
            options: 'Deep Purple',
         },
         {
            id: '3',
            name: 'AppleCare+ for iPhone 14 Pro',
            image: '/placeholder.svg?height=80&width=80',
            price: '$199.00',
            quantity: 1,
         },
      ],
      shippingAddress: {
         name: 'John Doe',
         street: '123 Apple Street',
         city: 'Cupertino',
         state: 'CA',
         zip: '95014',
         country: 'United States',
      },
      paymentMethod: 'Visa ending in 4242',
      estimatedDelivery: 'Expected to ship in 1-2 business days',
   },
};
