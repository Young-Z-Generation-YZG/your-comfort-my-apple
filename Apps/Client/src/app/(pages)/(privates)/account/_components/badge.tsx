import { cn } from '~/infrastructure/lib/utils';

interface BadgeProps {
   className?: string;
   variants?: 'default' | 'success' | 'warning' | 'error' | 'info' | 'enabled';
}

const variantStyles = {
   default: 'bg-blue-100/50 text-blue-700',
   success: 'bg-green-100/50 text-green-700',
   warning: 'bg-yellow-100/50 text-yellow-700',
   error: 'bg-red-100/50 text-red-700',
   info: 'bg-sky-100/50 text-sky-700',
   enabled: 'bg-green-100/50 text-green-700',
};

const Badge = ({ className = '', variants = 'default' }: BadgeProps) => {
   return (
      <span
         className={cn(
            'text-blue-700 px-2 py-1 h-fit text-xs rounded-full bg-blue-100/50 font-SFProText select-none',
            variantStyles[variants],
            className,
         )}
      >
         {variants.charAt(0).toUpperCase() + variants.slice(1)}
      </span>
   );
};

export default Badge;
