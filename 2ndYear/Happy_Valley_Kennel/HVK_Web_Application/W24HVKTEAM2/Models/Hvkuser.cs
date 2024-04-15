using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace W24HVKTEAM2.Models {

    public partial class Hvkuser {

        public Hvkuser() {
            Pets = new HashSet<Pet>();
        }

        public enum Provinces { QC, ON }


        public int HvkuserId { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [MaxLength(25)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        [Required]
        [MaxLength(25)]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = null!;

        [Display(Name = "Email")]
        [MaxLength(50)]
        [OneContactMin]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Display(Name = "Password")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        //[Display(Name = "Confirm Password")]
        //[MaxLength(50)]
        //[Required]
        //[DataType(DataType.Password)]
        //[Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
        //public string? ConfirmPassword { get; set; }

        [Display(Name = "Address")]
        [MaxLength(40)]
        [DataType(DataType.Text)]
        public string? Street { get; set; }

        [Display(Name = "City")]
        [MaxLength(25)]
        [DataType(DataType.Text)]
        public string? City { get; set; }

        [Display(Name = "Province")]
        //[EnumDataType(typeof(Provinces))]
        public string? Province { get; set; }

        [Display(Name = "Postal Code")]
        [DataType(DataType.PostalCode)]
        [RegularExpression("^[a-zA-Z]{1}[0-9]{1}[a-zA-Z]{1}\\s{0,1}[0-9]{1}[a-zA-Z]{1}[0-9]{1}$", ErrorMessage = "Invalid postal code. Please use the format A1A1A1.")]
        public string? PostalCode { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [OneContactMin]
        [RegularExpression("^[0-9]{3}[-]*[0-9]{3}[-]*[0-9]{4}$", ErrorMessage = "Invalid phone number. Please use the format 123-123-1234.")]
        public string? Phone { get; set; }

        [Display(Name = "Cell Number")]
        [DataType(DataType.PhoneNumber)]
        [OneContactMin]
        [RegularExpression("^[0-9]{3}[-]*[0-9]{3}[-]*[0-9]{4}$", ErrorMessage = "Invalid phone number. Please use the format 123-123-1234.")]
        public string? CellPhone { get; set; }

        [Display(Name = "First Name")]
        [SpecialRequired]
        [DataType(DataType.Text)]
        public string? EmergencyContactFirstName { get; set; }

        [Display(Name = "Last Name")]
        [SpecialRequired]
        [DataType(DataType.Text)]
        public string? EmergencyContactLastName { get; set; }

        [Display(Name = "Phone Number")]
        [SpecialRequired]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]{3}[-]*[0-9]{3}[-]*[0-9]{4}$", ErrorMessage = "Invalid phone number. Please use the format 123-123-1234.")]
        public string? EmergencyContactPhone { get; set; }

        public string UserType { get; set; } = null!;

        public virtual ICollection<Pet> Pets { get; set; }
    }
}