using BonusPaymentSystem.Api.Common;
using BonusPaymentSystem.Api.Models.Constants;
using BonusPaymentSystem.Api.Models.Dtos;
using BonusPaymentSystem.Api.Models.Dtos.Responses;
using BonusPaymentSystem.Commons.Securities;
using BonusPaymentSystem.Commons.Utilities;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Controllers
{

    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public UserController(IUserService userService)
        {
            _userService = userService;
            _passwordHasher = new PasswordHasher<IdentityUser>();
        }

        // GET: UserController
        [HttpGet]
        public ActionResult Get([FromQuery]string userName,[FromHeader] int pageNumber = 1, [FromHeader] int pageSize = 1)
        {

            if (!string.IsNullOrEmpty(userName))
                return Ok(_userService.Get(userName));

            long pageTotal = _userService.Count();

            pageSize = pageSize <= PageListHelper.PAGE_MAX_SIZE_VALUE ? pageSize : PageListHelper.PAGE_MAX_SIZE_VALUE;


            var result = _userService.Get(
                    pageSize: pageSize,
                    pageNumber: pageNumber);

            PageListHelper.PageHeaders[PageListHelper.PAGE_NUMBER] = pageNumber.ToString();
            PageListHelper.PageHeaders[PageListHelper.PAGE_SIZE] = pageSize.ToString();
            PageListHelper.PageHeaders[PageListHelper.TOTAL_ITEM] = pageTotal.ToString();
            PageListHelper.PageHeaders[PageListHelper.PAGE_MAX_SIZE] = PageListHelper.PAGE_MAX_SIZE_VALUE.ToString();

            PageListHelper.SetHeaderParamPage(Response.Headers);

            return Ok(result);
        }

        // GET: UserController/Create
        [HttpPost]
        public ActionResult Create([FromBody] User user)
        {
            try
            {
                var response = new ResponseDto();
                StringBuilder message = new StringBuilder(user.GetType().Name + " [" + user.UserName + "]");
                if (_userService.Exist(user.UserName))
                {
                    response.Messsage =  message.Append(" already exist!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_NOT_ACCEPTABLE, response);
                }

                var userCreated = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = user.UserName,
                    BankAccount = user.BankAccount,
                    EmployeeCode = user.EmployeeCode,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = string.IsNullOrEmpty(user.PhoneNumber),
                    Email = user.Email,
                    EmailConfirmed = true,
                    PasswordHash = HashConverter.ConvertToHash(user.PasswordHash),
                };

                response.Messsage = message.Append(" created succesfully!").ToString();
                return Created( "", userCreated);
            }
            catch (Exception ex)
            {

                return StatusCode(HtmlStatusCode.SERVER_ERROR_INTERNAL, ex);
            }
        }


        // GET: UserController/Edit/5
        [HttpPut]
        public ActionResult Edit([FromBody] User user)
        {
            try
            {
                var response = new ResponseDto();
                StringBuilder message = new StringBuilder(user.GetType().Name + " [" + user.UserName + "]");

                var userToEdit = _userService.Get(user.UserName);

                if (userToEdit == null)
                {
                    response.Messsage = message.Append(" doesn't exist!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }

                userToEdit.BankAccount = user.BankAccount;
                userToEdit.EmployeeCode = user.EmployeeCode;
                userToEdit.FirstName = user.FirstName;
                userToEdit.LastName = user.LastName;
                userToEdit.PhoneNumber = user.PhoneNumber;
                userToEdit.PhoneNumberConfirmed = string.IsNullOrEmpty(user.PhoneNumber);
                userToEdit.Email = user.Email;
                userToEdit.Status = user.Status;
                userToEdit.PasswordHash = string.IsNullOrEmpty(user.PasswordHash) ?  userToEdit.PasswordHash : HashConverter.ConvertToHash(user.PasswordHash);

                response.Messsage = message.Append(" updated successfully!").ToString();
                _userService.Update(userToEdit);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(HtmlStatusCode.SERVER_ERROR_INTERNAL, ex);
            }
        }

    }
}
