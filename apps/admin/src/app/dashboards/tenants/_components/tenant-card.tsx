import { Badge } from '@components/ui/badge';
import { Button } from '@components/ui/button';
import { Building2, Calendar, CheckCircle2, MapPin } from 'lucide-react';
import Link from 'next/link';
import { ETenantType } from '~/src/domain/enums/tenant-type.enum';
import { cn } from '~/src/infrastructure/lib/utils';
import { TTenant } from '../page';

const TenantCard = ({ tenant }: { tenant: TTenant }) => {
   return (
      <div className="h-auto w-auto">
         <div className="p-4 rounded-xl border border-slate-200 shadow">
            <div className="flex flex-col gap-4">
               <div className="flex items-center gap-5">
                  <span
                     className={cn('p-3 rounded-lg', {
                        'bg-purple-100 dark:bg-purple-900/30':
                           tenant.tenant_type === ETenantType.WARE_HOUSE,
                        'bg-blue-100 dark:bg-blue-900/30':
                           tenant.tenant_type === ETenantType.BRANCH,
                     })}
                  >
                     {tenant.tenant_type === ETenantType.WARE_HOUSE && (
                        <Building2 className="h-6 w-6 text-purple-600 dark:text-purple-400" />
                     )}

                     {tenant.tenant_type === ETenantType.BRANCH && (
                        <MapPin className="h-6 w-6 text-blue-600 dark:text-blue-400" />
                     )}
                  </span>
                  <span>
                     <h3 className="font-semibold text-lg leading-none mb-1">
                        Ware house
                     </h3>
                     <p className="text-xs font-mono text-muted-foreground bg-slate-100 dark:bg-slate-800 px-2 py-1 rounded w-fit">
                        WARE_HOUSE
                     </p>
                  </span>
               </div>

               <div>
                  <p className="text-sm text-muted-foreground line-clamp-2 min-h-[2.5rem]">
                     Main warehouse facility
                  </p>
               </div>

               <div className="flex items-center gap-2">
                  <Badge
                     className={cn(
                        'border select-none text-xs font-medium flex items-center gap-1.5 py-1 px-2.5 rounded-lg',
                        'bg-purple-50 text-purple-700 border-purple-200',
                     )}
                  >
                     <Building2 className="h-3.5 w-3.5" />
                     WARE_HOUSE
                  </Badge>

                  <Badge
                     className={cn(
                        'border select-none text-xs font-medium flex items-center gap-1.5 py-1 px-2.5 rounded-lg',
                        'bg-green-50 text-green-700 border-green-200',
                     )}
                  >
                     <CheckCircle2 className="h-3.5 w-3.5" />
                     ACTIVE
                  </Badge>
               </div>

               <div className="flex items-center text-xs text-muted-foreground pt-2 border-t border-slate-100 dark:border-slate-800">
                  <Calendar className="h-3.5 w-3.5 mr-1.5" />
                  Created{' '}
                  {new Date('2024-05-14T10:30:00Z').toLocaleDateString(
                     'en-US',
                     {
                        year: 'numeric',
                        month: 'short',
                        day: 'numeric',
                     },
                  )}
               </div>

               <div>
                  <Link
                     href={`/dashboards/tenants/664355f845e56534956be32b`}
                     className="w-full"
                  >
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
