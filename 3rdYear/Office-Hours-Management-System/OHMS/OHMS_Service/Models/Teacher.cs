using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OHMS_Service.Models;

public partial class Teacher
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
}
