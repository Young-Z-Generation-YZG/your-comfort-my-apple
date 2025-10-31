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
         tenantId: '664355f845e56534956be32b',
         branchId: '664357a235e84033bbd0e6b6',
         tenantCode: 'WARE_HOUSE',
         tenantName: 'Ware house',
         tenantType: 'WARE_HOUSE',
         tenantAddress: '123 Nguyen Van Linh, Q9, TP.HCM',
      },
      {
         tenantId: '664355f845e56534956be32c',
         branchId: '664357a235e84033bbd0e6b7',
         tenantCode: 'HN_CENTRAL',
         tenantName: 'Hanoi Central Store',
         tenantType: 'RETAIL_STORE',
         tenantAddress: '45 Tran Hung Dao, Hoan Kiem, Ha Noi',
      },
      {
         tenantId: '664355f845e56534956be32d',
         branchId: '664357a235e84033bbd0e6b8',
         tenantCode: 'DN_BRANCH',
         tenantName: 'Da Nang Branch',
         tenantType: 'RETAIL_STORE',
         tenantAddress: '78 Bach Dang, Hai Chau, Da Nang',
      },
      {
         tenantId: '664355f845e56534956be32e',
         branchId: '664357a235e84033bbd0e6b9',
         tenantCode: 'DIST_CENTER',
         tenantName: 'Distribution Center',
         tenantType: 'DISTRIBUTION',
         tenantAddress: '234 Xa Lo Ha Noi, Q9, TP.HCM',
      },
      {
         tenantId: '664355f845e56534956be32f',
         branchId: '664357a235e84033bbd0e6ba',
         tenantCode: 'PMH_STORE',
         tenantName: 'Phu My Hung Store',
         tenantType: 'RETAIL_STORE',
         tenantAddress: '89 Nguyen Van Linh, Q7, TP.HCM',
      },
      {
         tenantId: '664355f845e56534956be330',
         branchId: '664357a235e84033bbd0e6bb',
         tenantCode: 'CT_BRANCH',
         tenantName: 'Can Tho Branch',
         tenantType: 'RETAIL_STORE',
         tenantAddress: '156 Tran Hung Dao, Ninh Kieu, Can Tho',
      },
      {
         tenantId: '664355f845e56534956be331',
         branchId: '664357a235e84033bbd0e6bc',
         tenantCode: 'VT_WAREHOUSE',
         tenantName: 'Vung Tau Warehouse',
         tenantType: 'WARE_HOUSE',
         tenantAddress: '67 Truong Cong Dinh, TP. Vung Tau',
      },
      {
         tenantId: '664355f845e56534956be332',
         branchId: '664357a235e84033bbd0e6bd',
         tenantCode: 'NTG_STORE',
         tenantName: 'Nha Trang Store',
         tenantType: 'RETAIL_STORE',
         tenantAddress: '234 Tran Phu, TP. Nha Trang, Khanh Hoa',
      },
      {
         tenantId: '664355f845e56534956be333',
         branchId: '664357a235e84033bbd0e6be',
         tenantCode: 'BD_BRANCH',
         tenantName: 'Binh Duong Branch',
         tenantType: 'RETAIL_STORE',
         tenantAddress: '45 Dai Lo Binh Duong, Thu Dau Mot, Binh Duong',
      },
      {
         tenantId: '664355f845e56534956be334',
         branchId: '664357a235e84033bbd0e6bf',
         tenantCode: 'HUE_CENTER',
         tenantName: 'Hue Center',
         tenantType: 'DISTRIBUTION',
         tenantAddress: '123 Le Loi, TP. Hue, Thua Thien Hue',
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
   const { userEmail } = useAppSelector((state) => state.auth);

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
               user={{ name: 'Admin', email: userEmail || '', avatar: '' }}
            />
         </SidebarFooter>
      </Sidebar>
   );
}
