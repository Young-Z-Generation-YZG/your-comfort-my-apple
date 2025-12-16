## Frontend Improvement Ideas for `apps/client`

This document captures concrete improvement ideas for the `apps/client` Next.js app, with code examples for each area.

---

### 1. Configuration Cleanup & Hardening

#### 1.1 Next config: re‑enable `reactStrictMode` and keep linting in CI

**Goal**: Catch unsafe side effects in development and keep lint out of production builds while still enforcing lint in CI.

```ts
// apps/client/next.config.mjs
/** @type {import('next').NextConfig} */

const nextConfig = {
    output: "standalone",
    env: {
        API_ENDPOINT: process.env.API_ENDPOINT,
        APP_URL: process.env.APP_URL,
    },
    // Enable strict mode in development
    reactStrictMode: process.env.NODE_ENV === "development",
    eslint: {
        // Let CI run `npm run lint` instead of blocking `next build`
        ignoreDuringBuilds: true,
    },
    webpack: (config) => {
        config.module.rules
            .find(({ oneOf }) => !!oneOf)
            .oneOf.filter(({ use }) =>
                JSON.stringify(use)?.includes("css-loader")
            )
            .reduce((acc, { use }) => acc.concat(use), [])
            .forEach(({ options }) => {
                if (options.modules) {
                    options.modules.exportLocalsConvention = "camelCase";
                }
            });

        return config;
    },
    images: {
        remotePatterns: [
            // ... keep existing patterns
        ],
    },
};

export default nextConfig;
```

Example CI step (GitHub Actions style) to ensure lint still runs:

```yaml
steps:
    - name: Install dependencies
      run: npm ci

    - name: Lint
      run: npm run lint --workspace apps/client

    - name: Build
      run: npm run build --workspace apps/client
```

#### 1.2 TypeScript: tighten `allowJs` and keep paths simple

**Goal**: Treat the frontend as a fully typed TypeScript codebase and reduce noise in `content` globbing.

```jsonc
// apps/client/tsconfig.json
{
    "compilerOptions": {
        "lib": ["dom", "dom.iterable", "esnext"],
        "allowJs": false, // <‑ turn off once migration is done
        "skipLibCheck": true,
        "strict": true,
        "noEmit": true,
        "esModuleInterop": true,
        "module": "esnext",
        "moduleResolution": "bundler",
        "resolveJsonModule": true,
        "isolatedModules": true,
        "jsx": "preserve",
        "incremental": true,
        "plugins": [{ "name": "next" }],
        "paths": {
            "@constants/*": ["./constants/*"],
            "@components/*": ["./src/components/*"],
            "@assets/*": ["./assets/*"],
            "@public/*": ["./public/*"],
            "~/*": ["./src/*"]
        },
        "target": "ES2017"
    },
    "include": ["next-env.d.ts", "**/*.ts", "**/*.tsx", ".next/types/**/*.ts"],
    "exclude": ["node_modules"]
}
```

#### 1.3 Tailwind config: simplify `content` and fix container padding

```ts
// apps/client/tailwind.config.ts
import type { Config } from "tailwindcss";

const config: Config = {
    darkMode: ["class"],
    // Only scan this app’s source by default
    content: ["./src/**/*.{ts,tsx}"],
    theme: {
        container: {
            center: true,
            // sensible default padding for all breakpoints
            padding: "1.5rem",
            screens: {
                sm: "640px",
                md: "768px",
                lg: "960px",
                xl: "1200px",
            },
        },
        fontFamily: {
            SFProDisplay: "var(--font-SFProDisplay)",
            SFProText: "var(--font-SFProText)",
        },
        extend: {
            // keep existing extensions...
        },
    },
    plugins: [require("tailwindcss-animate")],
};

export default config;
```

---

### 2. ESLint Consolidation & Stricter Typing

#### 2.1 Single flat config with controlled `any` usage

**Goal**: Use one ESLint config (flat) and surface `any` as a warning instead of globally disabling it.

```ts
// apps/client/eslint.config.mjs
import { dirname } from "path";
import { fileURLToPath } from "url";
import { FlatCompat } from "@eslint/eslintrc";

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const compat = new FlatCompat({
    baseDirectory: __dirname,
});

const eslintConfig = [
    ...compat.config({
        extends: ["next/core-web-vitals", "next", "next/typescript"],
        rules: {
            // Prefer warnings instead of completely turning them off
            "@typescript-eslint/no-explicit-any": "warn",
            "@typescript-eslint/no-unused-vars": [
                "warn",
                {
                    args: "all",
                    argsIgnorePattern: "^_",
                    caughtErrors: "all",
                    caughtErrorsIgnorePattern: "^_",
                    destructuredArrayIgnorePattern: "^_",
                    varsIgnorePattern: "^_",
                    ignoreRestSiblings: true,
                },
            ],
            "react/react-in-jsx-scope": "off",
            "no-empty": "warn",
            "no-unused-vars": "off", // handled by TS rule above
        },
    }),
];

export default eslintConfig;
```

Then delete `.eslintrc.json` once this is in place so there is a single source of truth.

---

### 3. Shared Async Result & Hook Helpers

You already use a `{ isSuccess, isError, data, error }` pattern in many hooks. Make it a shared type + helpers to reduce boilerplate.

#### 3.1 Shared result type

```ts
// apps/client/src/domain/types/async-result.type.ts

export type AsyncResult<TData, TError = unknown> = {
    isSuccess: boolean;
    isError: boolean;
    data: TData | null;
    error: TError | null;
};
```

#### 3.2 Generic wrappers for RTK Query

```ts
// apps/client/src/infrastructure/utils/rtk-result-helpers.ts
import type { AsyncResult } from "~/domain/types/async-result.type";

export async function wrapLazyQuery<TArg, TResult>(
    trigger: (arg: TArg) => Promise<TResult>,
    arg: TArg
): Promise<AsyncResult<TResult>> {
    try {
        const data = await trigger(arg);
        return { isSuccess: true, isError: false, data, error: null };
    } catch (error) {
        return { isSuccess: false, isError: true, data: null, error };
    }
}

export async function wrapMutation<TArg, TResult>(
    mutate: (arg: TArg) => Promise<TResult>,
    arg: TArg
): Promise<AsyncResult<TResult>> {
    try {
        const data = await mutate(arg);
        return { isSuccess: true, isError: false, data, error: null };
    } catch (error) {
        return { isSuccess: false, isError: true, data: null, error };
    }
}
```

#### 3.3 Example: refactor `useReviewService`

Before (simplified):

```ts
// Many try/catch blocks that manually build { isSuccess, isError, ... }
const createReviewAsync = useCallback(
    async (payload: IReviewPayload) => {
        try {
            const result = await createReviewMutation(payload).unwrap();
            return {
                isSuccess: true,
                isError: false,
                data: result,
                error: null,
            };
        } catch (error) {
            return {
                isSuccess: false,
                isError: true,
                data: null,
                error,
            };
        }
    },
    [createReviewMutation]
);
```

After (using helper):

```ts
// apps/client/src/components/hooks/api/use-review-service.ts
import { useCallback, useMemo } from "react";
import {
    useCreateReviewAsyncMutation,
    // ... other imports
} from "~/infrastructure/services/review.service";
import {
    wrapMutation,
    wrapLazyQuery,
} from "~/infrastructure/utils/rtk-result-helpers";
import type { AsyncResult } from "~/domain/types/async-result.type";
import type {
    IReviewPayload,
    IUpdateReviewPayload,
} from "~/domain/interfaces/catalog.interface";

const useReviewService = () => {
    const [createReviewMutation, createReviewMutationState] =
        useCreateReviewAsyncMutation();

    const createReviewAsync = useCallback(
        (payload: IReviewPayload): Promise<AsyncResult<unknown>> =>
            wrapMutation((p) => createReviewMutation(p).unwrap(), payload),
        [createReviewMutation]
    );

    // similar refactors for getReviewByProductModelIdAsync, updateReviewAsync, etc.

    const isLoading = useMemo(
        () => createReviewMutationState.isLoading /* || other states... */,
        [createReviewMutationState.isLoading]
    );

    return {
        isLoading,
        createReviewMutationState,
        createReviewAsync,
        // other helpers...
    };
};

export default useReviewService;
```

---

### 4. Type Safety: Remove `any` Payloads

Example from `useBasketService` where `storeEventItemAsync` uses `any`.

#### 4.1 Define a proper domain type

```ts
// apps/client/src/domain/types/basket.type.ts

export interface IStoreEventItemPayload {
    skuId: string;
    quantity: number;
    // add other required properties here
}
```

#### 4.2 Use the type in the hook

```ts
// apps/client/src/components/hooks/api/use-basket-service.ts
import type {
    IStoreEventItemPayload,
    // ... existing imports
} from "~/domain/types/basket.type";

const storeEventItemAsync = useCallback(
    async (payload: IStoreEventItemPayload) => {
        try {
            const result = await storeEventItemMutation(payload).unwrap();
            return {
                isSuccess: true,
                isError: false,
                data: result,
                error: null,
            };
        } catch (error) {
            return { isSuccess: false, isError: true, data: null, error };
        }
    },
    [storeEventItemMutation]
);
```

---

### 5. React‑Hook‑Form & UI Cleanup for `ReviewModal`

Key ideas:

-   Use React Hook Form as the single source of truth for `rating` and `content`.
-   Remove direct mutation of `form.formState.errors`.
-   Improve accessibility of the rating control.

#### 5.1 Refactored component sketch

```tsx
// apps/client/src/app/(pages)/(privates)/account/orders/_components/review-model.tsx
"use client";

import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { motion, AnimatePresence } from "framer-motion";
import { Star, AlertCircle } from "lucide-react";
import NextImage from "next/image";
import { Button } from "@components/ui/button";
import { Textarea } from "@components/ui/textarea";
import { cn } from "~/infrastructure/lib/utils";
import {
    ReviewFormType,
    ReviewResolver,
} from "~/domain/schemas/catalog.schema";
import type { IReviewPayload } from "~/domain/interfaces/catalog.interface";
import useReviewService from "@components/hooks/api/use-review-service";
import isDifferentValue from "~/infrastructure/utils/is-different-value";

type ReviewModalProps = {
    item: {
        product_id: string;
        model_id: string;
        order_id: string;
        order_item_id: string;
        sku_id?: string | null;
        name: string;
        image: string;
        isReviewed: boolean;
        options?: string;
    };
    reviewedData: {
        reviewId?: string;
        rating?: number;
        content?: string;
    };
    onClose: () => void;
    onSubmit: () => void;
};

export function ReviewModal({
    item,
    reviewedData,
    onClose,
    onSubmit,
}: ReviewModalProps) {
    const {
        createReviewAsync,
        updateReviewAsync,
        deleteReviewAsync,
        isLoading,
    } = useReviewService();

    const {
        register,
        setValue,
        watch,
        reset,
        handleSubmit,
        formState: { errors },
        clearErrors,
    } = useForm<ReviewFormType>({
        resolver: ReviewResolver,
        defaultValues: {
            sku_id: item.sku_id || "",
            order_id: item.order_id,
            order_item_id: item.order_item_id,
            rating: reviewedData.rating || 0,
            content: reviewedData.content || "",
        },
    });

    const rating = watch("rating");
    const content = watch("content");

    useEffect(() => {
        reset({
            sku_id: item.sku_id || "",
            order_id: item.order_id,
            order_item_id: item.order_item_id,
            rating: reviewedData.rating || 0,
            content: reviewedData.content || "",
        });
    }, [item, reviewedData, reset]);

    const onCreateSubmit = async (data: ReviewFormType) => {
        if (!item.sku_id) return;

        const payload: IReviewPayload = {
            sku_id: item.sku_id,
            order_id: item.order_id,
            order_item_id: item.order_item_id,
            content: data.content.trim(),
            rating: data.rating,
        };

        const result = await createReviewAsync(payload);
        if (result.isSuccess) {
            onSubmit();
            onClose();
        }
    };

    const onUpdateClick = async () => {
        if (!reviewedData.reviewId) return;

        const trimmedContent = content.trim();

        const result = await updateReviewAsync(reviewedData.reviewId, {
            rating,
            content: trimmedContent,
        });

        if (result.isSuccess) {
            onSubmit();
            onClose();
        }
    };

    const onDeleteClick = async () => {
        if (!reviewedData.reviewId) return;

        const result = await deleteReviewAsync(reviewedData.reviewId);
        if (result.isSuccess) {
            onSubmit();
            onClose();
        }
    };

    const hasChanged = isDifferentValue(
        { rating, content: content.trim() },
        reviewedData
    );

    return (
        <div className="space-y-6">
            <div className="flex items-start space-x-4">
                <div className="flex-shrink-0 w-20 h-20 bg-gray-100 rounded-md overflow-hidden">
                    <NextImage
                        src={item.image || "/placeholder.svg"}
                        alt={item.name}
                        width={80}
                        height={80}
                        className="w-full h-full object-center object-cover"
                    />
                </div>
                <div>
                    <h3 className="text-lg font-medium text-gray-900">
                        {item.name}
                    </h3>
                    {item.options && (
                        <p className="mt-1 text-sm text-gray-500">
                            {item.options}
                        </p>
                    )}
                </div>
            </div>

            {/* Rating */}
            <div className="space-y-4">
                <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                        Your Rating
                    </label>
                    <div
                        className="flex items-center"
                        role="radiogroup"
                        aria-label="Rating"
                    >
                        {[1, 2, 3, 4, 5].map((star) => (
                            <motion.button
                                key={star}
                                type="button"
                                whileHover={{ scale: 1.1 }}
                                whileTap={{ scale: 0.9 }}
                                className="p-1 focus:outline-none"
                                onClick={() => {
                                    setValue("rating", star, {
                                        shouldValidate: true,
                                    });
                                    clearErrors("rating");
                                }}
                                role="radio"
                                aria-checked={rating === star}
                                aria-label={`${star} star${
                                    star > 1 ? "s" : ""
                                }`}
                            >
                                <Star
                                    className={cn("h-8 w-8", {
                                        "text-yellow-400 fill-yellow-400":
                                            star <= rating,
                                        "text-gray-300": star > rating,
                                    })}
                                />
                            </motion.button>
                        ))}
                    </div>
                    <AnimatePresence>
                        {errors.rating && (
                            <motion.p
                                initial={{ opacity: 0, height: 0 }}
                                animate={{ opacity: 1, height: "auto" }}
                                exit={{ opacity: 0, height: 0 }}
                                className="text-sm text-red-600 flex items-center mt-1"
                            >
                                <AlertCircle className="h-3 w-3 mr-1" />
                                {errors.rating.message}
                            </motion.p>
                        )}
                    </AnimatePresence>
                </div>

                {/* Comment */}
                <div>
                    <label
                        htmlFor="comment"
                        className="block text-sm font-medium text-gray-700 mb-2"
                    >
                        Your Review
                    </label>
                    <Textarea
                        id="comment"
                        {...register("content")}
                        value={content}
                        onChange={(e) => {
                            setValue("content", e.target.value, {
                                shouldValidate: true,
                            });
                            if (e.target.value.length > 0) {
                                clearErrors("content");
                            }
                        }}
                        placeholder="Share your experience with this product..."
                        rows={4}
                        className={cn(
                            "resize-none transition-all duration-200",
                            errors.content &&
                                "border-red-300 focus:ring-red-500"
                        )}
                    />
                    <AnimatePresence>
                        {errors.content && (
                            <motion.p
                                initial={{ opacity: 0, height: 0 }}
                                animate={{ opacity: 1, height: "auto" }}
                                exit={{ opacity: 0, height: 0 }}
                                className="text-sm text-red-600 flex items-center mt-1"
                            >
                                <AlertCircle className="h-3 w-3 mr-1" />
                                {errors.content.message}
                            </motion.p>
                        )}
                    </AnimatePresence>
                </div>
            </div>

            {/* Footer actions */}
            <div
                className={cn(
                    "flex justify-end space-x-3 pt-4 border-t border-gray-200",
                    { "justify-between": item.isReviewed }
                )}
            >
                {item.isReviewed && (
                    <motion.div
                        whileHover={{ scale: 1.05 }}
                        whileTap={{ scale: 0.95 }}
                    >
                        <Button
                            type="button"
                            variant="destructive"
                            onClick={onDeleteClick}
                            disabled={isLoading}
                        >
                            Delete
                        </Button>
                    </motion.div>
                )}

                <div className="flex gap-2">
                    <motion.div
                        whileHover={{ scale: 1.05 }}
                        whileTap={{ scale: 0.95 }}
                    >
                        <Button
                            type="button"
                            variant="outline"
                            onClick={onClose}
                            disabled={isLoading}
                        >
                            Cancel
                        </Button>
                    </motion.div>

                    {!item.isReviewed && (
                        <motion.div
                            whileHover={{ scale: 1.05 }}
                            whileTap={{ scale: 0.95 }}
                        >
                            <Button
                                type="button"
                                onClick={handleSubmit(onCreateSubmit)}
                                disabled={isLoading}
                            >
                                {isLoading ? "Submitting..." : "Submit Review"}
                            </Button>
                        </motion.div>
                    )}

                    {item.isReviewed && (
                        <motion.div
                            whileHover={{ scale: 1.05 }}
                            whileTap={{ scale: 0.95 }}
                        >
                            <Button
                                type="button"
                                onClick={onUpdateClick}
                                disabled={isLoading || !hasChanged}
                            >
                                {isLoading ? "Updating..." : "Update Review"}
                            </Button>
                        </motion.div>
                    )}
                </div>
            </div>
        </div>
    );
}
```

---

### 6. DX: Pre‑commit Hooks (Optional)

To enforce formatting and linting on changed files only:

```jsonc
// package.json (apps/client)
{
    "scripts": {
        "lint": "next lint",
        "lint:fix": "eslint --fix src/**/*.ts src/**/*.tsx",
        "prettier": "prettier --check \"src/**/*.{ts,tsx,css,scss}\"",
        "prettier:fix": "prettier --write \"src/**/*.{ts,tsx,css,scss}\"",
        "prepare": "husky install"
    },
    "devDependencies": {
        "husky": "^9.0.0",
        "lint-staged": "^15.0.0"
    },
    "lint-staged": {
        "src/**/*.{ts,tsx}": ["eslint --fix", "prettier --write"]
    }
}
```

Then initialize Husky:

```bash
cd apps/client
npx husky init
```

And in `.husky/pre-commit`:

```bash
npx lint-staged
```

This keeps code quality high without slowing down full builds.
