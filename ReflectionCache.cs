using System;
using System.Reflection;
using Terraria.ModLoader;

namespace ModInfoLocalizer
{
    internal static class ReflectionCache
    {
        private static Assembly _tML;
        internal static Assembly tML => _tML ??= typeof(ModLoader).Assembly;

        internal static class Mod
        {
            private static Type _type;
            internal static Type Type => _type ??= typeof(Terraria.ModLoader.Mod);

            private static PropertyInfo _File;
            internal static PropertyInfo File => _File ??= Type.GetProperty("File", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        internal static class TmodFile
        {
            private static Type _type;
            internal static Type Type => _type ??= tML.GetType("Terraria.ModLoader.Core.TmodFile", true);

            private static MethodInfo _GetFileNames;
            internal static MethodInfo GetFileNames => _GetFileNames ??= Type.GetMethod("GetFileNames", BindingFlags.Instance | BindingFlags.Public);
        }

        internal static class UIModItem
        {
            private static Type _type;
            internal static Type Type => _type ??= tML.GetType("Terraria.ModLoader.UI.UIModItem", true);

            private static MethodInfo _OnInitialize;
            internal static MethodInfo OnInitialize => _OnInitialize ??= Type.GetMethod("OnInitialize", BindingFlags.Instance | BindingFlags.Public);

            private static PropertyInfo _ModName;
            internal static PropertyInfo ModName => _ModName ??= Type.GetProperty("ModName", BindingFlags.Instance | BindingFlags.Public);
        }

        internal static class UIModInfo
        {
            private static Type _type;
            internal static Type Type => _type ??= tML.GetType("Terraria.ModLoader.UI.UIModInfo", true);

            private static MethodInfo _Show;
            internal static MethodInfo Show => _Show ??= Type.GetMethod("Show", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}
