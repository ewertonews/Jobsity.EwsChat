using Jobsity.EwsChat.Server.ExternalClients;
using Jobsity.EwsChat.Server.Queuing;
using Jobsity.EwsChat.Server.Queuing.Options;
using Jobsity.EwsChat.Server.Services;
using Jobsity.EwsChat.Shared;
using Jobsity.EwsChat.Shared.SignalR;
using Jobsity.EwsChat.Shared.SignalR.Providers;
using Microsoft.OpenApi.Models;

namespace Jobsity.EwsChat.Server.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ILoggingService, LoggingService>();
            builder.Services.AddSingleton<IHubConnectionProvider, HubConnectionProvider>();
            builder.Services.AddSingleton<IHubHandler, ChatHubHandler>();
            builder.Services.AddSingleton<IStockInfoRequestSender, StockInfoRequestSender>();

            var urls = builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey);
            string hostUrl;
            
            if (urls != null)
            {
                hostUrl = urls.Split(";")[0];
            }
            else
            {
                hostUrl = builder.Configuration["hostUrl"] ?? throw new ApplicationException("Base host url needs to be set in the appSettings. Key: hostUrl");
            }
            builder.Services.AddSingleton<IChatBotService>(services => new ChatBotService(
                services.GetRequiredService<IHubHandler>(),
                services.GetRequiredService<IStockInfoRequestSender>(),
                services.GetRequiredService<ILoggingService>(),
                hostUrl));

            builder.Services.AddHostedService<StockInfoRequestReceiver>();
        }

        public static void AddRequiredServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddOptions();
            builder.Services.AddSignalR();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Jobsity Chat - Messages API", Version = "v1" });
            });
        }

        public static void AddConfigurations(this WebApplicationBuilder builder)
        {
            var rabbitMqConfigSection = builder.Configuration.GetSection("RabbitMq");
            builder.Services.Configure<RabbitMqConfiguration>(rabbitMqConfigSection);
        }

        public static void AddApplicationHttpClients(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient<IStockClient, StockClient>(c =>
            {
                c.BaseAddress = new Uri(builder.Configuration["stockApiUrl"]);
            });

        }
    }
}
