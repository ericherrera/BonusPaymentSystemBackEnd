using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Commons.Utilities
{
    public static class StringTool
    {
        public static bool IsIntegerNumber(this String str)
        {
            return int.TryParse(str, out _);
        }
    }
}
