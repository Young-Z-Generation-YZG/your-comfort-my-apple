import { cn } from '~/infrastructure/lib/utils';
import Badge from './badge';

interface CardContentProps {
   className?: string;
   children: React.ReactNode;
}

const CardContext = ({ children, className = '' }: CardContentProps) => {
   return (
      <div className={cn('bg-white py-5 px-7 rounded-2xl w-full', className)}>
         {children}
      </div>
   );
};

interface DefaultActionContentProps {
   className?: string;
   children: React.ReactNode;
}

const DefaultActionContent = ({
   children,
   className,
}: DefaultActionContentProps) => {
   return (
      <div
         className={cn(
            'flex justify-between w-full border px-5 py-4 rounded-lg hover:bg-slate-100/50 transition-all duration-200 ease-in-out',
            className,
         )}
      >
         <div className="w-full">{children}</div>
      </div>
   );
};

export { CardContext, DefaultActionContent };
