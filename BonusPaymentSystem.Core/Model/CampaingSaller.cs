using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    public class CampaingSaller
    {
        public int Id { get; set; }
        public int CampaingId { get; set; }
        public string CreatedById { get; set; }
        public DateTimeOffset CreateOn { get; set; }
        public int Status { get; set; }
    }
}
