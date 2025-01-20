using HFA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using HFA.Resources;
using HFA.Models.ViewModels;

namespace HFA.Controllers
{
    [AllowAnonymous]
    public class HomeController(IDbAccess dbAccess, IMapper mapper) : Controller
    {
        private readonly IDbAccess _dbAccess = dbAccess;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> Index()
        {
            var allArticles = await _dbAccess.GetPublishedArticlesAsync();
            var featuredArticles = allArticles.Take(3);
            var featuredArticleViewModels = _mapper.Map<List<ArticleViewModel>>(featuredArticles);
            var HomePage = new HomePageViewModel()
            {
                Articles = featuredArticleViewModels,
                Mission = await _dbAccess.GetMissionAsync()
            };

            return View(HomePage);
        }


        public IActionResult Translate()
        {
            if (HFA_Resources.Culture == null || HFA_Resources.Culture.Name == "en-CA")
            {
                HFA_Resources.Culture = new System.Globalization.CultureInfo("fr-CA");
            } else if (HFA_Resources.Culture.Name == "fr-CA")
            {
                HFA_Resources.Culture = new System.Globalization.CultureInfo("en-CA");

            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("ReleaseNotes")]
        public async Task<IActionResult> ReleaseNotes()
        {
            return View(await _dbAccess.GetReleaseNotesAsync());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
