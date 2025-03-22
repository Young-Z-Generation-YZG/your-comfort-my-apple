/* eslint-disable react/react-in-jsx-scope */
import { SFDisplayFont } from '../../assets/fonts/font.config';
import { cn } from '~/infrastructure/lib/utils';
import { cookies } from 'next/headers';
import { redirect } from 'next/navigation'

export default function Home() {
   async function create() {
      'use server';

      // const store = cookies();
      // store.set('access-token', '1234'); // Setting the cookie inside a server action

   }

   // redirect("/home");

   return (
      <div className={cn('')}>
         <p className="font-SFProText">
            Lorem ipsum dolor sit amet consectetur adipisicing elit. Eveniet
            provident tenetur officia cupiditate ratione, suscipit ea, magni
            sint iure aut, aliquam repellendus repellat cum quo laudantium
            soluta! Similique, cum corporis?
         </p>

         <p className="">
            Lorem ipsum dolor sit amet consectetur adipisicing elit. Eveniet
            provident tenetur officia cupiditate ratione, suscipit ea, magni
            sint iure aut, aliquam repellendus repellat cum quo laudantium
            soluta! Similique, cum corporis?
         </p>

         <div className='font-SFProText mt-5'>
         <p>
            Lorem ipsum dolor sit amet consectetur adipisicing elit. Eveniet
            provident tenetur officia cupiditate ratione, suscipit ea, magni
            sint iure aut, aliquam repellendus repellat cum quo laudantium
            soluta! Similique, cum corporis?
         </p>
         </div>
      </div>
   );
}
