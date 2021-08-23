using System;
using System.Collections.Generic;
using System.Text;

namespace YenePay.SDK.Core.Client.Configuration
{
    public class Constants
    {
        public const string YenePayApiBaseUrl = "https://endpoint.yenepay.com";
        //public const string YenePayApiBaseUrl = "https://localhost:44327";
        public const string YenePayCheckoutBaseUrl = "https://yenepay.com/checkout";
        public const string AccessTokenConfigurationKey = "YenePayAccessToken";
        public const string SigningKeyConfigurationKey = "YenePaySigningKey";
    }
}
