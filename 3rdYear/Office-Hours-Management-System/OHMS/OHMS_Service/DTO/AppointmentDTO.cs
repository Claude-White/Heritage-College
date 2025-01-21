namespace OHMS_Service.DTO
{
    public class AppointmentDTO
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public bool Status { get; set; }

        public bool Type { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int CalendarId { get; set; }

    }
}
