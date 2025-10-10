import { Button } from '@components/ui/button';
import RatingStar from '@components/ui/rating-star';
import { Separator } from '@components/ui/separator';
import { Fragment } from 'react';
import { cn } from '~/infrastructure/lib/utils';
import NextImage from 'next/image';
import {
   IModel,
   IStorage,
   IColor,
   IAverageRating,
   ISKUPrice,
} from '~/domain/interfaces/common/value-objects.interface';
import { useRouter } from 'next/navigation';

interface IphoneModelProps {
   models: IModel[];
   colors: IColor[];
   storages: IStorage[];
   averageRating: IAverageRating;
   skuPrices: ISKUPrice[];
   modelSlug: string;
}

const IphoneModel = ({
   models,
   colors,
   storages,
   averageRating,
   skuPrices,
   modelSlug,
}: IphoneModelProps) => {
   const router = useRouter();

   // Calculate min and max prices from skuPrices
   const priceRange = skuPrices.reduce(
      (acc, sku) => {
         if (sku.unit_price < acc.min) acc.min = sku.unit_price;
         if (sku.unit_price > acc.max) acc.max = sku.unit_price;
         return acc;
      },
      { min: Infinity, max: -Infinity },
   );

   // Format price for display
   const formatPrice = (price: number) => {
      return new Intl.NumberFormat('en-US', {
         style: 'currency',
         currency: 'USD',
         minimumFractionDigits: 0,
         maximumFractionDigits: 0,
      }).format(price);
   };

   return (
      <Fragment>
         <div className="flex flex-row bg-white px-5 py-5 rounded-md">
            {/* image */}
            <div className="basis-[20%] overflow-hidden relative h-[300px] rounded-lg">
               <NextImage
                  src={
                     'https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp'
                  }
                  alt={`test`}
                  width={Math.round((1000 * 16) / 9)}
                  height={1000}
                  className="absolute top-0 left-0 w-full h-full object-cover"
               />
            </div>

            {/* content */}
            <div className="flex flex-col relative basis-[45%] px-7">
               <div className="content flex flex-col gap-2">
                  <h2 className="font-SFProText text-xl text-start font-semibold">
                     {models.length > 1 ? (
                        <span>
                           {models[0].name} & {models[1].name}
                        </span>
                     ) : (
                        models[0].name
                     )}
                  </h2>

                  <span className="flex flex-row gap-2">
                     <p className="first-letter:uppercase text-sm">colors:</p>
                  </span>

                  {/* start colors */}
                  <div className="flex flex-row gap-2">
                     {colors.map((color, index) => (
                        <div
                           key={index}
                           className={cn(
                              'h-[30px] w-[30px] cursor-pointer rounded-full border-2 border-solid shadow-color-selector transition-all duration-300 ease-in-out',
                           )}
                           style={{ backgroundColor: color.hex_code }}
                        />
                     ))}
                  </div>
                  {/* end color */}

                  <span className="flex flex-row gap-2 mt-2">
                     <p className="first-letter:uppercase text-sm">Storages:</p>
                  </span>

                  {/* start storage */}
                  <div className="flex flex-row gap-2">
                     {storages.map((storage, index) => (
                        <span
                           key={index}
                           className={cn(
                              'uppercase select-none text-xs font-medium min-w-[70px] py-1 border rounded-full text-center',
                           )}
                        >
                           {storage.name}
                        </span>
                     ))}
                  </div>
                  {/* end storage */}

                  <Separator className="mt-2" />

                  <span className="flex flex-row gap-2 mt-2 items-center">
                     <RatingStar
                        rating={averageRating.rating_average_value}
                        size="md"
                        className="text-sm"
                     />
                     <span className="text-base inline-block leading-7">
                        {averageRating.rating_average_value}
                     </span>
                     <span className="text-sm">
                        ({averageRating.rating_count})
                     </span>
                  </span>

                  {/* prices */}
                  <div className="flex flex-row gap-2 mt-2 justify-end">
                     {priceRange.min === priceRange.max ? (
                        <span className="text-lg font-semibold text-gray-900">
                           {formatPrice(priceRange.min)}
                        </span>
                     ) : (
                        <span className="text-lg font-semibold text-gray-900">
                           {formatPrice(priceRange.min)} -{' '}
                           {formatPrice(priceRange.max)}
                        </span>
                     )}
                  </div>
               </div>
            </div>

            {/* start feature */}
            <div className="flex flex-col justify-between flex-1 border-l-2 border-[#E5E7EB] px-4">
               <div className="flex flex-col gap-2">
                  <div className="flex flex-row gap-4 items-center border-b border-[#E5E7EB] pb-2">
                     <span className="">
                        <svg
                           height="35"
                           viewBox="0 0 35 35"
                           width="35"
                           aria-hidden="true"
                        >
                           <path d="m0 0h35v35h-35z" fill="none"></path>
                           <path
                              d="m12.6631 22.3228a.5.5 0 0 1 0 .707l-4.9697 4.9702h4.3066a.5.5 0 0 1 0 1h-5.5a.5.5 0 0 1 -.5-.5v-5.5a.5.5 0 0 1 1 0v4.2792l4.9561-4.9564a.5.5 0 0 1 .707 0zm16.3369-15.8228a.5.5 0 0 0 -.5-.5h-5.5a.5.5 0 0 0 0 1h4.314l-4.974 4.9741a.5.5 0 0 0 .7071.7071l4.9529-4.9527v4.2832a.5.5 0 0 0 1 0v-5.5l-.0012-.0058zm-14.5474 12.1035a2.2014 2.2014 0 0 1 -2.3554 2.2227c-1.4688 0-2.5362-1.08-2.5362-3.252 0-2.1347.9893-3.4 2.5616-3.4a2.09 2.09 0 0 1 2.2421 1.7129h-1.1347a1.02 1.02 0 0 0 -1.085-.7861c-.94 0-1.4892.8262-1.499 2.3428h.0752a1.7449 1.7449 0 0 1 1.6435-.9454 2.0325 2.0325 0 0 1 2.0879 2.1051zm-1.1123.03a1.2525 1.2525 0 1 0 -2.5049-.0225 1.2528 1.2528 0 1 0 2.5049.0225zm2.9366.7607a.6834.6834 0 1 0 .7031.6807.6712.6712 0 0 0 -.7031-.6804zm1.2773-3.8984v1.0684l1.5771-1.1172h.0752v5.2256h1.1329v-6.3406h-1.1294zm4.3779 1.4805h.8037l.7342-2.6443h-1.1033zm1.8936 0h.8043l.8086-2.6446h-1.1032z"
                              fill="#1d1d1f"
                           ></path>
                        </svg>
                     </span>
                     <p className="text-xs font-semibold">
                        Stunning 6.1-inch Super Retina XDR display¹ protected by
                        durable Ceramic Shield, tougher than any smartphone
                        glass
                     </p>
                  </div>

                  <div className="flex flex-row gap-4 items-center border-b border-[#E5E7EB] pb-2">
                     <span className="">
                        <svg
                           width="35"
                           height="35"
                           viewBox="0 0 35 35"
                           aria-hidden="true"
                        >
                           <path fill="none" d="M0 0H35V35H0z"></path>
                           <path
                              d="M24.5 29h-14A4.505 4.505 0 0 1 6 24.5v-14C6 8.019 8.019 6 10.5 6h14C26.98 6 29 8.019 29 10.5v14c0 2.481-2.019 4.5-4.5 4.5zm-14-22C8.57 7 7 8.57 7 10.5v14c0 1.93 1.57 3.5 3.5 3.5h14c1.93 0 3.5-1.57 3.5-3.5v-14C28 8.57 26.43 7 24.5 7h-14zm2.469 8.042h1.214l1.715 4.933h-1.087l-.366-1.197h-1.74l-.365 1.197h-1.083l1.712-4.933zm-.028 2.964h1.269l-.602-1.98h-.061l-.606 1.98zm4.455-1.99h-.062l-1.21.841v-.93l1.275-.885h1.029v4.933h-1.032v-3.958zm2.162 2.622v-.006c0-.643.479-1.118 1.153-1.252v-.02c-.578-.14-.97-.557-.97-1.094v-.007c0-.766.747-1.327 1.821-1.327 1.07 0 1.822.561 1.822 1.327v.007c0 .537-.397.95-.971 1.094v.02c.67.13 1.151.61 1.151 1.252v.006c0 .855-.816 1.446-2.002 1.446s-2.004-.59-2.004-1.446zm2.926-.069v-.006c0-.445-.375-.765-.922-.765s-.926.32-.926.765v.006c0 .446.382.763.926.763.54 0 .922-.317.922-.763zm-.123-2.204v-.007c0-.403-.33-.68-.799-.68-.472 0-.803.277-.803.68v.007c0 .404.328.684.803.684.471 0 .8-.28.8-.684z"
                              fill="#1d1d1f"
                           ></path>
                        </svg>
                     </span>
                     <p className="text-xs font-semibold">
                        A18 chip powers Apple Intelligence, gaming, and iOS
                        updates for years to come
                     </p>
                  </div>

                  <div className="flex flex-row gap-4 items-center border-b border-[#E5E7EB] pb-2">
                     <span className="">
                        <svg width="35" height="35" aria-hidden="true">
                           <defs>
                              <linearGradient
                                 id="16e"
                                 x1="10.595"
                                 y1="6.682"
                                 x2="25.102"
                                 y2="28.189"
                                 gradientUnits="userSpaceOnUse"
                              >
                                 <stop offset=".09" stopColor="#fd9700"></stop>
                                 <stop offset=".28" stopColor="#f40"></stop>
                                 <stop offset=".49" stopColor="#ff2469"></stop>
                                 <stop offset=".78" stopColor="#c65cff"></stop>
                                 <stop offset="1" stopColor="#0092ff"></stop>
                              </linearGradient>
                           </defs>
                           <path fill="none" d="M0 0H35V35H0z"></path>
                           <path
                              d="M30.212 18.243a4.712 4.712 0 0 0-.657-1.082 8.453 8.453 0 0 0-.993-1.02l-1.158-.926.438-.902c.08-.178.156-.351.224-.517.341-.824.53-1.59.563-2.277.04-.827-.225-1.603-.788-2.309a3.376 3.376 0 0 0-1.81-1.195 4.73 4.73 0 0 0-1.257-.16c-.45.003-.925.051-1.415.14l-1.446.327-.433-.905c-.09-.175-.177-.342-.264-.498-.433-.781-.914-1.406-1.429-1.86-.622-.546-1.394-.824-2.297-.824-.77 0-1.465.226-2.063.67-.352.262-.65.551-.91.883a8.45 8.45 0 0 0-.77 1.192l-.648 1.335-.979-.225a20.478 20.478 0 0 0-.553-.104c-.88-.148-1.667-.162-2.344-.042-.815.145-1.514.577-2.076 1.283a3.38 3.38 0 0 0-.762 2.03c-.015.44.025.852.122 1.262.104.434.256.887.453 1.35l.64 1.336-.784.622c-.15.126-.293.248-.428.37-.664.593-1.166 1.201-1.495 1.807-.395.726-.493 1.54-.291 2.42a3.382 3.382 0 0 0 1.112 1.863c.334.286.681.512 1.062.69.406.192.856.355 1.333.487l1.445.333-.004.626.005.379c.005.196.011.383.022.562.05.89.212 1.662.48 2.297.322.76.899 1.346 1.712 1.737.49.236.993.354 1.504.354.215 0 .43-.02.647-.063a4.687 4.687 0 0 0 1.2-.4c.402-.198.81-.447 1.215-.74l1.16-.923.783.628c.157.118.308.23.456.335.728.515 1.432.87 2.093 1.055.288.08.578.12.87.12.516 0 1.037-.125 1.557-.375a3.384 3.384 0 0 0 1.568-1.498c.203-.388.346-.777.437-1.19.095-.44.154-.914.177-1.411l.025-.9-.007-.587.963-.217c.19-.048.372-.097.545-.147.858-.248 1.574-.578 2.131-.98.67-.484 1.113-1.175 1.314-2.055a3.386 3.386 0 0 0-.195-2.16zm-4.894-.928a11.245 11.245 0 0 0-.852 3.716l-.01.213a31.437 31.437 0 0 0-.026 1.464l.01.307-1.055.248c-.149.034-.474.11-.757.186l-.13.036a11.191 11.191 0 0 0-3.439 1.65l-.172.125a26.76 26.76 0 0 0-.597.447c-.083.062-.345.27-.534.42l-.264.225-.949-.748c-.135-.105-.331-.258-.517-.396l-.112-.082a11.137 11.137 0 0 0-1.651-.978 11.45 11.45 0 0 0-1.786-.681l-.204-.058c-.223-.06-.465-.124-.72-.186-.102-.027-.434-.103-.67-.157L10.55 23l-.006-1.142c-.002-.168-.004-.458-.012-.69l-.007-.167a11.273 11.273 0 0 0-.845-3.72l-.082-.194a28.075 28.075 0 0 0-.6-1.305l-.154-.297.123-.098.75-.61c.129-.105.364-.295.542-.448l.13-.112a11.226 11.226 0 0 0 2.383-2.982l.1-.184c.11-.204.224-.426.344-.662.043-.084.167-.342.269-.552l.167-.375 1.155.256c.168.037.427.093.636.134l.166.03c.635.11 1.277.165 1.911.165.635 0 1.276-.054 1.948-.168l.163-.03c.229-.041.473-.09.733-.143.11-.022.488-.108.723-.162l.276-.072.506 1.036c.076.153.2.406.303.603l.08.149a11.155 11.155 0 0 0 2.384 2.992l.15.133c.175.152.366.313.57.483l.476.384.33.248-.49 1.034c-.072.153-.195.415-.288.625l-.067.155zm-1.78-8.336c.434-.079.853-.12 1.246-.124.33.001.656.038.991.126.516.136.936.415 1.284.852.413.518.6 1.054.571 1.638-.027.571-.191 1.225-.488 1.943-.064.157-.136.319-.212.49l-.323.673-.004-.004-.005.01-.14-.111c-.175-.14-.39-.312-.46-.371-.2-.165-.384-.32-.556-.471l-.146-.129a10.198 10.198 0 0 1-2.167-2.725l-.069-.126c-.1-.192-.22-.435-.292-.582l-.414-.84 1.184-.248zm-8.912-1.523c.21-.39.439-.744.68-1.052.203-.26.438-.488.718-.696a2.37 2.37 0 0 1 1.466-.473c.662 0 1.197.188 1.637.576.429.377.837.913 1.215 1.593.082.148.164.305.247.468l.328.676h-.005l.004.01-.05.011c-.228.053-.593.136-.703.158-.253.053-.49.1-.712.14l-.194.034a10.31 10.31 0 0 1-3.472-.003l-.15-.028c-.203-.039-.452-.093-.615-.128l-.937-.206.543-1.08zM7.385 11.29a2.371 2.371 0 0 1 .545-1.44c.412-.518.893-.819 1.468-.921.246-.044.513-.066.8-.066.37 0 .772.036 1.203.11.168.028.343.061.522.097l.732.166-.002.004.01.003-.077.16c-.097.201-.217.45-.26.533-.117.231-.227.445-.334.645l-.095.173c-.282.51-.608.998-.967 1.45-.361.451-.767.878-1.205 1.267l-.11.096c-.176.15-.402.333-.583.48-.099.082-.48.392-.645.525l-.507-1.098a7.48 7.48 0 0 1-.399-1.188 3.712 3.712 0 0 1-.096-.996zm.994 11.21a7.414 7.414 0 0 1-1.176-.427 3.716 3.716 0 0 1-.839-.546 2.375 2.375 0 0 1-.787-1.325c-.147-.645-.083-1.208.196-1.722.274-.503.706-1.022 1.282-1.539.127-.112.261-.228.405-.347l.582-.468.002.003.01-.008.045.096a26.54 26.54 0 0 1 .582 1.266l.077.182a10.216 10.216 0 0 1 .77 3.389l.007.148c.007.224.01.503.012.754l.007.834-1.175-.29zm7.37 5.24c-.36.26-.719.48-1.07.653a3.709 3.709 0 0 1-.948.315 2.39 2.39 0 0 1-1.527-.21c-.596-.287-.997-.689-1.225-1.227-.223-.528-.359-1.19-.403-1.964a17.07 17.07 0 0 1-.02-.533l-.005-.367.002-.38.006.001v-.011l.1.023c.228.053.55.126.653.152.251.062.485.123.698.18l.192.056c.561.162 1.11.372 1.628.62.522.253 1.028.552 1.51.896l.114.083c.157.116.345.264.475.365l.777.609-.958.739zm8.688-2.495a7.44 7.44 0 0 1-.155 1.242 3.707 3.707 0 0 1-.345.939c-.248.473-.613.82-1.116 1.062-.597.287-1.161.35-1.723.193-.551-.155-1.152-.46-1.785-.907-.14-.1-.282-.205-.432-.319l-.585-.464.005-.004-.009-.006.088-.07c.182-.145.435-.347.517-.41.205-.156.398-.3.58-.434l.161-.115c.476-.338.98-.636 1.5-.886a10.33 10.33 0 0 1 1.638-.62l.139-.039c.242-.063.557-.137.73-.178l.813-.194.004.352-.024.858zm4.996-5.063c-.148.645-.45 1.125-.924 1.467-.464.334-1.077.614-1.825.83-.163.047-.333.092-.52.14l-.722.166v-.006l-.011.003v-.073c0-.234 0-.589.003-.697.005-.26.012-.501.022-.725l.01-.198a10.27 10.27 0 0 1 .776-3.387l.059-.137c.09-.204.208-.457.315-.683l.356-.756.932.769c.336.294.63.595.876.897.207.255.377.535.518.855.215.488.259.99.135 1.535z"
                              fill="url(#16e)"
                           ></path>
                        </svg>
                     </span>
                     <p className="text-xs font-semibold">
                        Built for Apple Intelligence. footnote 2 Write, express
                        yourself, and get things done effortlessly.
                     </p>
                  </div>

                  <div className="flex flex-row gap-4 items-center">
                     <span className="">
                        <svg
                           height="35"
                           viewBox="0 0 35 35"
                           width="35"
                           aria-hidden="true"
                        >
                           <path d="m0 0h35v35h-35z" fill="none"></path>
                           <path d="m16.2444 16.886c.3408.2773.3408.7842 0 1.0615l-3.061 2.4917c-.4675.3804-1.1824.0596-1.1825-.5308l-.0004-1.9084h-5.5006c-.2764 0-.5-.2236-.5-.5s.2236-.5.5-.5h5.5005l-.0004-2.0754c0-.5906.7148-.9114 1.1824-.531zm9.3917-10.886h-.0039c-1.501 0-2.918.5908-3.9912 1.6631-1.0586 1.0586-1.6415 2.3652-1.641 3.8369v3.0012l-.3044-.0005c-.2688-.0002-.4918.0779-.6689.2346-.1772.1567-.2661.3557-.2666.5964l-.01 5.3362c-.0005.239.0876.437.2642.5945.1766.1572.3994.2361.6682.2366l.3176.001v7.5h.9999v-17.5c-.0004-1.2051.4785-2.2598 1.3481-3.1299.8843-.8838 2.0503-1.3701 3.2842-1.3701h3.3677v-1h-3.3638z"></path>
                        </svg>
                     </span>
                     <p className="text-xs font-semibold">
                        Customize the Action button to use visual intelligence,
                        launch your favorite app, and more
                     </p>
                  </div>
               </div>

               <Button
                  className="rounded-full text-base mt-5"
                  onClick={() => router.push(`/products/iphone/${modelSlug}`)}
               >
                  Visit
               </Button>
            </div>
            {/* end feature */}
         </div>
      </Fragment>
   );
};

export default IphoneModel;
