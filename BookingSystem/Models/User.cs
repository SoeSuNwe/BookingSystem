namespace BookingSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsEmailVerified { get; set; }
        public string VerificationToken { get; set; }
        public string ResetToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Credits { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public ICollection<PurchasedPackage> PurchasedPackages { get; set; } = new List<PurchasedPackage>();
    }
}
