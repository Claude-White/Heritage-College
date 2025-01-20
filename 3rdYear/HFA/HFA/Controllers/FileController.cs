using HFA.Models;
using HFA.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace HFA.Controllers
{    
    [AuthorizeForScopes(ScopeKeySection = "files.read")]
    [Authorize(Policy = "HasAccessToMemberContent")]
    public class FileController : Controller
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly IConfiguration _iConfig;

        public FileController(GraphServiceClient graphServiceClient, IConfiguration iConfig)
        {
            _graphServiceClient = graphServiceClient;
            _iConfig = iConfig;
        }
   
        public async Task<IActionResult> Index()
        {
            Files files = new(_graphServiceClient, _iConfig);
            ViewBag.CurrentFolder = null;


            return View(await files.GetFilesAsync());
        }

        public async Task<IActionResult> Search(string searchValue)
        {
            Files files = new(_graphServiceClient, _iConfig);

            if(searchValue == null)
            {
                return RedirectToAction("Index");
            }

            var response = await files.GetFilesFromSearchAsync(searchValue);

            if (response == null)
            {
                return RedirectToAction("Index");
            }

            return View("Index", await files.GetFilesFromSearchAsync(searchValue));
        }

        public async Task<IActionResult> ViewSubFolderFiles(string folderId)
        {
           
            Files files = new(_graphServiceClient, _iConfig);
 
            if(folderId == null)
            {
                return RedirectToAction("Index");
            }

            var response = await files.GetCurrentFolder(folderId);

            if(response == null)
            {
                return RedirectToAction("Index");

            }

            ViewBag.CurrentFolder = response.Name;
            ViewBag.ParentReference = response.ParentReference;

            return View("Index", await files.GetFilesFromFolderAsync(folderId));
        }

    }
}
