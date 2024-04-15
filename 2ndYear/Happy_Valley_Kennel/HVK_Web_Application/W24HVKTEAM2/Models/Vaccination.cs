using System;
using System.Collections.Generic;

namespace W24HVKTEAM2.Models
{
    public partial class Vaccination
    {
        public Vaccination()
        {
            PetVaccinations = new HashSet<PetVaccination>();
        }

        public int VaccinationId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<PetVaccination> PetVaccinations { get; set; }
    }
}
