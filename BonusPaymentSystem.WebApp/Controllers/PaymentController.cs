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
    public class PaymentController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGenericService<Sale> _saleService;
        private readonly IGenericService<Payment> _paymentService;
        private readonly IGenericService<Campaing> _campaingService;
        private readonly IEnumerable<Parameter> _parameters;

        public PaymentController(IGenericService<Payment> paymentService, IGenericService<Sale> saleService,
                            IUserService userService, IGenericService<Campaing> campaingService, IGenericService<Parameter> parameterService)
        {
            _paymentService = paymentService;
            _saleService = saleService;
            _userService = userService;
            _campaingService = campaingService;
            _parameters = parameterService.Get();
        }

        public ActionResult Index()
        {
            return View(_paymentService.Get());
        }

        // GET: PaymentController/Details/5
        public ActionResult Details(int id)
        {
            if (id > 0)
                return Ok(_paymentService.Get(id));

            return RedirectToAction("Index");
        }

        // GET: PaymentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaymentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PaymentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaymentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PaymentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
