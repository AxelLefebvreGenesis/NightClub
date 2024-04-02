using System;
using System.Collections.Generic;

namespace NightClubTestCase.Models
{
    public partial class IdentityCard
    {
        public int IdentityCardId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string? NationalRegisterNumber { get; set; }
        public DateTime ValidityDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool HasExpired { get; set; }
        public Member? Member { get; set; }
    }
}
