using AutoMapper;
using OHMS_Service.DTO;
using OHMS_Service.Models;

namespace OHMS_Service.DTO
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<AppointmentDTO, Appointment>();
            CreateMap<Appointment, AppointmentDTO>();
        }
    }
}
