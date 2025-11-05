'use client';

import { Fragment } from 'react';
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

const DashboardLayout = ({
   children,
}: Readonly<{
   children: React.ReactNode;
}>) => {
   const isLoading = useAppSelector((state: RootState) => state.app.isLoading);

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
