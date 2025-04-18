export const serializeUrl = (
   url: string,
   params: Record<string, string> = {},
) => {
   if (url.startsWith('/')) {
      url = url.slice(1);
   }

   const urlParams = new URLSearchParams();

   for (const key in Object.keys(params)) {
      urlParams.append(key, params[key]);
   }

   return `${url}?${urlParams}`;
};
