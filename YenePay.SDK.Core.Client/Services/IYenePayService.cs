using System.Threading.Tasks;
using YenePay.SDK.Core.Client.Models;

namespace YenePay.SDK.Core.Client.Services
{
    public interface IYenePayService
    {
        SendMoneyRequest CreateSignedRequest(string messageToRecipients, params MoneyRecipient[] recipients);
        Task<SendMoneyResult> SendMoney(SendMoneyRequest request);
    }
}
