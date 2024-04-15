using System;
using System.Collections.Generic;

namespace W24HVKTEAM2.Models
{
    public partial class Discount
    {
        public Discount()
        {
            PetReservationDiscounts = new HashSet<PetReservationDiscount>();
            ReservationDiscounts = new HashSet<ReservationDiscount>();
        }

        public int DiscountId { get; set; }
        public string Desciption { get; set; } = null!;
        public decimal Percentage { get; set; }
        public string Type { get; set; } = null!;

        public virtual ICollection<PetReservationDiscount> PetReservationDiscounts { get; set; }
        public virtual ICollection<ReservationDiscount> ReservationDiscounts { get; set; }
    }
}
