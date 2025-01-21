namespace OHMS.Models;

public class SetOfficeHoursDto {
    public int TeacherId { get; set; }
    public int CalendarId { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
    public string DayOfWeek { get; set; }
}