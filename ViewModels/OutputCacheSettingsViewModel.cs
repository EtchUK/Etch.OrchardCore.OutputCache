namespace Etch.OrchardCore.OutputCache.ViewModels
{
    public class OutputCacheSettingsViewModel
    {
        public string BypassCookies { get; set; }
        public int Expiration { get; set; }
        public string Tag { get; set; }
        public string VaryByQueryStrings { get; set; }
    }
}
