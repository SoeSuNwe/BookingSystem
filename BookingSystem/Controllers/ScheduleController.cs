using BookingSystem.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("by-country/{country}")]
        public IActionResult GetSchedulesByCountry(string country)
        {
            var schedules = _scheduleService.GetSchedulesByCountry(country);

            if (schedules.Count == 0)
                return NotFound("No schedules available for the specified country");

            return Ok(schedules);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("list")]
        public IActionResult GetClassSchedule()
        {
            // Get available class schedule for the user's country
            var schedule = _scheduleService.GetClassSchedule(User.Identity.Name);
            return Ok(schedule);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("book")]
        public IActionResult BookClass([FromBody] Booking model)
        {
            // Validate and process class booking
            _scheduleService.BookClass(User.Identity.Name, model.Id);
            return Ok(new { Message = "Class booked successfully." });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("cancel/{classId}")]
        public IActionResult CancelBooking(int classId)
        {
            // Cancel class booking
            _scheduleService.CancelBooking(User.Identity.Name, classId);
            return Ok(new { Message = "Booking canceled successfully." });
        }

        // Other schedule-related endpoints can be added here
    }
}
