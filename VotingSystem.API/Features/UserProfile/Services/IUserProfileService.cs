using VotingSystem.API.Features.UserProfile.DTOs;
using VotingSystem.Common.ResponseModel;

namespace VotingSystem.API.Features.UserProfile.Services
{
    public interface IUserProfileService
    {
        Task<ApiResponse<UserResDTO>> GetUserDetail(string userId);
        Task<ApiResponse<UserResDTO>> UpdateUser(string userId, UpdateUserDTO dto);
    }
}
