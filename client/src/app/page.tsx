/* eslint-disable react/react-in-jsx-scope */
import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
import '~/styles/globals.css';
import { cookies } from 'next/headers';

export default function Home() {
   async function create() {
      'use server';

      const store = cookies();
      store.set('access-token', '1234'); // Setting the cookie inside a server action
   }

   return (
      <form action={create}>
         <button type="submit">Create</button>
      </form>
   );

   // return (
   //    <div className={cn(SFDisplayFont.variable, 'font-SFProDisplay')}>
   //       TEST
   //       <p className="font-SFProDisplay">
   //          Lorem ipsum dolor sit amet consectetur adipisicing elit. Eveniet
   //          provident tenetur officia cupiditate ratione, suscipit ea, magni
   //          sint iure aut, aliquam repellendus repellat cum quo laudantium
   //          soluta! Similique, cum corporis?
   //       </p>
   //    </div>
   // );
}
