using BookingSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BookingSystem.DTOs.UserDto;

namespace BookingSystem.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationRequest request)
        {
            var newUser = _userService.RegisterUser(request.Email, request.Password);
            return Ok(new { newUser.Id, newUser.Email, newUser.IsEmailVerified });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {
            var user = _userService.AuthenticateUser(request.Email, request.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            if (!user.IsEmailVerified)
                return BadRequest("Email not verified");

            // You may generate and return a JWT token for authenticated users

            return Ok(new { user.Id, user.Email });
        }

        [HttpPost("purchase-package/{packageId}")]
        public IActionResult PurchasePackage(int packageId)
        {
            var userId = GetUserIdFromClaims();
            var success = _userService.PurchasePackage(userId.ToString(), packageId);

            if (success)
                return Ok("Package purchased successfully");

            return BadRequest("Failed to purchase the package");
        }

        [HttpGet("verify-email")]
        public IActionResult VerifyEmail([FromQuery] string email, [FromQuery] string token)
        {
            var result = _userService.VerifyEmail(email, token);

            if (result)
                return Ok("Email verification successful");

            return BadRequest("Invalid email verification token");
        }
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userId = GetUserIdFromClaims(); // A helper method to extract user ID from claims
            var user = _userService.GetUserById(userId.ToString());

            if (user == null)
                return NotFound("User not found");

            return Ok(new { user.Id, user.Email, user.FirstName, user.LastName });
        }

        [HttpPut("profile")]
        public IActionResult UpdateProfile([FromBody] UserProfileUpdateRequest request)
        {
            var userId = GetUserIdFromClaims();
            _userService.UpdateUserProfile(userId.ToString(), request.FirstName, request.LastName);

            return Ok("Profile updated successfully");
        }
        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = GetUserIdFromClaims();
            var success = _userService.ChangePassword(userId.ToString(), request.CurrentPassword, request.NewPassword);

            if (success)
                return Ok("Password changed successfully");

            return BadRequest("Invalid current password");
        }

        [HttpPost("request-password-reset")]
        [AllowAnonymous] // Allow access without authentication
        public IActionResult RequestPasswordReset([FromBody] PasswordResetRequest request)
        {
            var success = _userService.RequestPasswordReset(request.Email);

            if (success)
                return Ok("Password reset request sent successfully");

            return BadRequest("User not found");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous] // Allow access without authentication
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var success = _userService.ResetPassword(request.Email, request.ResetToken, request.NewPassword);

            if (success)
                return Ok("Password reset successful");

            return BadRequest("Invalid reset token or email");
        }
        private int GetUserIdFromClaims()
        {
            // A helper method to extract user ID from claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }

            return 0; // Return an appropriate default value or handle the situation accordingly
        }

    }

}
