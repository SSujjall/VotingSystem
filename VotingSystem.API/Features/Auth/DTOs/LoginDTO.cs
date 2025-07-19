using System.ComponentModel.DataAnnotations;

namespace VotingSystem.API.Features.Auth.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string JwtToken { get; set; }
    }
}
