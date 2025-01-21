namespace OHMS_Service.DTO;

public class AvailableHoursDTO {
    public List<TimeChunk> TimeChunks { get; set; }
}

public class TimeChunk {
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string DayOfWeek { get; set; }
    // Only for calculation, no value in using outside of calendar repo
    public DateTime? Date { get; set; }
}