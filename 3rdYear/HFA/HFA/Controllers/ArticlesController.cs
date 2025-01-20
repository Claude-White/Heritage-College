using AutoMapper;
using HFA.Models;
using HFA.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HFA.Controllers
{
    [AllowAnonymous]
    public class ArticlesController(IDbAccess dbAccess, IMapper mapper) : Controller
    {
        private readonly IDbAccess _dbAccess = dbAccess;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> Index()
        {
            var articles = await _dbAccess.GetPublishedArticlesAsync();
            var articleViewModels = _mapper.Map<List<ArticleViewModel>>(articles);
            var categories = await _dbAccess.GetArticleCategoriesAsync();
            var articlesPage = new ArticlesPage()
            {
                Articles = articleViewModels,
                FilteringCategories = categories
            };

            return View(articlesPage);
        }

        public async Task<IActionResult> Article(int articleId)
        {
            var article = await _dbAccess.GetArticleAsync(articleId);

            return View(article);
        }
    }
}
