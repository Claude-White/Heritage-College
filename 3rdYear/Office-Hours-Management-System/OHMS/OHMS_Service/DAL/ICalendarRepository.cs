using OHMS_Service.DTO;
using OHMS_Service.Models;

namespace OHMS_Service.DAL
{
    public interface ICalendarRepository
    {
        public Task<Calendar> GetById(int id);
        public Task<List<Calendar>> GetCalendars(int id);
        public Task<List<OfficeHour>> GetOfficeHours(int id);
        public Task<AvailableHoursDTO> GetAvailableHours(int id, DateTime from, DateTime to);
    }
}
