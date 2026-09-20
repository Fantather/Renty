namespace Renty.Web.Models.InputModels.Properties
{
    public class TagsInputModel
    {
        // максимум 2
        public List<Guid> TagIds { get; set; } = new();
    }
}
