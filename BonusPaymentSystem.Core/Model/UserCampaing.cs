using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Core.Model
{
    [Table("UserCampaings")]
    public class UserCampaing
    {
        public int CampaingId { get; set; }
        public string SallerId { get; set; }
    }
}
