using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace YenePay.SDK.Core.Client.Models
{
    public class SendMoneyRequest
    {
        
        public string PayerSignature { get; set; }
        public string LinkedAccountId { get; set; } = "YP005"; //YenePay Balance Code
        public string Currency { get; set; } = "ETB"; //This is important since only ETB is the only currency supported by YenePay Balance
        [JsonPropertyName("msgToRecipients")]
        public string MessageToRecipients { get; set; } = "Money deposited";
        public List<MoneyRecipient> Recipients { get; set; } = new List<MoneyRecipient>();
        public double TotalPayment { get => Recipients?.Sum(p => p.Amount) ?? 0; }
    }

    public class MoneyRecipient
    {
        public string CustomerCode { get; set; }
        [JsonPropertyName("email")]
        public string EmailOrPhone { get; set; }
        public double Amount { get; set; }
    }
    public class SendMoneyValidationResponse
    {
        public List<MoneyRecipient> DuplicatedRecipients { get; set; } = new List<MoneyRecipient>();
        public List<MoneyRecipient> NonExistingRecipients { get; set; } = new List<MoneyRecipient>();
        public List<MoneyRecipient> Email_CodeMismatches { get; set; } = new List<MoneyRecipient>();
        public bool IsTotalPaymentCorrect { get; set; }
        public bool IsCommissionPaymentCorrect { get; set; }
        public bool IsOrderValid { get; set; }

        internal string ProcessValidationError()
        {
            if (!IsOrderValid)
            {
                return "Send money request validation failed";
            }
            if (!IsCommissionPaymentCorrect)
            {
                return "Commision amount validation failed";
            }
            if (!IsTotalPaymentCorrect)
            {
                return "Total amount is not correct";
            }
            if (DuplicatedRecipients.Any())
            {
                return "Found some duplicate recipients";
            }
            if (NonExistingRecipients.Any())
            {
                return "Could not find some recipients";
            }
            if (Email_CodeMismatches.Any())
            {
                return "Found some recipients whose email/phone does not match with the supplied customer code";
            }
            return null;
        }
    }
    internal class SendMoneyResponse : SendMoneyValidationResponse
    {
        public string TransactionId { get; set; }
        public TransactionModel Order { get; set; }
        public PaymentResultModel PaymentResult { get; set; }
    }

    public class TransactionModel
    {
        public int Status { get; set; }
        public string StatusText { get; set; }
        public string StatusDescription { get; set; }
        public double TotalAmount { get; set; }
        public double TotalAmountETB { get; set; }
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodText { get; set; }
        public int ItemsCount { get; set; }
        public string Process { get; set; }
        public string PaymentSignature { get; set; }
    }

    internal class PaymentResultModel
    {
        public int CurrentStatus { get; set; }
        public string CurrentStatusTxt { get; set; }
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public string ResultType { get; set; }
        public bool Suceeded { get; set; }
        public string ErrorMsg { get; set; }
    }
}
