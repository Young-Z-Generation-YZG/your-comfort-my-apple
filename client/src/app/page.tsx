import { SFDisplayFont } from '~/fonts/font.config';
import { cn } from '~/lib/utils';
import '~/styles/globals.css';

import envConfig from '~/config/env.config';

export default function Home() {
   return (
      <div className={cn(SFDisplayFont.variable, 'font-SFProDisplay')}>
         TEST
         <p className="font-SFProDisplay">
            Lorem ipsum dolor sit amet consectetur adipisicing elit. Eveniet
            provident tenetur officia cupiditate ratione, suscipit ea, magni
            sint iure aut, aliquam repellendus repellat cum quo laudantium
            soluta! Similique, cum corporis?
         </p>
      </div>
   );
}
