using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YenePay.SDK.Core.Client.Models;

namespace YenePay.SDK.Core.Client.Example.Models
{
    public class PaymentModel
    {
        public SendMoneyRequest Request { get; set; }
        public SendMoneyResult Response { get; set; }
    }
}
