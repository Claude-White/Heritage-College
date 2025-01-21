using Microsoft.EntityFrameworkCore;
using OHMS_Service.DTO;
using OHMS_Service.Models;

namespace OHMS_Service.DAL {
    public class CalendarRepository : ICalendarRepository {
        private readonly StarsF24Context _context;

        public CalendarRepository(StarsF24Context context) {
            _context = context;
        }

        public Task<Calendar> GetById(int id) {
            return _context.Calendars
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AvailableHoursDTO> GetAvailableHours(int calendarId, DateTime from, DateTime to) {
            ValidateDateRange(from, to);

            var officeHours = await FetchOfficeHours(calendarId);
            var existingAppointments = await FetchExistingAppointments(calendarId, from, to);

            var availableTimeChunks = CalculateAvailableTimeChunks(officeHours, existingAppointments, from, to);
            
            var availableTimeBlocks = SplitInto15MinuteBlocks(availableTimeChunks);

            return new AvailableHoursDTO {
                TimeChunks = availableTimeBlocks
            };
        }
        
        private List<TimeChunk> SplitInto15MinuteBlocks(List<TimeChunk> availableTimeChunks) {
            var splitChunks = new List<TimeChunk>();

            foreach (var chunk in availableTimeChunks) {
                var currentBlockStart = chunk.StartDate;
                var currentBlockEnd = chunk.EndDate;

                // Split the time chunk into 15-minute blocks
                while (currentBlockStart.AddMinutes(15) <= currentBlockEnd) {
                    var blockEnd = currentBlockStart.AddMinutes(15);
                    splitChunks.Add(new TimeChunk {
                        StartDate = currentBlockStart,
                        EndDate = blockEnd,
                        DayOfWeek = currentBlockStart.DayOfWeek.ToString(),
                        Date = currentBlockStart.Date
                    });
                    
                    currentBlockStart = blockEnd;
                }
            }

            return splitChunks;
        }

        private void ValidateDateRange(DateTime from, DateTime to) {
            if (from > to)
                throw new ArgumentException("From date must be earlier than or equal to the to date.");
        }

        private async Task<List<OfficeHour>> FetchOfficeHours(int calendarId)
        {
            return await _context.OfficeHours
                .Where(oh => oh.CalendarId == calendarId)
                .ToListAsync();
        }
        public Task<List<Calendar>> GetCalendars(int id)
        {
            return _context.Calendars
                    .Where(c => c.Students.Any(s => s.Id == id))
                    .Include(t => t.Teacher)
                    .ToListAsync();
        }

        public Task<List<OfficeHour>> GetOfficeHours(int id)
        {
            return _context.OfficeHours
                    .Where(oh => oh.Calendar.Id == id)
                    .ToListAsync();
        }

        private async Task<List<Appointment>> FetchExistingAppointments(int calendarId, DateTime from, DateTime to) {
            return await _context.Appointments
                .Where(a => a.CalendarId == calendarId &&
                            a.StartDate <= to &&
                            a.EndDate >= from &&
                            a.Status)
                .ToListAsync();
        }

        private List<TimeChunk> CalculateAvailableTimeChunks(
            List<OfficeHour> officeHours,
            List<Appointment> existingAppointments,
            DateTime from,
            DateTime to) {
            var availableTimeChunks = new List<TimeChunk>();

            for (var currentDate = from.Date; currentDate <= to.Date; currentDate = currentDate.AddDays(1)) {
                var dayOfficeHours = FilterOfficeHoursForDay(officeHours, currentDate);

                foreach (var officeHour in dayOfficeHours) {
                    var (startTime, endTime) = CalculateDailyOfficeHourTimeRange(officeHour, currentDate, from, to);

                    if (startTime >= endTime)
                        continue;

                    var dayAppointments = FilterAppointmentsForDay(existingAppointments, startTime, endTime);

                    availableTimeChunks.AddRange(
                        FindAvailableTimeChunksForDay(startTime, endTime, dayAppointments, currentDate)
                    );
                }
            }

            return availableTimeChunks;
        }

        private List<OfficeHour> FilterOfficeHoursForDay(List<OfficeHour> officeHours, DateTime currentDate) {
            return officeHours
                .Where(oh => oh.DayOfWeek.Equals(currentDate.ToString("dddd"), StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private (DateTime startTime, DateTime endTime) CalculateDailyOfficeHourTimeRange(
            OfficeHour officeHour,
            DateTime currentDate,
            DateTime from,
            DateTime to) {
            var startTime = currentDate.Date.Add(officeHour.StartDate);
            var endTime = currentDate.Date.Add(officeHour.EndDate);

            // Limit start and end times to the requested range
            startTime = startTime < from ? from : startTime;
            endTime = endTime > to ? to : endTime;

            return (startTime, endTime);
        }

        private List<Appointment> FilterAppointmentsForDay(
            List<Appointment> existingAppointments,
            DateTime startTime,
            DateTime endTime) {
            return existingAppointments
                .Where(a => a.StartDate < endTime && a.EndDate > startTime)
                .OrderBy(a => a.StartDate)
                .ToList();
        }

        private List<TimeChunk> FindAvailableTimeChunksForDay(
            DateTime startTime,
            DateTime endTime,
            List<Appointment> dayAppointments,
            DateTime currentDate) {
            var availableTimeChunks = new List<TimeChunk>();
            var currentChunkStart = startTime;

            foreach (var appointment in dayAppointments) {
                // Add available time before the appointment
                if (currentChunkStart < appointment.StartDate) {
                    availableTimeChunks.Add(CreateTimeChunk(currentChunkStart, appointment.StartDate, currentDate));
                }

                // Move current chunk start to after the appointment
                currentChunkStart = appointment.EndDate > currentChunkStart
                    ? appointment.EndDate
                    : currentChunkStart;
            }

            // Add final chunk if there's remaining time after appointments
            if (currentChunkStart < endTime) {
                availableTimeChunks.Add(CreateTimeChunk(currentChunkStart, endTime, currentDate));
            }

            return availableTimeChunks;
        }

        private TimeChunk CreateTimeChunk(DateTime startTime, DateTime endTime, DateTime currentDate) {
            return new TimeChunk {
                StartDate = startTime,
                EndDate = endTime,
                DayOfWeek = currentDate.ToString("dddd"),
                Date = currentDate
            };
        }
    }
}