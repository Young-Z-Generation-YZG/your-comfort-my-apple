'use client';

import { motion } from 'framer-motion';
import { LockKeyhole, ShieldCheck, Clock } from 'lucide-react';

type SecuritySettingsProps = {
   activeSection: string;
   setActiveSection: (section: string) => void;
};

export function SecuritySettings({
   activeSection,
   setActiveSection,
}: SecuritySettingsProps) {
   const sections = [
      {
         id: 'password',
         title: 'Password',
         description:
            "Change your password or reset it if you've forgotten it.",
         icon: <LockKeyhole className="h-5 w-5" />,
      },
      {
         id: 'two-factor',
         title: 'Two-Factor Authentication',
         description: 'Add an extra layer of security to your account.',
         icon: <ShieldCheck className="h-5 w-5" />,
      },
      {
         id: 'activity',
         title: 'Login History',
         description: 'View your recent account activity.',
         icon: <Clock className="h-5 w-5" />,
      },
   ];

   return (
      <motion.div
         className="bg-white rounded-lg border border-gray-200 overflow-hidden"
         initial={{ opacity: 0, y: 20 }}
         animate={{ opacity: 1, y: 0 }}
         transition={{ duration: 0.3 }}
      >
         <div className="divide-y divide-gray-200">
            {sections.map((section) => (
               <motion.div
                  key={section.id}
                  className={`px-6 py-4 cursor-pointer hover:bg-gray-50 transition-colors duration-200 ${
                     activeSection === section.id ? 'bg-gray-50' : ''
                  }`}
                  onClick={() => setActiveSection(section.id)}
                  whileHover={{ backgroundColor: 'rgba(249, 250, 251, 0.8)' }}
                  whileTap={{ backgroundColor: 'rgba(243, 244, 246, 1)' }}
               >
                  <div className="flex items-start">
                     <div
                        className={`flex-shrink-0 p-1.5 rounded-full ${
                           activeSection === section.id
                              ? 'bg-blue-100 text-blue-600'
                              : 'bg-gray-100 text-gray-500'
                        }`}
                     >
                        {section.icon}
                     </div>
                     <div className="ml-4 flex-1">
                        <h3 className="text-sm font-medium text-gray-900">
                           {section.title}
                        </h3>
                        <p className="mt-1 text-sm text-gray-500">
                           {section.description}
                        </p>
                     </div>
                     <div className="ml-3">
                        <svg
                           className={`h-5 w-5 text-gray-400 ${activeSection === section.id ? 'transform rotate-90' : ''}`}
                           xmlns="http://www.w3.org/2000/svg"
                           viewBox="0 0 20 20"
                           fill="currentColor"
                           aria-hidden="true"
                        >
                           <path
                              fillRule="evenodd"
                              d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z"
                              clipRule="evenodd"
                           />
                        </svg>
                     </div>
                  </div>
               </motion.div>
            ))}
         </div>
      </motion.div>
   );
}
