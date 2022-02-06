using BonusPaymentSystem.Api.Filters;
using BonusPaymentSystem.Api.Models;
using BonusPaymentSystem.Api.Models.Constants;
using BonusPaymentSystem.Api.Services;
using BonusPaymentSystem.Commons.Securities;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service;
using BonusPaymentSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuracion;

        public AuthController(IConfiguration configuration)
        {
            _configuracion = configuration;
        }


        [HttpPost]
        [Route("Token")]
        [AllowAnonymous]
        public  ActionResult GenerateToken()
        {
            try
            {
                var user = (User) RouteData.Values["User"];

                var key = _configuracion["Jwt:Key"];
                var issuer = _configuracion["Jwt:Issuer"];
                var audience = _configuracion["Jwt:Audience"];
                var subject = _configuracion["Jwt:Subject"];
                int expiredInSeconds = int.Parse(_configuracion["Jwt:ExpireInSeconds"]);

                var token = TokenService.CreateTokenV2(user, key,subject,issuer,audience,expiredInSeconds);
                
                return Ok(token);

            }
            catch (Exception ex)
            {
                return StatusCode(HtmlStatusCode.SERVER_ERROR_INTERNAL, ex);

            }

        }
    }
}
