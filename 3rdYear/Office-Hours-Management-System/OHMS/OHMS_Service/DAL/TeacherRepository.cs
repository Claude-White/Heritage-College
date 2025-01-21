using Microsoft.EntityFrameworkCore;
using OHMS_Service.Models;
using System.Globalization;
using OHMS_Service.DTO;

namespace OHMS_Service.DAL;

public class TeacherRepository : ITeacherRepository
{
    private readonly StarsF24Context _context;

    public TeacherRepository(StarsF24Context context)
    {
        _context = context;
    }

    // Allows a teacher to set their office hours
    public async Task SetOfficeHoursAsync(int teacherId, int calendarId, TimeSpan start, TimeSpan end, string dayOfWeek) {
        // Checks if the calendar exists in the database
        var calendarExists = await _context.Calendars.AnyAsync(c => c.Id == calendarId);
        if (!calendarExists)
        {
            throw new ArgumentException("Calendar does not exist");
        }

        // Checks if the teacher exists in the database
        var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == teacherId);
        if (!teacherExists)
        {
            throw new ArgumentException("Teacher does not exist");
        }

        // Checks if the office hours already exist for the given time
        var checkHoursAlreadyExist = await _context.OfficeHours.AnyAsync(oh =>
            oh.CalendarId == calendarId &&
            oh.DayOfWeek == dayOfWeek &&
            ((start >= oh.StartDate && start < oh.EndDate) || (end > oh.StartDate && end <= oh.EndDate) || 
             (start <= oh.StartDate && end >= oh.EndDate)));

        if (checkHoursAlreadyExist)
        {
            throw new InvalidOperationException("Office hours already exist for this time");
        }

        // Checks if the start time is after 8:00 AM and the end time is before 6:00 PM
        var minStartTime = new TimeSpan(8, 0, 0);
        if (start < minStartTime) {
            throw new ArgumentException("Start time must be after 8:00 AM");
        }
        
        var maxEndTime = new TimeSpan(18, 0, 0);
        if (end > maxEndTime) {
            throw new ArgumentException("End time must be before 6:00 PM");
        }

        // Checks if the office hours are longer than 10 hours
        var maxOfficeHourDuration = new TimeSpan(10, 0, 0);
        if (end - start > maxOfficeHourDuration)
        {
            throw new ArgumentException("Office hours cannot be longer than 10 hours");
        }

        // Checks if the start date is before the end date
        if (start >= end)
        {
            throw new ArgumentException("Start date must be before end date");
        }

        // Adds the office hours to the database and sets the status to active (true) by default
        var officeHours = new OfficeHour
        {
            CalendarId = calendarId,
            StartDate = start,
            EndDate = end,
            Status = true,
            DayOfWeek = dayOfWeek
        };

        _context.OfficeHours.Add(officeHours);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Appointment>> GetWeeklyAppointmentsAsync(int calendarId, DateTime startOfWeek, DateTime endOfWeek)
    {
        var endOfWeekAdjusted = endOfWeek.Date.AddDays(1).AddSeconds(-1); 

        var appointments = await _context.Appointments
            .Where(x => x.CalendarId == calendarId &&
                        x.StartDate >= startOfWeek &&
                        x.StartDate < endOfWeekAdjusted && 
                        x.Status == true)
            .ToListAsync();

        return appointments;

    }
    
    // Returns a list of the teacher's office hours and checks if the status is true (not cancelled)
    public async Task<IEnumerable<GetOfficeHoursDTO>> GetOfficeHoursByTeacherIdAsync(int teacherId)
    {
        var officeHours = await _context.OfficeHours
            .Where(oh => oh.Calendar.TeacherId == teacherId && oh.Status == true)
            .Select(oh => new GetOfficeHoursDTO
            {
                StartDate = oh.StartDate,
                EndDate = oh.EndDate,
                DayOfWeek = oh.DayOfWeek
            })
            .ToListAsync();

        return officeHours;
    }

    public Task<Teacher> GetTeacher(int id)
    {
        throw new NotImplementedException();
    }
}