namespace Renty.Web.Models.InputModels.Properties
{
    public class BookingSettingsInputModel
    {
        // false = сначала подтверждаем бронирования вручную, true = мгновенное бронирование
        public bool InstantBookEnabled { get; set; } = false;
    }
}
