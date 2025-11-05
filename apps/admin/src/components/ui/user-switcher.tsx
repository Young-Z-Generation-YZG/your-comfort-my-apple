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
import useIdentityService from '~/src/hooks/api/use-identity-service';
import usePagination from '~/src/hooks/use-pagination';
import { useDispatch } from 'react-redux';
import { setIsLoading } from '~/src/infrastructure/redux/features/app.slice';
import { ERole } from '~/src/domain/enums/role.enum';

const fakeStaffData = {
   total_records: 6,
   total_pages: 1,
   page_size: 10,
   current_page: 1,
   items: [
      {
         id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
         tenant_id: null,
         branch_id: null,
         tenant_code: null,
         user_name: 'user@gmail.com',
         normalized_user_name: 'USER@GMAIL.COM',
         email: 'user@gmail.com',
         normalized_email: 'USER@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0333284890',
         profile: {
            id: '422452ca-6ac1-4fac-8d37-8f59c5305d63',
            user_id: 'c3127b01-9101-4713-8e18-ae1f8f9ffd01',
            first_name: 'USER',
            last_name: 'USER',
            birth_day: '2006-11-19T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T09:33:04.828081Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         id: 'e79d0b6f-af5a-4162-a6fd-8194d5a5f616',
         tenant_id: '690b6214ed407c59d0537d18',
         branch_id: null,
         tenant_code: null,
         user_name: 'staff@gmail.com',
         normalized_user_name: 'STAFF@GMAIL.COM',
         email: 'staff@gmail.com',
         normalized_email: 'STAFF@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0333284890',
         profile: {
            id: '573ff10d-f412-4291-b6d8-89594fc7c0fd',
            user_id: 'e79d0b6f-af5a-4162-a6fd-8194d5a5f616',
            first_name: 'STAFF',
            last_name: 'USER',
            birth_day: '2005-10-18T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T09:33:05.487937Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         id: '65dad719-7368-4d9f-b623-f308299e9575',
         tenant_id: '690b6214ed407c59d0537d18',
         branch_id: null,
         tenant_code: null,
         user_name: 'admin@gmail.com',
         normalized_user_name: 'ADMIN@GMAIL.COM',
         email: 'admin@gmail.com',
         normalized_email: 'ADMIN@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0333284890',
         profile: {
            id: 'e5676437-47bd-47ad-b5f9-21f7278c061f',
            user_id: '65dad719-7368-4d9f-b623-f308299e9575',
            first_name: 'ADMIN',
            last_name: 'USER',
            birth_day: '2004-09-17T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T09:33:05.834715Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         id: 'be0cd669-237a-484d-b3cf-793e0ad1b0ea',
         tenant_id: null,
         branch_id: null,
         tenant_code: null,
         user_name: 'adminsuper@gmail.com',
         normalized_user_name: 'ADMINSUPER@GMAIL.COM',
         email: 'adminsuper@gmail.com',
         normalized_email: 'ADMINSUPER@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0333284890',
         profile: {
            id: '79ce1c51-a7f5-47df-9c51-d3797dbc4818',
            user_id: 'be0cd669-237a-484d-b3cf-793e0ad1b0ea',
            first_name: 'ADMIN SUPER',
            last_name: 'USER',
            birth_day: '2003-08-16T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T09:33:06.084494Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         id: '8d8059c4-38b8-4f62-a776-4527e059b14a',
         tenant_id: null,
         branch_id: null,
         tenant_code: null,
         user_name: 'foobar@gmail.com',
         normalized_user_name: 'FOOBAR@GMAIL.COM',
         email: 'foobar@gmail.com',
         normalized_email: 'FOOBAR@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0333284890',
         profile: {
            id: 'd46461c7-e1f0-4784-b729-f421bc17eb04',
            user_id: '8d8059c4-38b8-4f62-a776-4527e059b14a',
            first_name: 'FOO',
            last_name: 'BAR',
            birth_day: '2007-12-20T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T09:33:06.271631Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
      {
         id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
         tenant_id: null,
         branch_id: null,
         tenant_code: null,
         user_name: 'lov3rinve146@gmail.com',
         normalized_user_name: 'LOV3RINVE146@GMAIL.COM',
         email: 'lov3rinve146@gmail.com',
         normalized_email: 'LOV3RINVE146@GMAIL.COM',
         email_confirmed: true,
         phone_number: '0123456789',
         profile: {
            id: '943788f8-298e-4fba-9c6e-1b322719671b',
            user_id: '7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f',
            first_name: 'Bach',
            last_name: 'Le',
            birth_day: '2003-08-16T00:00:00Z',
            gender: 'OTHER',
            image_id: '',
            image_url: '',
            created_at: '0001-01-01T00:00:00',
            updated_at: '2025-10-27T14:34:18.202777Z',
            updated_by: null,
            is_deleted: false,
            deleted_at: null,
            deleted_by: null,
         },
         created_at: '0001-01-01T00:00:00',
         updated_at: '0001-01-01T00:00:00',
         updated_by: null,
         is_deleted: false,
         deleted_at: null,
         deleted_by: null,
      },
   ],
   links: {
      first: '?_page=1&_limit=10',
      prev: null,
      next: null,
      last: '?_page=1&_limit=10',
   },
};

export type TUser = (typeof fakeStaffData.items)[number];

const UserSwitcher = ({ users }: { users: TUser[] }) => {
   const { currentUser, impersonatedUser } = useAppSelector(
      (state: RootState) => state.auth,
   );

   const [searchQuery, setSearchQuery] = useState('');
   const [selectedUser, setSelectedUser] = useState<TUser | null>(null);

   const { impersonateUserAsync } = useKeycloakService();

   useEffect(() => {
      if (users.length > 0) {
         if (impersonatedUser) {
            const foundUser = users.find(
               (user: TUser) => user.email === impersonatedUser?.userEmail,
            );

            if (foundUser) {
               setSelectedUser(foundUser);
            }
         } else {
            const foundUser = users.find(
               (user: TUser) => user.email === currentUser?.userEmail,
            );

            if (foundUser) {
               setSelectedUser(foundUser);
            }
         }
      }
   }, [users, currentUser, impersonatedUser]);

   const filteredUsers = useMemo(() => {
      if (!searchQuery) return users;

      return users.filter(
         (user) =>
            user.user_name.toLowerCase().includes(searchQuery.toLowerCase()) ||
            user.email.toLowerCase().includes(searchQuery.toLowerCase()),
      );
   }, [users, searchQuery]);

   const handleSelectUser = async (user: TUser) => {
      setSelectedUser(user);
      setSearchQuery('');

      await impersonateUserAsync(user.id);
   };

   const getInitials = (firstName: string, lastName: string) => {
      return (firstName[0] + lastName[0]).toUpperCase();
   };

   useEffect(() => {
      if (users.length === 0) return;

      if (impersonatedUser) {
         const foundUser = users.find(
            (user: TUser) => user.id === impersonatedUser?.userId,
         );

         if (foundUser) {
            setSelectedUser(foundUser);
         }
      } else {
         const foundUser = users.find(
            (user: TUser) => user.id === currentUser?.userId,
         );

         if (foundUser) {
            setSelectedUser(foundUser);
         }
      }
   }, [users, currentUser, impersonatedUser]);

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
                  <AvatarImage src={selectedUser.profile?.image_url} />
                  <AvatarFallback className="text-[10px]">
                     {getInitials(
                        selectedUser.profile?.first_name || '',
                        selectedUser.profile?.last_name || '',
                     )}
                  </AvatarFallback>
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
               {filteredUsers.length === 0 ? (
                  <div className="py-6 text-center text-sm text-muted-foreground">
                     No user found.
                  </div>
               ) : (
                  filteredUsers.map((user) => {
                     const isActive = selectedUser?.id === user.id;

                     return (
                        <DropdownMenuItem
                           key={user.id}
                           disabled={isActive}
                           onSelect={() => handleSelectUser(user)}
                           className="flex items-center gap-2 py-2"
                        >
                           <Avatar className="h-6 w-6">
                              <AvatarImage src={user.profile?.image_url} />
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
