using BonusPaymentSystem.Api.Common;
using BonusPaymentSystem.Api.Models.Constants;
using BonusPaymentSystem.Api.Models.Dtos;
using BonusPaymentSystem.Api.Models.Dtos.Responses;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IGenericService<Campaing> _campaingService;
        private readonly IUserService _userService;
        private readonly IGenericService<Sale> _saleService;
        private readonly IGenericService<Payment> _paymentService;

        public ReportController(IGenericService<Sale> saleService, IUserService userService, IGenericService<Campaing> campaingService, IGenericService<Payment> paymentService)
        {
            _campaingService = campaingService;
            _userService = userService;
            _saleService = saleService;
            _paymentService = paymentService;
        }

        // GET: ReportController
        [HttpGet]
        public ActionResult Get([FromQuery] int id)
        {
            var response = new ResponseDto();
            StringBuilder message = new StringBuilder(this.GetType().Name + " [" + id + "]");

            if (id < 0)
            {
                response.Messsage = message.Append(" doesn't exist or some data missing!").ToString();
                return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
            }

            try
            {

                var result = _campaingService.Get(id);

                if (result == null)
                {
                    response.Messsage = message.Append(" doesn't exist or some data missing!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }
                var campaing = new CampaingDto { Campaing = result };

                campaing.Sales = _saleService.Get(p => p.CampaingId == campaing.Campaing.Id);

                return Ok(campaing);
            }
            catch (Exception ex)
            {
                return StatusCode(HtmlStatusCode.SERVER_ERROR_INTERNAL, message.Append(" error getting data! [" + ex.Message + "]").ToString());
            }
        }


    }
}
