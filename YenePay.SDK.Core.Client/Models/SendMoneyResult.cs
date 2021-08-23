using System.Collections.Generic;
using YenePay.SDK.Core.Client.Configuration;

namespace YenePay.SDK.Core.Client.Models
{
    public class SendMoneyResult : HttpResult<TransactionModel>
    {
        public bool IsPaid { get => SuccessResult?.Status == 9; }
        public bool ShouldManuallyContinue { get => !IsPaid && SuccessResult != null; }
        public string ManualContinueUrl { get => !ShouldManuallyContinue ? null : $"{Constants.YenePayCheckoutBaseUrl}/Home/Continue/{SuccessResult?.OrderId}"; }

        public List<MoneyRecipient> DuplicatedRecipients { get; internal set; } = new List<MoneyRecipient>();
        public List<MoneyRecipient> NonExistingRecipients { get; internal set; } = new List<MoneyRecipient>();
        public List<MoneyRecipient> CodeMismatchRecipients { get; internal set; } = new List<MoneyRecipient>();
        public SendMoneyResult(string error, int statusCode) : base(error, statusCode)
        {
        }

        public SendMoneyResult(TransactionModel result) : base(result) { }

        internal SendMoneyResult(SendMoneyResponse response): base(response.Order)
        {
            var validationErr = response.ProcessValidationError();
            if (!string.IsNullOrEmpty(validationErr))
            {
                IsError = true;
                ErrorMessage = validationErr;
                DuplicatedRecipients = response.DuplicatedRecipients;
                NonExistingRecipients = response.NonExistingRecipients;
                CodeMismatchRecipients = response.Email_CodeMismatches;
            }
            else
            {
                IsError = response.PaymentResult != null ? !response.PaymentResult.Suceeded : true;
                ErrorMessage = response.PaymentResult?.ErrorMsg;
            }
        }
            
    }
}
