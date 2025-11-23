'use client';

import { ChevronRight, type LucideIcon } from 'lucide-react';

import {
   Collapsible,
   CollapsibleContent,
   CollapsibleTrigger,
} from '@components/ui/collapsible';
import {
   SidebarGroup,
   SidebarGroupLabel,
   SidebarMenu,
   SidebarMenuAction,
   SidebarMenuButton,
   SidebarMenuItem,
   SidebarMenuSub,
   SidebarMenuSubButton,
   SidebarMenuSubItem,
} from '@components/ui/sidebar';
import Link from 'next/link';

export function MainNav({
   items,
}: {
   items: {
      title: string;
      url: string;
      icon: LucideIcon;
      isActive?: boolean;
      items?: {
         title: string;
         url: string;
         items?: {
            title: string;
            url: string;
         }[];
      }[];
   }[];
}) {
   return (
      <SidebarGroup>
         <SidebarGroupLabel>Managements</SidebarGroupLabel>
         <SidebarMenu>
            {items.map((item) => (
               <Collapsible
                  key={item.title}
                  asChild
                  defaultOpen={item.isActive}
               >
                  <SidebarMenuItem>
                     <SidebarMenuButton asChild tooltip={item.title}>
                        <Link href={item.url}>
                           <item.icon />
                           <span>{item.title}</span>
                        </Link>
                     </SidebarMenuButton>
                     {item.items?.length ? (
                        <>
                           <CollapsibleTrigger asChild>
                              <SidebarMenuAction className="data-[state=open]:rotate-90">
                                 <ChevronRight />
                                 <span className="sr-only">Toggle</span>
                              </SidebarMenuAction>
                           </CollapsibleTrigger>
                           <CollapsibleContent>
                              <SidebarMenuSub>
                                 {item.items?.map((subItem) => (
                                    <SidebarMenuSubItem key={subItem.title}>
                                       <SidebarMenuSubButton asChild>
                                          <Link href={subItem.url}>
                                             <span>{subItem.title}</span>
                                          </Link>
                                       </SidebarMenuSubButton>

                                       {subItem.items?.length ? (
                                          <Collapsible className="relative">
                                             <CollapsibleTrigger
                                                asChild
                                                className="absolute -top-6"
                                             >
                                                <SidebarMenuAction className="data-[state=open]:rotate-90">
                                                   <ChevronRight />
                                                   <span className="sr-only">
                                                      Toggle
                                                   </span>
                                                </SidebarMenuAction>
                                             </CollapsibleTrigger>
                                             <CollapsibleContent>
                                                <SidebarMenuSub>
                                                   {subItem.items?.map(
                                                      (subSubItem) => (
                                                         <SidebarMenuSubItem
                                                            key={
                                                               subSubItem.title
                                                            }
                                                         >
                                                            <SidebarMenuSubButton
                                                               asChild
                                                            >
                                                               <Link
                                                                  href={
                                                                     subSubItem.url
                                                                  }
                                                               >
                                                                  <span>
                                                                     {
                                                                        subSubItem.title
                                                                     }
                                                                  </span>
                                                               </Link>
                                                            </SidebarMenuSubButton>
                                                         </SidebarMenuSubItem>
                                                      ),
                                                   )}
                                                </SidebarMenuSub>
                                             </CollapsibleContent>
                                          </Collapsible>
                                       ) : null}
                                    </SidebarMenuSubItem>
                                 ))}
                              </SidebarMenuSub>
                           </CollapsibleContent>
                        </>
                     ) : null}
                  </SidebarMenuItem>
               </Collapsible>
            ))}
         </SidebarMenu>
      </SidebarGroup>
   );
}
