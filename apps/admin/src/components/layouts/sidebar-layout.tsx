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
   Users,
   User,
   Box,
   Warehouse,
   Laptop,
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
import { NavUser } from '@components/ui/nav-user';
import { useAppSelector } from '~/src/infrastructure/redux/store';
import { TenantSwitcher } from '@components/ui/tenant-switcher';
// import { NavEmployees } from './nav-employees';

const data = {
   user: {
      name: 'Bách Lê',
      email: 'lxbach1608@gmail.com',
      avatar: '',
   },
   tenants: [
      {
         tenantId: null,
         branchId: '664357a235e84033bbd0e6b6',
         tenantCode: 'WARE_HOUSE',
         tenantName: 'Ware house',
         tenantType: 'WARE_HOUSE',
         tenantAddress: '123 Nguyen Van Linh, Q9, TP.HCM',
      },
      {
         tenantId: 'hcm-td-kvc-01',
         branchId: '664357a235e84033bbd0e6b7',
         tenantCode: 'HCM_TD_KVC_01',
         tenantName: 'HCM TD KVC 01',
         tenantType: 'BRANCH',
         tenantAddress: 'Số 1060, Kha Vạn Cân, Linh Chiểu, Thủ Đức',
      },
   ],
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
         url: '/dashboards/tenant-management',
         icon: Building2,
         isActive: true,
         items: [
            {
               title: 'iPhone',
               url: '/dashboards/tenant-management/iphone',
               icon: Box,
            },
            {
               title: 'Mac',
               url: '/dashboards/tenant-management/mac',
               icon: Laptop,
            },
            {
               title: 'Skus',
               url: '/dashboards/tenant-management/skus',
               icon: Tag,
            },
         ],
      },
      {
         title: 'Warehouses',
         url: '/dashboards/warehouses',
         icon: Warehouse,
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
         title: 'HRM',
         url: '/dashboards/hrm',
         icon: Users,
         isActive: true,
      },
      {
         title: 'Customer Management',
         url: '/dashboards/customer-management',
         icon: UsersRound,
         isActive: true,
      },
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
      //   {
      //      name: 'User Reports',
      //      url: '/dashboards/user-reports',
      //      icon: UsersRound,
      //   },
      //   {
      //      name: "User'agent Reports",
      //      url: '/dashboards/user-agent-reports',
      //      icon: MonitorSmartphone,
      //   },
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
   const { currentUser } = useAppSelector((state) => state.auth);

   return (
      <Sidebar variant="inset" {...props}>
         <SidebarHeader>
            <SidebarMenu>
               <SidebarMenuItem>
                  <TenantSwitcher tenants={data.tenants} />
               </SidebarMenuItem>
            </SidebarMenu>
         </SidebarHeader>
         <SidebarContent>
            <DashboardNav dashboards={data.dashboards} />
            <MainNav items={data.navMain} />
            {/* <NavEmployees employees={data.employees} /> */}
            {/* <NavSecondary items={data.navSecondary} className="mt-auto" /> */}
         </SidebarContent>
         <SidebarFooter>
            <NavUser
               user={{
                  name: currentUser?.username || '',
                  email: currentUser?.userEmail || '',
                  avatar: '',
               }}
            />
         </SidebarFooter>
      </Sidebar>
   );
}
