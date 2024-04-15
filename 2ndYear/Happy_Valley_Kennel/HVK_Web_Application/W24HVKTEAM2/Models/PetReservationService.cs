using System;
using System.Collections.Generic;

namespace W24HVKTEAM2.Models
{
    public partial class PetReservationService
    {
        public int PetReservationId { get; set; }
        public int ServiceId { get; set; }
        public string? NullHelper { get; set; }

        public virtual PetReservation PetReservation { get; set; } = null!;
        public virtual Service Service { get; set; } = null!;
    }
}
