use anchor_lang::prelude::*;

declare_id!("HRbJQmmbR6i16CZppbvspAq6bN9NQSgU1ZaZSYb7Dpwt");

pub mod constants;
pub mod instructions;
pub mod states;

use instructions::*;

#[program]
pub mod payment {
    use super::*;

    pub fn create_order(
        ctx: Context<CreateOrder>,
        order_id: u64,
        amount: u64,
        currency: String,
    ) -> Result<()> {
        let clock = Clock::get()?;
        let order = &mut ctx.accounts.order;

        // Set order fields
        order.id = order_id;
        order.owner = ctx.accounts.user.key();
        order.amount = amount;
        order.currency = currency;
        order.created_at = clock.unix_timestamp as u64;
        order.updated_at = clock.unix_timestamp as u64;

        msg!("Order created with ID: {}", order_id);
        Ok(())
    }
}
