using System.Collections.Generic;

namespace ModInfoLocalizer.Localization
{
    internal class LocalizedModInfo
    {
        private readonly Dictionary<string, LocalizedDescription> _descriptionByLanguageCode;

        internal LocalizedModInfo()
        {
            _descriptionByLanguageCode = new Dictionary<string, LocalizedDescription>();
        }

        internal void AddLocalizedDescription(in string languageCode, LocalizedDescription localizedDescription, bool doNotOverrideSamePriority = false)
        {
            if (!_descriptionByLanguageCode.TryGetValue(languageCode, out LocalizedDescription registered))
            {
                _descriptionByLanguageCode.Add(languageCode, localizedDescription);
                return;
            }
            if (!doNotOverrideSamePriority)
            {
                _descriptionByLanguageCode[languageCode] += localizedDescription;
                return;
            }
            if (registered.Priority >= localizedDescription.Priority)
            {
                return;
            }
            _descriptionByLanguageCode[languageCode] = localizedDescription;
        }

        internal bool TryGetLocalizedDescription(in string languageCode, out string localizedDescription)
        {
            if (HasLocalizedDescription(languageCode))
            {
                localizedDescription = GetLocalizedDescription(languageCode);
                return true;
            }
            localizedDescription = "";
            return false;
        }

        internal bool HasLocalizedDescription(in string languageCode)
        {
            return _descriptionByLanguageCode.ContainsKey(languageCode);
        }

        internal string GetLocalizedDescription(in string languageCode)
        {
            return _descriptionByLanguageCode[languageCode].Text;
        }
    }
}
