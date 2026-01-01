'use client';

import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
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
   RefreshCw,
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
import useNotificationService from '~/src/hooks/api/use-notification-service';
import { RootState, useAppSelector } from '~/src/infrastructure/redux/store';
import { ERole } from '~/src/domain/enums/role.enum';
import { useDispatch } from 'react-redux';
import { setIsLoading } from '~/src/infrastructure/redux/features/app.slice';
import { TNotification } from '~/src/domain/types/ordering.type';
import { INotificationQueryParams } from '~/src/infrastructure/services/notification.service';
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
const formatRelativeTime = (dateString?: string) => {
   if (!dateString) return '';
   const date = new Date(dateString);
   if (Number.isNaN(date.getTime())) return '';

   const diffInSeconds = Math.floor((Date.now() - date.getTime()) / 1000);
   if (diffInSeconds < 60) return 'Just now';
   if (diffInSeconds < 3600) return `${Math.floor(diffInSeconds / 60)}m ago`;
   if (diffInSeconds < 86400) return `${Math.floor(diffInSeconds / 3600)}h ago`;
   if (diffInSeconds < 604800)
      return `${Math.floor(diffInSeconds / 86400)}d ago`;

   return date.toLocaleDateString();
};

const formatMetaLabel = (value?: string | null) =>
   value ? value.replace(/_/g, ' ') : '';

const getStatusBadgeClass = (status?: string | null) => {
   if (!status) return 'bg-neutral-100 text-neutral-700';
   switch (status) {
      case 'PENDING':
         return 'bg-amber-100 text-amber-800';
      case 'COMPLETED':
      case 'SENT':
         return 'bg-emerald-100 text-emerald-800';
      case 'FAILED':
      case 'CANCELLED':
         return 'bg-rose-100 text-rose-800';
      default:
         return 'bg-neutral-100 text-neutral-700';
   }
};

type NotificationTab = 'all' | 'unread' | 'read';

export function ActionNav() {
   const [isOpen, setIsOpen] = useState(false);
   const [isNotificationOpen, setIsNotificationOpen] = useState(false);
   const [notificationTab, setNotificationTab] =
      useState<NotificationTab>('all');

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
      isLoading: isNotificationLoading,
   } = useNotificationService();

   const isAuthenticated = useMemo(() => {
      return !!(currentUser?.accessToken || impersonatedUser?.accessToken);
   }, [currentUser?.accessToken, impersonatedUser?.accessToken]);

   const [notificationData, setNotificationData] = useState<{
      all: TNotification[];
      unread: TNotification[];
      read: TNotification[];
   }>({
      all: [],
      unread: [],
      read: [],
   });
   const [notificationCounts, setNotificationCounts] = useState<{
      all: number;
      unread: number;
      read: number;
   }>({
      all: 0,
      unread: 0,
      read: 0,
   });
   const [notificationPages, setNotificationPages] = useState<{
      all: number;
      unread: number;
      read: number;
   }>({
      all: 1,
      unread: 1,
      read: 1,
   });

   const staffItems = useMemo(() => {
      return getListUsersState.isSuccess && getListUsersState.data
         ? getListUsersState.data
         : [];
   }, [getListUsersState.isSuccess, getListUsersState.data]);

   const roles = useMemo(
      () => currentUser?.roles || impersonatedUser?.roles || [],
      [currentUser, impersonatedUser],
   );

   const buildNotificationQueryParams = useCallback(
      (tab: NotificationTab, page = 1): INotificationQueryParams => {
         const baseParams: INotificationQueryParams = {
            _limit: 5,
            _page: page,
         };

         if (tab === 'unread') {
            baseParams._isRead = false;
         } else if (tab === 'read') {
            baseParams._isRead = true;
         }
         // For 'all' tab, don't include _isRead parameter

         return baseParams;
      },
      [],
   );

   const fetchNotificationsByTab = useCallback(
      async (tab: NotificationTab, page = 1, append = false) => {
         if (!isAuthenticated) {
            setNotificationData((prev) => ({ ...prev, [tab]: [] }));
            setNotificationCounts((prev) => ({ ...prev, [tab]: 0 }));
            setNotificationPages((prev) => ({ ...prev, [tab]: 1 }));
            return;
         }

         const queryParams = buildNotificationQueryParams(tab, page);
         const response = await getNotificationsAsync(queryParams);

         if (response?.isSuccess && response.data) {
            const items = response.data.items ?? [];
            const count = response.data.total_records ?? items.length ?? 0;
            setNotificationCounts((prev) => ({
               ...prev,
               [tab]: count,
            }));
            setNotificationData((prev) => ({
               ...prev,
               [tab]: append ? [...prev[tab], ...items] : items,
            }));
            setNotificationPages((prev) => ({
               ...prev,
               [tab]: page,
            }));
         }
      },
      [buildNotificationQueryParams, getNotificationsAsync, isAuthenticated],
   );

   const fetchAllNotificationTabs = useCallback(async () => {
      if (!isAuthenticated) {
         setNotificationData({ all: [], unread: [], read: [] });
         setNotificationCounts({ all: 0, unread: 0, read: 0 });
         return;
      }

      const tabs: NotificationTab[] = ['all', 'unread', 'read'];
      await Promise.all(tabs.map((tab) => fetchNotificationsByTab(tab)));
   }, [fetchNotificationsByTab, isAuthenticated]);

   const notifications = useMemo(() => {
      return notificationData[notificationTab] ?? [];
   }, [notificationData, notificationTab]);

   const totalNotifications = notificationCounts.all;
   const unreadCount = notificationCounts.unread;
   const readCount = notificationCounts.read;

   const hasMoreForActiveTab = useMemo(() => {
      const data = notificationData[notificationTab] ?? [];
      const totalForTab =
         notificationTab === 'all'
            ? totalNotifications
            : notificationTab === 'unread'
              ? unreadCount
              : readCount;

      return data.length < totalForTab;
   }, [
      notificationData,
      notificationTab,
      totalNotifications,
      unreadCount,
      readCount,
   ]);

   const handleMarkNotification = useCallback(
      async (notification: TNotification) => {
         await markAsReadAsync(notification.id);
         await fetchAllNotificationTabs();
         return notification;
      },
      [markAsReadAsync, fetchAllNotificationTabs],
   );

   const handleMarkAllNotifications = useCallback(async () => {
      await markAllAsReadAsync();
      await fetchAllNotificationTabs();
   }, [markAllAsReadAsync, fetchAllNotificationTabs]);

   const handleLoadMoreNotifications = useCallback(async () => {
      if (!isAuthenticated) return;

      const currentTab = notificationTab;
      const currentItems = notificationData[currentTab] ?? [];

      const totalForTab =
         currentTab === 'all'
            ? totalNotifications
            : currentTab === 'unread'
              ? unreadCount
              : readCount;

      // No more items to load
      if (currentItems.length >= totalForTab) {
         return;
      }

      const nextPage = (notificationPages[currentTab] ?? 1) + 1;
      await fetchNotificationsByTab(currentTab, nextPage, true);
   }, [
      isAuthenticated,
      notificationTab,
      notificationData,
      totalNotifications,
      unreadCount,
      readCount,
      notificationPages,
      fetchNotificationsByTab,
   ]);

   useEffect(() => {
      const fetchUsers = async () => {
         if (roles.includes(ERole.ADMIN_SUPER_YBZONE)) {
            await getListUsersAsync({
               roles: [
                  ERole.ADMIN_SUPER_YBZONE,
                  ERole.ADMIN_YBZONE,
                  ERole.ADMIN_BRANCH,
                  ERole.STAFF_BRANCH,
               ],
            });
         }
      };

      fetchUsers();
   }, [getListUsersAsync, roles]);

   useEffect(() => {
      fetchAllNotificationTabs();
   }, [fetchAllNotificationTabs]);

   useEffect(() => {
      if (!isAuthenticated) {
         setIsNotificationOpen(false);
         setNotificationTab('all');
      }
   }, [isAuthenticated]);

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

         {/* Notification Button */}
         {isAuthenticated && (
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
                  <NotificationPanel
                     notifications={notifications}
                     unreadCount={unreadCount}
                     readCount={readCount}
                     totalCount={totalNotifications}
                     activeTab={notificationTab}
                     onTabChange={setNotificationTab}
                     isLoading={isNotificationLoading}
                     isActionLoading={isNotificationLoading}
                     onMarkAllRead={handleMarkAllNotifications}
                     onMarkRead={handleMarkNotification}
                     onRefresh={fetchAllNotificationTabs}
                     onClose={() => setIsNotificationOpen(false)}
                     hasMore={hasMoreForActiveTab}
                     onLoadMore={handleLoadMoreNotifications}
                  />
               </PopoverContent>
            </Popover>
         )}

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

interface NotificationPanelProps {
   notifications: TNotification[];
   unreadCount: number;
   readCount: number;
   totalCount: number;
   activeTab: NotificationTab;
   onTabChange: (tab: NotificationTab) => void;
   isLoading: boolean;
   isActionLoading: boolean;
   onMarkAllRead(): void;
   onMarkRead(notification: TNotification): Promise<typeof notification>;
   onRefresh(): void;
   onClose(): void;
   hasMore: boolean;
   onLoadMore(): void;
}

const NotificationPanel = ({
   notifications,
   unreadCount,
   readCount,
   totalCount,
   activeTab,
   onTabChange,
   isLoading,
   isActionLoading,
   onMarkAllRead,
   onMarkRead,
   onRefresh,
   onClose,
   hasMore,
   onLoadMore,
}: NotificationPanelProps) => {
   const hasNotifications = notifications.length > 0;
   const isInitialLoading = isLoading && !hasNotifications;

   const loadMoreRef = useRef<HTMLDivElement | null>(null);

   const tabOptions: Array<{
      key: NotificationTab;
      label: string;
      count: number;
   }> = [
      { key: 'all', label: 'ALL', count: totalCount },
      { key: 'unread', label: 'unread', count: unreadCount },
      { key: 'read', label: 'readed', count: readCount },
   ];

   useEffect(() => {
      if (!hasMore || isLoading) return;

      const sentinel = loadMoreRef.current;
      if (!sentinel) return;

      const observer = new IntersectionObserver(
         (entries) => {
            const [entry] = entries;
            if (entry.isIntersecting && !isLoading) {
               onLoadMore();
            }
         },
         {
            root: null,
            threshold: 0.1,
         },
      );

      observer.observe(sentinel);

      return () => {
         observer.disconnect();
      };
   }, [hasMore, isLoading, onLoadMore]);

   return (
      <div className="flex flex-col">
         {/* Header */}
         <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-2 sm:gap-3 px-3 sm:px-4 pt-3 sm:pt-4 border-b">
            <div className="flex-1 min-w-0">
               <div className="flex items-center gap-2">
                  <Bell className="h-4 w-4 text-muted-foreground" />
                  <p className="text-sm font-semibold">Notifications</p>
                  {unreadCount > 0 && (
                     <span className="h-5 min-w-[20px] px-1.5 rounded-full bg-primary text-primary-foreground text-[10px] font-semibold flex items-center justify-center">
                        {unreadCount}
                     </span>
                  )}
               </div>
            </div>
            <div className="flex items-center gap-2 w-full sm:w-auto justify-end sm:justify-start">
               <button
                  className="p-1.5 rounded-full hover:bg-muted transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                  onClick={onRefresh}
                  disabled={isLoading || isActionLoading}
                  title="Refresh notifications"
               >
                  <RefreshCw
                     className={`h-4 w-4 text-muted-foreground ${
                        isLoading ? 'animate-spin' : ''
                     }`}
                  />
               </button>
               <button
                  className="text-[11px] font-semibold uppercase tracking-wide text-primary disabled:text-muted-foreground whitespace-nowrap"
                  onClick={onMarkAllRead}
                  disabled={!unreadCount || isActionLoading}
               >
                  Mark all read
               </button>
            </div>
         </div>

         {/* Tabs */}
         <div className="px-3 sm:px-4 pt-3">
            <div className="flex gap-1 rounded-full bg-muted p-1">
               {tabOptions.map((tab) => {
                  const isActive = activeTab === tab.key;
                  return (
                     <button
                        key={tab.key}
                        className={`flex-1 rounded-full px-1.5 sm:px-2 py-1 text-[10px] sm:text-[11px] font-semibold transition ${
                           isActive
                              ? 'bg-background text-foreground shadow-sm'
                              : 'text-muted-foreground'
                        }`}
                        onClick={() => onTabChange(tab.key)}
                     >
                        <span className="hidden sm:inline">{tab.label}</span>
                        <span className="sm:hidden">
                           {tab.label.charAt(0).toUpperCase()}
                        </span>
                        <span className="ml-1 rounded-full bg-muted-foreground/20 px-1 sm:px-1.5 text-[9px] sm:text-[10px]">
                           {tab.count}
                        </span>
                     </button>
                  );
               })}
            </div>
         </div>

         {/* Notification List */}
         <div className="mt-3 max-h-[360px] min-h-[200px] overflow-y-auto px-3 sm:px-4 pb-3 sm:pb-4 space-y-2">
            {isInitialLoading ? (
               <div className="flex items-center justify-center py-8 text-xs sm:text-sm text-muted-foreground">
                  <span className="mr-2 h-4 w-4 animate-spin rounded-full border border-muted-foreground/30 border-t-foreground" />
                  <span className="hidden sm:inline">
                     Loading notifications...
                  </span>
                  <span className="sm:hidden">Loading...</span>
               </div>
            ) : !hasNotifications ? (
               <div className="rounded-xl border border-dashed border-border bg-muted px-3 sm:px-4 py-5 sm:py-6 text-center">
                  <p className="text-xs sm:text-sm font-semibold">
                     Nothing new right now
                  </p>
                  <p className="text-[10px] sm:text-xs text-muted-foreground mt-1">
                     We&apos;ll let you know when there&apos;s something to see.
                  </p>
               </div>
            ) : (
               notifications.map((notification) => {
                  const isUnread = !notification.is_read;
                  const hasLink =
                     typeof notification.link === 'string' &&
                     notification.link.trim().length > 0;

                  const handleNotificationClick = async () => {
                     if (isUnread) {
                        await onMarkRead(notification);
                     }
                     if (hasLink) {
                        window.location.href = notification.link;
                     }
                  };

                  return (
                     <div
                        key={notification.id}
                        onClick={handleNotificationClick}
                        className={`rounded-2xl border px-3 sm:px-3.5 py-2.5 sm:py-3 transition hover:border-primary cursor-pointer ${
                           isUnread
                              ? 'border-primary/20 bg-primary/5'
                              : 'border-border bg-background'
                        }`}
                     >
                        <div className="flex items-start justify-between gap-2 sm:gap-3">
                           <div className="space-y-1 flex-1 min-w-0">
                              <p className="text-sm font-semibold line-clamp-1">
                                 {notification.title}
                              </p>
                              {notification.content && (
                                 <p className="text-xs text-muted-foreground leading-relaxed line-clamp-2">
                                    {notification.content}
                                 </p>
                              )}
                           </div>
                           {isUnread && (
                              <span className="mt-1 h-2 w-2 rounded-full bg-primary flex-shrink-0" />
                           )}
                        </div>
                        <div className="mt-2 flex flex-wrap items-center gap-1.5 sm:gap-2 text-[10px] sm:text-[11px] text-muted-foreground">
                           <span>
                              {formatRelativeTime(notification.created_at)}
                           </span>
                           {notification.type && (
                              <Badge
                                 variant="outline"
                                 className="rounded-full px-1.5 sm:px-2 py-0.5 uppercase tracking-wide text-[9px] sm:text-[10px] font-semibold"
                              >
                                 {formatMetaLabel(notification.type)}
                              </Badge>
                           )}
                           {notification.status && (
                              <Badge
                                 variant="outline"
                                 className={`rounded-full px-1.5 sm:px-2 py-0.5 uppercase tracking-wide text-[9px] sm:text-[10px] font-semibold ${getStatusBadgeClass(
                                    notification.status,
                                 )}`}
                              >
                                 {formatMetaLabel(notification.status)}
                              </Badge>
                           )}
                        </div>
                     </div>
                  );
               })
            )}
            {hasNotifications && isLoading && (
               <div className="flex items-center justify-center py-3 text-[10px] sm:text-xs text-muted-foreground">
                  <span className="mr-1 h-3 w-3 animate-spin rounded-full border border-muted-foreground/30 border-t-foreground" />
                  <span className="hidden sm:inline">Loading more...</span>
                  <span className="sm:hidden">Loading...</span>
               </div>
            )}
            {hasMore && !isLoading && <div ref={loadMoreRef} className="h-4" />}
         </div>
      </div>
   );
};
