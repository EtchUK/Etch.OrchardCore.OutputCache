using Etch.OrchardCore.OutputCache.Settings;
using Etch.OrchardCore.OutputCache.ViewModels;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Environment.Shell;
using OrchardCore.Settings;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.OutputCache.Drivers
{
    public class OutputCacheSettingsDisplayDriver : SectionDisplayDriver<ISite, OutputCacheSettings>
    {
        private readonly IShellHost _shellHost;
        private readonly ShellSettings _shellSettings;

        public OutputCacheSettingsDisplayDriver(IShellHost shellHost, ShellSettings shellSettings)
        {
            _shellHost = shellHost;
            _shellSettings = shellSettings;
        }

        public override IDisplayResult Edit(OutputCacheSettings section, BuildEditorContext context)
        {
            return Initialize<OutputCacheSettingsViewModel>("OutputCacheSettings_Edit", model =>
            {
                model.Expiration = section.Expiration;
                model.Tag = section.Tag;
                model.VaryByQueryStrings = string.Join(", ", section.VaryByQueryStrings);
            })
            .Location("Content:5")
            .OnGroup(Constants.GroupId);
        }

        public override async Task<IDisplayResult> UpdateAsync(OutputCacheSettings settings, BuildEditorContext context)
        {
            if (context.GroupId == Constants.GroupId)
            {
                var model = new OutputCacheSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                if (context.Updater.ModelState.IsValid)
                {
                    settings.Expiration = model.Expiration;
                    settings.Tag = model.Tag;
                    settings.VaryByQueryStrings = model.VaryByQueryStrings
                        .Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .ToArray();

                    await _shellHost.ReleaseShellContextAsync(_shellSettings);
                }
            }

            return await EditAsync(settings, context);
        }
    }
}
