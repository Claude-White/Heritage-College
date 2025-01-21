using Microsoft.EntityFrameworkCore;
using OHMS_Service.Models;

namespace OHMS_Service.DAL
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly StarsF24Context _context;
        public AppointmentRepository(StarsF24Context context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAppointments(int studentid)
        {
            return await _context.Appointments
                    .Where(a => a.StudentId == studentid)
                    .OrderBy(a => a.StartDate)
                    .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointments(int calendarId, string dayOfWeek)
        {
            return await _context.Appointments
                    .Where(a => a.CalendarId == calendarId)
                    .ToListAsync();
        }

        public async Task<Appointment> GetById(int id)
        {
            return await _context.Appointments
                    .Where(a => a.Id == id)
                    .FirstOrDefaultAsync();
        }

        public async Task Create(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var appointment = await GetById(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
