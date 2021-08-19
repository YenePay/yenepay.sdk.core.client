using System;
using System.Collections.Generic;
using System.Text;

namespace YenePay.SDK.Core.Client.Configuration
{
    public class YenePayConfiguration
    {
        public string AccessToken { get; set; }
        public string SigningKey { get; set; }
        public string CustomerCode { get; set; }
        public string KeyType { get; set; } = "pkcs1";
        public bool KeyPemFormatted { get; set; } = true;
    }
}
