using OHMS_Service.Models;
namespace OHMS_Service.DAL
{
    public interface IAppointmentRepository
    {
        public Task<List<Appointment>> GetAppointments(int studentid);
        public Task<Appointment> GetById(int id);
        public Task Create(Appointment appointment);
        public Task Update(Appointment appointment);
        public Task Delete(int id);
    }
}
