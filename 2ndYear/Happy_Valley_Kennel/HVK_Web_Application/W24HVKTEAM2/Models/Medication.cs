using System;
using System.Collections.Generic;

namespace W24HVKTEAM2.Models
{
    public partial class Medication
    {
        public int MedicationId { get; set; }
        public string? Name { get; set; }
        public string? Dosage { get; set; }
        public string? SpecialInstruct { get; set; }
        public DateTime? EndDate { get; set; }
        public int PetReservationId { get; set; }

        public virtual PetReservation PetReservation { get; set; } = null!;
    }
}
