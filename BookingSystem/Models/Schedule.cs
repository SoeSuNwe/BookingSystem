namespace BookingSystem.Models
{
    public class Schedule
    {

        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int RequiredCredits { get; set; }
        public int MaxCapacity { get; set; }
        public int PackageId { get; set; }
        public string Country { get; set; }

        // Navigation properties
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Waitlist> Waitlist { get; set; } = new List<Waitlist>();
        public Package Package { get; set; } = new Package();
    }
}
