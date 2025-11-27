/* eslint-disable react-hooks/exhaustive-deps */
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
   TicketPercent,
   Calendar,
   Ticket,
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
import { useEffect, useMemo } from 'react';
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
         url: '/dashboard/online/orders',
         icon: ScrollText,
         isActive: true,
         items: [
            {
               title: 'Orders List',
               url: '/dashboard/online/orders',
               icon: ScrollText,
            },
         ],
      },
      {
         title: 'Tenant Management',
         url: '/dashboard/tenant-management',
         icon: Building2,
         isActive: true,
         items: [
            {
               title: 'iPhone',
               url: '/dashboard/tenant-management/iphone',
               icon: Box,
            },
            {
               title: 'Mac',
               url: '/dashboard/tenant-management/mac',
               icon: Laptop,
            },
            {
               title: 'Skus',
               url: '/dashboard/tenant-management/skus',
               icon: Tag,
            },
         ],
      },
      {
         title: 'Warehouses',
         url: '/dashboard/warehouses',
         icon: Warehouse,
         isActive: true,
         items: [],
      },
      {
         title: 'HRM',
         url: '/dashboard/hrm',
         icon: Users,
         isActive: true,
      },
      {
         title: 'Customer Management',
         url: '/dashboard/customer-management',
         icon: UsersRound,
         isActive: true,
      },
      {
         title: 'Order Management',
         url: '/dashboard/orders',
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
   dashboard: [
      {
         name: 'Revenue Analytics',
         url: '/dashboard/revenue-analytics',
         icon: ChartNoAxesCombined,
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

const superAdminSidebarData = [
   {
      title: 'Tenant Management',
      url: '/dashboard/tenant-management',
      icon: Building2,
      isActive: true,
      items: [],
   },
   {
      title: 'Online Shop',
      url: '/dashboard/online/orders',
      icon: ScrollText,
      isActive: true,
      items: [
         {
            title: 'Orders List',
            url: '/dashboard/online/orders',
            icon: ScrollText,
         },
      ],
   },
   {
      title: 'Product Management',
      url: '/dashboard/product-management',
      icon: Building2,
      isActive: true,
      items: [
         {
            title: 'iPhone',
            url: '/dashboard/product-management/iphone',
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
      title: 'Warehouses',
      url: '/dashboard/warehouses',
      icon: Warehouse,
      isActive: true,
      items: [],
   },
   {
      title: 'Categories',
      url: '/dashboard/categories',
      icon: Tag,
      isActive: true,
      items: [],
   },
   {
      title: 'Promotion Management',
      url: '/dashboard/promotion-management',
      icon: TicketPercent,
      isActive: true,
      items: [
         {
            title: 'Events',
            url: '/dashboard/promotion-management/events',
            icon: Calendar,
         },
         {
            title: 'Coupons',
            url: '#',
            icon: Ticket,
         },
      ],
   },
   {
      title: 'HRM',
      url: '/dashboard/hrm',
      icon: Users,
      isActive: true,
   },
   {
      title: 'Customer Management',
      url: '/dashboard/customer-management',
      icon: UsersRound,
      isActive: true,
   },
   {
      title: 'Order Management',
      url: '/dashboard/order-management',
      icon: ScrollText,
      isActive: true,
      items: [
         {
            title: 'Orders List',
            url: '/dashboard/order-management',
            icon: ScrollText,
         },
      ],
   },
];

const adminSidebarData = [
   {
      title: 'Product Management',
      url: '/dashboard/product-management',
      icon: Building2,
      isActive: true,
      items: [
         {
            title: 'iPhone',
            url: '/dashboard/product-management/iphone',
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
      url: '/dashboard/warehouses',
      icon: Warehouse,
      isActive: true,
      items: [],
   },
   {
      title: 'Categories',
      url: '/dashboard/categories',
      icon: Tag,
      isActive: true,
      items: [],
   },
   {
      title: 'Promotion Management',
      url: '/dashboard/promotion-management',
      icon: TicketPercent,
      isActive: true,
      items: [
         {
            title: 'Events',
            url: '/dashboard/promotion-management/events',
            icon: Calendar,
         },
         {
            title: 'Coupons',
            url: '#',
            icon: Ticket,
         },
      ],
   },

   {
      title: 'Branch HRM',
      url: '/dashboard/hrm',
      icon: Users,
      isActive: true,
   },
   {
      title: 'Customer Management',
      url: '/dashboard/customer-management',
      icon: UsersRound,
      isActive: true,
   },
   {
      title: 'Order Management',
      url: '/dashboard/order-management',
      icon: ScrollText,
      isActive: true,
      items: [
         {
            title: 'Orders List',
            url: '/dashboard/order-management',
            icon: ScrollText,
         },
      ],
   },
];

const staffSidebarData = [
   {
      title: 'Product Management',
      url: '/dashboard/product-management',
      icon: Building2,
      isActive: true,
      items: [
         {
            title: 'iPhone',
            url: '/dashboard/product-management/iphone',
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
      title: 'Categories',
      url: '/dashboard/categories',
      icon: Tag,
      isActive: true,
      items: [],
   },
   {
      title: 'Promotion Management',
      url: '/dashboard/promotion-management',
      icon: TicketPercent,
      isActive: true,
      items: [
         {
            title: 'Events',
            url: '/dashboard/promotion-management/events',
            icon: Calendar,
         },
         {
            title: 'Coupons',
            url: '#',
            icon: Ticket,
         },
      ],
   },
   {
      title: 'Customer Management',
      url: '/dashboard/customer-management',
      icon: UsersRound,
      isActive: true,
   },
   {
      title: 'Order Management',
      url: '/dashboard/order-management',
      icon: ScrollText,
      isActive: true,
      items: [
         {
            title: 'Orders List',
            url: '/dashboard/order-management',
            icon: ScrollText,
         },
      ],
   },
];

export function SidebarLayout({
   ...props
}: React.ComponentProps<typeof Sidebar>) {
   const { currentUser, impersonatedUser, currentUserKey } = useAppSelector(
      (state) => state.auth,
   );

   const dispatch = useDispatch();
   const {
      getListTenantsAsync,
      getTenantByIdAsync,
      getTenantByIdState,
      getListTenantsState,
      isLoading,
   } = useTenantService();

   // Get roles directly from Redux state (stored when switching users)
   // Use currentUserKey to determine which user's roles to use
   const roles = useMemo(() => {
      const activeUser =
         currentUserKey === 'impersonatedUser' ? impersonatedUser : currentUser;
      return activeUser?.roles || [];
   }, [currentUserKey, currentUser, impersonatedUser]);

   const tenantItems = useMemo(() => {
      return getListTenantsState.isSuccess && getListTenantsState.data
         ? getListTenantsState.data
         : [];
   }, [getListTenantsState.isSuccess, getListTenantsState.data]);

   const currentTenant = useMemo(() => {
      return getTenantByIdState.isSuccess && getTenantByIdState.data
         ? getTenantByIdState.data
         : null;
   }, [getTenantByIdState.isSuccess, getTenantByIdState.data]);

   // Fetch tenants only for super admin
   useEffect(() => {
      if (roles.includes(ERole.ADMIN_SUPER)) {
         getListTenantsAsync();
      }
      if (currentUser?.tenantId) {
         getTenantByIdAsync(currentUser.tenantId);
      }
   }, [getListTenantsAsync, getTenantByIdAsync]);

   // Update global loading state
   useEffect(() => {
      dispatch(setIsLoading(isLoading));
   }, [isLoading, dispatch]);

   // Determine sidebar data based on roles
   const sidebarData = useMemo(() => {
      const isSuperAdmin = roles.includes(ERole.ADMIN_SUPER);
      const isImpersonatingAdmin = impersonatedUser?.roles?.includes(
         ERole.ADMIN,
      );

      if (isSuperAdmin && !isImpersonatingAdmin) {
         return superAdminSidebarData;
      }
      return adminSidebarData;
   }, [roles, impersonatedUser?.roles]);

   return (
      <Sidebar variant="inset" {...props}>
         <SidebarHeader>
            <SidebarMenu>
               <SidebarMenuItem>
                  <TenantSwitcher
                     tenants={tenantItems}
                     currentTenant={currentTenant}
                  />
               </SidebarMenuItem>
            </SidebarMenu>
         </SidebarHeader>
         <SidebarContent>
            <DashboardNav dashboard={data.dashboard} />
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
