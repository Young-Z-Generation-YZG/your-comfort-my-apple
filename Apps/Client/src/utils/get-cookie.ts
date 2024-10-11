export const getCookies = (key: string) => {
   const cookies = document.cookie.split(';');
   const cookie = cookies.find((cookie) => cookie.includes(key));

   if (cookie) {
      return cookie.split('=')[1];
   }

   return null;
};
