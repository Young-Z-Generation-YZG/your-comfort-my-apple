'use client';

import {
   BadgeCheck,
   Bell,
   ChevronsUpDown,
   CreditCard,
   LogOut,
   Sparkles,
} from 'lucide-react';
import { useEffect, useMemo } from 'react';

import { Avatar, AvatarFallback, AvatarImage } from '@components/ui/avatar';
import {
   DropdownMenu,
   DropdownMenuContent,
   DropdownMenuGroup,
   DropdownMenuItem,
   DropdownMenuLabel,
   DropdownMenuSeparator,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import {
   SidebarMenu,
   SidebarMenuButton,
   SidebarMenuItem,
   useSidebar,
} from '@components/ui/sidebar';
import useAuthService from '~/src/hooks/api/use-auth-service';
import useUserService from '~/src/hooks/api/use-user-service';
import { useRouter } from 'next/navigation';

// Helper function to get initials from name
const getInitials = (name: string): string => {
   if (!name) return 'U';
   const parts = name.trim().split(/\s+/);
   if (parts.length >= 2) {
      return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
   }
   return name.substring(0, 2).toUpperCase();
};

export function NavUser({
   user,
}: {
   user: {
      name: string;
      email: string;
      avatar: string;
   };
}) {
   const router = useRouter();

   const { isMobile } = useSidebar();

   const { logoutAsync } = useAuthService();
   const { getAccountDetailsAsync, getAccountDetailsQueryState } =
      useUserService();

   // Fetch account details on mount
   useEffect(() => {
      getAccountDetailsAsync();
   }, [getAccountDetailsAsync]);

   // Get account data
   const account = getAccountDetailsQueryState.data;

   // Use account data if available, otherwise fall back to user prop
   const displayName = useMemo(() => {
      if (account?.profile?.full_name) {
         return account.profile.full_name;
      }
      if (account?.user_name) {
         return account.user_name;
      }
      return user.name || 'User';
   }, [account, user.name]);

   const displayEmail = useMemo(() => {
      if (account?.email) {
         return account.email;
      }
      return user.email || '';
   }, [account, user.email]);

   const avatarUrl = useMemo(() => {
      if (account?.profile?.image_url) {
         return account.profile.image_url;
      }
      return user.avatar || '';
   }, [account, user.avatar]);

   const avatarInitials = useMemo(() => {
      return getInitials(displayName);
   }, [displayName]);

   return (
      <SidebarMenu>
         <SidebarMenuItem>
            <DropdownMenu>
               <DropdownMenuTrigger asChild>
                  <SidebarMenuButton
                     size="lg"
                     className="data-[state=open]:bg-sidebar-accent data-[state=open]:text-sidebar-accent-foreground"
                  >
                     <Avatar className="h-8 w-8 rounded-lg">
                        <AvatarImage src={avatarUrl} alt={displayName} />
                        <AvatarFallback className="rounded-lg">
                           {avatarInitials}
                        </AvatarFallback>
                     </Avatar>
                     <div className="grid flex-1 text-left text-sm leading-tight">
                        <span className="truncate font-medium">
                           {displayName}
                        </span>
                        <span className="truncate text-xs">{displayEmail}</span>
                     </div>
                     <ChevronsUpDown className="ml-auto size-4" />
                  </SidebarMenuButton>
               </DropdownMenuTrigger>
               <DropdownMenuContent
                  className="w-[--radix-dropdown-menu-trigger-width] min-w-56 rounded-lg"
                  side={isMobile ? 'bottom' : 'right'}
                  align="end"
                  sideOffset={4}
               >
                  <DropdownMenuLabel className="p-0 font-normal">
                     <div className="flex items-center gap-2 px-1 py-1.5 text-left text-sm">
                        <Avatar className="h-8 w-8 rounded-lg">
                           <AvatarImage src={avatarUrl} alt={displayName} />
                           <AvatarFallback className="rounded-lg">
                              {avatarInitials}
                           </AvatarFallback>
                        </Avatar>
                        <div className="grid flex-1 text-left text-sm leading-tight">
                           <span className="truncate font-medium">
                              {displayName}
                           </span>
                           <span className="truncate text-xs">
                              {displayEmail}
                           </span>
                        </div>
                     </div>
                  </DropdownMenuLabel>
                  <DropdownMenuSeparator />
                  <DropdownMenuGroup>
                     <DropdownMenuItem disabled>
                        <Sparkles />
                        Upgrade to Pro
                     </DropdownMenuItem>
                  </DropdownMenuGroup>
                  <DropdownMenuSeparator />
                  <DropdownMenuGroup>
                     <DropdownMenuItem
                        onClick={() => {
                           router.push('/dashboard/account');
                        }}
                     >
                        <BadgeCheck />
                        Account
                     </DropdownMenuItem>
                     <DropdownMenuItem disabled>
                        <CreditCard />
                        Billing
                     </DropdownMenuItem>
                     <DropdownMenuItem
                        onClick={() => {
                           // Navigate to notifications or open notifications popover
                           // For now, we'll just enable it - you can add navigation later
                        }}
                     >
                        <Bell />
                        Notifications
                     </DropdownMenuItem>
                  </DropdownMenuGroup>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem
                     onClick={async () => {
                        await logoutAsync();
                     }}
                  >
                     <LogOut />
                     Log out
                  </DropdownMenuItem>
               </DropdownMenuContent>
            </DropdownMenu>
         </SidebarMenuItem>
      </SidebarMenu>
   );
}
