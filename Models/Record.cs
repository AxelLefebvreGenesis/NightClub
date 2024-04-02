namespace NightClubTestCase.Models
{
    public partial class Record
    {
        public int MemberId { get; set; }
        public int MemberCardId { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual MemberCard MemberCardNavigation { get; set; } = null!;
    }
}
