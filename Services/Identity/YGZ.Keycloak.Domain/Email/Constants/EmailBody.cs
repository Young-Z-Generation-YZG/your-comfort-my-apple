
namespace YGZ.Keycloak.Domain.Email.Constants;

public static class EmailBody
{
    public static string Verification(string fullName, string url)
    {
        return @$"
                <p>Dear {fullName},</p>
                <p>Congratulations and welcome to Template! This email serves as confirmation that your registration has been successfully completed.  We're thrilled to have you as a part of our community and look forward to providing you with a rewarding experience.</p>
                <a href={url}>Complete the Registration process by clicking the link</a>
                <br/>
                <p>Best Regards,</p>
                <p>Your Template Team</p>";
    }
}
