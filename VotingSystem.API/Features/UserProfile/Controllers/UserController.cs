using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.API.Features.UserProfile.DTOs;
using VotingSystem.API.Features.UserProfile.Services;
using VotingSystem.Common.Extensions;
using VotingSystem.Common.ResponseModel;

namespace VotingSystem.API.Features.UserProfile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserProfileService _userPService;

        public UserController(IUserProfileService userPService)
        {
            _userPService = userPService;
        }

        [Authorize]
        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirst("UserId")?.Value.Decrypt();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated."));
            }

            var response = await _userPService.GetUserDetail(userId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO requestModel)
        {
            var userId = User.FindFirst("UserId")?.Value.Decrypt();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated."));
            }

            var response = await _userPService.UpdateUser(userId, requestModel);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
