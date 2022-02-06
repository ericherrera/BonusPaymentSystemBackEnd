using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Constants
{
    public enum LoginStatus
    {
        SUCCESS_LOGIN = 1,
        WRONG_PASSWORD = 2,
        LOCKED_USER = 3,
        NOT_FOUND_USER = 4,
        INTERNAL_ERROR = 5,
    }
}
