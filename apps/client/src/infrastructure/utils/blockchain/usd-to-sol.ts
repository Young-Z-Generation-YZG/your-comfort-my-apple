const SOL_PRICE_USD = 10000;
const LAMPORTS_PER_SOL = 1000000000;

const usdToLamports = (usdAmount: string) => {
   const usdAmountFloat = parseFloat(usdAmount);

   if (isNaN(usdAmountFloat)) {
      throw new Error('Invalid USD amount');
   }
   const solAmount = usdAmountFloat / SOL_PRICE_USD;

   const amountLamports = BigInt(Math.floor(solAmount * LAMPORTS_PER_SOL));

   return amountLamports;
};

export default usdToLamports;
