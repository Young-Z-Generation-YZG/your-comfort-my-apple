/* eslint-disable react/react-in-jsx-scope */
'use client';
import { Provider } from 'react-redux';
import { store } from '~/redux/store';

export default function CartLayout({
   children,
}: Readonly<{
   children: React.ReactNode;
}>) {
   return (
      <html lang="en">
         <Provider store={store}>
            <body className={`antialiased`}>
               <h1>Cart layout</h1>
               {children}
            </body>
         </Provider>
      </html>
   );
}
