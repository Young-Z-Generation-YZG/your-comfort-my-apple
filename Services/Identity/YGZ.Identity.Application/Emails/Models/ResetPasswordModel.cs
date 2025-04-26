

namespace YGZ.Identity.Application.Emails.Models;

public class ResetPasswordModel
{
    required public string FullName { get; set; }
    required public string ResetPasswordLink { get; set; }
}
