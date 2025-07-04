using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ModInfoLocalizer
{
    [Label("$Mods.ModInfoLocalizer.Configs.Client.Title")]
    public class ModInfoLocalizerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("$Mods.ModInfoLocalizer.Configs.Headers.Localization")]

        /*
        [Label("$Mods.ModInfoLocalizer.Configs.Client.Localization_UseVanillaOption_Label")] // Use vanilla language option
        [Tooltip("$Mods.ModInfoLocalizer.Configs.Client.Localization_UseVanillaOption_Tooltip")] // On by default
        [DefaultValue(true)]
        public bool useVanillaOption;
        */

        [Label("$Mods.ModInfoLocalizer.Configs.Client.Localization_LanguageCode_Label")]
        [Tooltip("$Mods.ModInfoLocalizer.Configs.Client.Localization_LanguageCode_Tooltip")]
        [DefaultValue("")]
        public string languageCode;

        [Header("$Mods.ModInfoLocalizer.Configs.Headers.UI")]

        [Label("$Mods.ModInfoLocalizer.Configs.Client.ShowLocalizedIcon_Label")]
        [Tooltip("$Mods.ModInfoLocalizer.Configs.Client.ShowLocalizedIcon_Tooltip")]
        [DefaultValue(false)]
        public bool showLocalizedIcon;
    }
}
