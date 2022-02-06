using BonusPaymentSystem.Api.Common;
using BonusPaymentSystem.Api.Models.Constants;
using BonusPaymentSystem.Api.Models.Dtos.Responses;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    public class ParameterController : Controller
    {
        private readonly IGenericService<Parameter> _parameterServiceg;
        private readonly IUserService _userService;

        public ParameterController(IGenericService<Parameter> parameterServiceg, IUserService userService)
        {
            _parameterServiceg = parameterServiceg;
            _userService = userService;
        }

        // GET: CampaingController
        [HttpGet]
        public ActionResult Get([FromQuery] int id, [FromHeader] int pageNumber = 1, [FromHeader] int pageSize = 1)
        {

            if (id > 0)
                return Ok(_parameterServiceg.Get(id));

            long pageTotal = _parameterServiceg.Count();

            pageSize = pageSize <= PageListHelper.PAGE_MAX_SIZE_VALUE ? pageSize : PageListHelper.PAGE_MAX_SIZE_VALUE;


            PageListHelper.PageHeaders[PageListHelper.PAGE_NUMBER] = pageNumber.ToString();
            PageListHelper.PageHeaders[PageListHelper.PAGE_SIZE] = pageSize.ToString();
            PageListHelper.PageHeaders[PageListHelper.TOTAL_ITEM] = pageTotal.ToString();
            PageListHelper.PageHeaders[PageListHelper.PAGE_MAX_SIZE] = PageListHelper.PAGE_MAX_SIZE_VALUE.ToString();

            PageListHelper.SetHeaderParamPage(Response.Headers);

            var result = _parameterServiceg.Get(
                            pageSize: pageSize,
                            pageNumber: pageNumber);

            return Ok(result);
        }

        // POST: CampaingController
        [HttpPost]
        public ActionResult Create([FromBody] Parameter parameter)
        {
            try
            {
                var response = new ResponseDto();
                StringBuilder message = new StringBuilder(parameter.GetType().Name + " [" + parameter.Name + "]");
                if (_parameterServiceg.Exist(parameter.Id))
                {
                    response.Messsage = message.Append(" already exist!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_NOT_ACCEPTABLE, response);
                }

                _parameterServiceg.Add(parameter);

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
        public ActionResult Edit([FromBody] Parameter parameter)
        {
            try
            {
                var response = new ResponseDto();
                StringBuilder message = new StringBuilder(parameter.GetType().Name + " [" + parameter.Name + "]");

                var item = _parameterServiceg.Get(parameter.Id);

                if(_userService.Get(p => p.Id == parameter.UpdatedBy).FirstOrDefault() == null)
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, new { message = "User UpdatedBy doesn't exist"});

                item.DataType = parameter.DataType;
                item.Label = parameter.Label;
                item.State = parameter.State;
                item.Valor = parameter.Valor;
                item.UpdatedBy = parameter.UpdatedBy;
                item.UpdatedOn = DateTimeOffset.Now;

                _parameterServiceg.Update(item);
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
