'use client';

import * as React from 'react';
import { Check, ChevronsUpDown, Search } from 'lucide-react';

import {
   DropdownMenu,
   DropdownMenuContent,
   DropdownMenuItem,
   DropdownMenuTrigger,
} from '@components/ui/dropdown-menu';
import { Button } from '@components/ui/button';
import { Input } from '@components/ui/input';
import { Avatar, AvatarFallback, AvatarImage } from '@components/ui/avatar';

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
      userId: '1',
      userName: 'Admin Super',
      userEmail: 'adminsuper@gmail.com',
      userRole: 'Super Admin',
      userAvatar: '',
   },
   {
      userId: '2',
      userName: 'John Doe',
      userEmail: 'john.doe@gmail.com',
      userRole: 'Manager',
      userAvatar: '',
   },
   {
      userId: '3',
      userName: 'Jane Smith',
      userEmail: 'jane.smith@gmail.com',
      userRole: 'Staff',
      userAvatar: '',
   },
   {
      userId: '4',
      userName: 'Mike Johnson',
      userEmail: 'mike.j@gmail.com',
      userRole: 'Staff',
      userAvatar: '',
   },
   {
      userId: '5',
      userName: 'Sarah Wilson',
      userEmail: 'sarah.w@gmail.com',
      userRole: 'Manager',
      userAvatar: '',
   },
   {
      userId: '6',
      userName: 'David Brown',
      userEmail: 'david.brown@gmail.com',
      userRole: 'Staff',
      userAvatar: '',
   },
   {
      userId: '7',
      userName: 'Emily Davis',
      userEmail: 'emily.davis@gmail.com',
      userRole: 'Manager',
      userAvatar: '',
   },
   {
      userId: '8',
      userName: 'Chris Martinez',
      userEmail: 'chris.m@gmail.com',
      userRole: 'Staff',
      userAvatar: '',
   },
   {
      userId: '9',
      userName: 'Lisa Anderson',
      userEmail: 'lisa.anderson@gmail.com',
      userRole: 'Admin',
      userAvatar: '',
   },
   {
      userId: '10',
      userName: 'Tom Garcia',
      userEmail: 'tom.garcia@gmail.com',
      userRole: 'Staff',
      userAvatar: '',
   },
];

const UserSwitcher = ({ users = defaultUsers }: UserSwitcherProps) => {
   const [selectedUser, setSelectedUser] = React.useState(users[0]);
   const [searchQuery, setSearchQuery] = React.useState('');

   const filteredUsers = React.useMemo(() => {
      if (!searchQuery) return users;

      return users.filter(
         (user) =>
            user.userName.toLowerCase().includes(searchQuery.toLowerCase()) ||
            user.userEmail.toLowerCase().includes(searchQuery.toLowerCase()),
      );
   }, [users, searchQuery]);

   const handleSelectUser = (user: UserData) => {
      setSelectedUser(user);
      setSearchQuery('');
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
               <div className="flex flex-col items-start gap-0.5">
                  <span className="text-sm font-medium leading-none">
                     {selectedUser.userName}
                  </span>
                  <span className="text-xs text-muted-foreground leading-none">
                     {selectedUser.userEmail}
                  </span>
               </div>
               <ChevronsUpDown className="h-3 w-3 opacity-50 ml-auto" />
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
                  filteredUsers.map((user) => (
                     <DropdownMenuItem
                        key={user.userId}
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
                        {user.userId === selectedUser.userId && (
                           <Check className="ml-auto h-4 w-4" />
                        )}
                     </DropdownMenuItem>
                  ))
               )}
            </div>
         </DropdownMenuContent>
      </DropdownMenu>
   );
};

export default UserSwitcher;
