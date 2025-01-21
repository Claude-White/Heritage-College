using System;
using System.Collections.Generic;

namespace OHMS_Service.Models;

public partial class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
}
