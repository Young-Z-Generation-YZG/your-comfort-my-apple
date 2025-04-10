'use client';

import { Button } from '~/components/ui/button';
import { CardContext, DefaultActionContent } from '../_components/card-content';
import Badge from '../_components/badge';
import { FiEdit3 } from 'react-icons/fi';
import { cn } from '~/infrastructure/lib/utils';
import { useState } from 'react';
import {
   Dialog,
   DialogContent,
   DialogDescription,
   DialogFooter,
   DialogHeader,
   DialogTitle,
   DialogTrigger,
} from '~/components/ui/dialog';
import { Label } from '~/components/ui/label';
import { Input } from '~/components/ui/input';

const AddressesPage = () => {
   return (
      <CardContext>
         <div className="flex flex-col">
            <div className="flex justify-between">
               <h3 className="text-xl font-medium">Personal Information</h3>

               <Dialog>
                  <DialogTrigger>
                     <Button variant="outline">+ Add Address</Button>
                  </DialogTrigger>
                  <DialogContent className="sm:max-w-[425px]">
                     <DialogHeader>
                        <DialogTitle>Edit profile</DialogTitle>
                        <DialogDescription>
                           Make changes to your profile here. Click save when
                           you're done.
                        </DialogDescription>
                     </DialogHeader>
                     <div className="grid gap-4 py-4">
                        <div className="grid grid-cols-4 items-center gap-4">
                           <Label htmlFor="name" className="text-right">
                              Name
                           </Label>
                           <Input
                              id="name"
                              value="Pedro Duarte"
                              className="col-span-3"
                           />
                        </div>
                        <div className="grid grid-cols-4 items-center gap-4">
                           <Label htmlFor="username" className="text-right">
                              Username
                           </Label>
                           <Input
                              id="username"
                              value="@peduarte"
                              className="col-span-3"
                           />
                        </div>
                     </div>
                     <DialogFooter>
                        <Button type="submit">Save changes</Button>
                     </DialogFooter>
                  </DialogContent>
               </Dialog>
            </div>

            <DefaultActionContent className="w-full mt-2 border-b-0 rounded-b-none">
               <div className="flex w-full flex-col gap-1 text-slate-500 font-SFProText text-sm font-light">
                  <div className="flex justify-between items-center">
                     <div className="flex gap-2 items-center">
                        <h3 className="text-xl font-medium text-black/80 font-SFProDisplay">
                           Home
                        </h3>
                        <Badge variants="default" />
                     </div>

                     <Button
                        variant="outline"
                        className="select-none rounded-full h-[22px] px-2 py-1 font-SFProText text-sm font-medium"
                     >
                        <span>edit</span>
                        <span>
                           <FiEdit3 />
                        </span>
                     </Button>
                  </div>
                  <p>Foo Bar</p>
                  <p>106* Kha Van Can</p>
                  <p>Linh Chieu, Thu Duc</p>
                  <p>Ho Chi Minh, Vietnam</p>

                  <p className="font-medium w-fit text-blue-600 mt-2 select-none cursor-pointer hover:underline">
                     Set as Default
                  </p>
               </div>
            </DefaultActionContent>

            <DefaultActionContent className="w-full border-t rounded-t-none">
               <div className="flex w-full flex-col gap-1 text-slate-500 font-SFProText text-sm font-light">
                  <div className="flex justify-between items-center">
                     <div className="flex gap-2 items-center">
                        <h3 className="text-xl font-medium text-black/80 font-SFProDisplay">
                           Work
                        </h3>
                        <Badge variants="default" />
                     </div>

                     <Button
                        variant="outline"
                        className="select-none rounded-full h-[22px] px-2 py-1 font-SFProText text-sm font-medium"
                     >
                        <span>edit</span>
                        <span>
                           <FiEdit3 />
                        </span>
                     </Button>
                  </div>
                  <p>Foo Bar</p>
                  <p>106* Kha Van Can</p>
                  <p>Linh Chieu, Thu Duc</p>
                  <p>Ho Chi Minh, Vietnam</p>

                  <p className="font-medium text-blue-600 mt-2 select-none cursor-pointer hover:underline">
                     Set as Default
                  </p>
               </div>
            </DefaultActionContent>
         </div>
      </CardContext>
   );
};

export default AddressesPage;
