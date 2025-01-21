namespace OHMS_Service.DTO;

public class GetOfficeHoursDTO {
    public TimeSpan StartDate { get; set; }
    public TimeSpan EndDate { get; set; }
    public string DayOfWeek { get; set; }
}