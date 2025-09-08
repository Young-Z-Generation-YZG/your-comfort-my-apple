'use client';

import type React from 'react';

import { useState, useRef } from 'react';
import { Button } from '@components/ui/button';
import { Camera, Upload, Trash2 } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { DefaultActionContent } from './card-content';
import { useToast } from '~/hooks/use-toast';

export function ProfilePicture() {
   const { toast } = useToast();
   const [profileImage, setProfileImage] = useState<string>(
      '/placeholder.svg?height=120&width=120',
   );
   const [isHovering, setIsHovering] = useState(false);
   const [isUploading, setIsUploading] = useState(false);
   const fileInputRef = useRef<HTMLInputElement>(null);

   const handleImageUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
      const file = e.target.files?.[0];
      if (!file) return;

      setIsUploading(true);

      // Simulate upload delay
      setTimeout(() => {
         const reader = new FileReader();

         reader.onload = (event) => {
            if (event.target?.result) {
               setProfileImage(event.target.result as string);
               setIsUploading(false);
               toast({
                  title: 'Profile picture updated',
                  description:
                     'Your profile picture has been successfully updated.',
                  duration: 3000,
               });
            }
         };
         reader.readAsDataURL(file);
      }, 1500);
   };

   const handleRemoveImage = () => {
      setProfileImage('/placeholder.svg?height=120&width=120');
      toast({
         title: 'Profile picture removed',
         description: 'Your profile picture has been removed.',
         duration: 3000,
      });
   };

   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         initial={{ opacity: 0, y: 20 }}
         animate={{ opacity: 1, y: 0 }}
         transition={{ duration: 0.3 }}
      >
         <div className="p-6 flex flex-col sm:flex-row items-center">
            <motion.div
               className="relative"
               onMouseEnter={() => setIsHovering(true)}
               onMouseLeave={() => setIsHovering(false)}
               whileHover={{ scale: 1.05 }}
               transition={{ type: 'spring', stiffness: 300, damping: 20 }}
            >
               <div className="h-32 w-32 rounded-full overflow-hidden bg-gray-100 flex items-center justify-center">
                  {isUploading ? (
                     <div className="absolute inset-0 bg-black bg-opacity-50 rounded-full flex items-center justify-center">
                        <div className="h-8 w-8 border-4 border-t-blue-500 border-r-transparent border-b-transparent border-l-transparent rounded-full animate-spin"></div>
                     </div>
                  ) : (
                     <img
                        src={profileImage || '/placeholder.svg'}
                        className="h-full w-full object-cover"
                        style={{ objectFit: 'cover' }}
                     />
                  )}
               </div>

               <AnimatePresence>
                  {isHovering && !isUploading && (
                     <motion.div
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        exit={{ opacity: 0 }}
                        className="absolute inset-0 bg-black bg-opacity-50 rounded-full flex items-center justify-center"
                     >
                        <Button
                           variant="ghost"
                           size="icon"
                           className="text-white hover:text-white hover:bg-black hover:bg-opacity-30"
                           onClick={() => fileInputRef.current?.click()}
                        >
                           <Camera className="h-6 w-6" />
                        </Button>
                     </motion.div>
                  )}
               </AnimatePresence>

               <input
                  type="file"
                  ref={fileInputRef}
                  onChange={handleImageUpload}
                  accept="image/*"
                  className="hidden"
                  disabled={isUploading}
               />
            </motion.div>

            <div className="mt-6 sm:mt-0 sm:ml-8 flex flex-col space-y-4">
               <p className="text-sm text-gray-500 max-w-md font-SFProText">
                  Upload a profile picture to personalize your account. The
                  image should be at least 400x400 pixels and less than 2MB in
                  size.
               </p>
               <div className="flex space-x-3">
                  <motion.div
                     whileHover={{ scale: 1.05 }}
                     whileTap={{ scale: 0.95 }}
                  >
                     <Button
                        variant="outline"
                        className="flex items-center text-blue-600 hover:text-blue-800 border-blue-200 hover:border-blue-300"
                        onClick={() => fileInputRef.current?.click()}
                        disabled={isUploading}
                     >
                        <Upload className="h-4 w-4 mr-2" />
                        Upload New Picture
                     </Button>
                  </motion.div>
                  {profileImage !== '/placeholder.svg?height=120&width=120' && (
                     <motion.div
                        whileHover={{ scale: 1.05 }}
                        whileTap={{ scale: 0.95 }}
                     >
                        <Button
                           variant="outline"
                           className="flex items-center text-red-600 hover:text-red-800 border-red-200 hover:border-red-300"
                           onClick={handleRemoveImage}
                           disabled={isUploading}
                        >
                           <Trash2 className="h-4 w-4 mr-2" />
                           Remove
                        </Button>
                     </motion.div>
                  )}
               </div>
            </div>
         </div>
      </motion.div>
   );
}
