using BonusPaymentSystem.Core.Constants;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service.Interfaces;
using BonusPaymentSystem.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserApplicationService _userService;

        public UserController(UserManager<IdentityUser> userManager, IUserApplicationService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        // GET: UserController
        public ActionResult Index()
        {
            return View(_userService.Get());
        }

        [Authorize(Roles = "Admin")]
        // GET: UserController/Details/5
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            return View(_userService.Get(id));
        }

        [Authorize(Roles = "Admin")]
        // GET: UserController/Create
        public ActionResult Create()
        {
            return View(new UserViewModel());
        }

        [Authorize(Roles = "Admin")]
        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel modalUser)
        {
            if (modalUser == null || string.IsNullOrEmpty(modalUser.UserName) || string.IsNullOrEmpty(modalUser.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Favor completar todos los campos requeridos");
                return View(modalUser);
            }
            try
            {
                if (_userService.Get().Any(p => p.EmployeeCode == modalUser.EmployeeCode || p.UserName == modalUser.UserName))
                {
                    ModelState.AddModelError(string.Empty, "Usuario o Codigo de empleado no disponible, tomado por otro usuario");
                    return View(modalUser);
                }


                var user = new ApplicationUser
                {
                    UserName = modalUser.UserName,
                    BankAccount = modalUser.BankAccount,
                    Email = modalUser.UserName,
                    NormalizedEmail = modalUser.UserName.ToUpper(),
                    AccessFailedCount = 0,
                    EmailConfirmed = true,
                    EmployeeCode = modalUser.EmployeeCode,
                    FirstName = modalUser.FirstName,
                    LastName = modalUser.LastName,
                    LockoutEnabled = false,
                    Status = (int)Status.ACTIVE,
                    NormalizedUserName = modalUser.UserName.ToUpper(),
                    PhoneNumber = modalUser.PhoneNumber                   
                };

                var result = await _userManager.CreateAsync(user, modalUser.PasswordHash);


                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, $"Usuario { modalUser.UserName } no fue creado, favor intente nuevamente");
                    return View(modalUser);
                }

                _userService.Update(user);

                await _userManager.AddToRoleAsync(user, modalUser.RolName);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error! procesando su solicitud");
                return View(modalUser);
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            return View(_userService.Get(id));
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, ApplicationUser modalUser)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            try
            {
                var user = _userService.Get(id);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuario no encontrado");
                    return View(modalUser);
                }

                if (_userService.Get().Any(p => p.EmployeeCode == modalUser.EmployeeCode && p.UserName != modalUser.UserName))
                {
                    ModelState.AddModelError(string.Empty, "Codigo de empleado no disponible, tomado por otro usuario" + modalUser.EmployeeCode);
                    return View(modalUser);
                }

                if (modalUser.UserName != User.Identity.Name)
                {
                    ModelState.AddModelError(string.Empty, "Este usuario no es el mismo que el usuario logeado");
                    return View(modalUser);
                }

                user.FirstName = modalUser.FirstName;
                user.LastName = modalUser.LastName;
                user.EmployeeCode = modalUser.EmployeeCode;
                user.PhoneNumber = modalUser.PhoneNumber;
                user.Status = modalUser.Status;

                _userService.Update(user);


               return RedirectToAction("Index","Home");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error! procesando su solicitud");
                return  View(modalUser);
            }
        }

        public ActionResult ChangePassword(string id)
        {
            return View(new ChangePasswordViewModel() { UsernName = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrEmpty(model.UsernName)|| string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword))
            {
                ModelState.AddModelError(string.Empty, "Usuario en blanco");
                return View(model);
            }

            if (model.NewPassword != model.NewPasswordConfirm)
            {
                ModelState.AddModelError(string.Empty, "Contraseña nueva no coincide con confirmar contraseña nueva");
                return View(model);
            }

            if (model.UsernName != User.Identity.Name)
            {
                ModelState.AddModelError(string.Empty, "Este usuario no es el mismo que el usuario logeado");
                return View(model);
            }
            try
            {

                var user = await _userManager.FindByNameAsync(model.UsernName);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuario no encontrado");
                    return View(model);
                }

                if (!(await _userManager.CheckPasswordAsync(user, model.CurrentPassword)))
                {
                    ModelState.AddModelError(string.Empty, "Contraseña actual incorrecta");
                    return View(model);
                }

                await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                return RedirectToAction("Edit", new { id = model.UsernName });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error procesando su solicitud");
                return View(model);
            }

        }
    }
}
