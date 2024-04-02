using Microsoft.EntityFrameworkCore;
using NightClubTestCase.DBContext;
using NightClubTestCase.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace NightClubTestCase.Services
{
    public class MemberService
    {
        private readonly NightClubContext _context;

        public MemberService(NightClubContext context)
        {
            _context = context;
        }

        public bool CreateMember(Member member, MemberCard memberCard)
        {
            _context.Members.Add(member);
            _context.MemberCards.Add(memberCard);

            Record record = new()
            { 
                MemberId = member.MemberId,
                MemberCardId = memberCard.MemberCardId
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
            var existingMember = _context.Members.Find(member.MemberId);

            if (existingMember != null)
            {
                existingMember = member;
                existingMember.IdentityCardNavigation = member.IdentityCardNavigation;
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

        public bool IsMemberBlacklisted(int memberId)
        {
            var member = _context.Members.Find(memberId);
            return member?.BlacklistEndDate.HasValue == true && member.BlacklistEndDate.Value > DateTime.UtcNow;
        }
    }
}
