using System;
using System.Collections.Generic;

namespace W24HVKTEAM2.Models
{
    public partial class DailyRate
    {
        public int DailyRateId { get; set; }
        public decimal Rate { get; set; }
        public string? DogSize { get; set; }
        public int ServiceId { get; set; }

        public virtual Service Service { get; set; } = null!;
    }
}
