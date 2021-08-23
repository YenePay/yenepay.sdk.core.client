using System;
using System.Text;

namespace YenePay.SDK.Core.Client.Models
{
    public class HttpResult<T>
    {
        public bool IsError { get; internal set; }
        public string ErrorMessage { get; internal set; }
        public T SuccessResult { get; internal set; }
        public int HttpStatusCode { get; internal set; } = 200;

        public HttpResult(string error, int statusCode)
        {
            IsError = true;
            ErrorMessage = error;
            HttpStatusCode = statusCode;
        }

        public HttpResult(T successResult)
        {
            IsError = false;
            SuccessResult = successResult;
        }
    }
}
