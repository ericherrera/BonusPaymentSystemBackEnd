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
    [Authorize]
    public class CampaingController : Controller
    {
            
        private readonly IGenericService<Campaing> _campaingServiceg;
        private readonly IUserApplicationService _userService;

        public CampaingController(IGenericService<Campaing> campaingServiceg, IUserApplicationService userService)
        {
            _campaingServiceg = campaingServiceg;
            _userService = userService;
        }

        // GET: CampaingController
        public ActionResult Index()
        {
            return View(_campaingServiceg.Get());
        }

        // GET: CampaingController/Details/5
        public ActionResult Details(int id)
        {
            if (id < 1)
            {
                ModelState.AddModelError(string.Empty, $"La campaña [{ id }] no existe!");
                return View();
            }

            return View(_campaingServiceg.Get(id));
        }

        // GET: CampaingController/Create
        public ActionResult Create()
        {
            return View(new Campaing());
        }

        // POST: CampaingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Campaing model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || model.EnddDate <= DateTime.Now || model.Id > 0)
            {
                ModelState.AddModelError(string.Empty, "Favor completar todos los campos requeridos correctamente");
                return View(model);
            }
            try
            {

                if (_campaingServiceg.Exist(model.Id))
                {
                    ModelState.AddModelError(string.Empty, $"El id de la campaña [{model.Id}] ya existe");
                    return View(model);
                }

                var item = _campaingServiceg.Get(p => p.Name.ToUpper() == model.Name.ToUpper());
                if (item != null && item.Any())
                {
                    ModelState.AddModelError(string.Empty, $"El nombre de la campaña [{model.Name}] ya existe");
                    return View(model);
                }

                model.CreateOn = DateTime.Now;
                model.CreatedById = _userService.Get(User.Identity.Name).Id;

                _campaingServiceg.Add(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error! procesando su solicitud");
                return View(model);
            }
        }

        // GET: CampaingController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id < 1)
            {
                ModelState.AddModelError(string.Empty, $"La campaña [{ id }] no existe!");
                return View();
            }

            return View(_campaingServiceg.Get(id));
        }

        // POST: CampaingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Campaing model)
        {
            try
            {
                var item = _campaingServiceg.Get(id);

                if (item == null)
                {
                    ModelState.AddModelError(string.Empty, "Campaña no existe");
                    return View(model);
                }

                if (item.EnddDate < DateTimeOffset.Now)
                {
                    ModelState.AddModelError(string.Empty, "Campaña vencida!");
                    return View(model);
                }

                item.Amount = model.Amount;
                item.MaxAllowedRate = model.MaxAllowedRate;
                item.MaxTerm = model.MaxTerm;
                item.MinAllowedAmount = model.MinAllowedAmount;
                item.MinTerm = model.MinTerm;
                item.Name = model.Name;
                item.ProfitRate = model.ProfitRate;
                item.SavingAccountSource = model.SavingAccountSource;
                item.StartedDate = model.StartedDate;

                item.EnddDate = model.EnddDate;
                item.UpdatedById = _userService.Get(User.Identity.Name).Id;
                item.UpdatedOn = DateTime.Now;

                _campaingServiceg.Update(item);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Ocurrio un error procesando su solicitud");
                return Redirect("/Error");
            }
        }

    }
}
