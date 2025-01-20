using HFA.Models.ViewModels;

namespace HFA.Models
{
    public class ArticlesPage
    {

        public List<string?>? FilteringCategories { get; set; }

        public List<ArticleViewModel>? Articles { get; set; }    
    }
}
