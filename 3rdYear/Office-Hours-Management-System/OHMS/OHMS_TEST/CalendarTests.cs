using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OHMS_Service.DAL;
using OHMS_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OHMS_TEST
{
    public class CalendarTests : IDisposable
    {
        private readonly DbContextOptions<StarsF24Context> _options;
        private readonly StarsF24Context _context;
        private readonly CalendarRepository _repository;

        public CalendarTests()
        {
            _options = new DbContextOptionsBuilder<StarsF24Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider())
                .Options;

            _context = new StarsF24Context(_options);

            var student = new Student
            {
                Id = 1,
                Name = "Raymond",
                Email = "raymond@gmail.com",
            };

            var calendar = new Calendar
            {
                Id = 1,
                TeacherId = 1,
                Name = "Test Calendar",
                CalendarCode = "12345"
            };

            var appointment = new Appointment
            {
                Id = 1,
                StudentId = 1,
                Status = true,
                Type = true,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMinutes(15),
                Title = "Test",
                CalendarId = 1,
                Student = student,
                Calendar = calendar
            };

            var appointment2 = new Appointment
            {
                Id = 2,
                StudentId = 1,
                Status = true,
                Type = true,
                StartDate = DateTime.Now.AddMinutes(15),
                EndDate = DateTime.Now.AddMinutes(30),
                Title = "Test 2",
                CalendarId = 1,
                Student = student,
                Calendar = calendar
            };

            var teacher = new Teacher
            {
                Id = 1,
                Name = "Test Teacher",
                Email = "",
            };

            student.Appointments.Add(appointment);
            student.Appointments.Add(appointment2);
            student.Calendars.Add(calendar);

            _context.Students.Add(student);
            _context.Teachers.Add(teacher);
            _context.SaveChanges();

            _repository = new CalendarRepository(_context);
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetById()
        {
            var calendar = await _repository.GetById(1);
            var appointment = calendar.Appointments.FirstOrDefault();
            var student = appointment.Student;
            var teacher = calendar.Teacher;

            Assert.NotNull(calendar);
            Assert.Equal(1, calendar.Id);
            Assert.Equal("12345", calendar.CalendarCode);
            Assert.NotNull(appointment);
            Assert.Equal("Test", appointment.Title);
            Assert.NotNull(student);
            Assert.Equal("Raymond", student.Name);
            Assert.NotNull(teacher);
            Assert.Equal(1, teacher.Id);
            Assert.Equal(2, calendar.Appointments.Count);
        }
    }
}
