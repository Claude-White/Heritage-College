using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CWhiteH60Customer.Models;

public interface IPartialViewUtility
{
    Task<string> RenderPartialToStringAsync(Controller controller, string viewName, object model);
}

public class PartialViewUtility : IPartialViewUtility {
    private readonly ICompositeViewEngine _viewEngine;

    public PartialViewUtility(ICompositeViewEngine viewEngine) {
        _viewEngine = viewEngine;
    }

    public async Task<string> RenderPartialToStringAsync(Controller controller, string viewName, object model) {
        var actionContext = new ActionContext(
            controller.HttpContext,
            controller.RouteData,
            controller.ControllerContext.ActionDescriptor
        );

        using (var sw = new StringWriter()) {
            var viewResult = _viewEngine.FindView(controller.ControllerContext, viewName, false);

            if (!viewResult.Success) {
                throw new InvalidOperationException($"Couldn't find view '{viewName}'");
            }

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                sw,
                new HtmlHelperOptions()
            );

            viewContext.ViewData.Model = model;
            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
}