namespace Renty.Application.Extensions
{
    public static class GenerationExtensions
    {
        // 1995 → "90"
        public static string ToDecade(this DateOnly dateOfBirth) => (dateOfBirth.Year / 10 * 10 % 100).ToString("00");
    }
}
