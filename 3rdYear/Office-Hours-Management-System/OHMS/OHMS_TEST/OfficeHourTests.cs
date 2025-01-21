using OHMS_Service.Models;
using OHMS_Service.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace OHMS_TEST
{
    public class OfficeHourTests : IDisposable
    {
        private readonly DbContextOptions<StarsF24Context> _options;
        private readonly StarsF24Context _context;
        private readonly TeacherRepository _repository;

        public OfficeHourTests()
        {
            _options = new DbContextOptionsBuilder<StarsF24Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider())
                .Options;

            _context = new StarsF24Context(_options);
            _repository = new TeacherRepository(_context);

            SeedDB();
        }

        private void SeedDB()
        {
            var teacher = new Teacher
            {
                Id = 1,
                Name = "John",
                Email = "johndoe@gmail.com",
            };

            var calendar = new Calendar
            {
                Id = 1,
                TeacherId = 1,
                Name = "Test Calendar",
                CalendarCode = "12345"
            };

            _context.Teachers.Add(teacher);
            _context.Calendars.Add(calendar);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task SetOfficeHoursAsync_SetSuccessfully()
        {
            var teacherId = 1;
            var calendarId = 1;
            var start = new TimeSpan(8, 0, 0);
            var end = new TimeSpan(10, 0, 0);
            var dayOfWeek = "Tuesday";

            await _repository.SetOfficeHoursAsync(teacherId, calendarId, start, end, dayOfWeek);

            var officeHours = await _context.OfficeHours.FirstOrDefaultAsync();

            Assert.NotNull(officeHours);
            Assert.Equal(calendarId, officeHours.CalendarId);
            Assert.Equal(dayOfWeek, officeHours.DayOfWeek);
            Assert.True(officeHours.Status);
        }
        
        [Fact]
        public async Task SetOfficeHoursAsync_OverlappingOfficeHours_ThrowsException()
        {
            var teacherId = 1;
            var calendarId = 1;
            var start = new TimeSpan(8, 0, 0);
            var end = new TimeSpan(10, 0, 0);
            var dayOfWeek = "Tuesday";

            await _repository.SetOfficeHoursAsync(teacherId, calendarId, start, end, dayOfWeek);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _repository.SetOfficeHoursAsync(teacherId, calendarId, start, end, dayOfWeek));

            Assert.Equal("Office hours already exist for this time", ex.Message);
        }
        
        [Fact]
        public async Task SetOfficeHoursAsync_StartTimeBefore8AM_ThrowsException()
        {
            var teacherId = 1;
            var calendarId = 1;
            var start = new TimeSpan(7, 0, 0);
            var end = new TimeSpan(10, 0, 0);
            var dayOfWeek = "Tuesday";

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _repository.SetOfficeHoursAsync(teacherId, calendarId, start, end, dayOfWeek));

            Assert.Equal("Start time must be after 8:00 AM", ex.Message);
        }
        
        [Fact]
        public async Task SetOfficeHoursAsync_EndTimeAfter6PM_ThrowsException()
        {
            var teacherId = 1;
            var calendarId = 1;
            var start = new TimeSpan(8, 0, 0);
            var end = new TimeSpan(19, 0, 0);
            var dayOfWeek = "Tuesday";

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _repository.SetOfficeHoursAsync(teacherId, calendarId, start, end, dayOfWeek));

            Assert.Equal("End time must be before 6:00 PM", ex.Message);
        }
        
        [Fact]
        public async Task SetOfficeHoursAsync_TeacherDoesNotExist_ThrowsException()
        {
            var teacherId = 2;
            var calendarId = 1;
            var start = new TimeSpan(8, 0, 0);
            var end = new TimeSpan(10, 0, 0);
            var dayOfWeek = "Tuesday";

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _repository.SetOfficeHoursAsync(teacherId, calendarId, start, end, dayOfWeek));

            Assert.Equal("Teacher does not exist", ex.Message);
        }
        
        [Fact]
        public async Task SetOfficeHoursAsync_CalendarDoesNotExist_ThrowsException()
        {
            var teacherId = 1;
            var calendarId = 2;
            var start = new TimeSpan(8, 0, 0);
            var end = new TimeSpan(10, 0, 0);
            var dayOfWeek = "Tuesday";

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _repository.SetOfficeHoursAsync(teacherId, calendarId, start, end, dayOfWeek));

            Assert.Equal("Calendar does not exist", ex.Message);
        }
    }
}