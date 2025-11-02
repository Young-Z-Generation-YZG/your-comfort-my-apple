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
        order_id: String,
        amount: u64,
        currency: String,
    ) -> Result<()> {
        let clock = Clock::get()?;
        let order = &mut ctx.accounts.order;

        // Transfer payment from user to receiver
        // Note: Account rent is automatically paid by Anchor's init constraint
        anchor_lang::solana_program::program::invoke(
            &anchor_lang::solana_program::system_instruction::transfer(
                &ctx.accounts.user.key(),
                &ctx.accounts.receiver.key(),
                amount,
            ),
            &[
                ctx.accounts.user.to_account_info(),
                ctx.accounts.receiver.to_account_info(),
                ctx.accounts.system_program.to_account_info(),
            ],
        )?;

        msg!(
            "Order created with ID: {}, amount: {} lamports, currency: {}",
            order_id,
            amount,
            currency
        );
        msg!("Payment of {} lamports transferred to receiver", amount);

        // Set order fields
        order.id = order_id;
        order.owner = ctx.accounts.user.key();
        order.amount = amount;
        order.currency = currency;
        order.created_at = clock.unix_timestamp as u64;
        order.updated_at = clock.unix_timestamp as u64;
        Ok(())
    }
}
