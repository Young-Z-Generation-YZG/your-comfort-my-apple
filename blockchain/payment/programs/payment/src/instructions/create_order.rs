use anchor_lang::prelude::*;

use crate::constants::*;
use crate::states::*;

#[derive(Accounts)]
#[instruction(order_id: String, amount: u64, currency: String)]
pub struct CreateOrder<'info> {
    #[account(mut)]
    pub user: Signer<'info>,

    /// CHECK: Receiver account (PublicKey) to receive payments
    #[account(mut)]
    pub receiver: UncheckedAccount<'info>,

    #[account(
        init,
        payer = user,
        space = ORDER_SPACE,
        seeds = [
            ORDER_SEED, 
            user.key().as_ref(), 
            &order_id.as_bytes()[..32], // First 32 bytes of UUID
            &order_id.as_bytes()[32..], // Remaining 4 bytes of UUID (split into 2 seeds)
        ],
        bump
    )]
    pub order: Account<'info, Order>,

    pub system_program: Program<'info, System>,
}

// Excellent — that’s a really good question 👏

// Let’s break down this line carefully:

// ```rust
// order_id.to_le_bytes().as_ref()
// ```

// It looks small, but it does several important things — especially when used as a **PDA seed** (Program Derived Address seed) in Solana.

// ---

// ## 🧠 Step-by-step Breakdown

// ### 1. `order_id`

// This is a **Rust integer** (`u64` in this case), passed as an instruction argument:

// ```rust
// #[instruction(order_id: u64, ...)]
// ```

// So, it’s just a number like `42`, `123456`, etc.

// ---

// ### 2. `order_id.to_le_bytes()`

// This converts the integer into **an array of 8 bytes** (because `u64` = 8 bytes):

// * `to_le_bytes()` = “to **Little Endian** bytes”.

//   * Little Endian means **least significant byte first** (used by Solana for consistency).

// Example:

// ```rust
// let order_id: u64 = 42;
// let bytes = order_id.to_le_bytes();
// println!("{:?}", bytes);
// ```

// Output:

// ```
// [42, 0, 0, 0, 0, 0, 0, 0]
// ```

// Now `bytes` is a `[u8; 8]` array.

// ---

// ### 3. `.as_ref()`

// The `as_ref()` method converts the fixed-size array `[u8; 8]` into a **slice reference** `&[u8]`.

// This is necessary because Anchor’s `seeds = [...]` expects **byte slices** (`&[u8]`), not fixed-size arrays.

// So this line turns your `order_id` into a slice of bytes usable as part of a PDA seed.

// ---

// ## 🧩 Putting It Together

// In context:

// ```rust
// seeds = [ORDER_SEED, user.key().as_ref(), order_id.to_le_bytes().as_ref()]
// ```

// Each item in the `seeds` array is a byte slice (`&[u8]`):

// | Seed Component                    | Type    | Purpose                                           |
// | --------------------------------- | ------- | ------------------------------------------------- |
// | `ORDER_SEED`                      | `&[u8]` | Static seed constant (e.g., `"order".as_bytes()`) |
// | `user.key().as_ref()`             | `&[u8]` | The user’s public key as bytes                    |
// | `order_id.to_le_bytes().as_ref()` | `&[u8]` | The order ID as 8 bytes                           |

// All these are combined to derive a **unique PDA (Program Derived Address)**.

// ---

// ## 🧮 Example in Action

// Let’s say:

// ```rust
// ORDER_SEED = b"order";
// user.key() = Pubkey::from_str("User1111...").unwrap();
// order_id = 42;
// ```

// Then the full PDA seed combination would be:

// ```
// ["order", user_pubkey_bytes, [42, 0, 0, 0, 0, 0, 0, 0]]
// ```

// Anchor + Solana’s runtime will then call:

// ```rust
// Pubkey::find_program_address(seeds, program_id)
// ```

// to generate a **unique deterministic account address**.

// ---

// ## ✅ Summary

// | Code            | Meaning                                        | Type      |
// | --------------- | ---------------------------------------------- | --------- |
// | `to_le_bytes()` | Converts number → 8-byte array (little endian) | `[u8; 8]` |
// | `as_ref()`      | Turns array → slice reference                  | `&[u8]`   |
// | Usage           | Makes `u64` usable as PDA seed                 | `&[u8]`   |

// So `order_id.to_le_bytes().as_ref()` just means:

// > “Use the 8-byte little-endian encoding of `order_id` as one of the PDA seeds.”

// ---

// Would you like me to show a quick example of **deriving the same PDA manually in Rust** (like you would do off-chain or in tests)?
