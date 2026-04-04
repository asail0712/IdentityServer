using AetherCore.Module.Token.Interface;
using AetherCore.Module.Token;

namespace IdentityServer.Extension
{
    public static class AddServiceExtension
    {
        static public void SlaveServices(this IServiceCollection services)
        {
            // Add Module
            services.AddScoped<ITokenServiceFactory, JWTokenServiceFactory>(); // Token 服務工廠

            ///***********************************************************************************************
            //* add Singleton Services
            //************************************************************************************************/
            //services.AddSingleton<ISmtpEmailSender, SmtpEmailSender>(); // Email 寄信服務

            ///***********************************************************************************************
            //* 讓 DI 容器可以提供 IHttpClientFactory
            //************************************************************************************************/
            services.AddHttpClient();
        }
    }
}
