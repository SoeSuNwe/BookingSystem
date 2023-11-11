namespace BookingSystem.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public int Credits { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int DaysValidity { get; set; }
        public ICollection<PurchasedPackage> PurchasedPackages { get; set; } = new List<PurchasedPackage>();

    }
}
