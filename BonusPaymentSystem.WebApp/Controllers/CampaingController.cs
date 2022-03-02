using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.WebApp.Controllers
{
    public class CampaingController : Controller
    {

        // GET: CampaingController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CampaingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CampaingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CampaingController/Create
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

        // GET: CampaingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CampaingController/Edit/5
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

        // GET: CampaingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CampaingController/Delete/5
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
