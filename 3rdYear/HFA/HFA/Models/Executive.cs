using System.ComponentModel.DataAnnotations;

namespace HFA.Models
{
    public class Executive
    {
        public int ExecutiveId { get; set; }

        [Required(ErrorMessage = "The first name is required.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "The last name is required.")]
        public string? LastName { get; set;}

        [Required(ErrorMessage = "The email is required.")]
        [EmailAddress(ErrorMessage = "The email must be in the correct format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The HFA position is required.")]
        public string? Position { get; set; }

        public string? Description { get; set; }

        public Executive()
        {
            ExecutiveId = 0;
            FirstName = null;
            LastName = null;
            Email = null;
            Position = null;
            Description = null;
        }

        public Executive(string firstname, string lastname, string email, string position, string description)
        {
            ExecutiveId = 0;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            Position = position;
            Description = description;
        }

        public Executive(int id, string firstname, string lastname, string email, string position, string description)
        {
            ExecutiveId = id;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            Position = position;
            Description = description;
        }

    }
}
