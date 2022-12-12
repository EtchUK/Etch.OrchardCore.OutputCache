using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using System;

namespace Etch.OrchardCore.OutputCache
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddOutputCache(options =>
            {
                options.DefaultExpirationTimeSpan = new TimeSpan(0, 10, 0);

                // Create a new policy that will contain the DefaultPolicy and enable all endpoints (GET, un-authenticated)
                options.AddBasePolicy(build => { });
            });
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            app.UseOutputCache();
        }
    }
}
