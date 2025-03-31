using System.Security.Cryptography;
using YGZ.Identity.Application.Abstractions.Utils;

namespace YGZ.Identity.Infrastructure.Utils;

public class OtpGenerator : IOtpGenerator
{
    public string GenerateOtp(int digitLength)
    {
        if (digitLength <= 0)
        {
            throw new ArgumentException("Digit length must be greater than zero.", nameof(digitLength));
        }

        // Create a char array to store the OTP digits
        char[] otp = new char[digitLength];

        // Use cryptographic random number generator for security
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] randomBytes = new byte[digitLength];
            rng.GetBytes(randomBytes);

            // Convert random bytes to digits (0-9)
            for (int i = 0; i < digitLength; i++)
            {
                // Map the random byte to a digit (0-9) using modulo
                otp[i] = (char)('0' + (randomBytes[i] % 10));
            }
        }

        return new string(otp);
    }
}