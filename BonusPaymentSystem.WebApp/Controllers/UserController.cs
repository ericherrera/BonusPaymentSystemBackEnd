using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using BonusPaymentSystem.Service.Interfaces;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.WebApp.Models.Dtos;

namespace BonusPaymentSystem.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService clientService)
        {
            _userService = clientService;
        }

        // GET: UserController
        [HttpGet]
        public ActionResult Get()
        {
            return View(_userService.Get());
        }

        [HttpGet]
        public ActionResult Get(string userName)
        {
            return View(_userService.Get(userName));
        }

        // GET: UserController/Create
        [HttpPost]
        public ActionResult Create([FromBody] UserDto user)
        {

            try
            {
                var userCreated = new User
                {
                    UserName = user.UserName,
                    BankAccount = user.BankAccount,
                    EmployeeCode = user.EmployeeCode,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = string.IsNullOrEmpty(user.PhoneNumber),
                    RoleId = user.RolId,
                    Email = user.Email,
                    EmailConfirmed = true,
                };

                _userService.Add(userCreated);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(501, ex);
            }
        }


        // GET: UserController/Edit/5
        [HttpPut]
        public ActionResult Edit([FromBody] UserDto user)
        {
            try
            {
               var userToEdit =  _userService.Get(user.UserName);

               userToEdit.UserName = user.UserName;
               userToEdit.BankAccount = user.BankAccount;
               userToEdit.EmployeeCode = user.EmployeeCode;
               userToEdit.FirstName = user.FirstName;
               userToEdit.LastName = user.LastName;
               userToEdit.PhoneNumber = user.PhoneNumber;
               userToEdit.PhoneNumberConfirmed = string.IsNullOrEmpty(user.PhoneNumber);
               userToEdit.RoleId = user.RolId;
               userToEdit.Email = user.Email;
               userToEdit.Status = user.Status;

                _userService.Update(userToEdit);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(501, ex);
            }
        }
    }
}
