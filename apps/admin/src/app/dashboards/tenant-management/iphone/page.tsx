'use client';

import useCustomFilters from '~/src/hooks/use-custom-filter';

const IphonesPage = () => {
   const { filters, setFilters } = useCustomFilters();

   console.log('filters', filters);

   return (
      <div>
         <h1>iPhones</h1>
      </div>
   );
};

export default IphonesPage;
