using System.ComponentModel.DataAnnotations;

namespace HFA.Models.ViewModels
{
    public class ReleaseNotesViewModel
    {
        [Key]
        public int NotesId { get; set; }

        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Display(Name = "Version Number")]
        public string? ReleaseVersion { get; set; }

        [Display(Name = "Release Date")]
        public DateTime? ReleaseDate { get; set; }  
    }
}
