using Terraria.Localization;

namespace ModInfoLocalizer
{
    internal class ModInfoLocalizerOption
    {
        private readonly ModInfoLocalizerConfig _config;

        internal ModInfoLocalizerOption(ModInfoLocalizerConfig config)
        {
            _config = config;
        }

        internal bool ShowLocalizedIcon => _config.showLocalizedIcon;

        internal string GetCurrentLanguageCode()
        {
            string languageCode = _config.languageCode;
            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                return languageCode;
            }
            return Language.ActiveCulture.Name;
        }
    }
}
