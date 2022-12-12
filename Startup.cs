using Etch.OrchardCore.OutputCache.Drivers;
using Etch.OrchardCore.OutputCache.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Entities;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Settings;
using System;

namespace Etch.OrchardCore.OutputCache
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDisplayDriver<ISite>, OutputCacheSettingsDisplayDriver>();

            services.AddScoped<INavigationProvider, AdminMenu>();

            services.AddOutputCache(options =>
            {
                var siteService = services.BuildServiceProvider().GetService<ISiteService>();
                var settings = siteService.GetSiteSettingsAsync().Result.As<OutputCacheSettings>();

                options.DefaultExpirationTimeSpan = new TimeSpan(0, settings.Expiration, 0);

                options.AddBasePolicy(build => {
                    build.Tag(settings.Tag);

                    if (settings.VaryByQueryStrings != null)
                    {
                        build.VaryByQuery(settings.VaryByQueryStrings);
                    }
                });
            });
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            app.UseOutputCache();
        }
    }
}
