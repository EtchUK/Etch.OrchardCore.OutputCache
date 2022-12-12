using Microsoft.VisualBasic;
using System;

namespace Etch.OrchardCore.OutputCache.Settings
{
    public class OutputCacheSettings
    {
        public int Expiration { get; set; } = Constants.Defaults.Expiration;
        public string Tag { get; set; } = Constants.Defaults.Tag;
        public string[] VaryByQueryStrings { get; set; } = Array.Empty<string>();
    }
}
