import { ReactNode } from 'react';
import { cn } from '~/infrastructure/lib/utils';

interface ButtonProps {
   onClick?: () => void;
   className?: string;
   children?: ReactNode;
}

const Button = ({ onClick, className = '', children }: ButtonProps) => {
   return (
      <div className={cn('button', className)} onClick={onClick}>
         <button>{children}</button>
      </div>
   );
};

export default Button;
