using BonusPaymentSystem.Api.Common;
using BonusPaymentSystem.Api.Models.Constants;
using BonusPaymentSystem.Api.Models.Dtos.Responses;
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
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CampaingController : ControllerBase
    {
        private readonly IGenericService<Campaing> _campaingServiceg;
        private readonly IUserService _userService;

        public CampaingController(IGenericService<Campaing> campaingServiceg, IUserService userService)
        {
            _campaingServiceg = campaingServiceg;
            _userService = userService;
        }

        // GET: CampaingController
        [HttpGet]
        public ActionResult Get([FromQuery] int id, [FromHeader] int pageNumber = 1, [FromHeader] int pageSize = 1)
        {

            if (id > 0)
                return Ok(_campaingServiceg.Get(id));

            long pageTotal = _campaingServiceg.Count();

            pageSize = pageSize <= PageListHelper.PAGE_MAX_SIZE_VALUE ? pageSize : PageListHelper.PAGE_MAX_SIZE_VALUE;


            PageListHelper.PageHeaders[PageListHelper.PAGE_NUMBER] = pageNumber.ToString();
            PageListHelper.PageHeaders[PageListHelper.PAGE_SIZE] = pageSize.ToString();
            PageListHelper.PageHeaders[PageListHelper.TOTAL_ITEM] = pageTotal.ToString();
            PageListHelper.PageHeaders[PageListHelper.PAGE_MAX_SIZE] = PageListHelper.PAGE_MAX_SIZE_VALUE.ToString();

            PageListHelper.SetHeaderParamPage(Response.Headers);

            var result = _campaingServiceg.Get(
                            pageSize: pageSize,
                            pageNumber: pageNumber);

            return Ok(result);
        }

        // POST: CampaingController
        [HttpPost]
        public ActionResult Create([FromBody] Campaing campaing)
        {
            try
            {
                var response = new ResponseDto();
                StringBuilder message = new StringBuilder(campaing.GetType().Name + " [" + campaing.Name + "]");
                if (_campaingServiceg.Exist(campaing.Id) || campaing.Id > 0)
                {
                    response.Messsage = message.Append(" already exist!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_NOT_ACCEPTABLE, response);
                }

                var item = _campaingServiceg.Get(p => p.Name.ToUpper() == campaing.Name.ToUpper());
                if (item != null && item.Any())
                {
                    response.Messsage = message.Append(" doesn't exist or some data missing!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }

                _campaingServiceg.Add(campaing);

                response.Messsage = message.Append(" created succesfully!").ToString();
                return Created("/", response);
            }
            catch (Exception ex)
            {
                return StatusCode(HtmlStatusCode.SERVER_ERROR_INTERNAL, ex);
            }

        }

        // Put: CampaingController
        [HttpPut]
        public ActionResult Edit([FromBody] Campaing campaing)
        {
            try
            {
                var response = new ResponseDto();
                StringBuilder message = new StringBuilder(campaing.GetType().Name + " [" + campaing.Name + "]");

                var item = _campaingServiceg.Get(campaing.Id);
                var user = _userService.Get(p =>  p.Id == campaing.UpdatedById).FirstOrDefault();


                if (item == null || user == null || item.EnddDate < DateTimeOffset.Now)
                {
                    response.Messsage = message.Append(" doesn't exist or some data missing!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }

                item.Amount = campaing.Amount;
                item.EnddDate = campaing.EnddDate;
                item.StartedDate = campaing.StartedDate;
                item.MaxAllowedRate = campaing.MaxAllowedRate;
                item.MaxTerm = campaing.MaxTerm;
                item.MinTerm = campaing.MinTerm;
                item.Name = campaing.Name;
                item.UpdatedOn = DateTime.Now;
                item.UpdatedById = campaing.UpdatedById;
                item.ProfitRate = campaing.ProfitRate;

                _campaingServiceg.Update(item);
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
