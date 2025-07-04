using System;
using System.Text;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ModInfoLocalizer
{
	public class ModInfoLocalizer : Mod
	{
        internal static ModInfoLocalizerOption Option { get; private set; }

        public override void Load()
        {
            Option = new ModInfoLocalizerOption(ModContent.GetInstance<ModInfoLocalizerConfig>());

            ModInfoLocalizerDetour.AddHooks();
        }

        public override void Unload()
        {
            ModInfoLocalizerDetour.RemoveHooks();
        }

        public override object Call(params object[] args)
        {
            return ModCall.Call(args);
        }
    }
}