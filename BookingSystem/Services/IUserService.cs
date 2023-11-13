using BookingSystem.Data;
using BookingSystem.Models;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace BookingSystem.Services
{
    public interface IUserService
    {
        User RegisterUser(string email, string country);
        User AuthenticateUser(string email, string password);
        bool VerifyEmail(string email, string token);
        User GetUserById(string userId);
        void UpdateUserProfile(string userId, string firstName, string lastName);
        bool RequestPasswordReset(string email);
        bool ResetPassword(string email, string resetToken, string newPassword);
        bool PurchasePackage(string userId, int packageId);
        bool ChangePassword(string userId, string currentPassword, string newPassword);
    }
    public class UserService : IUserService
    {
        // Implement IUserService
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Mock payment functions
        public bool AddPaymentCard(string userId, string cardNumber, string expiryDate, string cvc)
        {
            // This is a mock function. In a real-world scenario, you'd integrate with a payment gateway.
            return true;
        }

        public bool PaymentCharge(string userId, decimal amount)
        {
            // This is a mock function. In a real-world scenario, you'd integrate with a payment gateway.
            return true;
        }
        public bool PurchasePackage(string userId, int packageId)
        {
            // Get the user and package from the database
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            var package = _dbContext.Packages.FirstOrDefault(p => p.Id == packageId);

            if (user != null && package != null)
            {
                // Check if the user has enough credits to purchase the package
                if (user.Credits >= package.Credits)
                {
                    // Use mock payment functions for processing
                    if (AddPaymentCard(userId, "MockCardNumber", "MockExpiryDate", "MockCVC") &&
                        PaymentCharge(userId, package.Price))
                    {
                        // Deduct credits from the user's package
                        user.Credits -= package.Credits;

                        // Create a new PurchasedPackage entity
                        var purchasedPackage = new PurchasedPackage
                        {
                            PackageId = package.Id,
                            PurchaseDate = DateTime.UtcNow,
                            ExpirationDate = DateTime.UtcNow.AddDays(package.DaysValidity)
                        };

                        // Add the purchased package to the user's profile
                        user.PurchasedPackages.Add(purchasedPackage);

                        // Update the package's purchased packages collection
                        package.PurchasedPackages.Add(purchasedPackage);

                        // Save changes to the database
                        _dbContext.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        public User GetUserById(string userId)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == userId);
        }

        public void UpdateUserProfile(string userId, string firstName, string lastName)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                _dbContext.SaveChanges();
            }
        }
        public User RegisterUser(string email, string password)
        {
            // Validate email uniqueness and password strength
            // Perform any necessary validation

            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                Password = HashPassword(password),
                FirstName="A",
                LastName="Z", 
                IsEmailVerified = false,
                VerificationToken = GenerateVerificationToken() // Generate a unique token for email verification
                                                                // Set other user-related properties
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            // Send verification email
            SendVerifyEmail(newUser.Email, newUser.VerificationToken);

            return newUser;
        }
        public bool VerifyEmail(string email, string token)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email && u.VerificationToken == token);

            if (user != null)
            {
                user.IsEmailVerified = true;
                user.VerificationToken = null; // Invalidate the verification token
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }
        private string GenerateVerificationToken()
        {
            // Generate a unique verification token (for example, using a GUID)
            return Guid.NewGuid().ToString("N");
        }
        public User AuthenticateUser(string email, string password)
        {
            // Validate email and password
            // Perform any necessary validation

            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == HashPassword(password));

            return user;
        }
        public bool ChangePassword(string userId, string currentPassword, string newPassword)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId && u.Password == HashPassword(currentPassword));

            if (user != null)
            {
                user.Password = HashPassword(newPassword);
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool RequestPasswordReset(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                // Generate a unique reset token and send it via email (mock function)
                string resetToken = Guid.NewGuid().ToString("N");
                SendPasswordResetEmail(user.Email, resetToken);

                // Save the reset token in the database
                user.ResetToken = resetToken;
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool ResetPassword(string email, string resetToken, string newPassword)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email && u.ResetToken == resetToken);

            if (user != null)
            {
                // Update the password and clear the reset token
                user.Password = HashPassword(newPassword);
                user.ResetToken = null;
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        private string HashPassword(string password)
        {
            // Use a secure password hashing library (e.g., BCrypt, Argon2)
            // For example purposes, we'll use a simple hashing here
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        public static void SendVerifyEmail(string toEmail, string verificationToken)
        {
            //string subject = "Email Verification";
            //string body = $"Click the following link to verify your email: https://yourapp.com/verify-email?email={toEmail}&token={verificationToken}";

            //using (SmtpClient smtpClient = new SmtpClient("your-smtp-server.com"))
            //{
            //    smtpClient.UseDefaultCredentials = false;
            //    smtpClient.Credentials = new NetworkCredential("your-email@example.com", "your-email-password");
            //    smtpClient.Port = 587; // or the port your SMTP server uses
            //    smtpClient.EnableSsl = true;

            //    using (MailMessage mailMessage = new MailMessage())
            //    {
            //        mailMessage.From = new MailAddress("your-email@example.com");
            //        mailMessage.To.Add(toEmail);
            //        mailMessage.Subject = subject;
            //        mailMessage.Body = body;

            //        smtpClient.Send(mailMessage);
            //    }
            //}
        }
        public void SendPasswordResetEmail(string toEmail, string resetToken)
        {
            // This is a mock function. In a real-world scenario, you'd use an email service provider.

            string subject = "Password Reset";
            string body = $"Click the following link to reset your password: https://yourapp.com/reset-password?token={resetToken}";

            Console.WriteLine($"Sending email to: {toEmail}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body: {body}");
            Console.WriteLine("Email sent successfully");
        }

    }
}