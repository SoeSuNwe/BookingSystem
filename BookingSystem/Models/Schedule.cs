namespace BookingSystem.Models
{
    public class Schedule
    {

        public int Id { get; set; }
        public string ClassName { get; set; }
        public string Country { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RequiredCredits { get; set; }
        public int MaxCapacity { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
