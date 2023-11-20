namespace BookingSystem.DTOs
{
    public class UserDto
    {
        // UserRegistrationRequest.cs
        public class UserRegistrationRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // UserLoginRequest.cs
        public class UserLoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class UserLogin
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        public class UserProfileUpdateRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        public class ChangePasswordRequest
        {
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
        }

        // PasswordResetRequest.cs
        public class PasswordResetRequest
        {
            public string Email { get; set; }
        }

        // ResetPasswordRequest.cs
        public class ResetPasswordRequest
        {
            public string Email { get; set; }
            public string ResetToken { get; set; }
            public string NewPassword { get; set; }
        }
    }
}
