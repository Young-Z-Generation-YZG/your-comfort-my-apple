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
import usePagination from '~/src/hooks/use-pagination';
import { useEffect, useMemo } from 'react';
import useIdentityService from '~/src/hooks/api/use-identity-service';
import useTenantService from '~/src/hooks/api/use-tenant-service';
import { ERole } from '~/src/domain/enums/role.enum';
import { setIsLoading } from '~/src/infrastructure/redux/features/app.slice';
import { useDispatch } from 'react-redux';
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

const superAdminSidebarData = [
   {
      title: 'Tenant Management',
      url: '/dashboards/tenant-management',
      icon: Building2,
      isActive: true,
      items: [],
   },
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
      title: 'Product Management',
      url: '/dashboards/product-management',
      icon: Building2,
      isActive: true,
      items: [
         {
            title: 'iPhone',
            url: '/dashboards/product-management/iphone',
            icon: Box,
         },
         {
            title: 'Mac',
            url: '/dashboards/tenant-management/mac',
            icon: Laptop,
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
];

const adminSidebarData = [
   {
      title: 'Product Management',
      url: '#',
      icon: Building2,
      isActive: true,
      items: [
         {
            title: 'iPhone',
            url: '#',
            icon: Box,
         },
         {
            title: 'Mac',
            url: '#',
            icon: Laptop,
         },
      ],
   },
   {
      title: 'Inventory Management',
      url: '#',
      icon: Warehouse,
      isActive: true,
      items: [],
   },
   {
      title: 'Branch HRM',
      url: '/dashboards/hrm',
      icon: Users,
      isActive: true,
   },
   {
      title: 'Customer Management',
      url: '#',
      icon: UsersRound,
      isActive: true,
   },
   {
      title: 'Order Management',
      url: '#',
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
];

const fakeTenantsData = {
   total_records: 2,
   total_pages: 1,
   page_size: 10,
   current_page: 1,
   items: [
      {
         id: '664355f845e56534956be32b',
         name: 'Ware house',
         sub_domain: 'admin',
         description: '',
         tenant_type: 'WARE_HOUSE',
         tenant_state: 'ACTIVE',
         embedded_branch: {
            id: '664357a235e84033bbd0e6b6',
            tenant_id: '664355f845e56534956be32b',
            name: 'Ware house branch',
            address: 'Ware house address',
            description: null,
            manager: null,
            created_at: '2025-11-05T14:36:11.364Z',
            updated_at: '2025-11-05T14:36:11.364Z',
            updated_by: '',
            is_deleted: false,
            deleted_at: null,
            deleted_by: '',
         },
         created_at: '2025-11-05T14:36:11.364Z',
         updated_at: '2025-11-05T14:36:11.364Z',
         updated_by: '',
         is_deleted: false,
         deleted_at: null,
         deleted_by: '',
      },
      {
         id: '690b6214ed407c59d0537d18',
         name: '1060 KVC TD',
         sub_domain: 'hcm-td-kvc-1060',
         description: 'tenant_description',
         tenant_type: 'BRANCH',
         tenant_state: 'ACTIVE',
         embedded_branch: {
            id: '690b6214ed407c59d0537d19',
            tenant_id: '690b6214ed407c59d0537d18',
            name: '1060 KVC TD',
            address: 'Số 1060, Kha Vạn Cân, Linh Chiểu, Thủ Đức',
            description: 'branch_description',
            manager: null,
            created_at: '2025-11-05T14:41:24.287Z',
            updated_at: '2025-11-05T14:41:24.287Z',
            updated_by: '',
            is_deleted: false,
            deleted_at: null,
            deleted_by: '',
         },
         created_at: '2025-11-05T14:41:24.288Z',
         updated_at: '2025-11-05T14:41:24.288Z',
         updated_by: '',
         is_deleted: false,
         deleted_at: null,
         deleted_by: '',
      },
   ],
   links: {
      first: '?_page=1&_limit=10',
      prev: null,
      next: null,
      last: '?_page=1&_limit=10',
   },
};

export type TTenantItem = (typeof fakeTenantsData.items)[number];

export function SidebarLayout({
   ...props
}: React.ComponentProps<typeof Sidebar>) {
   const { currentUser, impersonatedUser } = useAppSelector(
      (state) => state.auth,
   );

   const dispatch = useDispatch();

   const roles = useMemo(
      () => currentUser?.roles || impersonatedUser?.roles || [],
      [currentUser, impersonatedUser],
   );

   const { getTenantsAsync, getTenantsState, isLoading } = useTenantService();

   useEffect(() => {
      const fetchTenants = async () => {
         await getTenantsAsync('');
      };
      if (roles.includes(ERole.ADMIN_SUPER)) {
         fetchTenants();
      }
   }, [getTenantsAsync, roles]);

   const { paginationItems: tenantItems } = usePagination(
      getTenantsState.isSuccess &&
         getTenantsState.data &&
         getTenantsState.data.items.length > 0
         ? getTenantsState.data
         : {
              total_records: 0,
              total_pages: 0,
              page_size: 0,
              current_page: 0,
              items: [],
              links: {
                 first: null,
                 last: null,
                 prev: null,
                 next: null,
              },
           },
   );

   useEffect(() => {
      dispatch(setIsLoading(isLoading));
   }, [isLoading, dispatch]);

   const sidebarData = useMemo(() => {
      if (
         roles.includes(ERole.ADMIN_SUPER) &&
         !impersonatedUser?.roles?.includes(ERole.ADMIN)
      ) {
         return superAdminSidebarData;
      }
      return adminSidebarData;
   }, [roles, impersonatedUser]);

   return (
      <Sidebar variant="inset" {...props}>
         <SidebarHeader>
            <SidebarMenu>
               <SidebarMenuItem>
                  <TenantSwitcher tenants={tenantItems} />
               </SidebarMenuItem>
            </SidebarMenu>
         </SidebarHeader>
         <SidebarContent>
            <DashboardNav dashboards={data.dashboards} />
            <MainNav items={sidebarData} />
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
