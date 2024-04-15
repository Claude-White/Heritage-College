using System;
using System.Collections.Generic;

namespace W24HVKTEAM2.Models
{
    public partial class Service
    {
        public Service()
        {
            DailyRates = new HashSet<DailyRate>();
            PetReservationServices = new HashSet<PetReservationService>();
        }

        public int ServiceId { get; set; }
        public string ServiceDescription { get; set; } = null!;

        public virtual ICollection<DailyRate> DailyRates { get; set; }
        public virtual ICollection<PetReservationService> PetReservationServices { get; set; }
    }
}
