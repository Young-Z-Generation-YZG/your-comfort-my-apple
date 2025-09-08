const isDifferentValue = (value: any, newValue: any): boolean => {
   if (typeof value === 'object' && typeof newValue === 'object') {
      for (const key in value) {
         if (value[key] !== newValue[key]) {
            return true;
         }
      }

      return false;
   }

   return value !== newValue;
};

export default isDifferentValue;
