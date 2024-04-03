using NightClubTestCase.Models;

namespace NightClubTestCase.Extensions
{
    public class ControllerExtension
    {
        public static bool MemberCompliance(Member member, ILogger<object> logger)
        {
            if (string.IsNullOrWhiteSpace(member.MailAddress) && string.IsNullOrWhiteSpace(member.PhoneNumber))
            {
                logger.LogWarning("Mail address and phone number cannot be both empty.");
                return false;
            }

            if (member.IdentityCardNavigation == null ||
                string.IsNullOrWhiteSpace(member.IdentityCardNavigation.FirstName) ||
                string.IsNullOrWhiteSpace(member.IdentityCardNavigation.LastName) ||
                member.IdentityCardNavigation?.BirthDate == null ||
                member.IdentityCardNavigation?.ValidityDate == null ||
                member.IdentityCardNavigation?.ExpirationDate == null)
            {
                logger.LogWarning("Please fill in identity card information.");
                return false;
            }

            if (member.IdentityCardNavigation.BirthDate.AddYears(18) > DateTime.UtcNow)
            {
                logger.LogWarning("Member must be 18 years or older to be registered.");
                return false;
            }

            return true;
        }
    }
}
