using Hangfire;

namespace BookingSystem.Services
{

    public class BackgroundJobService : IBackgroundJobService
    {
        public void ConfigureRecurringJobs()
        {
            RecurringJob.AddOrUpdate(() => SomeRecurringTask(), Cron.Daily);
            // Add more recurring jobs as needed
        }

        public void SomeRecurringTask()
        {
            // Implement the logic for the recurring task
            Console.WriteLine("Executing some recurring task...");
        }
    }
}
