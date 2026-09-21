using Renty.Web.Models.Shared;

namespace Renty.Web.Models.Search
{
    public class SearchIndexViewModel
    {
        public List<PropertyCardViewModel> Properties { get; set; } = new();
        public CategoryStripViewModel CategoryStrip { get; set; } = new();  // Все категории для вывода в панели фильтра
        public PropertyFilterViewModel Filter { get; set; } = new();
        public int TotalCount { get; set; }     // Общее количество найденных квартир
    }
}
