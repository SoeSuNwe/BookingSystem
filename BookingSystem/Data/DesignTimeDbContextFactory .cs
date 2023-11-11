using BookingSystem.Data;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore; 
namespace BookingSystem.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // UseMySql instead of UseMySQL, and provide the server version as an instance of ServerVersion
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new AppDbContext(builder.Options);
        }
    }
}
