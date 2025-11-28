'use client';

import { useCallback, useMemo, useState } from 'react';
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
   X,
} from 'lucide-react';

import { Button } from '@components/ui/button';
import {
   Popover,
   PopoverContent,
   PopoverTrigger,
} from '@components/ui/popover';
import { Tabs, TabsList, TabsTrigger } from '@components/ui/tabs';
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
import useNotificationService from '~/src/hooks/api/use-notification-service';
import { INotificationQueryParams } from '~/src/infrastructure/services/notification.service';
import { TNotification } from '~/src/domain/types/ordering';
import { Badge } from '@components/ui/badge';

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

// Helper function to format relative time
const formatRelativeTime = (dateString: string): string => {
   const date = new Date(dateString);
   const now = new Date();
   const diffInSeconds = Math.floor((now.getTime() - date.getTime()) / 1000);

   if (diffInSeconds < 60) return 'Just now';
   if (diffInSeconds < 3600) {
      const minutes = Math.floor(diffInSeconds / 60);
      return `${minutes}m ago`;
   }
   if (diffInSeconds < 86400) {
      const hours = Math.floor(diffInSeconds / 3600);
      return `${hours}h ago`;
   }
   if (diffInSeconds < 604800) {
      const days = Math.floor(diffInSeconds / 86400);
      return `${days}d ago`;
   }
   return date.toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: date.getFullYear() !== now.getFullYear() ? 'numeric' : undefined,
   });
};

type NotificationTab = 'all' | 'unread' | 'read';

const notificationEmptyStates: Record<
   NotificationTab,
   { title: string; description: string }
> = {
   all: {
      title: 'No notifications',
      description: "You're all caught up! New notifications will appear here.",
   },
   unread: {
      title: 'No unread notifications',
      description: 'You have no pending alerts right now.',
   },
   read: {
      title: 'No read notifications yet',
      description: 'Mark notifications as read to see them here.',
   },
};

const formatNotificationMetaLabel = (value?: string | null) => {
   if (!value) return '';
   return value.replace(/_/g, ' ');
};

const getTypeBadgeClass = (type?: string | null) => {
   if (!type) return '';
   return 'bg-slate-100 text-slate-700 border-slate-200 dark:bg-slate-900/60 dark:text-slate-200 dark:border-slate-800';
};

const getStatusBadgeClass = (status?: string | null) => {
   if (!status) return '';
   switch (status) {
      case 'PENDING':
         return 'bg-amber-100 text-amber-800 border-amber-200 dark:bg-amber-900/40 dark:text-amber-300 dark:border-amber-800';
      case 'COMPLETED':
      case 'SENT':
         return 'bg-emerald-100 text-emerald-800 border-emerald-200 dark:bg-emerald-900/40 dark:text-emerald-300 dark:border-emerald-800';
      case 'FAILED':
      case 'CANCELLED':
         return 'bg-rose-100 text-rose-800 border-rose-200 dark:bg-rose-900/40 dark:text-rose-300 dark:border-rose-800';
      default:
         return 'bg-muted text-muted-foreground border-border';
   }
};

export function ActionNav() {
   const [isOpen, setIsOpen] = useState(false);
   const [isNotificationOpen, setIsNotificationOpen] = useState(false);
   const [notificationTab, setNotificationTab] =
      useState<NotificationTab>('all');
   const [notificationCounts, setNotificationCounts] = useState<
      Record<NotificationTab, number>
   >({
      all: 0,
      unread: 0,
      read: 0,
   });
   const [notificationData, setNotificationData] = useState<
      Record<NotificationTab, TNotification[]>
   >({
      all: [],
      unread: [],
      read: [],
   });

   const dispatch = useDispatch();

   const { currentUser, impersonatedUser } = useAppSelector(
      (state: RootState) => state.auth,
   );

   const { getListUsersAsync, getListUsersState, isLoading } =
      useIdentityService();

   const {
      getNotificationsAsync,
      markAsReadAsync,
      markAllAsReadAsync,
      isLoading: isLoadingNotifications,
   } = useNotificationService();

   const staffItems = useMemo(() => {
      return getListUsersState.isSuccess && getListUsersState.data
         ? getListUsersState.data
         : [];
   }, [getListUsersState.isSuccess, getListUsersState.data]);

   const notifications = notificationData[notificationTab] ?? [];
   const totalNotifications = notificationCounts.all;
   const unreadCount = notificationCounts.unread;
   const readCount = notificationCounts.read;
   const activeEmptyState = notificationEmptyStates[notificationTab];

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

   const buildNotificationQueryParams = useCallback(
      (tab: NotificationTab): INotificationQueryParams => {
         const baseParams: INotificationQueryParams = {
            _limit: 5,
            _page: 1,
         };

         if (tab === 'unread') {
            baseParams._isRead = false;
         } else if (tab === 'read') {
            baseParams._isRead = true;
         } else {
            baseParams._isRead = null;
         }

         return baseParams;
      },
      [],
   );

   const fetchNotificationsByTab = useCallback(
      async (tab: NotificationTab) => {
         const queryParams = buildNotificationQueryParams(tab);
         const response = await getNotificationsAsync(queryParams);

         if (response?.isSuccess && response.data) {
            const items = response.data.items ?? [];
            setNotificationCounts((prev) => ({
               ...prev,
               [tab]:
                  response.data.total_records ??
                  response.data.items?.length ??
                  0,
            }));
            setNotificationData((prev) => ({
               ...prev,
               [tab]: items,
            }));
         }
      },
      [buildNotificationQueryParams, getNotificationsAsync],
   );

   const fetchAllNotificationTabs = useCallback(async () => {
      const tabs: NotificationTab[] = ['all', 'unread', 'read'];
      await Promise.all(tabs.map((tab) => fetchNotificationsByTab(tab)));
   }, [fetchNotificationsByTab]);

   useEffect(() => {
      fetchNotificationsByTab(notificationTab);
   }, [notificationTab, fetchNotificationsByTab]);

   useEffect(() => {
      fetchAllNotificationTabs();
   }, [fetchAllNotificationTabs]);

   useEffect(() => {
      dispatch(setIsLoading(isLoading));
   }, [isLoading, dispatch]);

   const handleMarkAsRead = async (notificationId: string) => {
      await markAsReadAsync(notificationId);
      await fetchAllNotificationTabs();
   };

   const handleMarkAllAsRead = async () => {
      await markAllAsReadAsync();
      await fetchAllNotificationTabs();
   };

   return (
      <div className="flex items-center gap-2 text-sm">
         <div>
            <UserSwitcher users={staffItems} />
         </div>
         <ModeToggle />
         <Button variant="ghost" size="icon" className="h-7 w-7">
            <Star />
         </Button>

         {/* Notification Button */}
         <Popover
            open={isNotificationOpen}
            onOpenChange={setIsNotificationOpen}
         >
            <PopoverTrigger asChild>
               <Button
                  variant="ghost"
                  size="icon"
                  className="h-8 w-8 relative data-[state=open]:bg-accent"
               >
                  <Bell className="h-4 w-4" />
                  {unreadCount > 0 && (
                     <span className="absolute -top-0.5 -right-0.5 h-5 w-5 rounded-full bg-red-500 text-[10px] font-semibold text-white flex items-center justify-center border-2 border-background shadow-sm">
                        {unreadCount > 99 ? '99+' : unreadCount}
                     </span>
                  )}
               </Button>
            </PopoverTrigger>
            <PopoverContent
               className="w-[380px] p-0 shadow-lg border"
               align="end"
            >
               {/* Header + Tabs */}
               <div className="border-b bg-muted/20 px-4 py-3 space-y-2">
                  <div className="flex items-center justify-between gap-2">
                     <div className="flex items-center gap-2">
                        <Bell className="h-4 w-4 text-muted-foreground" />
                        <h3 className="font-semibold text-[13px]">
                           Notifications
                        </h3>
                        {unreadCount > 0 && (
                           <span className="h-5 min-w-[20px] px-1.5 rounded-full bg-primary text-primary-foreground text-[10px] font-semibold flex items-center justify-center">
                              {unreadCount}
                           </span>
                        )}
                     </div>
                     {unreadCount > 0 && (
                        <Button
                           variant="ghost"
                           size="sm"
                           className="h-7 px-2 text-xs text-muted-foreground hover:text-foreground"
                           onClick={handleMarkAllAsRead}
                           disabled={isLoadingNotifications}
                        >
                           Mark all read
                        </Button>
                     )}
                  </div>
                  <Tabs
                     value={notificationTab}
                     onValueChange={(value) =>
                        setNotificationTab(value as NotificationTab)
                     }
                     className="w-full"
                  >
                     <TabsList className="flex w-full gap-1 rounded-full bg-background/80 p-1">
                        <TabsTrigger
                           value="all"
                           className="flex-1 gap-2 rounded-full px-2 py-1 text-[11px] font-medium data-[state=active]:bg-white data-[state=active]:shadow-sm"
                        >
                           <span>All</span>
                           <span className="rounded-full bg-muted px-1.5 py-px text-[10px]">
                              {totalNotifications}
                           </span>
                        </TabsTrigger>
                        <TabsTrigger
                           value="unread"
                           className="flex-1 gap-2 rounded-full px-2 py-1 text-[11px] font-medium data-[state=active]:bg-white data-[state=active]:shadow-sm"
                        >
                           <span>Unread</span>
                           <span className="rounded-full bg-muted px-1.5 py-px text-[10px]">
                              {unreadCount}
                           </span>
                        </TabsTrigger>
                        <TabsTrigger
                           value="read"
                           className="flex-1 gap-2 rounded-full px-2 py-1 text-[11px] font-medium data-[state=active]:bg-white data-[state=active]:shadow-sm"
                        >
                           <span>Read</span>
                           <span className="rounded-full bg-muted px-1.5 py-px text-[10px]">
                              {readCount}
                           </span>
                        </TabsTrigger>
                     </TabsList>
                  </Tabs>
               </div>

               {/* Notification List */}
               <div className="max-h-[360px] overflow-y-auto">
                  {isLoadingNotifications ? (
                     <div className="flex flex-col items-center justify-center py-12">
                        <div className="animate-spin rounded-full h-8 w-8 border-2 border-primary border-t-transparent mb-3"></div>
                        <p className="text-xs text-muted-foreground">
                           Loading notifications...
                        </p>
                     </div>
                  ) : notifications.length === 0 ? (
                     <div className="flex flex-col items-center justify-center py-12 px-4">
                        <div className="h-16 w-16 rounded-full bg-muted flex items-center justify-center mb-3">
                           <Bell className="h-7 w-7 text-muted-foreground" />
                        </div>
                        <p className="text-sm font-medium text-foreground mb-1">
                           {activeEmptyState.title}
                        </p>
                        <p className="text-xs text-muted-foreground text-center">
                           {activeEmptyState.description}
                        </p>
                     </div>
                  ) : (
                     <div className="divide-y divide-border">
                        {notifications.map((notification: any) => {
                           const isUnread = !notification.is_read;
                           return (
                              <div
                                 key={notification.id}
                                 className={`group relative px-4 py-3 transition-colors ${
                                    isUnread
                                       ? 'cursor-pointer bg-primary/5 hover:bg-primary/10'
                                       : 'cursor-default hover:bg-muted/50'
                                 }`}
                                 onClick={() => {
                                    if (isUnread) {
                                       handleMarkAsRead(notification.id);
                                    }
                                 }}
                                 role={isUnread ? 'button' : undefined}
                                 tabIndex={isUnread ? 0 : -1}
                              >
                                 <div className="flex items-start gap-2.5">
                                    {/* Unread Indicator */}
                                    {isUnread && (
                                       <div className="mt-1 h-1.5 w-1.5 rounded-full bg-primary flex-shrink-0"></div>
                                    )}
                                    {/* Content */}
                                    <div className="flex-1 min-w-0 space-y-1">
                                       <div className="flex items-start justify-between gap-1.5">
                                          <p
                                             className={`text-[13px] font-medium leading-tight line-clamp-1 ${
                                                isUnread
                                                   ? 'text-foreground'
                                                   : 'text-muted-foreground'
                                             }`}
                                          >
                                             {notification.title}
                                          </p>
                                          {isUnread && (
                                             <button
                                                onClick={(e) => {
                                                   e.stopPropagation();
                                                   handleMarkAsRead(
                                                      notification.id,
                                                   );
                                                }}
                                                className="opacity-0 group-hover:opacity-100 transition-opacity p-1 hover:bg-muted rounded"
                                                title="Mark as read"
                                             >
                                                <X className="h-3 w-3 text-muted-foreground" />
                                             </button>
                                          )}
                                       </div>
                                       {notification.content && (
                                          <p className="text-[11px] text-muted-foreground line-clamp-2 leading-relaxed">
                                             {notification.content}
                                          </p>
                                       )}
                                       <div className="flex flex-wrap gap-1.5 pt-0.5">
                                          {notification.type && (
                                             <Badge
                                                variant="outline"
                                                className={`px-1.5 py-0.5 text-[9px] font-semibold uppercase tracking-wide border ${getTypeBadgeClass(notification.type)}`}
                                             >
                                                {formatNotificationMetaLabel(
                                                   notification.type,
                                                )}
                                             </Badge>
                                          )}
                                          {notification.status && (
                                             <Badge
                                                variant="outline"
                                                className={`px-1.5 py-0.5 text-[9px] font-semibold uppercase tracking-wide border ${getStatusBadgeClass(notification.status)}`}
                                             >
                                                {formatNotificationMetaLabel(
                                                   notification.status,
                                                )}
                                             </Badge>
                                          )}
                                       </div>
                                       <div className="flex items-center gap-1.5 pt-0.5">
                                          <span className="text-[9px] text-muted-foreground">
                                             {formatRelativeTime(
                                                notification.created_at,
                                             )}
                                          </span>
                                          {isUnread && (
                                             <span className="h-1 w-1 rounded-full bg-muted-foreground"></span>
                                          )}
                                       </div>
                                    </div>
                                 </div>
                              </div>
                           );
                        })}
                     </div>
                  )}
               </div>
            </PopoverContent>
         </Popover>

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
