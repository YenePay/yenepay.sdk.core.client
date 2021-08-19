using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YenePay.SDK.Core.Client.Example.Models;
using YenePay.SDK.Core.Client.Models;
using YenePay.SDK.Core.Client.Services;

namespace YenePay.SDK.Core.Client.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IYenePayService _yenePayService;

        public HomeController(ILogger<HomeController> logger,
            IYenePayService yenePayService)
        {
            _logger = logger;
            _yenePayService = yenePayService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Send()
        {
            var recipients = new MoneyRecipient[]
            {
                new MoneyRecipient
                {
                    CustomerCode = "<ADD ANOTHER CUSTOMER CODE>",
                    EmailOrPhone = "<ADD THEIR EMAIL ACCOUNT/PHONE NUMBER>",
                    Amount = 10
                },
                new MoneyRecipient
                {
                    CustomerCode = "<ADD ANOTHER CUSTOMER CODE>",
                    EmailOrPhone = "<ADD THEIR EMAIL ACCOUNT/PHONE NUMBER>",
                    Amount = 8
                }
        };
            
            var request = _yenePayService.CreateSignedRequest("Send Money Test", recipients);
            var result = await _yenePayService.SendMoney(request);


            return View(new PaymentModel
            {
                Request = request,
                Response = result
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
