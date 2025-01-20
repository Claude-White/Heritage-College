using System.ComponentModel.DataAnnotations;

namespace HFA.Models
{
    public class Article
    {
        [Key]
        public int? ArticleId { get; set; }

        [Required(ErrorMessage = "The title is required.")]
        public string? Title { get; set; }

        public string? Lead { get; set; }

        [Required(ErrorMessage = "The content is required.")]
        public string? ContentBody { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string? Author { get; set; }

        public string? MostRecentEditor { get; set; }

        public bool IsPublished { get; set; }

        public Image? HeroImage { get; set; }

        public int? HeroImageId { get; set; }

        public string? Category { get; set; }
    }
}
