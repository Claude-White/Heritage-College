using Microsoft.AspNetCore.Mvc;
using OHMS_Service.DAL;
using OHMS_Service.Models;
using OHMS_Service.DTO;
using AutoMapper;

namespace OHMS_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetAppointments(int studentId)
        {
            var appointments = await _repository.GetAppointments(studentId);
            if (appointments == null)
            {
                return NotFound();
            }
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _repository.GetById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(AppointmentDTO appointmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var appointment = _mapper.Map<Appointment>(appointmentDTO);
            await _repository.Create(appointment);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentDTO appointmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var appointment = await _repository.GetById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            _mapper.Map(appointmentDTO, appointment);
            await _repository.Update(appointment);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _repository.GetById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            await _repository.Delete(id);
            return Ok();
        }

    }
}
