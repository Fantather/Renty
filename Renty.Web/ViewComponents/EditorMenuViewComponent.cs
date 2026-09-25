using Microsoft.AspNetCore.Mvc;
using Renty.Web.Models.PropertyEdit;

namespace Renty.Web.ViewComponents
{
    public class EditorMenuViewComponent : ViewComponent
    {
        private static readonly (string Name, string Action)[] Sections =
        {
            ("Название", "Title"),
            ("Цена", "Pricing"),
        };

        public IViewComponentResult Invoke()
        {
            var propertyId = Guid.Parse(ViewContext.RouteData.Values["id"]!.ToString()!);
            var currentAction = ViewContext.RouteData.Values["action"]?.ToString();

            var items = Sections
                .Select(s => new EditorMenuItemViewModel(
                    s.Name,
                    s.Action,
                    string.Equals(s.Action, currentAction, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return View(new EditorMenuViewModel(propertyId, items));
        }
    }
}
