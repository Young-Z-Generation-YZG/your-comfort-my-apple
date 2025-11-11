'use client';

import { useMemo, useState } from 'react';
import {
   ArrowDown,
   ArrowUp,
   Bell,
   Copy,
   CornerUpLeft,
   CornerUpRight,
   FileText,
   GalleryVerticalEnd,
   LineChart,
   Link,
   MoreHorizontal,
   Settings2,
   Star,
   Trash,
   Trash2,
} from 'lucide-react';

import { Button } from '@components/ui/button';
import {
   Popover,
   PopoverContent,
   PopoverTrigger,
} from '@components/ui/popover';
import {
   Sidebar,
   SidebarContent,
   SidebarGroup,
   SidebarGroupContent,
   SidebarMenu,
   SidebarMenuButton,
   SidebarMenuItem,
} from '@components/ui/sidebar';
import { ModeToggle } from '@components/ui/mode-toggle';
import UserSwitcher from '@components/ui/user-switcher';
import useIdentityService from '~/src/hooks/api/use-identity-service';
import { useEffect } from 'react';
import { RootState, useAppSelector } from '~/src/infrastructure/redux/store';
import { ERole } from '~/src/domain/enums/role.enum';
import { useDispatch } from 'react-redux';
import { setIsLoading } from '~/src/infrastructure/redux/features/app.slice';

const data = [
   [
      {
         label: 'Customize Page',
         icon: Settings2,
      },
      {
         label: 'Turn into wiki',
         icon: FileText,
      },
   ],
   [
      {
         label: 'Copy Link',
         icon: Link,
      },
      {
         label: 'Duplicate',
         icon: Copy,
      },
      {
         label: 'Move to',
         icon: CornerUpRight,
      },
      {
         label: 'Move to Trash',
         icon: Trash2,
      },
   ],
   [
      {
         label: 'Undo',
         icon: CornerUpLeft,
      },
      {
         label: 'View analytics',
         icon: LineChart,
      },
      {
         label: 'Version History',
         icon: GalleryVerticalEnd,
      },
      {
         label: 'Show delete pages',
         icon: Trash,
      },
      {
         label: 'Notifications',
         icon: Bell,
      },
   ],
   [
      {
         label: 'Import',
         icon: ArrowUp,
      },
      {
         label: 'Export',
         icon: ArrowDown,
      },
   ],
];

export function ActionNav() {
   const [isOpen, setIsOpen] = useState(false);

   const dispatch = useDispatch();

   const { currentUser, impersonatedUser } = useAppSelector(
      (state: RootState) => state.auth,
   );

   const { getListUsersAsync, getListUsersState, isLoading } =
      useIdentityService();

   const staffItems = useMemo(() => {
      return getListUsersState.isSuccess && getListUsersState.data
         ? getListUsersState.data
         : [];
   }, [getListUsersState.isSuccess, getListUsersState.data]);

   console.log(staffItems);

   const roles = useMemo(
      () => currentUser?.roles || impersonatedUser?.roles || [],
      [currentUser, impersonatedUser],
   );

   useEffect(() => {
      const fetchUsers = async () => {
         if (roles.includes(ERole.ADMIN_SUPER)) {
            await getListUsersAsync({
               roles: [ERole.ADMIN_SUPER, ERole.ADMIN, ERole.STAFF],
            });
         }
      };

      fetchUsers();
   }, [getListUsersAsync, roles, dispatch]);

   useEffect(() => {
      dispatch(setIsLoading(isLoading));
   }, [isLoading, dispatch]);

   return (
      <div className="flex items-center gap-2 text-sm">
         <div>
            <UserSwitcher users={staffItems} />
         </div>
         <ModeToggle />
         <Button variant="ghost" size="icon" className="h-7 w-7">
            <Star />
         </Button>
         <Popover open={isOpen} onOpenChange={setIsOpen}>
            <PopoverTrigger asChild>
               <Button
                  variant="ghost"
                  size="icon"
                  className="h-7 w-7 data-[state=open]:bg-accent"
               >
                  <MoreHorizontal />
               </Button>
            </PopoverTrigger>
            <PopoverContent
               className="w-56 overflow-hidden rounded-lg p-0"
               align="end"
            >
               <Sidebar collapsible="none" className="bg-transparent">
                  <SidebarContent>
                     {data.map((group, index) => (
                        <SidebarGroup
                           key={index}
                           className="border-b last:border-none"
                        >
                           <SidebarGroupContent className="gap-0">
                              <SidebarMenu>
                                 {group.map((item, index) => (
                                    <SidebarMenuItem key={index}>
                                       <SidebarMenuButton>
                                          <item.icon />{' '}
                                          <span>{item.label}</span>
                                       </SidebarMenuButton>
                                    </SidebarMenuItem>
                                 ))}
                              </SidebarMenu>
                           </SidebarGroupContent>
                        </SidebarGroup>
                     ))}
                  </SidebarContent>
               </Sidebar>
            </PopoverContent>
         </Popover>
      </div>
   );
}
