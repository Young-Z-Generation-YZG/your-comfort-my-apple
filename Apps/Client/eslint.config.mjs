// import globals from 'globals';
// import pluginJs from '@eslint/js';
// import tseslint from 'typescript-eslint';
// import pluginReact from 'eslint-plugin-react';

// export default [
//    { files: ['**/*.{js,mjs,cjs,ts,jsx,tsx}'] },
//    { languageOptions: { globals: globals.browser } },
//    pluginJs.configs.recommended,
//    ...tseslint.configs.recommended,
//    pluginReact.configs.flat.recommended,
// ];

import { dirname } from 'path';
import { fileURLToPath } from 'url';
import { FlatCompat } from '@eslint/eslintrc';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const compat = new FlatCompat({
   baseDirectory: __dirname,
});

const eslintConfig = [
   // ...compat.extends("next/core-web-vitals", "next/typescript"),
   ...compat.config({
      extends: ['next/core-web-vitals', 'next', 'next/typescript'],
      rules: {
         '@typescript-eslint/no-explicit-any': 'off',
         '@typescript-eslint/no-unused-vars': 'off', // Changed from error to warn
         'react/react-in-jsx-scope': 'off',
         // Add more rules to silence specific errors
         'no-empty': 'warn',
         'no-unused-vars': 'warn',
      },
   }),
];

export default eslintConfig;
