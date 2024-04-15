using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace W24HVKTEAM2.Models
{
    public partial class PetVaccination
    {
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }
        public int VaccinationId { get; set; }
        public int PetId { get; set; }
        public bool VaccinationChecked { get; set; }

        public virtual Pet Pet { get; set; } = null!;
        public virtual Vaccination Vaccination { get; set; } = null!;
    }
}
