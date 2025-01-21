using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OHMS_Service.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    // True is active, False is cancelled
    public bool Status { get; set; }

    // True is in person, False is virtual
    public bool Type { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CalendarId { get; set; }

    [JsonIgnore]
    public virtual Calendar Calendar { get; set; } = null!;

    [JsonIgnore]
    public virtual Student Student { get; set; } = null!;
}
