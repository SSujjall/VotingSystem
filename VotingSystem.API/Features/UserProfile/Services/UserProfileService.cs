using VotingSystem.API.Features.UserProfile.DTOs;
using VotingSystem.Common.ResponseModel;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.API.Features.UserProfile.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserRepository _userRepository;

        public UserProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<UserResDTO>> GetUserDetail(string userId)
        {
            var user = await _userRepository.GetById(userId);
            if (user is null)
            {
                return ApiResponse<UserResDTO>.Failed(null, "No user found");
            }

            #region response model mapping
            var mappedModel = new UserResDTO
            {
                FullName = user.FullName,
                Username = user.UserName ?? "",
                Email = user.Email ?? ""
            };
            #endregion

            return ApiResponse<UserResDTO>.Success(mappedModel, "User detail loaded");
        }

        public async Task<ApiResponse<UserResDTO>> UpdateUser(string userId, UpdateUserDTO dto)
        {
            var user = await _userRepository.GetById(userId);
            if (user is null)
            {
                return ApiResponse<UserResDTO>.Failed(null, "No user found");
            }

            if (!string.IsNullOrEmpty(dto.FullName))
            {
                user.FullName = dto.FullName;
            }
            if (!string.IsNullOrEmpty(dto.Username))
            {
                user.UserName = dto.Username;
            }
            if (!string.IsNullOrEmpty(dto.Email))
            {
                user.Email = dto.Email;
            }

            var result = await _userRepository.Update(user);
            if (result is null)
            {
                return ApiResponse<UserResDTO>.Failed(null, "Failed to update user");
            }

            #region response model mapping
            var mappedModel = new UserResDTO
            {
                FullName = result.FullName,
                Username = result.UserName ?? "",
                Email = result.Email ?? ""
            };
            #endregion

            return ApiResponse<UserResDTO>.Success(mappedModel, "User detail loaded");
        }
    }
}
