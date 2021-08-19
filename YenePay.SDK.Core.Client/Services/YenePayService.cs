using RSAExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YenePay.SDK.Core.Client.Configuration;
using YenePay.SDK.Core.Client.Models;

namespace YenePay.SDK.Core.Client.Services
{
    public class YenePayService : IYenePayService
    {
        private readonly HttpClient _httpClient;
        private readonly YenePaySettings _yenePaySettings;

        private readonly JsonSerializerOptions _serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public YenePayService(HttpClient httpClient,
            YenePaySettings yenePaySettings)
        {
            _httpClient = httpClient;
            _yenePaySettings = yenePaySettings;
        }

        public async Task<SendMoneyResult> SendMoney(SendMoneyRequest request)
        {
            if (string.IsNullOrEmpty(request.PayerSignature))
            {
                SignRequest(request);
            }
            SetBearerToken();
            var json = JsonSerializer.Serialize(request, _serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/client/send", content);
            if (response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                return new SendMoneyResult(JsonSerializer.Deserialize<SendMoneyResponse>(resultString, _serializeOptions));
            }
            else
            {
                string resultString;
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    resultString = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    resultString = response.ReasonPhrase;
                }

                return new SendMoneyResult(resultString, (int)response.StatusCode);
            }
        }

        public SendMoneyRequest CreateSignedRequest(string messageToRecipients, params MoneyRecipient[] recipients)
        {
            if (recipients == null || recipients.Length == 0)
            {
                return null;
            }
            var request = new SendMoneyRequest()
            {
                MessageToRecipients = messageToRecipients
            };
            request.Recipients.AddRange(recipients);
            SignRequest(request);
            return request;
        }

        private void SignRequest(SendMoneyRequest request)
        {
            var dataToSignList = request.Recipients.OrderBy(p => p.CustomerCode).Select(r => $"{r.CustomerCode}={r.Amount.ToString("N2")}");
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            keyValues.Add("ReceiverCode", string.Join(",", dataToSignList));
            keyValues.Add("Amount", request.TotalPayment.ToString("N2"));
            keyValues.Add("Code", _yenePaySettings.CustomerCode);
            keyValues.Add("Currency", request.Currency ?? "ETB");

            var dataToSign = string.Join("&", keyValues.Select(p => $"{p.Key}={p.Value}"));

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportPrivateKey(_yenePaySettings.KeyType, _yenePaySettings.SigningKey, _yenePaySettings.IsKeyPemFormatted);
                var encoding = new UTF8Encoding();
                var data = encoding.GetBytes(dataToSign);
                request.PayerSignature = Convert.ToBase64String(rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            }
        }

        #region Utility Methods
        private void SetBearerToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _yenePaySettings.AccessToken);
        }
        #endregion
    }
}
