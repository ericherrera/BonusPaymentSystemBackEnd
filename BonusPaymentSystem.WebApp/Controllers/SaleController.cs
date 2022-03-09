using BonusPaymentSystem.Core.Constants;
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
    [Authorize(Roles = "Admin,Saller,Payer")]
    public class SaleController : Controller
    {
        private readonly IGenericService<Sale> _saleService;
        private readonly IUserApplicationService _userService;
        private readonly IGenericService<Campaing> _campaingService;
        private readonly IGenericService<Payment> _paymentService;
        private readonly IUserCampaingService _userCampaingService;

        public SaleController(IGenericService<Sale> saleService, IUserApplicationService userService, 
                        IGenericService<Campaing> campaingService, IGenericService<Payment> paymentService,
                        IUserCampaingService userCampaingService)
        {
            _saleService = saleService;
            _userService = userService;
            _campaingService = campaingService;
            _paymentService = paymentService;
            _userCampaingService = userCampaingService;
        }

        public ActionResult Index()
        {
            var saller = _userService.Get(User.Identity.Name);

            return View(_saleService.Get(p => p.UserId == saller.Id));
        }

        // GET: SaleController/Details/5
        public ActionResult Details(int id)
        {
            if (id < 1)
            {
                ModelState.AddModelError(string.Empty, $"La venta [{ id }] no existe!");
                return View();
            }

            return View(_saleService.Get(id));
        }

        // GET: SaleController/Create
        public ActionResult Create()
        {
            return View(new Sale());
        }

        // POST: SaleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sale model)
        {
            if (model == null || model.Term<0 || model.Amount < 0 || model.Id > 0)
            {
                ModelState.AddModelError(string.Empty, "Favor completar todos los campos requeridos correctamente");
                return View(model);
            }
            try
            {

                var campaing = _campaingService.Get(model.CampaingId.Value);
                if (campaing == null )
                {
                    ModelState.AddModelError(string.Empty, "Campaña [" + model.CampaingId + "] no existe!");
                    return View(model);
                }

                if (model.Rate < campaing.MinAllowedRate  || model.Term < campaing.MinTerm || model.Amount < campaing.MinAllowedAmount ||
                    model.Rate > campaing.MaxAllowedRate || model.Term > campaing.MaxTerm)
                {
                    ModelState.AddModelError(string.Empty, "Debe cumplir con los montos / plazos maximo y minimos de la Campaña!");
                    return View(model);
                }

                var user = _userService.Get(User.Identity.Name);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Usted no es un vendedor autorizado!");
                    return View(model);
                }

                if (!_userCampaingService.Exist(user.Id, campaing.Id))
                {
                    ModelState.AddModelError(string.Empty, "Usted no esta asignado a esta campaña!");
                    return View(model);
                }

                model.CreatedOn = DateTimeOffset.Now;
                model.UserId = user.Id;

                _saleService.Add(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error! procesando su solicitud");
                return View(model);
            }
        }

        // GET: SaleController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id < 1)
            {
                ModelState.AddModelError(string.Empty, $"La venta [{ id }] no existe!");
                return View();
            }
            var sale = _saleService.Get(id);

            if (sale  == null || sale.State == ((int)Status.PAYOFF))
            {
                ModelState.AddModelError(string.Empty, $"La venta [{ id }] ya fue pagada!");
                return RedirectToAction(nameof(Index));
            }

            return View(sale);
        }

        // POST: SaleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Sale model)
        {
            if (model == null || model.Term < 0 || model.Amount < 0 || model.Id > 0)
            {
                ModelState.AddModelError(string.Empty, "Favor completar todos los campos requeridos correctamente");
                return View(model);
            }
            try
            {
                var sale = _saleService.Get(id);

                var campaing = _campaingService.Get(model.CampaingId.Value);
                if (campaing == null)
                {
                    ModelState.AddModelError(string.Empty, "Campaña [" + model.CampaingId + "] no existe!");
                    return View(model);
                }

                if (model.Rate < campaing.MinAllowedRate || model.Amount < campaing.MinAllowedAmount || model.Term < campaing.MinTerm ||
                    model.Rate > campaing.MaxAllowedRate || model.Amount > campaing.Amount || model.Term > campaing.MaxTerm)
                {
                    ModelState.AddModelError(string.Empty, "Debe cumplir con los montos / plazos maximo y minimos de la Campaña!");
                    return View(model);
                }


                var localPayment = _paymentService.Get(p => p.SaleIdFinal == sale.Id && p.State == ((int)Status.PAYOFF));
                if (localPayment.Any())
                {
                    ModelState.AddModelError(string.Empty, "Esta venta recibio su bonificación, y no puede ser modificada!");
                    return View(model);
                }


                sale.Amount = model.Amount;
                sale.Rate = model.Rate;
                sale.State = model.State;
                sale.Term = model.Term;
                sale.UpdatedOn = DateTime.Now;

                _saleService.Update(sale);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error! procesando su solicitud");
                return View(model);
            }
        }
    }
}
