using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.API.Features.Polls.DTOs;
using VotingSystem.API.Features.Polls.Services;
using VotingSystem.Common.Extensions;
using VotingSystem.Common.ResponseModel;

namespace VotingSystem.API.Features.Polls.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly IPollsService _pollService;

        public PollsController(IPollsService pollService)
        {
            _pollService = pollService;
        }

        [Authorize(Roles = "Superadmin,Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePoll([FromForm] CreatePollDTO request)
        {
            var userId = User.FindFirst("UserId")?.Value.Decrypt();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated."));
            }

            var response = await _pollService.CreatePoll(userId, request);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllPolls()
        {
            var response = await _pollService.GetAllPolls();
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPost("get-by-id/{pollId}")]
        public async Task<IActionResult> GetAllPolls(int pollId)
        {
            var response = await _pollService.GetPollById(pollId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPatch("enable/{pollId}")]
        public async Task<IActionResult> EnablePoll(int pollId)
        {
            var userId = User.FindFirst("UserId")?.Value.Decrypt();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated."));
            }

            var response = await _pollService.EnablePoll(userId, pollId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPatch("disable/{pollId}")]
        public async Task<IActionResult> DisablePoll(int pollId)
        {
            var userId = User.FindFirst("UserId")?.Value.Decrypt();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated."));
            }

            var response = await _pollService.DisablePoll(userId, pollId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpDelete("delete/{pollId}")]
        public async Task<IActionResult> DeletePoll(int pollId)
        {
            var userId = User.FindFirst("UserId")?.Value.Decrypt();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated."));
            }

            var response = await _pollService.DeletePoll(userId, pollId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePoll([FromBody] UpdatePollDTO dto)
        {
            var userId = User.FindFirst("UserId")?.Value.Decrypt();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated."));
            }

            var response = await _pollService.UpdatePoll(userId, dto);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
