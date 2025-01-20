using HFA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace HFA.Controllers
{
    [AllowAnonymous]
    public class CalendarController : Controller
    {
        private readonly GraphServiceClient _graphServiceClient;

        public CalendarController(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        [AuthorizeForScopes(ScopeKeySection = "MicrosoftGraph:Scopes")]
        public async Task<IActionResult> Index()
        {
            CalendarService calendarService = new CalendarService(_graphServiceClient);
            var result = await calendarService.GetEventsAsync();
            return View();
        }

        [HttpGet("CalendarEvents")]
        public async Task<IUserEventsCollectionPage> GetEventsAsync()
        {
            var calendarService = new CalendarService(_graphServiceClient);
            var result = await calendarService.GetEventsAsync();
            return result;
        }
    }
}
