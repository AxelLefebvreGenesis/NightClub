using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NightClubTestCase.Extensions;
using NightClubTestCase.Models;
using NightClubTestCase.Services;

namespace NightClubTestCase.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly ILogger<MemberController> _logger;
        private readonly MemberService _memberService;

        public MemberController(ILogger<MemberController> logger, MemberService memberService)
        {
            _logger = logger;
            _memberService = memberService;
        }

        [HttpPost("create")]
        public IActionResult CreateMember([FromBody] Member member)
        {
            if (!ControllerExtension.MemberCompliance(member, _logger))
                return BadRequest();

            _memberService.CreateMember(member);
            return Ok();
        }

        [HttpGet("list")]
        public IActionResult GetMembersList()
        {
            return Ok(_memberService.GetMembersList());
        }

        [HttpGet("{memberId}")]
        public IActionResult GetMemberDetails(int memberId)
        {
            var member = _memberService.GetMemberDetails(memberId);
            return member != null ? Ok(member) : NotFound();
        }

        [HttpPut("update")]
        public IActionResult UpdateMember([FromBody] Member member)
        {
            if (!ControllerExtension.MemberCompliance(member, _logger))
                return BadRequest();

            var updated = _memberService.UpdateMember(member);
            return updated ? Ok() : NotFound();
        }

        [HttpPut("{memberId}/blacklist")]
        public IActionResult BlacklistMember(int memberId, int days)
        {
            var blacklisting = _memberService.BlacklistMember(memberId, days);
            return blacklisting ? Ok() : NotFound();
        }

        [HttpGet("{memberId}/isblacklisted")]
        public IActionResult IsMemberBlacklisted(int memberId)
        {
            bool? isBlacklisted = _memberService.IsMemberBlacklisted(memberId);
            return isBlacklisted != null ? Ok(isBlacklisted) : NotFound();
        }
    }
}
