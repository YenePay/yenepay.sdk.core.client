using Microsoft.Extensions.Configuration;
using RSAExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;

namespace YenePay.SDK.Core.Client.Configuration
{
    public class YenePaySettings
    {
        private readonly IConfiguration _configuration;

        public string AccessToken { get; private set; }
        public string SigningKey { get; private set; }
        public string CustomerCode { get; private set; }
        public RSAKeyType KeyType { get; private set; }
        public bool IsKeyPemFormatted { get; private set; }
        public YenePaySettings(IConfiguration configuration)
        {
            _configuration = configuration;
            var config = new YenePayConfiguration();
            _configuration.GetSection(nameof(YenePayConfiguration)).Bind(config);
            var keyType = RSAKeyType.Pkcs1;
            if ("pkcs8".Equals(config.KeyType, StringComparison.OrdinalIgnoreCase))
            {
                keyType = RSAKeyType.Pkcs8;
            }
            else if("xml".Equals(config.KeyType, StringComparison.OrdinalIgnoreCase))
            {
                keyType = RSAKeyType.Xml;
            }
            KeyType = keyType;
            IsKeyPemFormatted = config.KeyPemFormatted;
            CustomerCode = config.CustomerCode;
            AccessToken = config.AccessToken;
            if (!string.IsNullOrEmpty(config.SigningKey) && File.Exists(config.SigningKey))
            {
                SigningKey = File.ReadAllText(config.SigningKey);
            }
            
        }
    }
}
