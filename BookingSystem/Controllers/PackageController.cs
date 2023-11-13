using BookingSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public IActionResult GetAllPackages()
        {
            var packages = _packageService.GetAllPackages();
            return Ok(packages);
        }

        [HttpGet("by-country/{country}")]
        public IActionResult GetPackagesByCountry(string country)
        {
            var packages = _packageService.GetPackagesByCountry(country);

            if (packages.Count == 0)
                return NotFound("No packages available for the specified country");

            return Ok(packages);
        }
        [HttpGet("{id}")]
        public IActionResult GetPackageById(int id)
        {
            var package = _packageService.GetPackageById(id);

            if (package == null)
                return NotFound("Package not found");

            return Ok(package);
        }
    }

}
