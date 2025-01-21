using OHMS_Service.DTO;
using OHMS_Service.Models;

namespace OHMS_Service.DAL;

public interface ITeacherRepository {
    public Task<Teacher> GetTeacher(int id);
    public Task SetOfficeHoursAsync(int teacherId, int calendarId, TimeSpan start, TimeSpan end, string dayOfWeek);

    public Task <IEnumerable<Appointment>> GetWeeklyAppointmentsAsync(int calendarId, DateTime startOfWeek, DateTime endOfWeek);
    
    public Task <IEnumerable<GetOfficeHoursDTO>> GetOfficeHoursByTeacherIdAsync(int teacherId);
}