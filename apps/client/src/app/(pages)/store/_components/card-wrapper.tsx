import { cn } from '~/infrastructure/lib/utils';

interface CardWrapperProps {
   className?: string;
   children: React.ReactNode;
}

const CardWrapper = ({ children, className = '' }: CardWrapperProps) => {
   return (
      <div
         className={cn(
            'bg-white rounded-2xl overflow-hidden shadow-sm transition-all hover:shadow-md',
            className,
         )}
      >
         {children}
      </div>
   );
};

export default CardWrapper;
