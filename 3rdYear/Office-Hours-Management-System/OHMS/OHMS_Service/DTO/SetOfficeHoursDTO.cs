namespace OHMS_Service.DTO;

public class SetOfficeHoursDTO {
    public int TeacherId { get; set; }
    public int CalendarId { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
    public string DayOfWeek { get; set; }
}