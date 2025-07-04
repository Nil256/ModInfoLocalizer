using Microsoft.Xna.Framework.Input;
using ModInfoLocalizer.Textures;
using MonoMod.RuntimeDetour;
using System.Collections.Generic;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace ModInfoLocalizer
{
    internal static class ModInfoLocalizerDetour
    {
        private static Hook UIModInfo_Show_Hook;
        private static Hook UIModItem_OnInitialize_Hook;
        private delegate void UIModInfo_orig_Show(object self, string modName, string displayName, int gotoMenu, object localMod, string description, string url);

        private delegate void UIModItem_orig_OnInitialize(object self);

        internal static void AddHooks()
        {
            UIModInfo_Show_Hook = new Hook(ReflectionCache.UIModInfo.Show, HackModInfo);
            UIModItem_OnInitialize_Hook = new Hook(ReflectionCache.UIModItem.OnInitialize, AppendLocalizedIcon);
            UIModInfo_Show_Hook.Apply();
            UIModItem_OnInitialize_Hook.Apply();
        }

        private static void HackModInfo(UIModInfo_orig_Show orig, object self, string modName, string displayName, int gotoMenu, object localMod, string description, string url)
        {
            if (!IsPressedShiftOrControl() && LocalizedInfoRegistry.TryGetLocalizedInfo(modName, ModInfoLocalizer.Option.GetCurrentLanguageCode(), out string localizedDescription) && !string.IsNullOrEmpty(localizedDescription))
            {
                description = localizedDescription;
            }
            orig(self, modName, displayName, gotoMenu, localMod, description, url);
        }

        private static bool IsPressedShiftOrControl()
        {
            List<Keys> pressedKeys = PlayerInput.GetPressedKeys();
            for (var i = 0; i < pressedKeys.Count; i++)
            {
                switch (pressedKeys[i])
                {
                    case Keys.LeftShift:
                    case Keys.RightShift:
                    case Keys.LeftControl:
                    case Keys.RightControl:
                        return true;
                }
            }
            return false;
        }

        private static void AppendLocalizedIcon(UIModItem_orig_OnInitialize orig, object self)
        {
            orig(self);
            ModInfoLocalizerOption option = ModInfoLocalizer.Option;
            if (!option.ShowLocalizedIcon)
            {
                return;
            }
            PropertyInfo ModName = ReflectionCache.UIModItem.ModName;
            if (ModName.GetValue(self) is not string modName)
            {
                return;
            }
            if (!LocalizedInfoRegistry.HasLocalizedInfo(modName, option.GetCurrentLanguageCode()))
            {
                return;
            }
            UIElement uiModItem = (UIElement)self;
            UIImage localizedIcon = new UIImage(ModInfoLocalizerTextures.LocalizedIcon.Value)
            {
                Width = { Pixels = 21f },
                Height = { Pixels = 21f },
                Left =
                {
                    // Pixels = -36,
                    Pixels = -17,
                    Percent = 1f
                },
                Top =
                {
                    // Pixels = 40f,
                    Pixels = 63f
                },
                IgnoresMouseInteraction = true
            };
            uiModItem.Append(localizedIcon);
        }

        internal static void RemoveHooks()
        {
            UIModItem_OnInitialize_Hook.Dispose();
            UIModInfo_Show_Hook.Dispose();
        }
    }
}
