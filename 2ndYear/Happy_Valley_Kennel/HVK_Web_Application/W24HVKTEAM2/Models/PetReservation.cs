using System;
using System.Collections.Generic;

namespace W24HVKTEAM2.Models
{
    public partial class PetReservation
    {
        public PetReservation()
        {
            Medications = new HashSet<Medication>();
            PetReservationDiscounts = new HashSet<PetReservationDiscount>();
            PetReservationServices = new HashSet<PetReservationService>();
        }

        public int PetReservationId { get; set; }
        public int PetId { get; set; }
        public int ReservationId { get; set; }
        public int? RunId { get; set; }

        public virtual Pet Pet { get; set; } = null!;
        public virtual Reservation Reservation { get; set; } = null!;
        public virtual Run? Run { get; set; }
        public virtual ICollection<Medication> Medications { get; set; }
        public virtual ICollection<PetReservationDiscount> PetReservationDiscounts { get; set; }
        public virtual ICollection<PetReservationService> PetReservationServices { get; set; }
    }
}
