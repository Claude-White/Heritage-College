using System;
using System.Collections.Generic;

namespace OHMS.Models;

public partial class OfficeHour
{
    public int Id { get; set; }

    public int CalendarId { get; set; }

    public bool Status { get; set; }

    public TimeSpan StartDate { get; set; }

    public TimeSpan EndDate { get; set; }
    
    public string DayOfWeek { get; set; }

    public virtual Calendar Calendar { get; set; } = null!;
}