import { ReactNode } from 'react';
import { cn } from '~/infrastructure/lib/utils';

interface ButtonProps {
   onClick?: () => void;
   disabled?: boolean;
   className?: string;
   children?: ReactNode;
}

const Button = ({
   onClick,
   className = '',
   disabled = false,
   children,
}: ButtonProps) => {
   return (
      <div
         className={cn(
            'button flex justify-center items-center duration-200 ease-in-out transition-all cursor-pointer',
            className,
            disabled ? 'bg-blue-400 cursor-auto active:bg-blue-400' : null,
         )}
         onClick={onClick}
      >
         <button disabled={disabled}>{children}</button>
      </div>
   );
};

export default Button;
