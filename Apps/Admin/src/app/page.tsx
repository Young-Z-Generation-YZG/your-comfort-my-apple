'use client';

import { useDispatch } from 'react-redux';
import { setAccessToken } from '~/redux/slices/auth.slice';
import { useAppSelector } from '~/redux/store';

export default function Home() {
   const accessToken = useAppSelector((state) => state.auth.value.token);

   const dispatch = useDispatch();

   console.log({ accessToken });

   return (
      <div>
         <h1>Base page</h1>
         <button
            onClick={() => {
               dispatch(setAccessToken('123'));
            }}
         >
            set Token
         </button>
         {accessToken && <p>{accessToken}</p>}
      </div>
   );
}
