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
        public User User { get; set; }
        public Package Package { get; set; }
    }
}