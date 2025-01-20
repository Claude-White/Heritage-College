using AutoMapper;
using HFA.Models;
using HFA.Models.ViewModels;

namespace HFA.Mappings
{
    public class ArticleSummaryViewModelProfile : Profile
    {
        public ArticleSummaryViewModelProfile()
        {
            CreateMap<Article, ArticleSummaryViewModel>();
            CreateMap<ArticleSummaryViewModel, Article>();
        }
    }
}