'use client';

import useFilters from '~/src/hooks/use-filter';

type TOrderFilter = {
   _page?: number | null;
   _limit?: number | null;
   _orderStatus?: string[] | null;
   _customerEmail?: string | null;
};

const IphonesPage = () => {
   const { filters, setFilters } = useFilters<TOrderFilter>({
      _page: 'number',
      _limit: 'number',
      _orderStatus: { array: 'string' },
      _customerEmail: 'string',
   });

   console.log('filters', filters);

   return (
      <div>
         <h1>iPhones</h1>
      </div>
   );
};

export default IphonesPage;
