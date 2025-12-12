'use client';

import { Check, ChevronsUpDown, Search, ArrowLeftRight } from 'lucide-react';
import { useEffect, useMemo, useState } from 'react';

import {
   DropdownMenu,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import { Button } from '@components/ui/button';
import { Input } from '@components/ui/input';
import { Avatar, AvatarFallback, AvatarImage } from '@components/ui/avatar';
import { Badge } from '@components/ui/badge';
import useKeycloakService from '~/src/hooks/api/use-keycloak-service';
import { RootState, useAppSelector } from '~/src/infrastructure/redux/store';
import { TUser } from '~/src/domain/types/identity.type';

const UserSwitcher = ({ users }: { users: TUser[] }) => {
   const { currentUser, impersonatedUser } = useAppSelector(
      (state: RootState) => state.auth,
   );

   const [searchQuery, setSearchQuery] = useState('');
   const [selectedUser, setSelectedUser] = useState<TUser | null>(null);

   const { impersonateUserAsync } = useKeycloakService();

   // Initialize selectedUser based on impersonatedUser or currentUser
   useEffect(() => {
      if (users.length === 0) return;

      const activeUserId = impersonatedUser?.userId || currentUser?.userId;
      if (!activeUserId) return;

      const foundUser = users.find((user: TUser) => user.id === activeUserId);
      if (foundUser && foundUser.id !== selectedUser?.id) {
         setSelectedUser(foundUser);
      }
   }, [users, currentUser?.userId, impersonatedUser?.userId, selectedUser?.id]);

   const filteredUsersByEmail = useMemo(() => {
      if (!searchQuery.trim().length) return users;

      const query = searchQuery.trim().toLowerCase();
      return users.filter(
         (user) =>
            user.user_name.toLowerCase().includes(query) ||
            user.email.toLowerCase().includes(query),
      );
   }, [users, searchQuery]);

   const handleSelectUser = async (user: TUser) => {
      // Prevent unnecessary calls if selecting the same user
      if (user.id === selectedUser?.id) {
         setSearchQuery('');
         return;
      }

      setSelectedUser(user);
      setSearchQuery('');
      await impersonateUserAsync(user.id);
   };

   const getInitials = (firstName: string, lastName: string) => {
      if (!firstName || !lastName) return '';
      return (firstName[0] + lastName[0]).toUpperCase();
   };

   // Don't render if no user selected or users not loaded
   if (!selectedUser || users.length === 0) {
      return null;
   }

   return (
      <DropdownMenu>
         <DropdownMenuTrigger asChild>
            <Button
               variant="ghost"
               className="h-auto gap-2 px-2 py-1.5 data-[state=open]:bg-accent"
            >
               <Avatar className="h-6 w-6">
                  <AvatarImage
                     src={selectedUser.profile?.image_url || undefined}
                  />
                  <AvatarFallback className="text-[10px]">{''}</AvatarFallback>
               </Avatar>
               <div className="flex flex-col items-start gap-0.5 min-w-0 flex-1">
                  <div className="flex items-center gap-1.5">
                     <span className="text-sm font-medium leading-none">
                        {selectedUser.profile?.first_name}{' '}
                        {selectedUser.profile?.last_name}
                     </span>
                     {impersonatedUser?.userId === selectedUser?.id &&
                        impersonatedUser?.userId !== currentUser?.userId && (
                           <Badge
                              variant="default"
                              className="p-0.5 flex items-center justify-center"
                           >
                              <ArrowLeftRight size={10} />
                           </Badge>
                        )}
                  </div>
                  <span className="text-xs text-muted-foreground leading-none">
                     {selectedUser.email}
                  </span>
               </div>
               <ChevronsUpDown className="h-3 w-3 opacity-50 shrink-0" />
            </Button>
         </DropdownMenuTrigger>
         <DropdownMenuContent className="w-[280px] p-0" align="end">
            <div
               className="flex items-center border-b px-3 py-2"
               onKeyDown={(e) => e.stopPropagation()}
            >
               <Search className="mr-2 h-4 w-4 shrink-0 opacity-50" />
               <Input
                  placeholder="Search users..."
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  className="h-8 border-0 focus-visible:ring-0 focus-visible:ring-offset-0"
                  autoFocus
               />
            </div>
            <div className="max-h-[300px] overflow-y-auto p-1">
               {filteredUsersByEmail.length === 0 ? (
                  <div className="py-6 text-center text-sm text-muted-foreground">
                     No user found.
                  </div>
               ) : (
                  filteredUsersByEmail.map((user) => {
                     const isActive = selectedUser?.id === user.id;

                     return (
                        <DropdownMenuItem
                           key={user.id}
                           disabled={isActive}
                           onSelect={() => handleSelectUser(user)}
                           className="flex items-center gap-2 py-2"
                        >
                           <Avatar className="h-6 w-6">
                              <AvatarImage
                                 src={user.profile?.image_url || undefined}
                              />
                              <AvatarFallback className="text-[10px]">
                                 {getInitials(
                                    user.profile?.first_name || '',
                                    user.profile?.last_name || '',
                                 )}
                              </AvatarFallback>
                           </Avatar>
                           <div className="flex flex-col flex-1">
                              <span className="text-sm font-medium">
                                 {user.profile?.first_name}{' '}
                                 {user.profile?.last_name}
                              </span>
                              <span className="text-xs text-muted-foreground">
                                 {user.email}
                              </span>
                           </div>
                           {isActive && <Check className="ml-auto h-4 w-4" />}
                        </DropdownMenuItem>
                     );
                  })
               )}
            </div>
         </DropdownMenuContent>
      </DropdownMenu>
   );
};

export default UserSwitcher;
