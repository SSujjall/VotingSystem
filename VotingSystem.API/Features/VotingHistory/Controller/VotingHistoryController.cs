using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VotingSystem.API.Features.VotingHistory.Services;
using VotingSystem.Common.Extensions;
using VotingSystem.Common.ResponseModel;

namespace VotingSystem.API.Features.VotingHistory.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingHistoryController : ControllerBase
    {
        private readonly IVotingHistoryService _votingHistoryService;

        public VotingHistoryController(IVotingHistoryService votingHistoryService)
        {
            _votingHistoryService = votingHistoryService;
        }

        [Authorize]
        [HttpGet("get-history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = User.FindFirstValue("UserId")?.Decrypt();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated"));
            }

            var response = await _votingHistoryService.GetVotingHistory(userId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            return Ok(response);
        }
    }
}
