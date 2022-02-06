using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Models.Dtos.Resquests
{
    public class PaymentDtoRequest
    {
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "SaleId is Required")]
        public int SaleId { get; set; }
        [Required(ErrorMessage = "CampaingId is Required")]
        public int CampaingId { get; set; }

    }
}
