using BonusPaymentSystem.AfiHogarApi.Clients;
using BonusPaymentSystem.AfiHogarApi.Models.Requets;
using BonusPaymentSystem.Commons.Utilities;
using BonusPaymentSystem.Core.Constants;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service.Interfaces;
using BonusPaymentSystem.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            return View(_saleService.Get());
        }


        // GET: PaymentController/Details/5
        public async Task<ActionResult> Pay(int id)
        {
            try
            {
                var afiUser = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.AfiUser).Valor;
                var afiPassword = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.AfiPassword).Valor;
                var afitransferUrl = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.AfitransferUrl).Valor;
                var afiGrantType = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.AfiGrantType).Valor;
                var afiAccept = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.AfiAccept).Valor;
                var afiPasswordField = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.AfiPasswordField).Valor;
                var afiContentType = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.AfiContentType).Valor;
                var afiSecret = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.AfiSecret).Valor;
                var afiTokenURL = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.AfiTokenURL).Valor;
                var afiUserTokenURL = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.AfiUserTokenURL).Valor;
                var afiStatusTransfer = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.AfiStatusTransfer).Valor;
                var currency = _parameters.FirstOrDefault(p => p.Name == ParameterNameConst.Currency).Valor;


                var sale = _saleService.Get(id);
                var campaing = _campaingService.Get(sale.CampaingId.Value);
                var saller = _userService.Get(sale.UserId);
                var profit = BonusCalculation.CalcBonusPayment(sale.Amount.Value, campaing.Amount, campaing.ProfitRate.Value);
                var payerUser = _userService.Get(User.Identity.Name);


                ApiHogarHelper afiHogarClient = new ApiHogarHelper();
                var accessToken = await afiHogarClient.GetAccessToken(accept: afiAccept, grantType: afiGrantType, contentType: afiContentType, secret: afiSecret, url: afiTokenURL);

                if (accessToken == null || string.IsNullOrEmpty(accessToken.Token))
                {
                    ModelState.AddModelError(string.Empty, "Error conectando Apis de AFIHogar");
                    return View();
                }

                var userToken = await afiHogarClient.GetUserToken(usrName: afiUser, pass: afiPassword, accessTk: accessToken.Token,
                                                                  password: afiPasswordField, accept: afiAccept, contentType: afiContentType, url: afiUserTokenURL);


                if (userToken == null || string.IsNullOrEmpty(userToken.AccessToken))
                {
                    ModelState.AddModelError(string.Empty, "AFI hogar userToken fallido!");
                    return View();
                }

                var request = new TransferRequest();
                request.Initiation.InstructedAmount = new InstructedAmount
                {
                    Amount = profit.ToString(),
                    Currency = currency
                };

                request.Initiation.DebtorAccount = new DebtorAccount
                {
                    Identification = campaing.SavingAccountSource,
                };

                request.Initiation.CreditorAccount = new CreditorAccount
                {
                    Identification = saller.BankAccount,
                };

                var result = afiHogarClient.CreateTransfer(request, afitransferUrl, userToken.AccessToken, accessToken.Token).Result;

                bool isPaymentOk = result.Header.HttpStatusCode == (int)HttpStatusCode.OK &&
                                   result.Data.Status == afiStatusTransfer;

                if (isPaymentOk)
                {
                    var payoff = new Payment
                    {
                        CampaingId = campaing.Id,
                        SaleId = sale.Id,
                        PaymentDate = DateTime.Now,
                        Amount = profit,
                        ReferenceCode = result.Data.TransactionId,
                        SavingAccountFrom = campaing.SavingAccountSource,
                        SavingAccountTo = saller.BankAccount,
                        State = (int)Status.PAYOFF,
                        UserId = payerUser.Id,

                    };

                    _paymentService.Add(payoff);

                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error! procesando su solicitud, favor intente de nuevo o consulte con el admninistrador");
                return View();
            }


            return RedirectToAction("Index");
        }

    }
}
