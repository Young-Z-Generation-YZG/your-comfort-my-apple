use anchor_lang::prelude::*;

declare_id!("HRbJQmmbR6i16CZppbvspAq6bN9NQSgU1ZaZSYb7Dpwt");

#[program]
pub mod payment {
    use super::*;

    pub fn initialize(ctx: Context<Initialize>) -> Result<()> {
        msg!("Greetings from: {:?}", ctx.program_id);
        Ok(())
    }
}

#[derive(Accounts)]
pub struct Initialize {}
