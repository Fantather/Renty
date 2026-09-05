namespace Renty.Web.Models.Properties
{
    // Комментарий гостя к одной из категорий оценки (Чистота, Расположение и т.д.)
    public class ReviewCategoryCommentViewModel
    {
        public string CategoryName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
    }
}
