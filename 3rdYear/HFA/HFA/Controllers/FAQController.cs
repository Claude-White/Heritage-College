using HFA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace HFA.Controllers
{
    [AllowAnonymous]
    public class FAQController : Controller
    {
        private readonly IDbAccess _dbAccess;
        private readonly GraphServiceClient _graphServiceClient;
        private readonly IConfiguration _iConfig;


        public FAQController(IDbAccess dbAccess, GraphServiceClient graphServiceClient, IConfiguration iConfig)
        {
            _dbAccess = dbAccess;
            _iConfig = iConfig;
            _graphServiceClient = graphServiceClient;
        }

        [AuthorizeForScopes(ScopeKeySection = "MicrosoftGraph:Scopes")]
        [HttpPost]
        public async Task<IActionResult> SendEmail([Bind("FullName,EmailAddress,Subject,Message")] EmailContent content)
        {
            List<FrequentlyAskedQuestion> faqList = await _dbAccess.GetFAQAsync();

            if (!ModelState.IsValid)
            {
                return View(nameof(Index), faqList);
            }

            EmailService emailService = new(_graphServiceClient, _iConfig);

            if (await emailService.SetUpAsync(content))
            {
                return View(nameof(Index), faqList);
            }

            ModelState.AddModelError("", "Failed to send email. Please try again.");
            return View(nameof(Index), content);
        }



        public async Task<IActionResult> Index()
        {

            try
            {
                var graphClientUser = await _graphServiceClient.Me.Request().GetAsync();
                ViewBag.GraphClient = graphClientUser;
            } catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View(await _dbAccess.GetFAQAsync());


        }
    }
}
