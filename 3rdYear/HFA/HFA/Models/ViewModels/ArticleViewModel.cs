using System.ComponentModel.DataAnnotations;

namespace HFA.Models.ViewModels
{
    public class ArticleViewModel
    {
        [Key]
        public int ArticleId { get; set; }

        public string? Title { get; set; }

        public string? Lead { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsPublished { get; set; }

        public Image? HeroImage { get; set; }

        public int? HeroImageId { get; set; }

        public string? Category { get; set; }
    }
}
