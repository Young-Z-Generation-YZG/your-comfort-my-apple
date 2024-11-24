
namespace YGZ.Identity.Application.Emails.Commands;

public sealed record EmailConfirmationCommand(string Email, string FullName, string VerifiedUrl) { }
