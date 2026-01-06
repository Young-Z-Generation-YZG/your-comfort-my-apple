export const truncateAddress = (
   address: string,
   startChars: number,
   endChars: number,
): string => {
   if (address.length <= startChars + endChars) {
      return address;
   }
   return `${address.slice(0, startChars)}...${address.slice(-endChars)}`;
};
