using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using VotingSystem.API.Features.Auth.DTOs;
using VotingSystem.API.Features.Auth.Services;
using VotingSystem.Infrastructure.ExternalServices.EmailService;
using VotingSystem.Infrastructure.ExternalServices.EmailService.Models;
using VotingSystem.Infrastructure.ExternalServices.JwtService;

namespace VotingSystem.API.Features.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IEmailService emailService,
            IJwtService jwtService)
        {
            _authService = authService;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> RegisterUser(SignupDTO signupDto)
        {
            var response = await _authService.RegisterUser(signupDto);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return Accepted(response);
            }

            var verificationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token = response.Data.EmailConfirmToken, email = signupDto.Email }, Request.Scheme);
            var emailMessage = new EmailMessage(new[] { signupDto.Email }, "Please confirm your email", $"Please confirm your email by clicking the link: {verificationLink}");
            await _emailService.SendEmailAsync(emailMessage);
            return Ok(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var response = await _authService.ConfirmEmailVerification(token, email);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDTO model)
        {
            var response = await _authService.LoginUser(model);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return Accepted(response);
            }

            return Ok(response);
        }

        [HttpGet("auth-test")]
        [Authorize]
        public async Task<IActionResult> AuthorizeTest()
        {
            var response = $"You are authenticatred";
            return Ok(response);
        }
    }
}
