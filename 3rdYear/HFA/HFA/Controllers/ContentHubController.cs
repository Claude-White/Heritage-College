using AutoMapper;
using HFA.Models;
using HFA.Models.ViewModels;
using HFA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HFA.Controllers
{
    // [Authorize(Policy = "HasEditPermissions")]
    [AllowAnonymous]
    public class ContentHubController(IDbAccess dbAccess, IMapper mapper, IImageService imageService) : Controller
    {
        private readonly IImageService _imageService = imageService;
        private readonly IDbAccess _dbAccess = dbAccess;
        private readonly IMapper _mapper = mapper;

        public IActionResult Index()
        {
            return View();
        }

        #region FAQ

        public async Task<IActionResult> FAQ()
        {
            try
            {
                return View(await _dbAccess.GetFAQAsync());
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost("[controller]/FAQ/CreateFAQ")]
        public async Task<IActionResult> CreateFAQ([FromBody] FrequentlyAskedQuestion frequentlyAskedQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int FaqId = await _dbAccess.CreateFAQAsync(frequentlyAskedQuestion);
                return Ok(FaqId);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("[controller]/FAQ/UpdateFAQ")]
        public async Task<IActionResult> UpdateFAQ([FromBody] FrequentlyAskedQuestion frequentlyAskedQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _dbAccess.UpdateFAQAsync(frequentlyAskedQuestion);
                return Ok("FAQ updated successfully");
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("[controller]/FAQ/DeleteFAQ/{id}")]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            try
            {
                await _dbAccess.DeleteFAQAsync(id);
                return Ok("FAQ deleted successfully");
            } catch
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region Articles

        public async Task<IActionResult> Articles()
        {
            try
            {
                var articles = await _dbAccess.GetArticlesAsync();
                var articleSummaryViewModels = _mapper.Map<List<ArticleSummaryViewModel>>(articles);

                return View(articleSummaryViewModels);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("[controller]/Articles/CreateArticle")]
        public IActionResult CreateArticle()
        {
            ViewBag.Action = "action-create";
            return View("ArticleEditPage", new ArticleEditViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Articles/CreateArticle")]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleEditViewModel articleEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-create";
                return BadRequest(ModelState);
            }

            try
            {
                Article article = articleEditViewModel.Article;
                if (articleEditViewModel.HeroImageId != null)
                {
                    article.HeroImageId = articleEditViewModel.HeroImageId;
                }

                // Update log values
                var claimNameValue = User.Claims.First(c => c.Type == "name").Value;

                if (article.Author == null)
                {
                    article.Author ??= claimNameValue;
                    article.CreatedDate ??= DateTime.Now;
                }

                article.MostRecentEditor = claimNameValue;
                article.LastModifiedDate = DateTime.Now;

                await _dbAccess.UpdateArticleAsync(article);

                return Ok(articleEditViewModel.Article.ArticleId);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("[controller]/Articles/{id}")]
        public async Task<IActionResult> UpdateArticle(int id)
        {
            try
            {
                ViewBag.Action = "action-update";

                var article = await _dbAccess.GetArticleAsync(id);
                if (article == null)
                {
                    return StatusCode(404);
                }

                var articleEditViewModel = new ArticleEditViewModel(article, article.HeroImageId);

                return View("ArticleEditPage", articleEditViewModel);
            } catch
            {
                ViewBag.Action = "action-update";
                return StatusCode(500);
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Articles/UpdateArticle")]
        public async Task<IActionResult> UpdateArticle([FromForm] ArticleEditViewModel articleEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-update";
                return BadRequest(ModelState);
            }

            try
            {
                Article article = articleEditViewModel.Article;
                if (articleEditViewModel.HeroImageId != null)
                {
                    article.HeroImageId = articleEditViewModel.HeroImageId;
                }

                // Update log values
                var claimNameValue = User.Claims.First(c => c.Type == "name").Value;

                if (article.Author == null)
                {
                    article.Author ??= claimNameValue;
                    article.CreatedDate ??= DateTime.Now;
                }

                article.MostRecentEditor = claimNameValue;
                article.LastModifiedDate = DateTime.Now;

                await _dbAccess.UpdateArticleAsync(article);

                return Ok(articleEditViewModel.Article.ArticleId);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("[controller]/Articles/DeleteArticle/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                await _dbAccess.DeleteArticleAsync(id);

                return Ok(new { redirectToUrl = Url.Action("Articles") }); // Since it's being called with ajax
            } catch
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region ReleaseNotes

        public async Task<IActionResult> ReleaseNotes()
        {
            try
            {
                var notes = await _dbAccess.GetReleaseNotesAsync();
                return View(await _dbAccess.GetReleaseNotesViewModelsAsync(notes));
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("[controller]/ReleaseNotes/CreateReleaseNote")]
        public IActionResult CreateReleaseNotes()
        {
            try
            {
                ViewBag.Action = "action-create";
                return View("ReleaseNotesEditPage", new ReleaseNotes());
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/ReleaseNotes/CreateReleaseNote")]
        public async Task<IActionResult> CreateReleaseNotes([FromForm] ReleaseNotes newNote)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-create";
                return BadRequest(ModelState);
            }
            try
            {
                await _dbAccess.CreateReleaseNoteAsync(newNote);
                return Ok(newNote.ReleaseNoteId);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("[controller]/ReleaseNotes/{id}")]
        public async Task<IActionResult> UpdateReleaseNote(int id)
        {
            try
            {
                ViewBag.Action = "action-update";
                var note = await _dbAccess.GetReleaseNoteAsync(id);
                if (note.ReleaseVersion != null && note.ReleaseVersion.Contains("("))
                    note.ReleaseVersion = note.ReleaseVersion?.Substring(0, note.ReleaseVersion.IndexOf("(")-1);
                return View("ReleaseNotesEditPage", note);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("[controller]/ReleaseNotes/UpdateReleaseNote")]
        public async Task<IActionResult> UpdateReleaseNote(ReleaseNotes note)
        {
            note.ReleaseVersion = note.ReleaseVersion?.Trim();
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-update";
                return BadRequest(ModelState);
            }
            try
            {
                await _dbAccess.UpdateReleaseNoteAsync(note);
                return Ok(note.ReleaseNoteId);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("[controller]/ReleaseNotes/DeleteReleaseNote/{id}")]
        public async Task<IActionResult> DeleteReleaseNote(int id)
        {
            try
            {
                await _dbAccess.DeleteReleaseNoteAsync(id);
                return Ok(new { redirectToUrl = Url.Action("ReleaseNotes") }); // Since it's being called with ajax
            } catch
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region AboutUs

        #region Mission

        public async Task<IActionResult> Mission()
        {
            try
            {
                var mission = await _dbAccess.GetMissionAsync();
                return View(mission);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("[controller]/AboutUs/UpdateMission")]
        public async Task<IActionResult> UpdateMission([FromBody] Mission mission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _dbAccess.UpdateMissionAsync(mission);
                return Ok(mission);
            } catch
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region Executives

        public async Task<IActionResult> Executives()
        {
            try
            {
                var executives = await _dbAccess.GetExecutivesViewModelAsync();
                return View(executives);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("[controller]/Executives/CreateExecutive")]
        public IActionResult CreateExecutive()
        {
            try
            {
                ViewBag.Action = "action-create";
                return View("ExecutiveEditPage", new Executive());
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("[controller]/Executives/{id}")]
        public async Task<IActionResult> UpdateExecutive(int id)
        {
            try
            {
                ViewBag.Action = "action-update";
                var executive = await _dbAccess.GetExecutiveAsync(id);
                return View("ExecutiveEditPage", executive);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost("[controller]/Executives/CreateExecutive")]
        public async Task<IActionResult> CreateExecutive([FromForm] Executive executive)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-create";
                return BadRequest(ModelState);
            }
            try
            {
                await _dbAccess.CreateExecutiveAsync(executive);
                return Ok(executive.ExecutiveId);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("[controller]/Executives/UpdateExecutive")]
        public async Task<IActionResult> UpdateExecutive([FromForm] Executive executive)
        {
            try
            {
                await _dbAccess.UpdateExecutiveAsync(executive);
                return Ok(executive);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("[controller]/Executives/DeleteExecutive/{id}")]
        public async Task<IActionResult> DeleteExecutive(int id)
        {
            try
            {
                await _dbAccess.DeleteExecutiveAsync(id);
                return Ok(new { redirectToUrl = Url.Action("Executives") });
            } catch
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region Elected Members

        public async Task<IActionResult> ElectedMembers()
        {
            try
            {
                var members = await _dbAccess.GetElectedMembersAsync();
                return View(members);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("[controller]/ElectedMembers/CreateElectedMembers")]
        public IActionResult CreateElectedMember()
        {
            try
            {
                ViewBag.Action = "action-create";
                return View("ElectedMembersEditPage", new ElectedGroup());
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("[controller]/ElectedMembers/{id}")]
        public async Task<IActionResult> UpdateElectedMember(int id)
        {
            try
            {
                ViewBag.Action = "action-update";
                var group = await _dbAccess.GetElectedGroupAsync(id);
                return View("ElectedMembersEditPage", group);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost("[controller]/ElectedMembers/CreateElectedMembers")]
        public async Task<IActionResult> CreateElectedMembers([FromForm] ElectedGroup electedGroup)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-create";
                return BadRequest(ModelState);
            }
            try
            {
                var id = await _dbAccess.CreateElectedGroupAsync(electedGroup);
                return Ok(id);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("[controller]/ElectedMembers/UpdateElectedMembers")]
        public async Task<IActionResult> UpdateElectedMembers([FromForm] ElectedGroup electedGroup)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-update";
                return BadRequest(ModelState);
            }
            try
            {
                await _dbAccess.UpdateElectedGroupAsync(electedGroup);
                return Ok(electedGroup.GroupId);
            } catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("[controller]/ElectedMembers/DeleteElectedMembers/{id}")]
        public async Task<IActionResult> DeleteElectedMembers(int id)
        {
            try
            {
                await _dbAccess.DeleteElectedGroupAsync(id);
                return Ok(new { redirectToUrl = Url.Action("ElectedMembers") });
            } catch
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region Image

        [HttpPost]
        [Route("[controller]/Images/CreateImage")]
        public async Task<IActionResult> CreateImageAsync(IFormFile file, string imageEditData)
        {
            try
            {
                Image? image = await _imageService.CreateImageAsync(file, imageEditData);
                if (image == null)
                {
                    return StatusCode(500);
                }

                return Ok(image);

            } catch(Exception ex)
            {
                return StatusCode(500, new { Detail = ex.Message });
            }
        }

        [HttpGet]
        [Route("[controller]/Images/GetImage")]
        public async Task<IActionResult> GetImageAsync(int imageId)
        {
            try
            {
                Console.WriteLine(imageId);
                Image? image = await _dbAccess.GetImageAsync(imageId);
                if (image == null)
                {
                    return StatusCode(404);
                }

                return Ok(image);
            } catch
            {
                return StatusCode(500);
            }
        }
        #endregion

        #endregion
    }
}
