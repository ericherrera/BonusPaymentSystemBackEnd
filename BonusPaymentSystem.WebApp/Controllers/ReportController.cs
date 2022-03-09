using BonusPaymentSystem.Core.Constants;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service.Interfaces;
using BonusPaymentSystem.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly IGenericService<Campaing> _campaingService;
        private readonly IUserApplicationService _userService;
        private readonly IUserCampaingService _userCampaingService;
        private readonly IGenericService<Sale> _saleService;

        public ReportController(IGenericService<Sale> saleService, IUserApplicationService userService,
                IGenericService<Campaing> campaingService, IUserCampaingService userCampaingService)
        {
            _campaingService = campaingService;
            _userService = userService;
            _userCampaingService = userCampaingService;
            _saleService = saleService;
        }

        public ActionResult ReportByCampaing()
        {
            var model = new CampaingReportModelView
            {
                 Campaings = _campaingService.Get().ToDictionary(x => x.Id, y => y.Name),
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult ReportByCampaing(CampaingReportModelView model)
        {
            try
            {
                var campaing = _campaingService.Get(model.Campaing.Id);

                model.Campaings = _campaingService.Get().ToDictionary(x => x.Id, y => y.Name);
                model.Campaing = campaing;

                model.Sales = _saleService.Get(p => p.CampaingId == model.Campaing.Id);
            }
            catch 
            {
                ModelState.AddModelError(string.Empty, "Error! procesando su solicitud");
            }

            return View(model);
        }

        public ActionResult ReportBySeller()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReportBySeller(int id)
        {
            return View();
        }
    }
}
