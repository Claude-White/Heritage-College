namespace HFA.Models.ViewModels
{
    public class ArticleEditViewModel
    {
        public Article Article { get; set; }
        public int? HeroImageId { get; set; }

        public ArticleEditViewModel()
        {
        }

        public ArticleEditViewModel(Article article, int? heroImageId = null)
        {
            Article = article;
            HeroImageId = heroImageId;
        }
    }
}
