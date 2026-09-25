namespace Renty.Web.Models.PropertyEdit
{
    public record EditorMenuViewModel(Guid PropertyId, IReadOnlyList<EditorMenuItemViewModel> Items);
}
