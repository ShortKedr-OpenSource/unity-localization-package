using UnityEngine;

namespace Krugames.LocalizationSystem.Translation.LanguageEncoding {
    public abstract class SystemLanguageEncoding {

        public static readonly string UnknownLanguageCode = "unknown";
        public static readonly SystemLanguage UnknownSystemLanguage = SystemLanguage.Unknown;

        public static readonly SystemLanguageEncoding Iso639_1 = new Iso639_1LanguageEncoding();
        
        public abstract string GetCode(SystemLanguage language);
        public abstract SystemLanguage GetSystemLanguage(string code);
    }
}