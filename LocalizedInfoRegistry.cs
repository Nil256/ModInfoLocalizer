using log4net;
using ModInfoLocalizer.Localization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Terraria.ModLoader;

namespace ModInfoLocalizer
{
    internal class LocalizedInfoRegistry : ModType
    {
        private static readonly Dictionary<string, LocalizedModInfo> _registry = new Dictionary<string, LocalizedModInfo>();

        public override sealed void SetupContent()
        {
            RegisterLocalizedInfoFileAutomatically(Mod.Logger);
        }

        private static void RegisterLocalizedInfoFileAutomatically(ILog logger)
        {
            logger.Debug("Starts to register Localized description.txt automatically.");
            Mod[] mods = ModLoader.Mods;
            for (var i = 0; i < mods.Length; i++)
            {
                Mod mod = mods[i];
                if (mod.Name == "ModLoader")
                {
                    continue;
                }
                _registry.Add(mod.Name, new LocalizedModInfo());
                FindLocalizedInfoFile(logger, mod);
            }
        }

        private static void FindLocalizedInfoFile(ILog logger, Mod mod)
        {
            logger.Debug($"Finding Localized description.txt from {mod.Name}.");
            PropertyInfo Mod_File = ReflectionCache.Mod.File;
            object File = Mod_File.GetValue(mod);
            if (File == null || File.GetType().Name != ReflectionCache.TmodFile.Type.Name)
            {
                logger.Warn("Failed to get files from mod instance.");
                return;
            }
            MethodInfo TModFile_GetFileNames = ReflectionCache.TmodFile.GetFileNames;
            if (TModFile_GetFileNames.Invoke(File, new object[0]) is not List<string> fileNames)
            {
                logger.Warn("Failed to get file names from files.");
                return;
            }
            for (var i = 0; i < fileNames.Count; i++)
            {
                string fileName = fileNames[i];
                if (!TryGetLanguageCodeFromFileName(logger, fileName, out string languageCode, out bool inRoot))
                {
                    continue;
                }
                RegisterFromModFile(mod, languageCode, fileName, inRoot);
            }
        }

        private static bool TryGetLanguageCodeFromFileName(ILog logger, in string fileName, out string languageCode, out bool inRoot)
        {
            const string DESCRIPTION_WORKSHOP_FILE = "description_workshop.txt";
            const string DESCRIPTION_FILE_PREFIX = "description_";
            const string DESCRIPTION_FILE_PREFIX_WITH_DIRECTORY = "localization/description_";
            const string DESCRIPTION_FILE_EXTENSION = ".txt";

            languageCode = "";
            inRoot = false;

            string lowerFileName = fileName.ToLower();
            if (!lowerFileName.EndsWith(DESCRIPTION_FILE_EXTENSION))
            {
                return false;
            }
            string descriptionFilePrefix;
            if (lowerFileName.StartsWith(DESCRIPTION_FILE_PREFIX) && lowerFileName != DESCRIPTION_WORKSHOP_FILE)
            {
                descriptionFilePrefix = DESCRIPTION_FILE_PREFIX;
                inRoot = true;
            }
            else if (lowerFileName.StartsWith(DESCRIPTION_FILE_PREFIX_WITH_DIRECTORY))
            {
                descriptionFilePrefix = DESCRIPTION_FILE_PREFIX_WITH_DIRECTORY;
            }
            else
            {
                return false;
            }
            logger.Debug($"Found description file \"{fileName}\".");
            int languageCodeLength = fileName.Length - descriptionFilePrefix.Length - DESCRIPTION_FILE_EXTENSION.Length;
            languageCode = fileName.Substring(descriptionFilePrefix.Length, languageCodeLength);
            logger.Debug($"Extracted language code: {languageCode}");
            return true;
        }

        private static void RegisterFromModFile(in Mod mod, in string languageCode, in string fileName, in bool prioritize)
        {
            byte[] rawString = mod.GetFileBytes(fileName);
            string localzedDescription = Encoding.UTF8.GetString(rawString);
            LocalizedModInfo registry = _registry[mod.Name];
            registry.AddLocalizedDescription(languageCode, new LocalizedDescription(localzedDescription), !prioritize);
        }

        internal static void RegisterFromModCall(in string modName, in string languageCode, in string localizedModInfo, in int priority)
        {
            if (string.IsNullOrWhiteSpace(modName) || string.IsNullOrEmpty(languageCode) || localizedModInfo == null || priority == 0)
            {
                throw new Exception("This error is Mod Info Localizer fault! Please report the author of Mod Info Localizer.");
            }
            if (!_registry.ContainsKey(modName))
            {
                _registry.Add(modName, new LocalizedModInfo());
            }
            _registry[modName].AddLocalizedDescription(languageCode, new LocalizedDescription(localizedModInfo, priority));
        }

        internal static bool TryGetLocalizedInfo(in string modName, in string languageCode, out string localizedInfo)
        {
            if (!_registry.ContainsKey(modName))
            {
                localizedInfo = "";
                return false;
            }
            return _registry[modName].TryGetLocalizedDescription(languageCode, out localizedInfo);
        }

        internal static bool HasLocalizedInfo(in string modName, in string languageCode)
        {
            return _registry.ContainsKey(modName) && _registry[modName].HasLocalizedDescription(languageCode);
        }

        internal static string GetLocalizedInfo(in string modName, in string languageCode)
        {
            return _registry[modName].GetLocalizedDescription(languageCode);
        }

        protected override sealed void Register() { }
    }
}
