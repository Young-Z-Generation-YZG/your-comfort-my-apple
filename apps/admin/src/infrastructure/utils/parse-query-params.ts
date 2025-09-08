export const parseQueryParams = (url: string): Record<string, string> => {
   const queryString = url.split('?')[1]; // Get string after '?'
   if (!queryString) return {};

   const params: Record<string, string> = {};

   queryString.split('&').forEach((param) => {
      const [key, value] = param.split('=');
      params[key] = value;
   });

   return params;
};
