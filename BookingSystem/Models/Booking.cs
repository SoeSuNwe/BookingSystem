namespace BookingSystem.Models
{
    public class Booking
    { 
        public int Id { get; set; } 
        public int ScheduleId { get; set; }
        public DateTime BookingTime { get; set; }

        // Navigation properties
        public string UserId { get; set; }
        public User User { get; set; } = new User();
        public Schedule Schedule { get; set; } = new Schedule();
        public bool IsCheckedIn { get; internal set; }
    }
}
