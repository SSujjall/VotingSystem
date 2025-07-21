using Microsoft.AspNetCore.Identity;
using VotingSystem.Infrastructure.ExternalServices.JwtService;
using VotingSystem.Infrastructure.Repositories.Interfaces;
using VotingSystem.Domain.Entities;
using System.Net;
using VotingSystem.API.Features.Auth.DTOs;
using VotingSystem.Common.ResponseModel;
using VotingSystem.Domain.Enums;

namespace VotingSystem.API.Features.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IAuthRepository authRepository, UserManager<User> userManager,
                           RoleManager<IdentityRole> roleManager, IJwtService jwtService,
                           ILogger<AuthService> logger)
        {
            _authRepository = authRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<ApiResponse<SignupResponseDTO>> RegisterUser(SignupDTO signupDto, bool isAdmin = false)
        {
            try
            {
                // Check if the username already exists
                if (await _authRepository.UsernameExists(signupDto.Username))
                {
                    var errors = new Dictionary<string, string> { { "Username", "Username already used." } };
                    return ApiResponse<SignupResponseDTO>.Failed(errors, "Register Failed.");
                }
                if (await _authRepository.EmailExists(signupDto.Email))
                {
                    var errors = new Dictionary<string, string> { { "Email", "Email already used." } };
                    return ApiResponse<SignupResponseDTO>.Failed(errors, "Register Failed.");
                }

                #region request model mapping
                var user = new User
                {
                    UserName = signupDto.Username,
                    FullName = signupDto.FullName,
                    Email = signupDto.Email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                #endregion

                // Attempt to create the user
                var createResult = await _authRepository.CreateNewUser(user, signupDto.Password);
                if (!createResult)
                {
                    var errors = new Dictionary<string, string> { { "User", "Error when creating user." } };
                    return ApiResponse<SignupResponseDTO>.Failed(errors, "User Creation Failed.", HttpStatusCode.InternalServerError);
                }

                UserRoles role;
                if (isAdmin is true)
                {
                    role = UserRoles.Admin;
                }
                else
                {
                    role = UserRoles.User;
                }

                // Ensure the user role exists
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    var newRole = new IdentityRole(role.ToString())
                    {
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    };
                    var roleCreateResult = await _roleManager.CreateAsync(newRole);
                    if (!roleCreateResult.Succeeded)
                    {
                        var errors = new Dictionary<string, string> { { "Role", "Failed to create role" } };
                        return ApiResponse<SignupResponseDTO>.Failed(errors, "Register Failed.", HttpStatusCode.InternalServerError);
                    }
                }

                // Add user to the role
                var addToRoleResult = await _userManager.AddToRoleAsync(user, role.ToString());
                if (!addToRoleResult.Succeeded)
                {
                    var errors = new Dictionary<string, string> { { "User", "Failed to assign role to user" } };
                    return ApiResponse<SignupResponseDTO>.Failed(errors, "Register Failed.", HttpStatusCode.InternalServerError);
                }

                #region Generate Email verification token
                var existingUser = await _authRepository.FindByUsername(signupDto.Username);
                if (existingUser == null)
                {
                    var errors = new Dictionary<string, string> { { "User", "Failed to fetch newly created user." } };
                    return ApiResponse<SignupResponseDTO>.Failed(errors, "Failed to fetch user.");
                }
                var emailVerificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(existingUser);
                var response = new SignupResponseDTO
                {
                    EmailConfirmToken = emailVerificationToken
                };
                #endregion

                return ApiResponse<SignupResponseDTO>.Success(response, "User created successfully");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred in RegisterUser for username {Username}", signupDto.Username);
                return ApiResponse<SignupResponseDTO>.Failed(null, "An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<ApiResponse<LoginResponseDTO>> LoginUser(LoginDTO loginDto)
        {
            try
            {
                var user = await _authRepository.FindByUsername(loginDto.Username);

                if (user == null)
                {
                    var errors = new Dictionary<string, string> { { "Username", "Userame does not exist." } };
                    return ApiResponse<LoginResponseDTO>.Failed(errors, "Login failed", HttpStatusCode.NotFound);
                }

                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (isPasswordCorrect == false)
                {
                    var errors = new Dictionary<string, string> { { "Password", "Invalid password" } };
                    return ApiResponse<LoginResponseDTO>.Failed(errors, "Login failed", HttpStatusCode.Unauthorized);
                }

                //var isEmailVerified = await _userManager.IsEmailConfirmedAsync(user);

                //if (isEmailVerified == false)
                //{
                //    var errors = new Dictionary<string, string> { { "Email", "Email not verified" } };
                //    return ApiResponse<LoginResponseDTO>.Failed(errors, "Login failed", HttpStatusCode.Unauthorized);
                //}

                #region generate jwt token
                string generatedToken = await _jwtService.GenerateJwtToken(user);
                #endregion

                #region response map
                var response = new LoginResponseDTO
                {
                    JwtToken = generatedToken,
                };
                #endregion

                return ApiResponse<LoginResponseDTO>.Success(response, "User Validated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in LoginUser for username {Username}", loginDto.Username);
                return ApiResponse<LoginResponseDTO>.Failed(null, "An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<ApiResponse<string>> ConfirmEmailVerification(string token, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var errors = new Dictionary<string, string> { { "User", "Incorrect Email, user not found." } };
                    return ApiResponse<string>.Failed(errors, "User Not Found.");
                }

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (!result.Succeeded)
                {
                    var errors = new Dictionary<string, string> { { "Email Confirmation", "Failed to confirm Email, Invalid Token." } };
                    return ApiResponse<string>.Failed(errors, "Email Confirmation Failed.");
                }
                return ApiResponse<string>.Success(null, "Email Confirmed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in ConfirmEmailVerification.");
                return ApiResponse<string>.Failed(null, "An unexpected error occurred. Please try again later.");
            }
        }

    }
}
