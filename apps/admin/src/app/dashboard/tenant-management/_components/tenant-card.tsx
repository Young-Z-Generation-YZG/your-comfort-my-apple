import { Badge } from '@components/ui/badge';
import { Button } from '@components/ui/button';
import { Building2, Calendar, CheckCircle2, MapPin } from 'lucide-react';
import Link from 'next/link';
import { ETenantType } from '~/src/domain/enums/tenant-type.enum';
import { cn } from '~/src/infrastructure/lib/utils';
import { TTenant } from '~/src/domain/types/catalog.type';

const getTenantTypeStyle = (tenantType: string) => {
   switch (tenantType) {
      case ETenantType.WAREHOUSE:
         return 'bg-purple-100 text-purple-700 border-purple-300 hover:bg-purple-600 hover:text-white hover:border-purple-600 transition-colors';
      case ETenantType.BRANCH:
         return 'bg-blue-100 text-blue-700 border-blue-300 hover:bg-blue-600 hover:text-white hover:border-blue-600 transition-colors';
      default:
         return 'bg-gray-100 text-gray-700 border-gray-300 hover:bg-gray-600 hover:text-white hover:border-gray-600 transition-colors';
   }
};

const getTenantTypeTextStyle = (tenantType: string) => {
   switch (tenantType) {
      case ETenantType.WAREHOUSE:
         return 'text-purple-700';
      case ETenantType.BRANCH:
         return 'text-blue-700';
      default:
         return 'text-gray-700';
   }
};

const getTenantStateStyle = (tenantState: string) => {
   switch (tenantState) {
      case 'ACTIVE':
         return 'bg-green-100 text-green-700 border-green-300 hover:bg-green-600 hover:text-white hover:border-green-600 transition-colors';
      case 'INACTIVE':
         return 'bg-gray-100 text-gray-700 border-gray-300 hover:bg-gray-600 hover:text-white hover:border-gray-600 transition-colors';
      default:
         return 'bg-gray-100 text-gray-700 border-gray-300 hover:bg-gray-600 hover:text-white hover:border-gray-600 transition-colors';
   }
};

const TenantCard = ({ tenant }: { tenant: TTenant }) => {
   return (
      <div className="h-auto w-auto">
         <div className="p-4 rounded-xl border border-slate-200 shadow">
            <div className="flex flex-col gap-4">
               <div className="flex items-center gap-5">
                  <span
                     className={cn('p-3 rounded-lg', {
                        'bg-purple-100 dark:bg-purple-900/30':
                           tenant.tenant_type === ETenantType.WAREHOUSE,
                        'bg-blue-100 dark:bg-blue-900/30':
                           tenant.tenant_type === ETenantType.BRANCH,
                     })}
                  >
                     {tenant.tenant_type === ETenantType.WAREHOUSE && (
                        <Building2 className="h-6 w-6 text-purple-600 dark:text-purple-400" />
                     )}

                     {tenant.tenant_type === ETenantType.BRANCH && (
                        <MapPin className="h-6 w-6 text-blue-600 dark:text-blue-400" />
                     )}
                  </span>
                  <div className="flex flex-col gap-2">
                     <h3
                        className={cn(
                           'font-semibold text-lg leading-none',
                           getTenantTypeTextStyle(tenant.tenant_type),
                        )}
                     >
                        {tenant.name}
                     </h3>
                     <p className="text-xs font-mono text-muted-foreground bg-slate-100 dark:bg-slate-800 px-2 py-1 rounded w-fit">
                        {tenant.tenant_type}
                     </p>
                  </div>
               </div>

               <div>
                  <p className="text-sm text-muted-foreground line-clamp-2 min-h-[2.5rem]">
                     {tenant.description}
                  </p>
               </div>

               <div className="flex items-center gap-2">
                  <Badge
                     className={cn(
                        'border select-none text-xs font-medium flex items-center gap-1.5 py-1 px-2.5 rounded-lg',
                        getTenantTypeStyle(tenant.tenant_type),
                     )}
                  >
                     {tenant.tenant_type === ETenantType.WAREHOUSE && (
                        <Building2 className="h-3.5 w-3.5" />
                     )}
                     {tenant.tenant_type === ETenantType.BRANCH && (
                        <MapPin className="h-3.5 w-3.5" />
                     )}
                     {tenant.tenant_type}
                  </Badge>

                  <Badge
                     className={cn(
                        'border select-none text-xs font-medium flex items-center gap-1.5 py-1 px-2.5 rounded-lg',
                        getTenantStateStyle(tenant.tenant_state),
                     )}
                  >
                     <CheckCircle2 className="h-3.5 w-3.5" />
                     {tenant.tenant_state}
                  </Badge>
               </div>

               <div className="flex items-center text-xs text-muted-foreground pt-2 border-t border-slate-100 dark:border-slate-800">
                  <Calendar className="h-3.5 w-3.5 mr-1.5" />
                  Created{' '}
                  {new Date(tenant.created_at).toLocaleDateString('en-US', {
                     year: 'numeric',
                     month: 'short',
                     day: 'numeric',
                  })}
               </div>

               <div>
                  <Link href={`#`} className="w-full">
                     <Button variant="outline" className="w-full">
                        View Details
                     </Button>
                  </Link>
               </div>
            </div>
         </div>
      </div>
   );
};

export default TenantCard;
