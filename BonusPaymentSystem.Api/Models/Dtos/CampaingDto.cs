using BonusPaymentSystem.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Models.Dtos
{
    public class CampaingDto
    { 
        public Campaing Campaing { get; set; }
        public IEnumerable<Sale> Sales { get; set; }
    }
}
