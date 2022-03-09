using BonusPaymentSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Models
{
    public class UserCampaingViewModel 
    {
        public UserCampaingViewModel()
        {
            UserAvailableForCampaing = new List<ApplicationUser>();
            UserAvailableInCampaing = new List<ApplicationUser>();
        }
        public int CampaingId { get; set; }
        public IEnumerable<ApplicationUser> UserAvailableForCampaing { get; set; }
        public IEnumerable<ApplicationUser> UserAvailableInCampaing { get; set; }
    }
}
