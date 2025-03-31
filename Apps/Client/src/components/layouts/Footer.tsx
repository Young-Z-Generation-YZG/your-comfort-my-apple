'use client';
import { useState } from 'react';
// import {  } from '~/infrastructure/lib/utils';

const Footer = () => {
   const [isOpen, setIsOpen] = useState(false);
   console.log(isOpen);
   return (
      <footer className="w-full h-full">
         <div className="max-w-[1680px] w-[87.5vw] mx-auto font-SFProText mb-5 py-16">
            <h2 className="mb-[53px] text-[56px] font-[600] leading-[60px]">
               iPhone
            </h2>
            <div className="flex flex-wrap lg:flex-row ">
               <div className="w-full mb-14 lg:w-auto lg:mb-0 pr-[88px]">
                  <h3 className="mb-[14px] text-[#6E6E73] text-[17px] font-normal leading-[21px] font-SFProDisplay">
                     Explore iPhone
                  </h3>
                  <ul className="text-[28px] font-semibold leading-[32px] font-SFProDisplay">
                     <li className="mb-[11px] cursor-pointer">
                        <a href="#">Explore All iPhone</a>
                     </li>
                     <li className="mb-[11px] cursor-pointer">
                        <a href="#">iPhone 16 Pro</a>
                     </li>
                     <li className="mb-[11px] cursor-pointer">
                        <a href="#">iPhone 16</a>
                     </li>
                     <li className="mb-[11px] cursor-pointer">
                        <a href="#">iPhone 16e</a>
                     </li>
                     <li className="mb-[11px] cursor-pointer">
                        <a href="#">iPhone 15</a>
                     </li>
                     <li className="mb-[14px] mt-[30px] text-[17px] leading-[21px]">
                        <a href="#">Compare iPhone</a>
                     </li>
                     <li className="text-[17px] leading-[21px]">
                        <a href="#">Switch from Android</a>
                     </li>
                  </ul>
               </div>
               <div className="w-full mb-14 md:w-auto md:mb-0 pr-[44px] font-SFProDisplay">
                  <h3 className="mb-[14px] text-[#6E6E73] text-[17px] font-normal leading-[21px] font-SFProDisplay">
                     Shop iPhone
                  </h3>
                  <ul className="text-[17px] font-semibold leading-[21px]">
                     <li className="mb-[14px]">
                        <a href="#">Shop iPhone</a>
                     </li>
                     <li className="mb-[14px]">
                        <a href="#">iPhone Accessories</a>
                     </li>
                     <li className="mb-[14px]">
                        <a href="#">Apple Trade In</a>
                     </li>
                     <li className="">
                        <a href="#">Financing</a>
                     </li>
                  </ul>
               </div>
               <div className="w-full mb-14 md:w-auto md:mb-0 pr-[44px]">
                  <h3 className="mb-[14px] text-[#6E6E73] text-[17px] font-normal leading-[21px] font-SFProDisplay">
                     More from iPhone
                  </h3>
                  <ul className="text-[17px] font-semibold leading-[21px] font-SFProDisplay">
                     <li className="mb-[14px]">
                        <a href="#">iPhone Support</a>
                     </li>
                     <li className="mb-[14px]">
                        <a href="#">AppleCare+ for iPhone</a>
                     </li>
                     <li className="mb-[14px]">
                        <a href="#">iOS 18</a>
                     </li>
                     <li className="mb-[14px]">
                        <a href="#">Apple Intelligence</a>
                     </li>
                     <li className="mb-[14px]">
                        <a href="#">Apps by Apple</a>
                     </li>
                     <li className="mb-[14px]">
                        <a href="#">iPhone Privacy</a>
                     </li>
                     <li className="mb-[14px]">
                        <a href="#">iCloud+</a>
                     </li>
                     <li className="mb-[14px]">
                        <a href="#">Siri</a>
                     </li>
                  </ul>
               </div>
            </div>
         </div>
         <div className="w-full bg-[#FAFAFC]">
            <div className="w-[980px] mx-auto py-[17px] flex flex-col gap-2 text-[11px] font-light font-SFProText text-[#6E6E73]">
               <div className="w-full flex flex-col gap-2 text-[#0000008F]">
                  <p>
                     * Trade‑in values will vary based on the condition, year,
                     and configuration of your eligible trade‑in device. Not all
                     devices are eligible for credit. You must be at least the
                     age of majority to be eligible to trade in for credit or
                     for an Apple Gift Card. Trade‑in value may be applied
                     toward qualifying new device purchase, or added to an Apple
                     Gift Card. Actual value awarded is based on receipt of a
                     qualifying device matching the description provided when
                     estimate was made. Sales tax may be assessed on full value
                     of a new device purchase. In‑store trade‑in requires
                     presentation of a valid photo ID (local law may require
                     saving this information). Offer may not be available in all
                     stores, and may vary between in‑store and online trade‑in.
                     Some stores may have additional requirements. Apple or its
                     trade‑in partners reserve the right to refuse, cancel, or
                     limit quantity of any trade‑in transaction for any reason.
                     More details are available from Apple’s trade‑in partner
                     for trade‑in and recycling of eligible devices.
                     Restrictions and limitations may apply.
                  </p>
                  <p>
                     ** Pricing for iPhone 16 and iPhone 16 Plus includes a $30
                     connectivity discount that requires activation with AT&T,
                     Boost Mobile, T‑Mobile, or Verizon. Pricing shown for
                     iPhone 15 and iPhone 15 Plus includes a $30 connectivity
                     discount for Boost Mobile, T‑Mobile, and Verizon customers
                     that requires activation and would otherwise be $30 higher
                     for all other customers. Financing available to qualified
                     customers, subject to credit approval and credit limit, and
                     requires you to select Citizens One Apple iPhone Payments
                     or Apple Card Monthly Installments (ACMI) as your payment
                     type at checkout at Apple. You’ll need to select AT&T,
                     Boost Mobile, T‑Mobile, or Verizon as your carrier when you
                     checkout. An iPhone purchased with ACMI is always unlocked,
                     so you can switch carriers at any time, subject to your
                     carrier’s terms. Taxes and shipping on items purchased
                     using ACMI are subject to your card’s variable APR, not the
                     ACMI 0% APR. ACMI is not available for purchases made
                     online at special storefronts. The last month’s payment for
                     each product will be the product’s purchase price, less all
                     other payments at the monthly payment amount. ACMI
                     financing is subject to change at any time for any reason,
                     including but not limited to, installment term lengths and
                     eligible products. See the Apple Card Customer Agreement
                     for more information about ACMI. Additional Citizens One
                     Apple iPhone Payments terms are here.
                  </p>
               </div>
               <div className="w-full border border-[#ccc]" />
               <div className="w-full flex flex-row">
                  <div className="flex-1 flex flex-col gap-5">
                     <div className="flex flex-col gap-1">
                        <p className="font-medium">Shop and Learn</p>
                        <ul className="flex flex-col gap-1">
                           <li>Store</li>
                           <li>Mac</li>
                           <li>iPad</li>
                           <li>iPhone</li>
                           <li>Watch</li>
                           <li>Vision</li>
                           <li>AirPods</li>
                           <li>TV & Home</li>
                           <li>AirTag</li>
                           <li>Accessories</li>
                           <li>Gift Cards</li>
                        </ul>
                     </div>
                     <div className="flex flex-col gap-1">
                        <p className="font-medium">Apple Wallet</p>
                        <ul className="flex flex-col gap-1">
                           <li>Wallet</li>
                           <li>Apple Card</li>
                           <li>Apple Pay</li>
                           <li>Apple Cash</li>
                        </ul>
                     </div>
                  </div>

                  <div className="flex-1 flex flex-col gap-5">
                     <ul className="flex flex-col gap-1">
                        <p className="font-medium">Account</p>
                        <li>Manage Your Apple Account</li>
                        <li>Apple Store Account</li>
                        <li>iCloud.com</li>
                     </ul>
                     <ul className="flex flex-col gap-1">
                        <p className="font-medium">Entertainment</p>
                        <li>Apple One</li>
                        <li>Apple TV+</li>
                        <li>Apple Music</li>
                        <li>Apple Arcade</li>
                        <li>Apple Fitness+</li>
                        <li>Apple News+</li>
                        <li>Apple Podcasts</li>
                        <li>Apple Books</li>
                        <li>App Store</li>
                     </ul>
                  </div>

                  <div className="flex-1 flex flex-col gap-5">
                     <ul className="flex flex-col gap-1">
                        <p className="font-medium">Apple Store</p>
                        <li>Find a Store</li>
                        <li>Genius Bar</li>
                        <li>Today at Apple</li>
                        <li>Group Reservations</li>
                        <li>Apple Camp</li>
                        <li>Apple Store App</li>
                        <li>Certified Refurbished</li>
                        <li>Apple Trade In</li>
                        <li>Financing</li>
                        <li>Carrier Deals at Apple</li>
                        <li>Order Status</li>
                        <li>Shopping Help</li>
                     </ul>
                  </div>

                  <div className="flex-1 flex flex-col gap-5">
                     <ul className="flex flex-col gap-1">
                        <p className="font-medium">For Business</p>
                        <li>Apple and Business</li>
                        <li>Shop for Business</li>
                     </ul>
                     <ul className="flex flex-col gap-1">
                        <p className="font-medium">For Education</p>
                        <li>Apple and Education</li>
                        <li>Shop for K-12</li>
                        <li>Shop for College</li>
                     </ul>
                     <ul className="flex flex-col gap-1">
                        <p className="font-medium">For Healthcare</p>
                        <li>Apple in Healthcare</li>
                        <li>Mac in Healthcare</li>
                        <li>Health on Apple Watch</li>
                        <li>Health Records on iPhone and iPad</li>
                     </ul>
                     <ul className="flex flex-col gap-1">
                        <p className="font-medium">For Government</p>
                        <li>Shop for Government</li>
                        <li>Shop for Veterans and Military</li>
                     </ul>
                  </div>

                  <div className="flex-1 flex flex-col gap-5">
                     <ul className="flex flex-col gap-1">
                        <p className="font-medium">Apple Values</p>
                        <li>Accessibility</li>
                        <li>Education</li>
                        <li>Environment</li>
                        <li>Inclusion and Diversity</li>
                        <li>Privacy</li>
                        <li>Racial Equity and Justice</li>
                        <li>Supply Chain</li>
                     </ul>
                     <ul className="flex flex-col gap-1">
                        <p className="font-medium">About Apple</p>
                        <li>Newsroom</li>
                        <li>Newsroom</li>
                        <li>Apple Leadership</li>
                        <li>Career Opportunities</li>
                        <li>Investors</li>
                        <li>Ethics & Compliance</li>
                        <li>Events</li>
                        <li>Contact Apple</li>
                     </ul>
                  </div>
               </div>
            </div>
         </div>
      </footer>
   );
};

export default Footer;
