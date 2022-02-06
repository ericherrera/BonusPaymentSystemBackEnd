using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Models.Security
{
    public class Token
    {
        public string AccessToken { get; set; }
        public int ExpireIn { get; set; }
        public DateTime ExpireFrom { get; set; }
        public DateTime ExpireTo { get; set; }
    }
}
