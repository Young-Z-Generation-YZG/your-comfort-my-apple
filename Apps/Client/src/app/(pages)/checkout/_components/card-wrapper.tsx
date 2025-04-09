import { cn } from '~/infrastructure/lib/utils';

interface CardWrapperProps {
   children: React.ReactNode;
   className?: string;
   onClick?: () => void;
   onMouseEnter?: () => void;
   onMouseLeave?: () => void;
}

const CardWrapper = ({ children, className }: CardWrapperProps) => {
   return (
      <div
         className={cn(
            'bg-white rounded-lg shadow-sm p-6 border border-gray-200',
            className,
         )}
      >
         {children}
      </div>
   );
};

export default CardWrapper;
