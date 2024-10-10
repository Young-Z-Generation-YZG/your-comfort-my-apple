/* eslint-disable react/react-in-jsx-scope */
'use client';

import { useEffect } from 'react';
import { useGetUsersQuery } from '~/services/example/user.service';

const Test = () => {
   const { data } = useGetUsersQuery('');

   useEffect(() => {
      console.log(data);
   }, [data]);

   return (
      <div>
         <h1>Client call</h1>
      </div>
   );
};

export default Test;
