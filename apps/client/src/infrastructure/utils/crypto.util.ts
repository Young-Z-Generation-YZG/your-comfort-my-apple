import CryptoJS from 'crypto-js';

const SECRET_KEY =
   process.env.NEXT_PUBLIC_CRYPTO_SECRET ||
   'ygz-default-secret-key-change-in-production';

export const encryptPassword = (password: string): string => {
   return CryptoJS.AES.encrypt(password, SECRET_KEY).toString();
};

export const decryptPassword = (encryptedPassword: string): string => {
   const bytes = CryptoJS.AES.decrypt(encryptedPassword, SECRET_KEY);
   return bytes.toString(CryptoJS.enc.Utf8);
};
