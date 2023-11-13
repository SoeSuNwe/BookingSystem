using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BookingSystem.Services
{
    public class BookingService
    {
        private readonly AppDbContext _dbContext;
        private readonly IDistributedCache _distributedCache;

        public BookingService(AppDbContext dbContext, IDistributedCache distributedCache)
        {
            _dbContext = dbContext;
            _distributedCache = distributedCache;
        }

        public bool BookClass(string userId, int scheduleId)
        {
            var cacheKey = $"Schedule_{scheduleId}";

            // Check if the schedule is in the distributed cache (indicating ongoing booking process)
            if (_distributedCache.GetString(cacheKey) != null)
            {
                // Handle concurrent booking (return false, inform user, etc.)
                return false;
            }

            // Add the schedule to the distributed cache to indicate the start of the booking process
            _distributedCache.SetString(cacheKey, "InProcess", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10), // Set an expiration time to release the cache lock
            });

            try
            {
                // Your existing booking logic
                var user = _dbContext.Users.Include(u => u.Bookings).FirstOrDefault(u => u.Id == userId);
                var schedule = _dbContext.Schedules.FirstOrDefault(s => s.Id == scheduleId);

                if (user != null && schedule != null)
                {
                    // Check if there is a booking conflict
                    if (!HasBookingConflict(user, schedule))
                    {
                        // Check if there are available slots for the class
                        if (schedule.Bookings.Count < schedule.MaxCapacity)
                        {
                            // Create the booking
                            var booking = new Booking
                            {
                                UserId = userId,
                                ScheduleId = scheduleId,
                                BookingTime = DateTime.UtcNow
                            };

                            // Add the booking to the user's bookings
                            user.Bookings.Add(booking);

                            // Add the booking to the schedule's bookings
                            schedule.Bookings.Add(booking);

                            // Save changes to the database
                            _dbContext.SaveChanges();
                            return true;
                        }
                        // Handle case where class is full
                        // You may want to add logic for waitlist here
                    }
                    // Handle case where there is a booking conflict
                    // You may want to inform the user or handle it in some way
                }

                return false;
            }
            finally
            {
                // Remove the schedule from the distributed cache to release the lock
                _distributedCache.Remove(cacheKey);
            }
        }

        private bool HasBookingConflict(User user, Schedule schedule)
        {
            // Check if the user already has a booking at the same time
            return user.Bookings.Any(b => b.Schedule.StartTime == schedule.StartTime);
        }
        public bool CancelBooking(int bookingId)
        {
            // Get the booking from the database
            var booking = _dbContext.Bookings
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Waitlist)
                .FirstOrDefault(b => b.Id == bookingId);

            if (booking != null)
            {
                // Check if the cancellation is allowed (4 hours before class start time)
                if (IsCancellationAllowed(booking.Schedule.StartTime))
                {
                    // Refund credits to the user's package
                    var user = _dbContext.Users
                        .Include(u => u.PurchasedPackages)
                        .FirstOrDefault(u => u.Id == booking.UserId);

                    var purchasedPackage = user?.PurchasedPackages
                        .FirstOrDefault(pp => pp.PackageId == booking.Schedule.PackageId);

                    if (user != null && purchasedPackage != null)
                    {
                        // Calculate the refunded credits based on the class required credits
                        int refundedCredits = booking.Schedule.RequiredCredits;

                        // Refund credits to the user's package
                        purchasedPackage.Credits += refundedCredits;

                        // Remove the booking from the user's bookings
                        user.Bookings.Remove(booking);

                        // Remove the booking from the schedule's bookings
                        booking.Schedule.Bookings.Remove(booking);

                        // Check if there are users on the waitlist
                        if (booking.Schedule.Waitlist.Any())
                        {
                            // Get the first user from the waitlist
                            var waitlistUser = booking.Schedule.Waitlist.First();

                            // Move the user from the waitlist to the booked list
                            user.Bookings.Add(new Booking
                            {
                                UserId = waitlistUser.UserId.ToString(),
                                ScheduleId = booking.Schedule.Id,
                                BookingTime = DateTime.UtcNow
                            });

                            // Remove the user from the waitlist
                            booking.Schedule.Waitlist.Remove(waitlistUser);
                        }

                        // Save changes to the database
                        _dbContext.SaveChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsCancellationAllowed(DateTime classStartTime)
        {
            // Check if the current time is at least 4 hours before the class start time
            return DateTime.UtcNow.AddHours(4) < classStartTime;
        }
        public bool CheckIn(int bookingId)
        {
            // Get the booking from the database
            var booking = _dbContext.Bookings
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Waitlist)
                .FirstOrDefault(b => b.Id == bookingId);

            if (booking != null && !booking.IsCheckedIn)
            {
                // Check if it's time to check in (you may adjust this based on your requirements)
                if (IsCheckInTime(booking.Schedule.StartTime))
                {
                    // Mark the booking as checked in
                    booking.IsCheckedIn = true;

                    // Save changes to the database
                    _dbContext.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        private bool IsCheckInTime(DateTime classStartTime)
        {
            // Check if the current time is within a certain range of the class start time
            // Adjust the time range based on your check-in policy
            return DateTime.UtcNow.AddMinutes(-15) < classStartTime && DateTime.UtcNow.AddMinutes(15) > classStartTime;
        }

    }
}
