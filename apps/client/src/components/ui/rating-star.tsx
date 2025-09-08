type RatingStarProps = {
   rating: number;
   size?: 'sm' | 'md' | 'lg';
   className?: string;
};

const RatingStar = ({
   rating,
   size = 'md',
   className = '',
}: RatingStarProps) => {
   return (
      <div className="flex flex-row gap-1">
         {Array(5)
            .fill(0)
            .map((_, index) => {
               const fillPercentage = Math.min(
                  Math.max((rating - index) * 100, 0),
                  100,
               );
               return (
                  <svg
                     key={index}
                     xmlns="http://www.w3.org/2000/svg"
                     xmlnsXlink="http://www.w3.org/1999/xlink"
                     width={size === 'sm' ? '12' : size === 'lg' ? '24' : '16'}
                     height={size === 'sm' ? '12' : size === 'lg' ? '24' : '16'}
                     viewBox="0 0 16 16"
                     className="mdl-js"
                  >
                     <defs>
                        <linearGradient id={`star-gradient-${index}`}>
                           <stop
                              offset={`${fillPercentage}%`}
                              stopColor="#FFAA4E"
                           />
                           <stop
                              offset={`${fillPercentage}%`}
                              stopColor="#D9D9D9"
                           />
                        </linearGradient>
                     </defs>
                     <path
                        fill={`url(#star-gradient-${index})`}
                        d="M7.322 1.038c.255-.622 1.066-.633 1.341-.034l.015.034 1.773 4.316 4.685.341c.662.048.926.796.468 
                              1.245l-.025.023-.026.023-3.585 3.008 1.12 4.523c.16.644-.468 1.127-1.037.832l-.03-.015-.028-.017L8 
                              12.857l-3.993 2.46c-.564.348-1.217-.103-1.109-.735l.006-.032.008-.033 1.12-4.523L.446 6.986C-.063 
                              6.56.162 5.8.795 5.703l.034-.005.035-.003 4.685-.34 1.773-4.317z"
                        transform="translate(-116 -202) translate(37 186) translate(39 14) translate(0 2) translate(40)"
                     />
                     <path
                        stroke="#000"
                        strokeWidth="0.5"
                        fill="none"
                        d="M7.322 1.038c.255-.622 1.066-.633 1.341-.034l.015.034 1.773 4.316 4.685.341c.662.048.926.796.468 
                              1.245l-.025.023-.026.023-3.585 3.008 1.12 4.523c.16.644-.468 1.127-1.037.832l-.03-.015-.028-.017L8 
                              12.857l-3.993 2.46c-.564.348-1.217-.103-1.109-.735l.006-.032.008-.033 1.12-4.523L.446 6.986C-.063 
                              6.56.162 5.8.795 5.703l.034-.005.035-.003 4.685-.34 1.773-4.317z"
                        transform="translate(-116 -202) translate(37 186) translate(39 14) translate(0 2) translate(40)"
                     />
                  </svg>
               );
            })}
      </div>
   );
};

export default RatingStar;
