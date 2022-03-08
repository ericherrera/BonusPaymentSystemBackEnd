using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ParameterController : Controller
    {
        private readonly IGenericService<Parameter> _parameterServiceg;
        private readonly IUserApplicationService _userService;

        public ParameterController(IGenericService<Parameter> parameterServiceg, IUserApplicationService userService)
        {
            _parameterServiceg = parameterServiceg;
            _userService = userService;
        }


        // GET: ParameterController
        public ActionResult Index()
        {
            return View(_parameterServiceg.Get());
        }

        // GET: ParameterController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id < 1)
            {
                ModelState.AddModelError(string.Empty, $"El parametro [{ id }] no existe!");
                return View();
            }

            return View(_parameterServiceg.Get(id));
        }

        // POST: ParameterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Parameter model)
        {
            try
            {
                var item = _parameterServiceg.Get(id);

                if (item == null)
                {
                    ModelState.AddModelError(string.Empty, "parametro no existe");
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Valor) )
                {
                    ModelState.AddModelError(string.Empty, "Parmetro no debe estar en blanco!");
                    return View(model);
                }

                item.Valor = model.Valor;
                item.UpdatedBy = _userService.Get(User.Identity.Name).Id;
                item.UpdatedOn = DateTime.Now;

                _parameterServiceg.Update(item);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Ocurrio un error! Procesando su solicitud");
                return Redirect("/Error");
            }
        }

    }
}
