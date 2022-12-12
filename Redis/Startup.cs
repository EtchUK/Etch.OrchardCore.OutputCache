using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Etch.OrchardCore.OutputCache.Redis
{
    [Feature("Etch.OrchardCore.OutputCache.Redis")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();
        }
    }
}
