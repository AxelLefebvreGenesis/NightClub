using Microsoft.EntityFrameworkCore;
using NightClubTestCase.DBContext;
using NightClubTestCase.Models;

namespace NightClubTestCase.Services
{
    public class MemberService
    {
        private readonly NightClubContext _context;

        public MemberService(NightClubContext context)
        {
            _context = context;
        }

        public bool CreateMember(Member member)
        {
            member.IdentityCardNavigation.HasExpired = member.IdentityCardNavigation.ExpirationDate < DateTime.UtcNow;

            _context.Members.Add(member);
            _context.MemberCards.Add(new MemberCard { IsLost = false });

            _context.SaveChanges();

            var memberCardId = _context.MemberCards.OrderByDescending(mc => mc.MemberCardId).First().MemberCardId;

            Record record = new()
            { 
                MemberId = member.MemberId,
                MemberCardId = memberCardId
            };

            _context.Records.Add(record);

            _context.SaveChanges();
            return true;
        }

        public List<Member> GetMembersList()
        {
            return _context.Members.ToList();
        }

        public Member? GetMemberDetails(int memberId)
        {
            return _context.Members
                .Include(m => m.IdentityCardNavigation)
                .FirstOrDefault(m => m.MemberId == memberId);
        }

        public bool UpdateMember(Member member)
        {
            var existingMember = _context.Members
                .Include(m => m.IdentityCardNavigation)
                .FirstOrDefault(m => m.MemberId == member.MemberId);

            if (existingMember != null)
            {
                existingMember.PhoneNumber = member.PhoneNumber;
                existingMember.MailAddress = member.MailAddress;

                existingMember.IdentityCardNavigation.FirstName = member.IdentityCardNavigation.FirstName;
                existingMember.IdentityCardNavigation.LastName = member.IdentityCardNavigation.LastName;
                existingMember.IdentityCardNavigation.BirthDate = member.IdentityCardNavigation.BirthDate;
                existingMember.IdentityCardNavigation.NationalRegisterNumber = member.IdentityCardNavigation.NationalRegisterNumber;
                existingMember.IdentityCardNavigation.ValidityDate = member.IdentityCardNavigation.ValidityDate;
                existingMember.IdentityCardNavigation.ExpirationDate = member.IdentityCardNavigation.ExpirationDate;

                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool BlacklistMember(int memberId, int days)
        {
            var member = _context.Members.Find(memberId);

            if (member != null)
            {
                member.BlacklistEndDate = DateTime.UtcNow.AddDays(days);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool? IsMemberBlacklisted(int memberId)
        {
            var member = _context.Members.Find(memberId);
            if (member == null)
                return null;
            
            return member.BlacklistEndDate.HasValue == true && member.BlacklistEndDate.Value > DateTime.UtcNow;
        }
    }
}
