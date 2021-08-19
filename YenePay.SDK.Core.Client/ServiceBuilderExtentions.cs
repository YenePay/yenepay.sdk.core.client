using System;
using YenePay.SDK.Core.Client.Configuration;
using YenePay.SDK.Core.Client.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceBuilderExtentions
    {
        public static void AddYenePayClient(this IServiceCollection services)
        {
            services.AddSingleton<YenePaySettings>();
            services.AddHttpClient<IYenePayService, YenePayService>((client) =>
            {
                client.BaseAddress = new Uri(Constants.YenePayApiBaseUrl);
                client.Timeout = TimeSpan.FromMinutes(1);
            });
        }
    }
}
