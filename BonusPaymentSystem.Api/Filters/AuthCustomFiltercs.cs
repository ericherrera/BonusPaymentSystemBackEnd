using BonusPaymentSystem.Commons.Securities;
using BonusPaymentSystem.Service;
using BonusPaymentSystem.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Filters
{
    public class AuthCustomFiltercs : ActionFilterAttribute ,IAuthorizationFilter
    {
        private readonly IUserService _userService;

        public AuthCustomFiltercs()
        {
            _userService = new UserService("Server = LAP0301TRD028; Database = BonusPaymentSystem; Trusted_Connection = True; MultipleActiveResultSets = true");
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (context.HttpContext.User != null && string.IsNullOrEmpty(context.HttpContext.User.Identity.Name))
            { 
                var userName = context.HttpContext.Request.Headers["userName"];
                var password = context.HttpContext.Request.Headers["password"];

                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                {
                    if (string.IsNullOrEmpty(userName))
                        context.ModelState.AddModelError("userName", "UserName is Required");

                    if (string.IsNullOrEmpty(password))
                        context.ModelState.AddModelError("password", "Password is Required");


                    context.Result = new BadRequestObjectResult(context.ModelState);
                }
                else
                {
                    var user = _userService.Get(userName);
                     
                    if (user != null && HashConverter.VerifyPassword(password, user.PasswordHash))
                    {
                        var myIdentity = new GenericIdentity(userName);

                        String[] myStringArray = { "Manager" };
                        var myPrincipal = new GenericPrincipal(myIdentity, myStringArray);

                        context.HttpContext.User = myPrincipal;
                        context.RouteData.Values.Add("User", user);
                    }
                    else
                        context.Result = new BadRequestObjectResult("User or password invalid");
                }
            }

        }
    }
}
