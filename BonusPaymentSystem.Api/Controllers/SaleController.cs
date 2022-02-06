using BonusPaymentSystem.Api.Common;
using BonusPaymentSystem.Api.Models.Constants;
using BonusPaymentSystem.Api.Models.Dtos.Responses;
using BonusPaymentSystem.Core.Constants;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class SaleController : ControllerBase
    {
        private readonly IGenericService<Sale> _saleService;
        private readonly IUserService _userService;
        private readonly IGenericService<Campaing> _campaingService;
        private readonly IGenericService<Payment> _paymentService;

        public SaleController(IGenericService<Sale> saleService, IUserService userService, IGenericService<Campaing> campaingService, IGenericService<Payment> paymentService )
        {
            _saleService = saleService;
            _userService = userService;
            _campaingService = campaingService;
            _paymentService = paymentService;
        }

        // GET: SaleController
        [HttpGet]
        public ActionResult Get([FromQuery] int id, [FromHeader] int pageNumber = 1, [FromHeader] int pageSize = 1)
        {

            if (id > 0)
                return Ok(_saleService.Get(id));

            long pageTotal = _saleService.Count();

            pageSize = pageSize <= PageListHelper.PAGE_MAX_SIZE_VALUE ? pageSize : PageListHelper.PAGE_MAX_SIZE_VALUE;


            PageListHelper.PageHeaders[PageListHelper.PAGE_NUMBER] = pageNumber.ToString();
            PageListHelper.PageHeaders[PageListHelper.PAGE_SIZE] = pageSize.ToString();
            PageListHelper.PageHeaders[PageListHelper.TOTAL_ITEM] = pageTotal.ToString();
            PageListHelper.PageHeaders[PageListHelper.PAGE_MAX_SIZE] = PageListHelper.PAGE_MAX_SIZE_VALUE.ToString();

            PageListHelper.SetHeaderParamPage(Response.Headers);

            var result = _saleService.Get(
                            pageSize: pageSize,
                            pageNumber: pageNumber);

            return Ok(result);
        }

        // POST: SaleController
        [HttpPost]
        public  ActionResult Create([FromBody] Sale sale)
        {
            try
            {
                var response = new ResponseDto();
                StringBuilder message = new StringBuilder(sale.GetType().Name + " [" + sale.Id + "]");
                var user = _userService.Get(p => p.Id == sale.UserId).FirstOrDefault();
                if (user == null)
                {
                    response.Messsage = new StringBuilder(", userId [" + sale.UserId + "] doesn't exist!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_NOT_ACCEPTABLE, response);
                }

                var campaing = _campaingService.Get(sale.CampaingId.Value);
                if (campaing == null)
                {
                    response.Messsage = new StringBuilder(", CampaingId [" + sale.CampaingId.Value + "] doesn't exist!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_NOT_ACCEPTABLE, response);
                }

                if (sale.Rate < campaing.MinAllowedRate || sale.Amount < campaing.MinAllowedAmount || sale.Term < campaing.MinTerm ||
                    sale.Rate > campaing.MaxAllowedRate || sale.Amount > campaing.Amount || sale.Term > campaing.MaxTerm)
                {
                    response.Messsage = new StringBuilder(" is not valid!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }

                sale.CreatedOn = DateTimeOffset.Now;
                sale.UpdatedOn = DateTimeOffset.Now;

                _saleService.Add(sale);

                response.Messsage = message.Append(sale.GetType().Name + " [" + sale.Id + "] created succesfully!").ToString();
                return Created("/", response);
            }
            catch (Exception ex)
            {
                return StatusCode(HtmlStatusCode.SERVER_ERROR_INTERNAL, ex);
            }
        }

        // Put: SaleController
        [HttpPut]
        public ActionResult Edit([FromBody] Sale sale)
        {
            try
            {
                var response = new ResponseDto();
                StringBuilder message = new StringBuilder(sale.GetType().Name + " [" + sale.Id + "]");

                var item = _saleService.Get(sale.Id);

                if (item == null)
                {
                    response.Messsage = message.Append(" doesn't exist!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }

                var user = _userService.Get(p => p.Id == sale.UserId).FirstOrDefault();
                if (user == null)
                {
                    response.Messsage = message.Append(" userId ["+ sale.UserId + "] doesn't exist").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }

                var campaing = _campaingService.Get(item.CampaingId.Value);

                if (item.Rate < campaing.MinAllowedRate || item.Amount < campaing.MinAllowedAmount || item.Term < campaing.MinTerm ||
                    item.Rate > campaing.MaxAllowedRate || item.Amount > campaing.Amount || item.Term > campaing.MaxTerm)
                {
                    response.Messsage = message.Append(" is not valid!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }

                var localPayment = _paymentService.Get(p => p.SaleId == sale.Id && p.State == ((int)Status.PAYOFF));
                if (localPayment.Any())
                {
                    response.Messsage = message.Append(", was payoff! so you can't edit it!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }

                item.Amount = sale.Amount;
                item.Rate = sale.Rate;
                item.Term = sale.Term;
                item.ReferenceCode = sale.ReferenceCode;
                item.UpdatedOn = DateTimeOffset.Now;

                _saleService.Update(item);
                response.Messsage = message.Append(" updated successfully!").ToString();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(HtmlStatusCode.SERVER_ERROR_INTERNAL, ex);
            }
        }
    }
}
