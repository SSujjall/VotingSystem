using VotingSystem.API.Features.Auth.DTOs;
using VotingSystem.Common.ResponseModel;

namespace VotingSystem.API.Features.Auth.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<SignupResponseDTO>> RegisterUser(SignupDTO signupDto);
        Task<ApiResponse<LoginResponseDTO>> LoginUser(LoginDTO loginDto);
        Task<ApiResponse<string>> ConfirmEmailVerification(string token, string email);
    }
}
