using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace ModInfoLocalizer.Textures
{
    internal static class ModInfoLocalizerTextures
    {
        private static Asset<Texture2D> _localizedIcon;
        internal static Asset<Texture2D> LocalizedIcon => _localizedIcon ??= ModContent.Request<Texture2D>("ModInfoLocalizer/Textures/LocalizedIcon");
    }
}
