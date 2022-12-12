using Etch.OrchardCore.OutputCache.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Entities;
using OrchardCore.Settings;
using System.Threading;
using System.Threading.Tasks;

namespace Etch.OrchardCore.OutputCache.Controllers
{
    [Admin]
    public class PurgeController : Controller
    {
        private readonly AdminOptions _adminOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotifier _notifier;
        private readonly IOutputCacheStore _outputCacheStore;
        private readonly ISiteService _siteService;

        private readonly IHtmlLocalizer H;

        public PurgeController(
            IOptions<AdminOptions> adminOptions,
            IHtmlLocalizer<PurgeController> htmlLocalizer,
            IHttpContextAccessor httpContextAccessor,
            INotifier notifier,
            IOutputCacheStore outputCacheStore,
            ISiteService siteService)
        {
            _adminOptions = adminOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            _notifier = notifier;
            _outputCacheStore = outputCacheStore;
            _siteService = siteService;

            H = htmlLocalizer;
        }

        public async Task<IActionResult> Index()
        {
            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var settings = siteSettings.As<OutputCacheSettings>();

            if (!string.IsNullOrEmpty(settings.Tag))
            {
                await _outputCacheStore.EvictByTagAsync(settings.Tag, new CancellationTokenSource().Token);
            }

            await _notifier.SuccessAsync(H["Successfully purged output cache."]);

            return Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/{_adminOptions.AdminUrlPrefix}/Settings/OutputCache");
        }
    }
}
