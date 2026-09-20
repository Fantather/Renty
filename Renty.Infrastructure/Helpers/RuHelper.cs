using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Helpers
{
    public static class RuHelper
    {
        public static bool IsCyrillic(string input)
        {
            foreach (char c in input)
            {
                if ((c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё')
                {
                    return true;
                }
            }
            return false;
        }
    }
}
