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
            'flex justify-between w-full border px-5 py-4 rounded-lg mt-2',
            className,
         )}
      >
         <div>{children}</div>

         <Badge variants="default" />
      </div>
   );
};

export { CardContext, DefaultActionContent };
