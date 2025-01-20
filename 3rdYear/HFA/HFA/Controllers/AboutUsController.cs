using HFA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HFA.Controllers
{
    [AllowAnonymous]
    public class AboutUsController : Controller
    {
        private readonly IDbAccess _dbAccess;

        public AboutUsController(IDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dbAccess.GetAboutUsAsync());
        }
    }
}
