'use client';

import { useState } from 'react';
import { Button } from '@components/ui/button';
import { Label } from '@components/ui/label';
import { Switch } from '@components/ui/switch';
import {
   Select,
   SelectContent,
   SelectItem,
   SelectTrigger,
   SelectValue,
} from '@components/ui/select';
import { Separator } from '@components/ui/separator';
import { motion } from 'framer-motion';
import { useToast } from '@components/hooks/use-toast';
import { Save, Globe, Bell, Volume2, Monitor } from 'lucide-react';

type PreferencesData = {
   language: string;
   theme: string;
   timezone: string;
   emailNotifications: boolean;
   pushNotifications: boolean;
   smsNotifications: boolean;
   marketingEmails: boolean;
   soundEffects: boolean;
   autoPlay: boolean;
   highContrast: boolean;
   reducedMotion: boolean;
};

export function PreferencesForm() {
   const { toast } = useToast();
   const [preferences, setPreferences] = useState<PreferencesData>({
      language: 'English',
      theme: 'Light',
      timezone: 'America/New_York',
      emailNotifications: true,
      pushNotifications: true,
      smsNotifications: false,
      marketingEmails: true,
      soundEffects: true,
      autoPlay: true,
      highContrast: false,
      reducedMotion: false,
   });

   const [isSaving, setIsSaving] = useState(false);
   const [hasChanges, setHasChanges] = useState(false);

   const handleSelectChange = (name: string, value: string) => {
      setPreferences((prev) => ({ ...prev, [name]: value }));
      setHasChanges(true);
   };

   const handleSwitchChange = (name: string, checked: boolean) => {
      setPreferences((prev) => ({ ...prev, [name]: checked }));
      setHasChanges(true);
   };

   const handleSave = () => {
      setIsSaving(true);

      // Simulate API call
      setTimeout(() => {
         setIsSaving(false);
         setHasChanges(false);

         toast({
            title: 'Preferences saved',
            description: 'Your preferences have been updated successfully.',
            duration: 3000,
         });
      }, 1500);
   };

   const handleReset = () => {
      setPreferences({
         language: 'English',
         theme: 'Light',
         timezone: 'America/New_York',
         emailNotifications: true,
         pushNotifications: true,
         smsNotifications: false,
         marketingEmails: true,
         soundEffects: true,
         autoPlay: true,
         highContrast: false,
         reducedMotion: false,
      });
      setHasChanges(false);

      toast({
         title: 'Preferences reset',
         description: 'Your preferences have been reset to default values.',
         duration: 3000,
      });
   };

   // Animation variants
   const containerVariants = {
      hidden: { opacity: 0 },
      visible: {
         opacity: 1,
         transition: {
            staggerChildren: 0.05,
         },
      },
   };

   const itemVariants = {
      hidden: { opacity: 0, y: 20 },
      visible: {
         opacity: 1,
         y: 0,
         transition: {
            type: 'spring',
            stiffness: 300,
            damping: 24,
         },
      },
   };

   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         variants={containerVariants}
         initial="hidden"
         animate="visible"
      >
         <div className="px-6 py-4 border-b border-gray-200 flex justify-between items-center">
            <h2 className="text-lg font-medium text-gray-900">Preferences</h2>
            <div className="flex space-x-3">
               <motion.div
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
               >
                  <Button
                     variant="outline"
                     className="text-gray-600 hover:text-gray-800"
                     onClick={handleReset}
                     disabled={isSaving || !hasChanges}
                  >
                     Reset to Default
                  </Button>
               </motion.div>
               <motion.div
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
                  animate={isSaving ? { scale: [1, 0.95, 1.05, 1] } : {}}
                  transition={{ duration: 0.4 }}
               >
                  <Button
                     className="bg-blue-600 hover:bg-blue-700 text-white"
                     onClick={handleSave}
                     disabled={isSaving || !hasChanges}
                  >
                     {isSaving ? (
                        <span className="flex items-center">
                           <svg
                              className="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
                              xmlns="http://www.w3.org/2000/svg"
                              fill="none"
                              viewBox="0 0 24 24"
                           >
                              <circle
                                 className="opacity-25"
                                 cx="12"
                                 cy="12"
                                 r="10"
                                 stroke="currentColor"
                                 strokeWidth="4"
                              ></circle>
                              <path
                                 className="opacity-75"
                                 fill="currentColor"
                                 d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                              ></path>
                           </svg>
                           Saving...
                        </span>
                     ) : (
                        <span className="flex items-center">
                           <Save className="h-4 w-4 mr-2" />
                           Save Changes
                        </span>
                     )}
                  </Button>
               </motion.div>
            </div>
         </div>

         <div className="p-6 space-y-8">
            {/* Regional Settings */}
            <motion.div variants={itemVariants} className="space-y-4">
               <div className="flex items-center space-x-2">
                  <Globe className="h-5 w-5 text-blue-600" />
                  <h3 className="text-md font-medium text-gray-900">
                     Regional Settings
                  </h3>
               </div>
               <div className="grid grid-cols-1 md:grid-cols-2 gap-6 pl-7">
                  <div className="space-y-2">
                     <Label htmlFor="language">Language</Label>
                     <Select
                        value={preferences.language}
                        onValueChange={(value) =>
                           handleSelectChange('language', value)
                        }
                     >
                        <SelectTrigger className="transition-all duration-200">
                           <SelectValue placeholder="Select language" />
                        </SelectTrigger>
                        <SelectContent>
                           <SelectItem value="English">English</SelectItem>
                           <SelectItem value="Spanish">Spanish</SelectItem>
                           <SelectItem value="French">French</SelectItem>
                           <SelectItem value="German">German</SelectItem>
                           <SelectItem value="Japanese">Japanese</SelectItem>
                           <SelectItem value="Chinese">Chinese</SelectItem>
                           <SelectItem value="Vietnamese">
                              Vietnamese
                           </SelectItem>
                        </SelectContent>
                     </Select>
                  </div>

                  <div className="space-y-2">
                     <Label htmlFor="timezone">Time Zone</Label>
                     <Select
                        value={preferences.timezone}
                        onValueChange={(value) =>
                           handleSelectChange('timezone', value)
                        }
                     >
                        <SelectTrigger className="transition-all duration-200">
                           <SelectValue placeholder="Select time zone" />
                        </SelectTrigger>
                        <SelectContent>
                           <SelectItem value="America/New_York">
                              Eastern Time (ET)
                           </SelectItem>
                           <SelectItem value="America/Chicago">
                              Central Time (CT)
                           </SelectItem>
                           <SelectItem value="America/Denver">
                              Mountain Time (MT)
                           </SelectItem>
                           <SelectItem value="America/Los_Angeles">
                              Pacific Time (PT)
                           </SelectItem>
                           <SelectItem value="Europe/London">
                              Greenwich Mean Time (GMT)
                           </SelectItem>
                           <SelectItem value="Europe/Paris">
                              Central European Time (CET)
                           </SelectItem>
                           <SelectItem value="Asia/Tokyo">
                              Japan Standard Time (JST)
                           </SelectItem>
                        </SelectContent>
                     </Select>
                  </div>
               </div>
            </motion.div>

            <Separator />

            {/* Appearance */}
            <motion.div variants={itemVariants} className="space-y-4">
               <div className="flex items-center space-x-2">
                  <Monitor className="h-5 w-5 text-blue-600" />
                  <h3 className="text-md font-medium text-gray-900">
                     Appearance
                  </h3>
               </div>
               <div className="pl-7 space-y-4">
                  <div className="space-y-2">
                     <Label htmlFor="theme">Theme</Label>
                     <Select
                        value={preferences.theme}
                        onValueChange={(value) =>
                           handleSelectChange('theme', value)
                        }
                     >
                        <SelectTrigger className="transition-all duration-200">
                           <SelectValue placeholder="Select theme" />
                        </SelectTrigger>
                        <SelectContent>
                           <SelectItem value="Light">Light</SelectItem>
                           <SelectItem value="Dark">Dark</SelectItem>
                           <SelectItem value="System">
                              System Default
                           </SelectItem>
                        </SelectContent>
                     </Select>
                  </div>

                  <div className="flex items-center justify-between">
                     <div className="space-y-0.5">
                        <Label
                           htmlFor="highContrast"
                           className="text-sm font-medium"
                        >
                           High Contrast Mode
                        </Label>
                        <p className="text-xs text-gray-500">
                           Increase contrast for better visibility
                        </p>
                     </div>
                     <Switch
                        id="highContrast"
                        checked={preferences.highContrast}
                        onCheckedChange={(checked) =>
                           handleSwitchChange('highContrast', checked)
                        }
                     />
                  </div>

                  <div className="flex items-center justify-between">
                     <div className="space-y-0.5">
                        <Label
                           htmlFor="reducedMotion"
                           className="text-sm font-medium"
                        >
                           Reduced Motion
                        </Label>
                        <p className="text-xs text-gray-500">
                           Minimize animations throughout the interface
                        </p>
                     </div>
                     <Switch
                        id="reducedMotion"
                        checked={preferences.reducedMotion}
                        onCheckedChange={(checked) =>
                           handleSwitchChange('reducedMotion', checked)
                        }
                     />
                  </div>
               </div>
            </motion.div>

            <Separator />

            {/* Notifications */}
            <motion.div variants={itemVariants} className="space-y-4">
               <div className="flex items-center space-x-2">
                  <Bell className="h-5 w-5 text-blue-600" />
                  <h3 className="text-md font-medium text-gray-900">
                     Notifications
                  </h3>
               </div>
               <div className="pl-7 space-y-4">
                  <div className="flex items-center justify-between">
                     <div className="space-y-0.5">
                        <Label
                           htmlFor="emailNotifications"
                           className="text-sm font-medium"
                        >
                           Email Notifications
                        </Label>
                        <p className="text-xs text-gray-500">
                           Receive notifications via email
                        </p>
                     </div>
                     <Switch
                        id="emailNotifications"
                        checked={preferences.emailNotifications}
                        onCheckedChange={(checked) =>
                           handleSwitchChange('emailNotifications', checked)
                        }
                     />
                  </div>

                  <div className="flex items-center justify-between">
                     <div className="space-y-0.5">
                        <Label
                           htmlFor="pushNotifications"
                           className="text-sm font-medium"
                        >
                           Push Notifications
                        </Label>
                        <p className="text-xs text-gray-500">
                           Receive notifications on your device
                        </p>
                     </div>
                     <Switch
                        id="pushNotifications"
                        checked={preferences.pushNotifications}
                        onCheckedChange={(checked) =>
                           handleSwitchChange('pushNotifications', checked)
                        }
                     />
                  </div>

                  <div className="flex items-center justify-between">
                     <div className="space-y-0.5">
                        <Label
                           htmlFor="smsNotifications"
                           className="text-sm font-medium"
                        >
                           SMS Notifications
                        </Label>
                        <p className="text-xs text-gray-500">
                           Receive notifications via text message
                        </p>
                     </div>
                     <Switch
                        id="smsNotifications"
                        checked={preferences.smsNotifications}
                        onCheckedChange={(checked) =>
                           handleSwitchChange('smsNotifications', checked)
                        }
                     />
                  </div>

                  <div className="flex items-center justify-between">
                     <div className="space-y-0.5">
                        <Label
                           htmlFor="marketingEmails"
                           className="text-sm font-medium"
                        >
                           Marketing Emails
                        </Label>
                        <p className="text-xs text-gray-500">
                           Receive emails about new products and promotions
                        </p>
                     </div>
                     <Switch
                        id="marketingEmails"
                        checked={preferences.marketingEmails}
                        onCheckedChange={(checked) =>
                           handleSwitchChange('marketingEmails', checked)
                        }
                     />
                  </div>
               </div>
            </motion.div>

            <Separator />

            {/* Media */}
            <motion.div variants={itemVariants} className="space-y-4">
               <div className="flex items-center space-x-2">
                  <Volume2 className="h-5 w-5 text-blue-600" />
                  <h3 className="text-md font-medium text-gray-900">Media</h3>
               </div>
               <div className="pl-7 space-y-4">
                  <div className="flex items-center justify-between">
                     <div className="space-y-0.5">
                        <Label
                           htmlFor="soundEffects"
                           className="text-sm font-medium"
                        >
                           Sound Effects
                        </Label>
                        <p className="text-xs text-gray-500">
                           Play sound effects for interactions
                        </p>
                     </div>
                     <Switch
                        id="soundEffects"
                        checked={preferences.soundEffects}
                        onCheckedChange={(checked) =>
                           handleSwitchChange('soundEffects', checked)
                        }
                     />
                  </div>

                  <div className="flex items-center justify-between">
                     <div className="space-y-0.5">
                        <Label
                           htmlFor="autoPlay"
                           className="text-sm font-medium"
                        >
                           Auto-Play Videos
                        </Label>
                        <p className="text-xs text-gray-500">
                           Automatically play videos when browsing
                        </p>
                     </div>
                     <Switch
                        id="autoPlay"
                        checked={preferences.autoPlay}
                        onCheckedChange={(checked) =>
                           handleSwitchChange('autoPlay', checked)
                        }
                     />
                  </div>
               </div>
            </motion.div>
         </div>
      </motion.div>
   );
}
