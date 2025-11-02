use anchor_lang::prelude::*;

const MAX_CURRENCY_BYTES: usize = 3;
const MAX_ID_BYTES: usize = 64; // Maximum length for order ID string

#[account]
pub struct Order {
    pub id: String,
    pub owner: Pubkey,
    pub amount: u64,
    pub currency: String,
    pub created_at: u64,
    pub updated_at: u64,
}

pub const ORDER_SPACE: usize = 8 + 4 + MAX_ID_BYTES + 32 + 8 + 4 + MAX_CURRENCY_BYTES + 8 + 8;
// 8 bytes for the account discriminator
// 4 bytes for the id String length
// 64 bytes for the id String data
// 32 bytes for the owner
// 8 bytes for the amount
// 4 bytes for the currency String length
// 3 bytes for the currency String data
// 8 bytes for the created_at
// 8 bytes for the updated_at
