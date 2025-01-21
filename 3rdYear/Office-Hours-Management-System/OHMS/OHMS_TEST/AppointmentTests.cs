using OHMS_Service.Models;
using OHMS_Service.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace OHMS_TEST
{
    public class AppointmentTests : IDisposable
    {
        private readonly DbContextOptions<StarsF24Context> _options;
        private readonly StarsF24Context _context;
        private readonly AppointmentRepository _repository;

        public AppointmentTests()
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

            student.Appointments.Add(appointment);
            student.Calendars.Add(calendar);

            _context.Students.Add(student);
            _context.SaveChanges();

            _repository = new AppointmentRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdTest()
        {
            var result = await _repository.GetById(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Title);
        }

        [Fact]
        public async Task CreateAppointmentTest()
        {
            var appointment = new Appointment
            {
                Id = 2,
                StudentId = 1,
                Status = true,
                Type = true,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMinutes(15),
                Title = "Test",
                CalendarId = 1
            };
            await _repository.Create(appointment);

            var result = _context.Appointments.Find(2);
            Assert.NotNull(result);
            Assert.Equal(appointment.Id, result.Id);
            Assert.Equal(appointment.Title, result.Title);
        }

        [Fact]
        public async Task UpdateAppointmentTest()
        {
            var existingAppointment = await _context.Appointments.FindAsync(1);
            if (existingAppointment != null)
            {
                existingAppointment.Status = false;
                existingAppointment.Title = "Updated Test";

                await _repository.Update(existingAppointment);
            }

            var result = await _context.Appointments.FindAsync(1);
            Assert.NotNull(result);
            Assert.Equal(existingAppointment.Id, result.Id);
            Assert.Equal("Updated Test", result.Title);
        }

        [Fact]
        public async Task DeleteAppointmentTest()
        {
            await _repository.Delete(1);
            var result = await _context.Appointments.FindAsync(1);
            Assert.Null(result);
        }
    }
}
