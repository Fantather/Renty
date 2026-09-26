using Microsoft.AspNetCore.Mvc;
using Renty.Web.Models.PropertyEdit;

namespace Renty.Web.ViewComponents
{
    public class EditorMenuViewComponent : ViewComponent
    {
        private static readonly (string Name, string Action)[] MenuItems = [
            ("Точное место", "Location"),
            ("Видимость местоположения", "LocationVisibility"),
            ("Тип жилья", "Category"),
            ("Основная информация", "Basics"),
            ("Удобства", "Amenities"),
            ("Фото", "Photos"),
            ("Название", "Title"),
            ("Отличительные черты", "Tags"),
            ("Описание", "Description"),
            ("Параметры бронирования", "BookingSettings"),
            ("Цена", "Pricing"),
            ("Скидки", "Discounts")
            ];
        public IViewComponentResult Invoke()
        {
            Guid id = Guid.Parse(RouteData.Values["id"]!.ToString()!);
            string action = RouteData.Values["action"]!.ToString()!;
            List<EditorMenuItemViewModel> items = MenuItems
                .Select(item => new EditorMenuItemViewModel(
                    item.Name,
                    item.Action,
                    string.Equals(item.Action, action, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            var menu = new EditorMenuViewModel(id, items);

            return View(menu);
        }
    }
}
