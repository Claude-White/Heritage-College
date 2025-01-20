using System.ComponentModel.DataAnnotations;

namespace HFA.Models.ViewModels
{
    public class ArticleSummaryViewModel
    {
        [Key]
        public int ArticleId { get; set; }

        public string? Title { get; set; }

        public string? Lead { get; set; }

        public bool IsPublished { get; set; }
    }
}
