/* eslint-disable @typescript-eslint/no-unused-vars */
"use client";

import React, { useState } from "react";
import { Star } from "lucide-react";

import { cn } from "~/lib/utils";

const ratingVariants = {
  default: {
    star: "text-foreground",
    emptyStar: "text-muted-foreground",
  },
  destructive: {
    star: "text-red-500",
    emptyStar: "text-red-200",
  },
  yellow: {
    star: "text-yellow-500",
    emptyStar: "text-yellow-200",
  },
  ["yellow-small"]: {
    star: "text-yellow-500 w-[12px] h-[12px]",
    emptyStar: "text-yellow-200 w-[12px] h-[12px]",
  },
};

interface RatingsProps extends React.HTMLAttributes<HTMLDivElement> {
  rating: number;
  className?: string;
  totalStars?: number;
  size?: number;
  fill?: boolean;
  Icon?: React.ReactElement;
  variant?: keyof typeof ratingVariants;
  onRatingChange?: (rating: number) => void;
}

export const Ratings = ({ ...props }: RatingsProps) => {
  const {
    rating,
    totalStars = 5,
    size = 20,
    fill = true,
    Icon = <Star />,
    variant = "default",
    className = "",
  } = props;

  const fullStars = Math.floor(rating);

  const partialStar =
    rating % 1 > 0 ? (
      <PartialStar
        fillPercentage={rating % 1}
        size={size}
        className={cn(ratingVariants[variant].star, className)}
        Icon={Icon}
      />
    ) : null;

  return (
    <div className={cn("flex items-center gap-2")} {...props}>
      {[...Array(fullStars)].map((_, i) =>
        React.cloneElement(Icon, {
          key: i,
          size,
          className: cn(
            fill ? "fill-current" : "fill-transparent",
            ratingVariants[variant].star
          ),
        })
      )}
      {partialStar}
      {[...Array(totalStars - fullStars - (partialStar ? 1 : 0))].map((_, i) =>
        React.cloneElement(Icon, {
          key: i + fullStars + 1,
          size,
          className: cn(ratingVariants[variant].emptyStar),
        })
      )}
    </div>
  );
};

export const CommentRatings = ({
  rating: initialRating,
  totalStars = 5,
  size = 20,
  fill = true,
  Icon = <Star />,
  variant = "default",
  onRatingChange,
  ...props
}: RatingsProps) => {
  const [hoverRating, setHoverRating] = useState<number | null>(null);
  const [currentRating, setCurrentRating] = useState(initialRating);
  const [isHovering, setIsHovering] = useState(false);

  const handleMouseEnter = (event: React.MouseEvent<HTMLDivElement>) => {
    setIsHovering(true);
    const starIndex = parseInt(
      (event.currentTarget as HTMLDivElement).dataset.starIndex || "0"
    );
    setHoverRating(starIndex);
  };

  const handleMouseLeave = () => {
    setIsHovering(false);
    setHoverRating(null);
  };

  const handleClick = (event: React.MouseEvent<HTMLDivElement>) => {
    const starIndex = parseInt(
      (event.currentTarget as HTMLDivElement).dataset.starIndex || "0"
    );
    setCurrentRating(starIndex);
    setHoverRating(null);
    onRatingChange?.(starIndex);
  };

  const displayRating = hoverRating ?? currentRating;
  const fullStars = Math.floor(displayRating);
  const partialStar =
    displayRating % 1 > 0 ? (
      <PartialStar
        fillPercentage={displayRating % 1}
        size={size}
        className={cn(ratingVariants[variant].star)}
        Icon={Icon}
      />
    ) : null;

  return (
    <div
      className={cn("flex w-fit items-center gap-2 ")}
      onMouseLeave={handleMouseLeave}
      {...props}
    >
      <div className="flex items-center" onMouseEnter={handleMouseEnter}>
        {[...Array(fullStars)].map((_, i) =>
          React.cloneElement(Icon, {
            key: i,
            size,
            className: cn(
              fill ? "fill-current stroke-1" : "fill-transparent",
              ratingVariants[variant].star
            ),
            onClick: handleClick,
            onMouseEnter: handleMouseEnter,
            "data-star-index": i + 1,
          })
        )}
        {partialStar}
        {[
          ...Array(Math.max(0, totalStars - fullStars - (partialStar ? 1 : 0))),
        ].map((_, i) =>
          React.cloneElement(Icon, {
            key: i + fullStars + 1,
            size,
            className: cn("stroke-1", ratingVariants[variant].emptyStar),
            onClick: handleClick,
            onMouseEnter: handleMouseEnter,
            "data-star-index": i + fullStars + 1,
          })
        )}
      </div>
      <span className="text-muted-foreground">
        Current Rating: {`${currentRating}`}
      </span>
    </div>
  );
};

interface PartialStarProps {
  fillPercentage: number;
  size: number;
  className?: string;
  Icon: React.ReactElement;
}

const PartialStar = ({ ...props }: PartialStarProps) => {
  const { fillPercentage, size, className, Icon } = props;
  return (
    <div style={{ position: "relative", display: "inline-block" }}>
      {React.cloneElement(Icon, {
        size,
        className: cn("fill-transparent", className),
      })}
      <div
        style={{
          position: "absolute",
          top: 0,
          overflow: "hidden",
          width: `${fillPercentage * 100}%`,
        }}
      >
        {React.cloneElement(Icon, {
          size,
          className: cn("fill-current", className),
        })}
      </div>
    </div>
  );
};
