export const createQueryEncodedUrl = (url: string, queries: object): string => {
   const urlParams = new URLSearchParams();

   Object.entries(queries).forEach(([key, value]) => {
      if (Array.isArray(value) || typeof value === 'object') {
         if (value !== null) {
            urlParams.append(key, JSON.stringify(value));
         }
      } else {
         urlParams.append(key, value);
      }
   });

   const encodedParams = urlParams.toString();

   return encodedParams ? `${url}?${encodedParams}` : url;
};
