namespace Renty.Web.Models.InputModels.Properties
{
    public class LocationVisibilityInputModel
    {
        // показывать гостям точную метку или примерный район, пока гость не забронирует
        public bool ShowExactLocation { get; set; } = true;
    }
}
