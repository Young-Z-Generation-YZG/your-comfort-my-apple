/* eslint-disable @typescript-eslint/no-unused-vars */
import { CardContent } from "~/components/card-wrapper";
import { ModelContent } from "~/components/iphone-model";
import { StorageContent } from "~/components/iphone-storage";
import { CommentRatings, Ratings } from "~/components/rating";
import { Star } from "lucide-react";

const starRatings = {
  totalRating: 973,
  starRatings: [
    { star: 5, average: 4.5, count: 800 },
    { star: 4, average: 3.7, count: 100 },
    { star: 3, average: 2.7, count: 50 },
    { star: 2, average: 1.2, count: 20 },
    { star: 1, average: 0.1, count: 3 },
  ],
};
const ProductPage = () => {
  console.log(starRatings.starRatings);

  return (
    <div className="flex gap-4 p-4">
      <CardContent className="basis-[35%]"></CardContent>

      <CardContent className="basis-[65%]">
        <h2 className="text-2xl font-medium">iPhone 16</h2>

        <span className="flex gap-4">
          <Ratings rating={4.8} variant="yellow" totalStars={5} />

          <p className="font-bold">(4.8)</p>
          <p className="font-bold">973 Reviews</p>
        </span>

        <p className="font-bold">1,231 Sold</p>

        {/* Models */}
        <div className="mt-3 mb-3">
          <h3 className="text-lg font-medium mb-3">Available Models</h3>

          <div className="flex flex-col gap-4">
            <ModelContent
              model="iPhone 16"
              description="6.1-inch display"
              amount="799"
            />
            <ModelContent
              model="iPhone 16 Plus"
              description="6.7-inch display"
              amount="899"
            />
          </div>
        </div>

        {/* Colors */}
        <div className="mt-3 mb-3">
          <h3 className="text-lg font-medium mb-3">Available Colors</h3>
          <div className="flex gap-4">
            <span className="w-[28px] h-[28px] bg-red-300 rounded-full shadow"></span>
            <span className="w-[28px] h-[28px] bg-red-300 rounded-full shadow"></span>
            <span className="w-[28px] h-[28px] bg-red-300 rounded-full shadow"></span>
            <span className="w-[28px] h-[28px] bg-red-300 rounded-full shadow"></span>
          </div>
        </div>

        {/* Storages */}
        <div className="mt-3 mb-3">
          <h3 className="text-lg font-medium mb-3">Available Storages</h3>

          <div className="flex flex-col gap-4">
            <StorageContent storage="128GB" amount="799" />
            <StorageContent storage="256GB" amount="899" />
            <StorageContent storage="512GB" amount="1099" />
          </div>
        </div>

        {/* Ratings & Reviews */}
        <div className="mt-3 mb-3">
          <h1 className="text-lg font-medium mb-3">Ratings & Reviews</h1>

          <div className="flex gap-4">
            {/* Rating */}
            <div className="basis-[35%] flex flex-col items-center border border-dashed border-slate-400 p-2 rounded-lg">
              <p className="font-semibold mb-2">Customer Ratings</p>

              <div className="flex flex-col items-center">
                <span className="p-3 bg-muted rounded-xl">
                  <Ratings rating={4.8} variant="yellow" totalStars={5} />
                </span>
                <p className="text-sm font-semibold mt-2">{4.8} of 5</p>
              </div>

              <div>
                {starRatings.starRatings.map((rating) => (
                  <div
                    key={rating.star}
                    className="flex justify-center items-center gap-3 mb-2"
                  >
                    <div className="flex gap-1 justify-center items-center">
                      <span>{rating.star}</span>
                      <i>
                        <Star className="w-[18px] h-[18px] text-yellow-500 fill-current" />
                      </i>
                    </div>
                    <Ratings
                      rating={rating.average}
                      variant="yellow-small"
                      totalStars={5}
                    />
                    <p className="text-sm w-[20px]">{rating.count}</p>
                  </div>
                ))}
              </div>
            </div>
            <div className="basis-[65%]">asdasd</div>
          </div>
        </div>
      </CardContent>

      {/* <Ratings rating={1.5} />
      <Ratings rating={2} variant="destructive" Icon={<Heart />} />

      <CommentRatings
        rating={3}
        totalStars={5}
        size={24}
        variant="default"
        // disabled={true}
      /> */}
    </div>
  );
};

export default ProductPage;
