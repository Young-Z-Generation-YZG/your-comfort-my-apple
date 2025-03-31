

namespace YGZ.Identity.Application.Abstractions.Utils;

public interface IOtpGenerator
{
    string GenerateOtp(int digitLength);
}
