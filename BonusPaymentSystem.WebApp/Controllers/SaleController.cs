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
    [Authorize]
    public class SaleController : Controller
    {
        private readonly IGenericService<Sale> _saleService;
        private readonly IUserService _userService;
        private readonly IGenericService<Campaing> _campaingService;
        private readonly IGenericService<Payment> _paymentService;

        public SaleController(IGenericService<Sale> saleService, IUserService userService, IGenericService<Campaing> campaingService, IGenericService<Payment> paymentService)
        {
            _saleService = saleService;
            _userService = userService;
            _campaingService = campaingService;
            _paymentService = paymentService;
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
            return View(new Campaing());
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

                if (model.Rate < campaing.MinAllowedRate || model.Amount < campaing.MinAllowedAmount || model.Term < campaing.MinTerm ||
                    model.Rate > campaing.MaxAllowedRate || model.Amount > campaing.Amount || model.Term > campaing.MaxTerm)
                {
                    ModelState.AddModelError(string.Empty, "Debe cumplir con los montos / plazos maximo y minimos de la Campaña!");
                    return View(model);
                }

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


                var localPayment = _paymentService.Get(p => p.SaleId == sale.Id && p.State == ((int)Status.PAYOFF));
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
