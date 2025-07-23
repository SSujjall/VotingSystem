using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using VotingSystem.API.Features.Auth.DTOs;
using VotingSystem.API.Features.Auth.Services;
using VotingSystem.Domain.Entities;
using VotingSystem.Infrastructure.ExternalServices.JwtService;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authRepository = A.Fake<IAuthRepository>();
            _userManager = A.Fake<UserManager<User>>();
            _roleManager = A.Fake<RoleManager<IdentityRole>>();
            _jwtService = A.Fake<IJwtService>();
            _logger = A.Fake<ILogger<AuthService>>();

            _authService = new AuthService(
                _authRepository,
                _userManager, 
                _roleManager, 
                _jwtService, 
                _logger
            );
        }

        #region Register Tests
        [Fact]
        public async Task RegisterUser_UsernameExists_ReturnsFailed()
        {
            // Arrange
            var signupDto = new SignupDTO { Username = "test", Email = "test@example.com" };
            A.CallTo(() => _authRepository.UsernameExists(signupDto.Username)).Returns(true);

            // Act
            var result = await _authService.RegisterUser(signupDto);

            // Assert
            Assert.False(result.Status);
            Assert.Contains("Username", result.Errors.Keys);
        }

        [Fact]
        public async Task RegisterUser_EmailExists_ReturnsFailed()
        {
            // Arrange
            var signupDto = new SignupDTO { Username = "test", Email = "test@example.com" };
            A.CallTo(() => _authRepository.UsernameExists(signupDto.Username)).Returns(false);
            A.CallTo(() => _authRepository.EmailExists(signupDto.Email)).Returns(true);

            // Act
            var result = await _authService.RegisterUser(signupDto);

            // Assert
            Assert.False(result.Status);
            Assert.Contains("Email", result.Errors.Keys);
        }

        [Fact]
        public async Task RegisterUser_UserCreationFailed_ReturnsFailed()
        {
            // Arrange
            var signupDto = new SignupDTO { Username = "test", Email = "test@example.com", Password = "Password123" };
            A.CallTo(() => _authRepository.UsernameExists(signupDto.Username)).Returns(false);
            A.CallTo(() => _authRepository.EmailExists(signupDto.Email)).Returns(false);
            A.CallTo(() => _authRepository.CreateNewUser(A<User>.Ignored, signupDto.Password)).Returns(false);

            // Act
            var result = await _authService.RegisterUser(signupDto);

            // Assert
            Assert.False(result.Status);
            Assert.Contains("User", result.Errors.Keys);
        }

        [Fact]
        public async Task RegisterUser_Success_ReturnsSuccess()
        {
            // Arrange
            var signupDto = new SignupDTO { Username = "test", Email = "test@example.com", Password = "Password123" };
            var user = new User { UserName = signupDto.Username };

            A.CallTo(() => _authRepository.UsernameExists(signupDto.Username)).Returns(false);
            A.CallTo(() => _authRepository.EmailExists(signupDto.Email)).Returns(false);
            A.CallTo(() => _authRepository.CreateNewUser(A<User>._, signupDto.Password)).Returns(true);
            A.CallTo(() => _roleManager.RoleExistsAsync("User")).Returns(true);
            A.CallTo(() => _userManager.AddToRoleAsync(A<User>.Ignored, "User")).Returns(IdentityResult.Success);
            A.CallTo(() => _authRepository.FindByUsername(signupDto.Username)).Returns(user);
            A.CallTo(() => _userManager.GenerateEmailConfirmationTokenAsync(user)).Returns("token123");

            // Act
            var result = await _authService.RegisterUser(signupDto);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("token123", result.Data.EmailConfirmToken);
        }
        #endregion

        #region Login Tests
        [Fact]
        public async Task LoginUser_UserNotFound_ReturnsFailed()
        {
            // Arrange
            var loginDto = new LoginDTO { Username = "missing", Password = "pass" };
            A.CallTo(() => _authRepository.FindByUsername(loginDto.Username)).Returns((User)null);

            // Act
            var result = await _authService.LoginUser(loginDto);

            // Assert
            Assert.False(result.Status);
            Assert.Contains("Username", result.Errors.Keys);
        }

        [Fact]
        public async Task LoginUser_InvalidPassword_ReturnsFailed()
        {
            // Arrange
            var user = new User { UserName = "user" };
            var loginDto = new LoginDTO { Username = "user", Password = "wrong" };
            A.CallTo(() => _authRepository.FindByUsername(loginDto.Username)).Returns(user);
            A.CallTo(() => _userManager.CheckPasswordAsync(user, loginDto.Password)).Returns(false);

            // Act
            var result = await _authService.LoginUser(loginDto);

            // Assert
            Assert.False(result.Status);
            Assert.Contains("Password", result.Errors.Keys);
        }

        [Fact]
        public async Task LoginUser_Success_ReturnsToken()
        {
            // Arrange
            var user = new User { UserName = "user" };
            var loginDto = new LoginDTO { Username = "user", Password = "pass" };

            A.CallTo(() => _authRepository.FindByUsername(loginDto.Username)).Returns(user);
            A.CallTo(() => _userManager.CheckPasswordAsync(user, loginDto.Password)).Returns(true);
            A.CallTo(() => _jwtService.GenerateJwtToken(user)).Returns("jwt-token");

            // Act
            var result = await _authService.LoginUser(loginDto);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("jwt-token", result.Data.JwtToken);
        }

        #endregion
    }
}
