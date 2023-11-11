namespace BookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public DateTime BookingTime { get; set; }
        public User User { get; set; }
        public Schedule Schedule { get; set; }
    }
}
