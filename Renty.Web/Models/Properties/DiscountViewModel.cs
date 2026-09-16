using Renty.Domain.Models.LookupsTables;

namespace Renty.Web.Models.Properties
{
    public class DiscountViewModel
    {
        public DiscountTypeEnum Type { get; set; }
        public decimal Percentage { get; set; }
        public int? DaysBeforeCheckIn { get; set; }
        public int? MinNights { get; set; }
    }
}
