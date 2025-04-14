'use client';

import { persistor, reduxStore } from './store';
import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';

export const ReduxProvider = ({ children }: { children: React.ReactNode }) => {
   return (
      <Provider store={reduxStore}>
         <PersistGate loading={null} persistor={persistor}>
            {children}
         </PersistGate>
      </Provider>
   );
};
