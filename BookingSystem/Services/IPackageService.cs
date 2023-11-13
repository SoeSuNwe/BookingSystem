using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Services
{
    public interface IPackageService
    {
        List<Package> GetAllPackages();
        Package GetPackageById(int packageId);
        List<Package> GetPackagesByCountry(string country);
    }
    public class PackageService : IPackageService
    {
        private readonly AppDbContext _dbContext;

        public PackageService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Package> GetAllPackages()
        {
            return _dbContext.Packages.ToList();
        }

        public Package GetPackageById(int packageId)
        {
            return _dbContext.Packages.FirstOrDefault(p => p.Id == packageId);
        }
        public List<Package> GetPackagesByCountry(string country)
        {
            return _dbContext.Packages.Where(p => p.Country == country).ToList();
        }
    }
}