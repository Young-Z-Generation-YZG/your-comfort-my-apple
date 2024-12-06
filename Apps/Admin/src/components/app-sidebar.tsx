"use client";

import * as React from "react";
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
} from "lucide-react";

import { NavMain } from "~/components/nav-main";
import { NavSecondary } from "~/components/nav-secondary";
import { NavUser } from "~/components/nav-user";
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "~/components/ui/sidebar";
import { NavEmployees } from "./nav-employees";
import { NavDashboards } from "./nav-dashboards";

const data = {
  user: {
    name: "Bách Lê",
    email: "lxbach1608@gmail.com",
    avatar: "",
  },
  navMain: [
    {
      title: "Product managements",
      url: "/dashboards/products",
      icon: TabletSmartphone,
      isActive: true,
      items: [
        {
          title: "Analytics",
          url: "/dashboards/products/analytics",
        },
        {
          title: "List view",
          url: "#",
        },
        {
          title: "Grid view",
          url: "#",
        },
        {
          title: "Overview",
          url: "/dashboards/products/overview",
        },
        {
          title: "New Iphone Model",
          url: "/dashboards/products/new-iphone-model",
        },
        {
          title: "Create New Iphone",
          url: "/dashboards/products/new-iphone-item",
        },
        {
          title: "Trash",
          url: "#",
        },
      ],
    },
    {
      title: "Promotion Managements",
      url: "#",
      icon: Tag,
      items: [
        {
          title: "Analytics",
          url: "#",
        },
        {
          title: "Data tables",
          url: "#",
        },
        {
          title: "Managements",
          url: "#",
        },
      ],
    },
    {
      title: "Order Managements",
      url: "#",
      icon: ScrollText,
      items: [
        {
          title: "Data tables",
          url: "#",
        },
        {
          title: "Managements",
          url: "#",
        },
      ],
    },
  ],
  navSecondary: [
    {
      title: "Support",
      url: "#",
      icon: LifeBuoy,
    },
    {
      title: "Feedback",
      url: "#",
      icon: Send,
    },
  ],
  dashboards: [
    {
      name: "Revenue Analytics",
      url: "/dashboards/revenue-analytics",
      icon: ChartNoAxesCombined,
    },
    {
      name: "User Reports",
      url: "/dashboards/user-reports",
      icon: UsersRound,
    },
    {
      name: "User'agent Reports",
      url: "/dashboards/user-agent-reports",
      icon: MonitorSmartphone,
    },
  ],
  employees: [
    {
      name: "Employee tables",
      url: "#",
      icon: FileUser,
    },
    {
      name: "Authorizations",
      url: "#",
      icon: IdCard,
    },
    {
      name: "Managements",
      url: "#",
      icon: UserPen,
    },
  ],
};

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  return (
    <Sidebar variant="inset" {...props}>
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton size="lg" asChild>
              <a href="#">
                <div className="flex aspect-square size-8 items-center justify-center rounded-lg bg-sidebar-primary text-sidebar-primary-foreground">
                  <Command className="size-4" />
                </div>
                <div className="grid flex-1 text-left text-sm leading-tight">
                  <span className="truncate font-semibold">Acme Inc</span>
                  <span className="truncate text-xs">Enterprise</span>
                </div>
              </a>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>
      <SidebarContent>
        <NavDashboards dashboards={data.dashboards} />
        <NavMain items={data.navMain} />
        <NavEmployees employees={data.employees} />
        <NavSecondary items={data.navSecondary} className="mt-auto" />
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={data.user} />
      </SidebarFooter>
    </Sidebar>
  );
}
