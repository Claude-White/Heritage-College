using System;
using System.Collections.Generic;

namespace OHMS.Models;

public partial class Calendar
{
    public int Id { get; set; }

    public int TeacherId { get; set; }

    public string? Name { get; set; }

    public string CalendarCode { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<OfficeHour> OfficeHours { get; set; } = new List<OfficeHour>();

    public virtual Teacher Teacher { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
