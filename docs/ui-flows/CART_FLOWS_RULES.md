# Cart/Basket Management Flows - Rules & Architecture

## Overview

This document defines the two main flows for adding items to cart and managing cart state in the application. The implementation uses Redux for local state and API calls for server synchronization when authenticated.

---

## Service Integration Rule (@Services/Basket)

-   All cart/basket server interactions MUST go through the Basket service layer: `@Services/Basket`.
-   Do NOT call fetch/axios or raw endpoints directly from components/hooks. Use the service's exported hooks or RTK Query endpoints only.
-   Primary endpoints used (from `apps/client/src/infrastructure/services/basket.service.ts`):
    -   `storeBasket` (POST) – create/update entire basket (used on Cart page)
    -   `storeBasketItem` (POST) – add/update a single product-model item (used on Product page when authenticated)
    -   `syncBasket` (POST) – synchronize client cart with server (full payload)
    -   `getBasket` (GET) – fetch current basket
    -   `deleteBasket` (DELETE) – clear basket
    -   `proceedToCheckout` (POST)
    -   `checkoutBasket` / `checkoutBasketWithBlockchain` (POST)
    -   `storeEventItem` (POST) – optional event tracking
-   Error/401 handling is centralized in the service (`baseQueryHandler`): on 401 it dispatches logout and redirects to `/sign-in`.
-   UI code should consume these via the project's hook abstraction (e.g., `useBasketService`) which internally relies on `@Services/Basket`.
-   Types MUST come from `~/domain/types/basket.type` to keep payloads consistent across Redux and service.

### Naming Convention for API Request Payloads

Rule: All request body payload types MUST use the `Interface` naming convention with the `I` prefix and `Payload` suffix (e.g., `IStoreBasketPayload`, `IStoreBasketItemPayload`).

-   Request Payloads: Use `I` prefix + `Payload` suffix – e.g., `IStoreBasketPayload`, `IStoreBasketItemPayload`, `IStoreEventItemPayload`
-   Request Query Params: Use `I` prefix + `QueryParams` suffix – e.g., `IGetBasketQueryParams`
-   Response/State/Domain Types: Use `T` prefix – e.g., `TReduxCartState`, `TCartItem`, `TCheckoutBasket`
-   Rationale: Interfaces define structural contracts for API communication (requests); types model internal state and responses.

Examples:

```ts
// ✅ Request Payloads
export interface IStoreBasketPayload {
    cart_items: IStoreBasketItemPayload[];
}

export interface IStoreBasketItemPayload {
    is_selected: boolean;
    sku_id: string;
    quantity: number;
}

// ✅ Response/State
export type TReduxCartState = {
    user_email: string;
    cart_items: TCartItem[];
    total_amount: number;
};
```

Important payload update:

-   StoreBasket/SyncBasket payload items now ONLY include: `is_selected`, `sku_id`, `quantity`.
-   Model/Color/Storage/ModelId are NOT sent in requests anymore (server derives them by SKU).

---

## Flow 1: Add Single Item to Cart (Shop Product Page)

Location: `@apps/client/src/app/(pages)/shop/iphone/[slug]/page.tsx`

Status: ✅ IMPLEMENTED

Rules:

1. Item addition logic

-   User clicks "Add to Bag" on product detail page.
-   Always updates Redux immediately via `useCartSync().storeBasketSync([cartItem])` for responsive UI.
-   If user is authenticated, also call `storeBasketItem` with `{ sku_id, quantity, is_selected }` to persist the single item.

2. Quantity management

-   If same SKU + specs exists in Redux → increase quantity.
-   Otherwise → add new item with quantity = 1.

3. API usage on Product page

-   Do NOT auto-sync the whole basket here.
-   Do NOT call any API if user is not logged in.
-   When authenticated, call `storeBasketItem` (single-item persist) only.

4. Item identification (Composite Key)

-   Composite key: `sku_id + model.normalized_name + color.normalized_name + storage.normalized_name` to uniquely identify product variants in Redux.

---

## Flow 2: Cart Page Rendering & Management

Location: `@apps/client/src/app/(pages)/(privates)/cart/page.tsx`

### Flow 2.1: Unauthenticated User (Not Logged In)

Status: ✅ IMPLEMENTED

Rules:

-   Data source: Redux cart slice (`state.cart`).
-   Rendering: Display cart items directly from Redux; no API calls to fetch basket.
-   Operations: select (hidden UI for unauthenticated), update quantity, remove item, clear all → Redux only.
-   Persistence: Browser memory (Redux) only.

### Flow 2.2: Authenticated User (Logged In)

Status: ✅ IMPLEMENTED

Rules:

1. Automatic synchronization

-   The cart page opts into automatic sync using `useCartSync({ autoSync: true })`.
-   When authenticated and cart has items, it posts `storeBasket` with minimal payload.

2. Data fetching

-   On mount (or when `_couponCode` changes), call `getBasket`.
-   Server becomes the source of truth on the cart page.

3. Promo code rules

-   Coupon applies only when there are selected items.
-   If `_couponCode` is present but no items are selected, the page automatically removes the coupon from the URL (does NOT auto-select items).
-   If the server returns a coupon error like "No items selected in the basket", the page also removes the coupon (fallback cleanup).

4. Cart operations

-   Select item: Updates Redux immediately (checkbox only visible when authenticated). `useCartFormV2` will sync via `storeBasket`.
-   Update quantity: Updates Redux + calls `storeBasket` with minimal item payloads.
-   Remove item: Updates Redux + calls `storeBasket` with remaining items, or `deleteBasket` to clear all.
-   Clear all: `deleteBasket` on server; then refresh via `getBasket`. Redux cleared accordingly.

---

## Redux Cart Slice Actions

File: `apps/client/src/infrastructure/redux/features/cart.slice.ts`

Actions:

-   `SyncCart`: Replace entire cart state (used for server sync result).
-   `AddNewCartItem`: Add single item to cart (no forced selection; `is_selected` is preserved as provided).
-   `AddCartItems`: Replace cart items with new array (no forced selection defaults).
-   `UpdateQuantity`: Update quantity by composite key/sku_id.
-   `UpdateSelection`: Update `is_selected` flag of an item.
-   `CleanSelectedItems`: Remove all selected items.
-   `removeCartItem`: Remove item by index.
-   `clearCart`: Clear all items and reset total.

---

## useCartSync Hook

File: `apps/client/src/components/hooks/use-cart-sync.ts`

Functionality:

-   `useCartSync({ autoSync })` – when `autoSync` is true (used on Cart page), automatically posts `storeBasket` with minimal payload on relevant changes.
-   `storeBasketSync(cartItems)` – merges incoming items with Redux, handling duplicate detection and quantity merging. Does not perform network calls.

---

## UI & Calculation Rules

-   Price display:
    -   Original total: `unit_price × quantity`.
    -   Discount display is shown ONLY when `discount_amount > 0`.
    -   Discounted total: `max(0, original_total − discount_amount)`; render red discounted + strikethrough original.
-   Stock limit:
    -   `quantity_remain` limits max quantity in the UI; users cannot increase beyond available stock.

---

## Domain/Contract Updates

-   Server-side `ShoppingCartItem` now includes `QuantityRemain` (populated from `skuGrpc.AvailableInStock`).
-   API contract `CartItemResponse` (GetBasketResponse) includes `quantity_remain`.
-   Client `TCartItem` includes required `quantity_remain: number`.

---

## Types (by file) in `~/domain/types`

-   `basket.type.ts`
    -   Response/Domain: `TPromotion`, `TCartItem`, `TCart`, `TCheckoutBasketItem`, `TCheckoutBasket`, `TUSerInfo`.
    -   Request Payloads: `IStoreBasketPayload`, `IStoreBasketItemPayload`, `IStoreEventItemPayload`.
    -   Request Query Params: `IGetBasketQueryParams`.
-   `catalog.type.ts`
    -   Response/Domain: `TSku`, `TModel`, `TColor`, `TStorage`, `TImage`, `TRatingStar`, `TAverageRating`, `TSkuPrice`, `TIphoneModelDetails`, `TProductModel`, `TPopularProduct`, `TNewestProduct`, `TSuggestionProduct`, `TReviewItem`, `TReviewInOrderItem`, `TTenant`, `TBranch`, `TCategory`, `TEventItem`, `TEvent`.
-   `identity.type.ts`
    -   Response/Domain: `TUser`, `TProfile`.
-   `ordering.type.ts`
    -   Response/Domain: `TOrderItem`, `TOrder`, `TNotification`.

Global naming rule:

-   All request payload interfaces MUST use `I` prefix + `Payload` suffix.
-   All request query parameter interfaces MUST use `I` prefix + `QueryParams` suffix.
-   All response/state/domain shapes MUST use `T` prefix (Type).

---

## Key Differences Summary

| Aspect           | Unauthenticated      | Authenticated                                       |
| ---------------- | -------------------- | --------------------------------------------------- |
| Data Source      | Redux (local)        | Server (API) + Redux fallback                       |
| Initial Load     | Display Redux state  | Sync Redux → server (storeBasket), then `getBasket` |
| Item Selection   | No checkbox UI       | Checkbox UI enabled                                 |
| Promo Code       | Disabled             | Requires selected items; auto-remove if none        |
| Operations       | Redux mutations only | API calls + Redux updates                           |
| Persistence      | Browser memory       | Server database                                     |
| Auto Sync        | None                 | `useCartSync({ autoSync: true })` on cart page      |
| Add From Product | Redux only           | Redux + `storeBasketItem`                           |

---

## Implementation Status Summary

| Feature                               | Status         | Location                               |
| ------------------------------------- | -------------- | -------------------------------------- |
| Add item to cart (Flow 1)             | ✅ Implemented | `shop/iphone/[slug]/page.tsx`          |
| Unauthenticated cart display (2.1)    | ✅ Implemented | `cart/page.tsx`                        |
| Unauthenticated cart operations (2.1) | ✅ Implemented | `cart/page.tsx` + Redux                |
| Authenticated cart display (2.2)      | ✅ Implemented | `cart/page.tsx`                        |
| Authenticated cart sync (2.2)         | ✅ Implemented | `useCartSync({ autoSync: true })`      |
| Coupon auto-remove when none selected | ✅ Implemented | `cart/page.tsx`                        |
| Authenticated cart operations (2.2)   | ✅ Implemented | `useCartFormV2()` + `@Services/Basket` |






