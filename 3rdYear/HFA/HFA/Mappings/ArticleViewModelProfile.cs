using AutoMapper;
using HFA.Models;
using HFA.Models.ViewModels;

namespace HFA.Mappings
{
    public class ArticleViewModelProfile : Profile
    {
        public ArticleViewModelProfile()
        {
            CreateMap<Article, ArticleViewModel>();
            CreateMap<ArticleViewModel, Article>();
        }
    }
}