using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VotingSystem.API.Features.Voting.DTOs;
using VotingSystem.API.Features.Voting.Services;
using VotingSystem.Common.ResponseModel;

namespace VotingSystem.API.Features.Voting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VotingController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [Authorize]
        [HttpPost("vote")]
        public async Task<IActionResult> Vote([FromBody] VoteRequestDTO request)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated."));
            }

            var response = await _voteService.Vote(userId, request);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("remove-vote/{pollId}")]
        public async Task<IActionResult> RemoveVote(int pollId)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated."));
            }

            var response = await _voteService.RemoveVote(userId, pollId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

    }
}
