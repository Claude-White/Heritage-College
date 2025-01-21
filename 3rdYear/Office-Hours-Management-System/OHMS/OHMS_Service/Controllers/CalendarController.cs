using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OHMS_Service.DAL;
using OHMS_Service.DTO;

namespace OHMS_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarRepository _repository;
        private readonly IMapper _mapper;

        public CalendarController(ICalendarRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetCalendars(int studentId)
        {
            var calendars = await _repository.GetCalendars(studentId);
            if (calendars == null)
            {
                return NotFound();
            }
            return Ok(calendars);
        }

        [HttpGet("Calendar/{id}")]
        public async Task<IActionResult> GetCalendar(int id)
        {
            var calendar = await _repository.GetById(id);
            if (calendar == null)
            {
                return NotFound();
            }
            return Ok(calendar);
        }

        [HttpGet("OfficeHours/{id}")]
        public async Task<IActionResult> GetOfficeHours(int id)
        {
            var officeHours = await _repository.GetOfficeHours(id);
            if (officeHours == null)
            {
                return NotFound();
            }
            return Ok(officeHours);
        }

        [HttpGet("Availability/{id}/{from}/{to}")]
        public async Task<AvailableHoursDTO> GetAvailability(int id, DateTime from, DateTime to)
        {
            var availability = await _repository.GetAvailableHours(id, from, to);
            if (availability == null)
            {
                return new AvailableHoursDTO();
            }
            return availability;
        }
    }
}
