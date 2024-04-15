using System;
using System.Collections.Generic;

namespace W24HVKTEAM2.Models
{
    public partial class Run
    {
        public Run()
        {
            PetReservations = new HashSet<PetReservation>();
        }

        public int RunId { get; set; }
        public string Size { get; set; } = null!;
        public bool Covered { get; set; }
        public string? Location { get; set; }
        public decimal? Status { get; set; }

        public virtual ICollection<PetReservation> PetReservations { get; set; }
    }
}
