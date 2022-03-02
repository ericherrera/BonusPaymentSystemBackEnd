using BonusPaymentSystem.Api.Common;
using BonusPaymentSystem.Api.Models.Constants;
using BonusPaymentSystem.Api.Models.Dtos.Responses;
using BonusPaymentSystem.Core.Constants;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service.Interfaces;
using BonusPaymentSystem.AfiHogarApi.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BonusPaymentSystem.AfiHogarApi.Models.Requets;
using BonusPaymentSystem.Api.Models.Dtos.Resquests;
using Microsoft.AspNetCore.Authorization;
using BonusPaymentSystem.Api.Models;

namespace BonusPaymentSystem.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class PaymentController : ControllerBase
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

        // GET: PaymentController
        [HttpGet]
        public ActionResult Get([FromQuery] int id, [FromHeader] int pageNumber = 1, [FromHeader] int pageSize = 1)
        {

            if (id > 0)
                return Ok(_paymentService.Get(id));

            long pageTotal = _paymentService.Count();

            pageSize = pageSize <= PageListHelper.PAGE_MAX_SIZE_VALUE ? pageSize : PageListHelper.PAGE_MAX_SIZE_VALUE;


            PageListHelper.PageHeaders[PageListHelper.PAGE_NUMBER] = pageNumber.ToString();
            PageListHelper.PageHeaders[PageListHelper.PAGE_SIZE] = pageSize.ToString();
            PageListHelper.PageHeaders[PageListHelper.TOTAL_ITEM] = pageTotal.ToString();
            PageListHelper.PageHeaders[PageListHelper.PAGE_MAX_SIZE] = PageListHelper.PAGE_MAX_SIZE_VALUE.ToString();

            PageListHelper.SetHeaderParamPage(Response.Headers);

            var result = _paymentService.Get(
                            pageSize: pageSize,
                            pageNumber: pageNumber);

            return Ok(result);
        }

        // POST: PaymentController
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PaymentDtoRequest[] payments)
        {

            var response = new PaymentResponse();
            ApiHogarHelper afiHogarClient = new ApiHogarHelper();

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


            var accessToken = await afiHogarClient.GetAccessToken(accept: afiAccept, grantType: afiGrantType, contentType: afiContentType, secret: afiSecret, url: afiTokenURL);

            if (accessToken == null || string.IsNullOrEmpty(accessToken.Token))
            {
                return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, "AFI hogar accessToken failed!");
            }

            var userToken = await afiHogarClient.GetUserToken(usrName: afiUser, pass: afiPassword, accessTk: accessToken.Token,
                                                              password: afiPasswordField, accept: afiAccept, contentType: afiContentType, url: afiUserTokenURL);


            if (userToken == null || string.IsNullOrEmpty(userToken.AccessToken) )
            {
                return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, "AFI hogar userToken failed!");
            }

            foreach (var payment in payments)
            {
                var shouldPayoff = true;
                var localResponse = new PaymentDto();

                try
                {
                    StringBuilder message = new StringBuilder(" ");

                    //Identificar la persona 
                    var payerUser = _userService.Get(p => p.UserName == payment.UserName).SingleOrDefault();
                    if (payerUser == null)
                    {
                        localResponse.PaymentStatusList.Add(new BaseResponse
                        {
                            Message = message.Append("the payer user doesn't exist or some data missing!").ToString(),
                            Status = HtmlStatusCode.CLIENT_ERROR_CONFLICT
                        });

                        response.PaymentList.Add(localResponse);
                        continue;
                    }

                    //Identificar la venta para ser pagada 
                    var sale = _saleService.Get(payment.SaleId);
                    if (sale == null || sale.State == ((int)Status.INACTIVE))
                    {
                        localResponse.PaymentStatusList.Add(new BaseResponse
                        {
                            Message = message.Append("the sale doesn't exist or sale is inactive!").ToString(),
                            Status = HtmlStatusCode.CLIENT_ERROR_CONFLICT
                        });

                        response.PaymentList.Add(localResponse);
                        continue;
                    }
                    
                    //Validar si la venta ya fue pagada
                    var localPayment = _paymentService.Get(p => p.SaleIdFinal == sale.Id);
                    if (localPayment.Any())
                    {
                        localResponse.PaymentStatusList.Add(new BaseResponse
                        {
                            CampaingId = localPayment.FirstOrDefault().CampaingId,
                            PaymentId = localPayment.FirstOrDefault().Id,
                            SaleId = localPayment.FirstOrDefault().SaleIdFinal,
                            SallerId = localPayment.FirstOrDefault().UserId,
                            ReferenceCode = localPayment.FirstOrDefault().ReferenceCode,
                            Message = message.Append("the payment was payoff in " + localPayment.FirstOrDefault().PaymentDate.ToString("dd-MM-yyy hh:mm:ss")).ToString(),
                            Status = HtmlStatusCode.CLIENT_ERROR_CONFLICT
                        });

                        response.PaymentList.Add(localResponse);
                        continue;
                    }
                    
                    //Obtener el vendedor
                    var saller = _userService.Get(p => p.Id == sale.UserId).FirstOrDefault();
                    if (saller == null ||  saller.Status == ((int)Status.INACTIVE))
                    {
                        localResponse.PaymentStatusList.Add(new BaseResponse
                        {
                            Message = message.Append("the saller doesn't exist or some data missing!").ToString(),
                            Status = HtmlStatusCode.CLIENT_ERROR_CONFLICT
                        });

                        response.PaymentList.Add(localResponse);
                        continue;
                    }

                    //Obtener la campana que hizo la venta
                    var campaing = _campaingService.Get(payment.CampaingId);
                    if (campaing == null || campaing.EnddDate < DateTimeOffset.Now || campaing.State == ((int)Status.INACTIVE))
                    {
                        localResponse.PaymentStatusList.Add(new BaseResponse
                        {
                            Message = message.Append("the campaing doesn't exist or some data missing!").ToString(),
                            Status = HtmlStatusCode.CLIENT_ERROR_CONFLICT
                        });

                        response.PaymentList.Add(localResponse);
                        continue;
                    }

                    //calculo de pago de comision
                    var profit = (sale.Amount / campaing.Amount) * campaing.Amount * campaing.ProfitRate;

                    //Payment throught AfiHogar
                    if (shouldPayoff)
                    {
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

                        var result = await afiHogarClient.CreateTransfer(request,  afitransferUrl, userToken.AccessToken,accessToken.Token);

                        bool isPaymentOk = result.Header.HttpStatusCode == HtmlStatusCode.SUCCESSFUL_OK && 
                                           result.Data.Status == afiStatusTransfer;

                        if (isPaymentOk)
                        {
                            var payoff = new Payment
                            {
                                 CampaingId = payment.CampaingId,
                                 SaleId = saller.Id,
                                 SaleIdFinal = sale.Id,
                                 PaymentDate = DateTime.Now,
                                 Amount = profit.Value,
                                 ReferenceCode = result.Data.TransactionId,
                                 SavingAccountFrom = campaing.SavingAccountSource,
                                 SavingAccountTo = saller.BankAccount,
                                 State = (int)Status.PAYOFF,
                                 UserId = payerUser.Id,
    
                            };

                            _paymentService.Add(payoff);

                            localResponse.PaymentStatusList.Add(new BaseResponse
                            {  
                                CampaingId = payment.CampaingId,
                                PaymentId = payoff.Id,
                                SaleId = sale.Id,
                                SallerId = sale.UserId,
                                ReferenceCode = result.Data.TransactionId,
                                Message = message.Append("created succesfully!").ToString(),
                                Status = HtmlStatusCode.SUCCESSFUL_CREATED
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    localResponse.PaymentStatusList.Add(new BaseResponse
                    {
                        CampaingId = payment.CampaingId,
                        SaleId = payment.SaleId,
                        Message = "error! proccesing Payment [" + ex.Message +  "]",
                        Status = HtmlStatusCode.SERVER_ERROR_INTERNAL
                    });
                }

                response.PaymentList.Add(localResponse);
            }

            return Ok(response);
        }

        // Put: PaymentController
        [HttpPut]
        public ActionResult Edit([FromBody] Payment payment)
        {
            try
            {
                var response = new ResponseDto();
                StringBuilder message = new StringBuilder(payment.GetType().Name + " [" + payment.Id + "]");

                var item = _paymentService.Get(payment.Id);
                if (item == null || item.State == ((int)Status.PAYOFF))
                {
                    response.Messsage = message.Append(" doesn't exist or payment is off!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }

                var payerUser = _userService.Get(item.UserId);
                if (payerUser == null)
                {
                    response.Messsage = message.Append(" the payer user doesn't exist or some data missing!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }

                var sale = _saleService.Get(item.SaleIdFinal);
                if (sale == null)
                {
                    response.Messsage = message.Append(" the sale doesn't exist or sale payoff!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }

                var campaing = _campaingService.Get(sale.CampaingId.Value);
                if (campaing == null || campaing.EnddDate < DateTimeOffset.Now || campaing.State == ((int)Status.INACTIVE))
                {
                    response.Messsage = message.Append(" the campaing doesn't exist or some data missing!").ToString();
                    return StatusCode(HtmlStatusCode.CLIENT_ERROR_CONFLICT, response);
                }



                item.Amount = payment.Amount;
                item.ReferenceCode = payment.ReferenceCode;
                item.SavingAccountTo = payment.SavingAccountTo;
                item.SavingAccountFrom = payment.SavingAccountFrom;

                _paymentService.Update(item);
                response.Messsage = message.Append(" updated successfully!").ToString();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(HtmlStatusCode.SERVER_ERROR_INTERNAL, ex);
            }
        }

    }
}
