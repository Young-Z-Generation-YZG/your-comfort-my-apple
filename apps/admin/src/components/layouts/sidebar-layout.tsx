'use client';

import * as React from 'react';
import {
   Command,
   LifeBuoy,
   Send,
   MonitorSmartphone,
   UsersRound,
   ChartNoAxesCombined,
   TabletSmartphone,
   Tag,
   ScrollText,
   FileUser,
   IdCard,
   UserPen,
   Building2,
} from 'lucide-react';

import {
   Sidebar,
   SidebarContent,
   SidebarFooter,
   SidebarHeader,
   SidebarMenu,
   SidebarMenuButton,
   SidebarMenuItem,
} from '@components/ui/sidebar';
import { DashboardNav } from './sidebar-navigation/dashboard-nav';
import { MainNav } from './sidebar-navigation/main-nav';

// import { NavSecondary } from '@components/ui/nav-secondary';
// import { NavUser } from '@components/ui/nav-user';
// import { NavEmployees } from './nav-employees';

const data = {
   user: {
      name: 'Bách Lê',
      email: 'lxbach1608@gmail.com',
      avatar: '',
   },
   navMain: [
      {
         title: 'Online Shop',
         url: '/dashboards/online/orders',
         icon: ScrollText,
         isActive: true,
         items: [
            {
               title: 'Orders List',
               url: '/dashboards/online/orders',
               icon: ScrollText,
            },
         ],
      },
      {
         title: 'Tenant Management',
         url: '/dashboards/tenants',
         icon: Building2,
         isActive: true,
         items: [],
      },
      //   {
      //      title: 'iPhone Management',
      //      url: '#',
      //      icon: TabletSmartphone,
      //      isActive: true,
      //      items: [
      //         // {
      //         //    title: 'Analytics',
      //         //    url: '/dashboards/products/analytics',
      //         // },
      //         {
      //            title: 'Model Management',
      //            url: '/dashboards/iphone/models',
      //            items: [
      //               {
      //                  title: 'iPhone 15',
      //                  url: '/dashboards/iphone/models/iphone-14-pro',
      //               },
      //               {
      //                  title: 'iPhone 16e',
      //                  url: '/dashboards/iphone/models/iphone-14-pro-max',
      //               },
      //            ],
      //         },
      //      ],
      //   },
      //   {
      //      title: 'Promotions Management',
      //      url: '#',
      //      icon: Tag,
      //      isActive: true,
      //      items: [
      //         // {
      //         //    title: 'Analytics',
      //         //    url: '#',
      //         // },
      //         {
      //            title: 'Promotion Events',
      //            url: '/dashboards/promotions/events',
      //         },
      //         {
      //            title: 'Promotion iPhone',
      //            url: '/dashboards/promotions/items',
      //         },
      //         {
      //            title: 'Promotion Coupon',
      //            url: '/dashboards/promotions/coupons',
      //         },
      //      ],
      //   },
      {
         title: 'Order Management',
         url: '/dashboards/orders',
         icon: ScrollText,
         isActive: true,
         items: [
            {
               title: 'Orders List',
               url: '#',
               icon: ScrollText,
            },
         ],
      },
   ],
   navSecondary: [
      {
         title: 'Support',
         url: '#',
         icon: LifeBuoy,
      },
      {
         title: 'Feedback',
         url: '#',
         icon: Send,
      },
   ],
   dashboards: [
      {
         name: 'Revenue Analytics',
         url: '/dashboards/revenue-analytics',
         icon: ChartNoAxesCombined,
      },
      {
         name: 'User Reports',
         url: '/dashboards/user-reports',
         icon: UsersRound,
      },
      {
         name: "User'agent Reports",
         url: '/dashboards/user-agent-reports',
         icon: MonitorSmartphone,
      },
   ],
   employees: [
      {
         name: 'Employee tables',
         url: '#',
         icon: FileUser,
      },
      {
         name: 'Authorizations',
         url: '#',
         icon: IdCard,
      },
      {
         name: 'Managements',
         url: '#',
         icon: UserPen,
      },
   ],
};

export function SidebarLayout({
   ...props
}: React.ComponentProps<typeof Sidebar>) {
   return (
      <Sidebar variant="inset" {...props}>
         <SidebarHeader>
            <SidebarMenu>
               <SidebarMenuItem>
                  <SidebarMenuButton size="lg" asChild>
                     <a href="#">
                        <div className="flex aspect-square size-8 items-center justify-center rounded-lg bg-sidebar-primary text-sidebar-primary-foreground">
                           <Command className="size-4" />
                        </div>
                        <div className="grid flex-1 text-left text-sm leading-tight">
                           <span className="truncate font-semibold">
                              Acme Inc
                           </span>
                           <span className="truncate text-xs">Enterprise</span>
                        </div>
                     </a>
                  </SidebarMenuButton>
               </SidebarMenuItem>
            </SidebarMenu>
         </SidebarHeader>
         <SidebarContent>
            <DashboardNav dashboards={data.dashboards} />
            <MainNav items={data.navMain} />
            {/* <NavEmployees employees={data.employees} /> */}
            {/* <NavSecondary items={data.navSecondary} className="mt-auto" /> */}
         </SidebarContent>
         <SidebarFooter>{/* <NavUser user={data.user} /> */}</SidebarFooter>
      </Sidebar>
   );
}
