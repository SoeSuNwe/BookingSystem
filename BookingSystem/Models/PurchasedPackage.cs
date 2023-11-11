namespace BookingSystem.Models
{
    public class PurchasedPackage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Credits { get; set; }

        // Navigation properties
        public User User { get; set; } = new User();
        public Package Package { get; set; } = new Package();
    }
}