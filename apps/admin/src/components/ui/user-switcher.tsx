'use client';

import React from 'react';
import { Check, ChevronsUpDown, Search, ArrowLeftRight } from 'lucide-react';

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
import { useAppSelector } from '~/src/infrastructure/redux/store';

interface UserData {
   userId: string;
   userName: string;
   userEmail: string;
   userRole?: string;
   userAvatar?: string;
}

interface UserSwitcherProps {
   users?: UserData[];
}

const defaultUsers: UserData[] = [
   {
      userId: 'be0cd669-237a-484d-b3cf-793e0ad1b0ea',
      userName: 'Admin Super',
      userEmail: 'adminsuper@gmail.com',
      userRole: 'Super Admin',
      userAvatar: '',
   },
   {
      userId: '65dad719-7368-4d9f-b623-f308299e9575',
      userName: 'Admin',
      userEmail: 'admin@gmail.com',
      userRole: 'Admin',
      userAvatar: '',
   },
   //    {
   //       userId: '65dad719-7368-4d9f-b623-f308299e9575',
   //       userName: 'John Doe',
   //       userEmail: 'john.doe@gmail.com',
   //       userRole: 'Manager',
   //       userAvatar: '',
   //    },
   //    {
   //       userId: '65dad719-7368-4d9f-b623-f308299e9575',
   //       userName: 'Jane Smith',
   //       userEmail: 'jane.smith@gmail.com',
   //       userRole: 'Staff',
   //       userAvatar: '',
   //    },
   //    {
   //       userId: '65dad719-7368-4d9f-b623-f308299e9575',
   //       userName: 'Mike Johnson',
   //       userEmail: 'mike.j@gmail.com',
   //       userRole: 'Staff',
   //       userAvatar: '',
   //    },
   //    {
   //       userId: '65dad719-7368-4d9f-b623-f308299e9575',
   //       userName: 'Sarah Wilson',
   //       userEmail: 'sarah.w@gmail.com',
   //       userRole: 'Manager',
   //       userAvatar: '',
   //    },
   //    {
   //       userId: '65dad719-7368-4d9f-b623-f308299e9575',
   //       userName: 'David Brown',
   //       userEmail: 'david.brown@gmail.com',
   //       userRole: 'Staff',
   //       userAvatar: '',
   //    },
   //    {
   //       userId: '65dad719-7368-4d9f-b623-f308299e9575',
   //       userName: 'Emily Davis',
   //       userEmail: 'emily.davis@gmail.com',
   //       userRole: 'Manager',
   //       userAvatar: '',
   //    },
   //    {
   //       userId: '65dad719-7368-4d9f-b623-f308299e9575',
   //       userName: 'Chris Martinez',
   //       userEmail: 'chris.m@gmail.com',
   //       userRole: 'Staff',
   //       userAvatar: '',
   //    },
   //    {
   //       userId: '65dad719-7368-4d9f-b623-f308299e9575',
   //       userName: 'Lisa Anderson',
   //       userEmail: 'lisa.anderson@gmail.com',
   //       userRole: 'Admin',
   //       userAvatar: '',
   //    },
   //    {
   //       userId: '65dad719-7368-4d9f-b623-f308299e9575',
   //       userName: 'Tom Garcia',
   //       userEmail: 'tom.garcia@gmail.com',
   //       userRole: 'Staff',
   //       userAvatar: '',
   //    },
];

const UserSwitcher = ({ users = defaultUsers }: UserSwitcherProps) => {
   const [selectedUser, setSelectedUser] = React.useState(users[0]);
   const [searchQuery, setSearchQuery] = React.useState('');

   const { impersonatedUser, currentUser } = useAppSelector(
      (state) => state.auth,
   );

   const { impersonateUserAsync } = useKeycloakService();

   // Sync selected user with impersonated user or current user
   React.useEffect(() => {
      const activeUserId = impersonatedUser?.userId || currentUser?.userId;

      if (activeUserId) {
         const foundUser = users.find((user) => user.userId === activeUserId);
         if (foundUser) {
            setSelectedUser(foundUser);
         }
      }
   }, [impersonatedUser?.userId, currentUser?.userId, users]);

   const filteredUsers = React.useMemo(() => {
      if (!searchQuery) return users;

      return users.filter(
         (user) =>
            user.userName.toLowerCase().includes(searchQuery.toLowerCase()) ||
            user.userEmail.toLowerCase().includes(searchQuery.toLowerCase()),
      );
   }, [users, searchQuery]);

   const handleSelectUser = async (user: UserData) => {
      setSelectedUser(user);
      setSearchQuery('');

      await impersonateUserAsync(user.userId);
   };

   const getInitials = (name: string) => {
      return name
         .split(' ')
         .map((n) => n[0])
         .join('')
         .toUpperCase()
         .slice(0, 2);
   };

   return (
      <DropdownMenu>
         <DropdownMenuTrigger asChild>
            <Button
               variant="ghost"
               className="h-auto gap-2 px-2 py-1.5 data-[state=open]:bg-accent"
            >
               <Avatar className="h-6 w-6">
                  <AvatarImage src={selectedUser.userAvatar} />
                  <AvatarFallback className="text-[10px]">
                     {getInitials(selectedUser.userName)}
                  </AvatarFallback>
               </Avatar>
               <div className="flex flex-col items-start gap-0.5 min-w-0 flex-1">
                  <div className="flex items-center gap-1.5">
                     <span className="text-sm font-medium leading-none">
                        {selectedUser.userName}
                     </span>
                     {impersonatedUser?.userId && (
                        <Badge
                           variant="default"
                           className="p-0.5 flex items-center justify-center"
                        >
                           <ArrowLeftRight size={10} />
                        </Badge>
                     )}
                  </div>
                  <span className="text-xs text-muted-foreground leading-none">
                     {selectedUser.userEmail}
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
                     // Check if this user is currently active (either impersonated or current user)
                     const activeUserId =
                        impersonatedUser?.userId || currentUser?.userId;
                     const isActive = user.userId === activeUserId;

                     return (
                        <DropdownMenuItem
                           key={user.userId}
                           disabled={isActive}
                           onSelect={() => handleSelectUser(user)}
                           className="flex items-center gap-2 py-2"
                        >
                           <Avatar className="h-6 w-6">
                              <AvatarImage src={user.userAvatar} />
                              <AvatarFallback className="text-[10px]">
                                 {getInitials(user.userName)}
                              </AvatarFallback>
                           </Avatar>
                           <div className="flex flex-col flex-1">
                              <span className="text-sm font-medium">
                                 {user.userName}
                              </span>
                              <span className="text-xs text-muted-foreground">
                                 {user.userEmail}
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
