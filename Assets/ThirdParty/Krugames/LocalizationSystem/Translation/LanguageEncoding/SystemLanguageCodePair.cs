using UnityEngine;

namespace Krugames.LocalizationSystem.Translation.LanguageEncoding {
    public class SystemLanguageCodePair {
        public SystemLanguage SystemLanguage;
        public string Code;

        public SystemLanguageCodePair(SystemLanguage systemLanguage, string code) {
            SystemLanguage = systemLanguage;
            Code = code;
        }
    }
}