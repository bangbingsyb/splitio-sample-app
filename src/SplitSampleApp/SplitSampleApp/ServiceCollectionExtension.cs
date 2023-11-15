using Splitio.Services.Client.Classes;
using Splitio.Services.Client.Interfaces;

namespace SplitSampleApp
{
    public static class ServiceCollectionExtension
    {
        public static void AddSplitClient(this IServiceCollection services, SplitioConfig splitioConfig)
        {
            services.AddSingleton<ISplitClient>((sp) =>
            {
                var config = new ConfigurationOptions();
                var factory = new SplitFactory(splitioConfig.ApiKey, config);
//                var factory = new SplitFactory("90hl9jjn03dp3oee122upu8it6brllq8c9j4", config);
                var client = factory.Client();

                client.BlockUntilReady(10000);

                return client;
            });
        }
    }
}