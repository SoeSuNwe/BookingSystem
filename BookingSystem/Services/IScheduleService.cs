using BookingSystem.Data;
using BookingSystem.Models;

namespace BookingSystem.Services
{
    public interface IScheduleService
    {
        IEnumerable<Schedule> GetClassSchedule(string email);
        void BookClass(string email, int classId);
        void CancelBooking(string email, int classId);
        List<Schedule> GetSchedulesByCountry(string country);
    }
    public class ScheduleService : IScheduleService
    {
        private readonly AppDbContext _dbContext;

        public ScheduleService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Schedule> GetSchedulesByCountry(string country)
        {
            return _dbContext.Schedules.Where(s => s.Country == country).ToList();
        }
        // Implement IScheduleService
        public void BookClass(string email, int classId)
        {
            throw new NotImplementedException();
        }

        public void CancelBooking(string email, int classId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Schedule> GetClassSchedule(string email)
        {
            throw new NotImplementedException();
        }
    }

}