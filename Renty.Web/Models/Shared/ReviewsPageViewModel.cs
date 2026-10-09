namespace Renty.Web.Models.Shared
{
    public class ReviewsPageViewModel
    {
        public List<ReviewViewModel> Reviews { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
