using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Helpers
{
    static public class StripeAmountConverter
    {
        private static readonly HashSet<string> ZeroDecimal = new(StringComparer.OrdinalIgnoreCase)
    { "BIF","CLP","DJF","GNF","JPY","KMF","KRW","MGA","PYG","RWF","UGX","VND","VUV","XAF","XOF","XPF" };

        private static readonly HashSet<string> ThreeDecimal = new(StringComparer.OrdinalIgnoreCase)
    { "BHD","JOD","KWD","OMR","TND" };

        // Переводит в самое меньшее значение валюты
        public static long ToStripeAmount(decimal amount, string currency)
        {
            // У валют нет копеек
            if (ZeroDecimal.Contains(currency))
                return (long)Math.Round(amount, 0, MidpointRounding.AwayFromZero);

            // 3 знака, Stripe требует, чтобы последняя цифра была нулём
            if (ThreeDecimal.Contains(currency))
            {
                var thousandths = Math.Round(amount * 1000m, 0, MidpointRounding.AwayFromZero);
                return (long)(Math.Round(thousandths / 10m, 0, MidpointRounding.AwayFromZero) * 10);
            }

            // (USD, EUR, UAH) 2 знака
            return (long)Math.Round(amount * 100m, 0, MidpointRounding.AwayFromZero);
        }
        public static decimal FromStripeAmount(long amount, string currency)
        {
            if (ZeroDecimal.Contains(currency)) return amount;
            if (ThreeDecimal.Contains(currency)) return amount / 1000m;
            return amount / 100m;
        }
    }
}
