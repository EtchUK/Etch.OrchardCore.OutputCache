using Azure;
using Etch.OrchardCore.OutputCache.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Etch.OrchardCore.OutputCache.Policies
{
    public class DefaultOrchardCorePolicy : IOutputCachePolicy
    {
        private readonly OutputCacheSettings _settings;

        public DefaultOrchardCorePolicy(OutputCacheSettings settings)
        {
            _settings = settings;
        }

        public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            var attemptOutputCaching = AttemptOutputCaching(context);
            context.EnableOutputCaching = true;
            context.AllowCacheLookup = attemptOutputCaching;
            context.AllowCacheStorage = attemptOutputCaching;
            context.AllowLocking = true;

            // Vary by any query by default
            context.CacheVaryByRules.QueryKeys = "*";

            if (_settings.VaryByQueryStrings?.Any() ?? false)
            {
                context.CacheVaryByRules.QueryKeys = _settings.VaryByQueryStrings;
            }

            if (string.IsNullOrWhiteSpace(_settings.Tag))
            {
                context.Tags.Add(_settings.Tag);
            }

            return ValueTask.CompletedTask;
        }

        public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            var response = context.HttpContext.Response;

            // Verify existence of cookie headers
            if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie) && 
                response.Headers.SetCookie.Count != 1 &&
                !response.Headers.SetCookie[0].StartsWith(".AspNetCore.Mvc.CookieTempDataProvider=; expires=Thu, 01 Jan 1970 00:00:00 GMT;"))
            {
                context.AllowCacheStorage = false;
                return ValueTask.CompletedTask;
            }

            // Check response code
            if (response.StatusCode != StatusCodes.Status200OK)
            {
                context.AllowCacheStorage = false;
                return ValueTask.CompletedTask;
            }

            return ValueTask.CompletedTask;
        }

        private bool AttemptOutputCaching(OutputCacheContext context)
        {
            // Check if the current request fulfisls the requirements to be cached
            var request = context.HttpContext.Request;

            // Verify the method
            if (!HttpMethods.IsGet(request.Method) && !HttpMethods.IsHead(request.Method))
            {
                return false;
            }

            if (_settings.BypassCookies != null && request.Cookies.Any(x => _settings.BypassCookies.Any(c => c.Equals(x.Key, System.StringComparison.OrdinalIgnoreCase))))
            {
                return false;
            }

            // Verify existence of authorization headers
            if (!StringValues.IsNullOrEmpty(request.Headers.Authorization) || request.HttpContext.User?.Identity?.IsAuthenticated == true)
            {
                return false;
            }

            return true;
        }
    }
}
