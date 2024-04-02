using System;
using System.Collections.Generic;

namespace NightClubTestCase.Models
{
    public partial class Member
    {
        public int MemberId { get; set; }
        public int IdentityCardId { get; set; }
        public string? MailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BlacklistEndDate { get; set; }

        public virtual IdentityCard IdentityCardNavigation { get; set; } = null!;
    }
}
