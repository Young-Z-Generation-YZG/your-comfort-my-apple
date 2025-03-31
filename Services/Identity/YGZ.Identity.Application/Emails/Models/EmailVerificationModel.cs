
namespace YGZ.Identity.Application.Emails.Models;

public class EmailVerificationModel
{
    required public string FullName { get; set; }
    required public string VerificationLink { get; set; }
    required public string VerifyOtp { get; set; }
}
