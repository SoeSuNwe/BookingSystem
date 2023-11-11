using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Controllers
{
    public class BookingService
    {
        private readonly AppDbContext _dbContext;

        public BookingService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool BookClass(int userId, int scheduleId)
        {
            // Get user and schedule from the database
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            var schedule = _dbContext.Schedules.FirstOrDefault(s => s.Id == scheduleId);

            if (user != null && schedule != null)
            {
                // Check if the user has enough credits to book the class
                if (user.Credits >= schedule.RequiredCredits)
                {
                    // Check if the class is not already booked by the user
                    if (!_dbContext.Bookings.Any(b => b.UserId == userId && b.ScheduleId == scheduleId))
                    {
                        // Deduct credits from the user's package
                        user.Credits -= schedule.RequiredCredits;

                        // Create a new Booking entity
                        var booking = new Booking
                        {
                            UserId = userId,
                            ScheduleId = scheduleId,
                            BookingTime = DateTime.UtcNow
                        };

                        // Add the booking to the user's profile
                        user.Bookings.Add(booking);

                        // Add the booking to the schedule
                        schedule.Bookings.Add(booking);

                        // Save changes to the database
                        _dbContext.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
