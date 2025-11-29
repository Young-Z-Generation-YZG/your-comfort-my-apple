'use client';

import { Fragment, useEffect } from 'react';
import {
   Breadcrumb,
   BreadcrumbItem,
   BreadcrumbLink,
   BreadcrumbList,
   BreadcrumbPage,
   BreadcrumbSeparator,
} from '@components/ui/breadcrumb';
import { Separator } from '@components/ui/separator';
import {
   SidebarInset,
   SidebarProvider,
   SidebarTrigger,
} from '@components/ui/sidebar';
import { SidebarLayout } from '@components/layouts/sidebar-layout';
import { ActionNav } from '@components/layouts/sidebar-navigation/actions-nav';
import withAuth from '@components/HoCs/with-auth.hoc';
import { LoadingOverlay } from '@components/loading-overlay';
import { RootState, useAppSelector } from '~/src/infrastructure/redux/store';
import { useDispatch } from 'react-redux';
import { setImpersonatedUser } from '~/src/infrastructure/redux/features/auth.slice';
import { setTenant } from '~/src/infrastructure/redux/features/tenant.slice';

const DashboardLayout = ({
   children,
}: Readonly<{
   children: React.ReactNode;
}>) => {
   const isLoading = useAppSelector((state: RootState) => state.app.isLoading);
   const dispatch = useDispatch();

   useEffect(() => {
      //   let navEntries: PerformanceNavigationTiming[] = [];

      //   if (typeof performance !== 'undefined') {
      //      const entries = performance.getEntriesByType?.('navigation');
      //      if (Array.isArray(entries)) {
      //         navEntries = entries as PerformanceNavigationTiming[];
      //      }
      //   }

      //   const isReload =
      //      navEntries.length > 0
      //         ? navEntries.some(
      //              (entry: PerformanceNavigationTiming) =>
      //                 entry.type === 'reload',
      //           )
      //         : typeof performance !== 'undefined' &&
      //           performance.navigation.type ===
      //              (performance.navigation as PerformanceNavigation)?.TYPE_RELOAD;

      //   if (isReload) {
      //      dispatch(setImpersonatedUser({ impersonatedUser: null }));
      //      dispatch(
      //         setTenant({
      //            tenantId: null,
      //            branchId: null,
      //            tenantSubDomain: null,
      //         }),
      //      );
      //   }

      if (typeof window === 'undefined') {
         return;
      }

      const handleBeforeUnload = () => {
         dispatch(setImpersonatedUser({ impersonatedUser: null }));
         dispatch(
            setTenant({
               tenantId: null,
               branchId: null,
               tenantSubDomain: null,
            }),
         );
      };

      window.addEventListener('beforeunload', handleBeforeUnload);

      return () => {
         window.removeEventListener('beforeunload', handleBeforeUnload);
      };
   }, [dispatch]);

   return (
      <Fragment>
         <LoadingOverlay
            isLoading={isLoading}
            fullScreen={true}
            text="Switching tenant/user..."
         />
         <SidebarProvider>
            <SidebarLayout />
            <SidebarInset>
               <header className="flex h-16 shrink-0 items-center gap-2">
                  <div className="flex items-center gap-2 px-4">
                     <SidebarTrigger className="-ml-1" />
                     <Separator orientation="vertical" className="mr-2 h-4" />
                     <Breadcrumb>
                        <BreadcrumbList>
                           <BreadcrumbItem className="hidden md:block">
                              <BreadcrumbLink href="#">
                                 Building Your Application
                              </BreadcrumbLink>
                           </BreadcrumbItem>
                           <BreadcrumbSeparator className="hidden md:block" />
                           <BreadcrumbItem>
                              <BreadcrumbPage>Data Fetching</BreadcrumbPage>
                           </BreadcrumbItem>
                        </BreadcrumbList>
                     </Breadcrumb>
                  </div>
                  <div className="ml-auto px-3">
                     <ActionNav />
                  </div>
               </header>
               <main>{children}</main>
            </SidebarInset>
         </SidebarProvider>
      </Fragment>
   );
};

export default withAuth(DashboardLayout);
