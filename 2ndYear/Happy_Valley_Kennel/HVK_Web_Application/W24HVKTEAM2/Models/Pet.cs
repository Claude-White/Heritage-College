using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace W24HVKTEAM2.Models
{
    public partial class Pet
    {
        
        public enum DogGender { Male , Female };
        public enum DogSizeList { Small, Medium, Large };

        public Pet()
        {
            PetReservations = new HashSet<PetReservation>();
            PetVaccinations = new HashSet<PetVaccination>();
        }

        public int PetId { get; set; }
        [Display(Name = "Name:")]
        public string Name { get; set; } = null!;
        [Display(Name = "Gender:")]
        public string Gender { get; set; } = null!;
        [Display(Name = "Breed:")]
        public string? Breed { get; set; }
        [Display(Name = "Birth Year:")]
        public int? Birthyear { get; set; }
        public int HvkuserId { get; set; }
        [Display(Name = "Dog Size:")]
        public string? DogSize { get; set; }
        [Display(Name = "Climber:")]
        public bool Climber { get; set; }
        [Display(Name = "Barker:")]
        public bool Barker { get; set; }
        [Display(Name = "Notes:")]
        public string? SpecialNotes { get; set; }
        [Display(Name = "Sterilized:")]
        public bool Sterilized { get; set; }

        public virtual Hvkuser Hvkuser { get; set; } = null!;

        public virtual ICollection<PetReservation> PetReservations { get; set; }
        public virtual ICollection<PetVaccination> PetVaccinations { get; set; }
    }
}