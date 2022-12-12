namespace Etch.OrchardCore.OutputCache.ViewModels
{
    public class OutputCacheSettingsViewModel
    {
        public int Expiration { get; set; }
        public string Tag { get; set; }
        public string VaryByQueryStrings { get; set; }
    }
}
