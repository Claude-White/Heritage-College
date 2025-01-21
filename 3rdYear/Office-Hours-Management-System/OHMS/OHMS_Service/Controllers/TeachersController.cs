using Microsoft.AspNetCore.Mvc;
using OHMS_Service.DAL;
using OHMS_Service.DTO;

namespace OHMS_Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeachersController : Controller {
    
    private readonly ITeacherRepository _teacherRepository;
    
    public TeachersController(ITeacherRepository teacherRepository) {
        _teacherRepository = teacherRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacher(int id)
    {
        var teachers = await _teacherRepository.GetTeacher(id);
        return Ok(teachers);
    }

    [HttpPost("SetOfficeHours")]
    public async Task<IActionResult> SetOfficeHours([FromBody] SetOfficeHoursDTO dto) {
        try {
            await _teacherRepository.SetOfficeHoursAsync(dto.TeacherId, dto.CalendarId, dto.Start, dto.End, dto.DayOfWeek);
            return Ok("Office hours set successfully");
        }
        catch (ArgumentException ex) {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("GetWeeklyOfficeHours/{calendarId}/{startOfWeek}/{endOfWeek}")]
    public async Task<IActionResult> GetWeeklyAppointments(int calendarId, DateTime startOfWeek, DateTime endOfWeek)
    {
        try
        {
            var officeHours = await _teacherRepository.GetWeeklyAppointmentsAsync(calendarId, startOfWeek, endOfWeek);
            return Ok(officeHours);
        } catch (ArgumentException error)
        {
            return BadRequest(new { error = error.Message });
        } catch (Exception error)
        {
            return BadRequest(new { error = error.Message });
        }

    }
    
    [HttpGet("GetOfficeHoursByTeacherId/{teacherId}")]
    public async Task<IActionResult> GetOfficeHoursByTeacherId(int teacherId) {
        try {
            var officeHours = await _teacherRepository.GetOfficeHoursByTeacherIdAsync(teacherId);
            return Ok(officeHours);
        }
        catch (ArgumentException ex) {
            return BadRequest(new { error = ex.Message });
        }
    }
}