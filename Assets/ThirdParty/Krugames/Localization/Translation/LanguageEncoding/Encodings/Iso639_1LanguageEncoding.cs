using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Krugames.LocalizationSystem.Translation.LanguageEncoding {
    /// <summary>
    /// Language encoding for ISO 639-1 standard
    /// </summary>
    public class Iso639_1LanguageEncoding : SystemLanguageEncoding {

        public static readonly SystemLanguageCodePair[] RelationPairs = new[] {
            new SystemLanguageCodePair(SystemLanguage.Afrikaans, "af"),
            new SystemLanguageCodePair(SystemLanguage.Arabic, "ar"),
            new SystemLanguageCodePair(SystemLanguage.Basque, "eu"),
            new SystemLanguageCodePair(SystemLanguage.Belarusian, "be"),
            new SystemLanguageCodePair(SystemLanguage.Bulgarian, "bg"),
            new SystemLanguageCodePair(SystemLanguage.Catalan, "ca"),
            new SystemLanguageCodePair(SystemLanguage.Chinese, "zh"),
            new SystemLanguageCodePair(SystemLanguage.Czech, "cs"),
            new SystemLanguageCodePair(SystemLanguage.Danish, "da"),
            new SystemLanguageCodePair(SystemLanguage.Dutch, "nl"),
            new SystemLanguageCodePair(SystemLanguage.English, "en"),
            new SystemLanguageCodePair(SystemLanguage.Estonian, "et"),
            new SystemLanguageCodePair(SystemLanguage.Faroese, "fo"),
            new SystemLanguageCodePair(SystemLanguage.Finnish, "fi"),
            new SystemLanguageCodePair(SystemLanguage.French, "fr"),
            new SystemLanguageCodePair(SystemLanguage.German, "de"),
            new SystemLanguageCodePair(SystemLanguage.Greek, "el"),
            new SystemLanguageCodePair(SystemLanguage.Hebrew, "he"),
            new SystemLanguageCodePair(SystemLanguage.Icelandic, "is"),
            new SystemLanguageCodePair(SystemLanguage.Indonesian, "id"),
            new SystemLanguageCodePair(SystemLanguage.Italian, "it"),
            new SystemLanguageCodePair(SystemLanguage.Japanese, "ja"),
            new SystemLanguageCodePair(SystemLanguage.Korean, "ko"),
            new SystemLanguageCodePair(SystemLanguage.Latvian, "lv"),
            new SystemLanguageCodePair(SystemLanguage.Lithuanian, "lt"),
            new SystemLanguageCodePair(SystemLanguage.Norwegian, "no"),
            new SystemLanguageCodePair(SystemLanguage.Polish, "pl"),
            new SystemLanguageCodePair(SystemLanguage.Portuguese, "pt"),
            new SystemLanguageCodePair(SystemLanguage.Romanian, "ro"),
            new SystemLanguageCodePair(SystemLanguage.Russian, "ru"),
            new SystemLanguageCodePair(SystemLanguage.SerboCroatian, "sr"), //Serbian used instead
            new SystemLanguageCodePair(SystemLanguage.Slovak, "sk"),
            new SystemLanguageCodePair(SystemLanguage.Slovenian, "sl"),
            new SystemLanguageCodePair(SystemLanguage.Spanish, "es"),
            new SystemLanguageCodePair(SystemLanguage.Swedish, "sv"),
            new SystemLanguageCodePair(SystemLanguage.Thai, "th"),
            new SystemLanguageCodePair(SystemLanguage.Turkish, "tr"),
            new SystemLanguageCodePair(SystemLanguage.Ukrainian, "uk"),
            new SystemLanguageCodePair(SystemLanguage.Vietnamese, "vi"),
            new SystemLanguageCodePair(SystemLanguage.ChineseSimplified, "zh"), //Default Chinese used instead
            new SystemLanguageCodePair(SystemLanguage.ChineseTraditional, "zh"), //Default Chinese used instead
            new SystemLanguageCodePair(SystemLanguage.Hungarian, "hu"),
            new SystemLanguageCodePair(SystemLanguage.Unknown, UnknownLanguageCode), //Make sure this element is always included
        };

        public static class EncodingMaps {
            private static Dictionary<SystemLanguage, string> codeBySystemLanguage = null;
            private static Dictionary<string, SystemLanguage> systemLanguageByCode = null;

            public static Dictionary<SystemLanguage, string> CodeBySystemLanguage {
                get {
                    if (codeBySystemLanguage == null) {
                        var relationPairs = RelationPairs;
                        codeBySystemLanguage = new Dictionary<SystemLanguage, string>(relationPairs.Length);
                        for (int i = 0; i < relationPairs.Length; i++) {
                            codeBySystemLanguage.Add(relationPairs[i].SystemLanguage, relationPairs[i].Code);
                        }
                    }
                    return codeBySystemLanguage;
                }
            }
            
            public static Dictionary<string, SystemLanguage> SystemLanguageByCode {
                get {
                    if (systemLanguageByCode == null) {
                        var relationPairs = RelationPairs;
                        systemLanguageByCode = new Dictionary<string, SystemLanguage>(relationPairs.Length);
                        for (int i = 0; i < relationPairs.Length; i++) {
                            systemLanguageByCode.Add(relationPairs[i].Code, relationPairs[i].SystemLanguage);
                        }
                    }
                    return systemLanguageByCode;
                }
            }
        }

        public Iso639_1LanguageEncoding() {
            Dictionary<SystemLanguage, string> codeMap = EncodingMaps.CodeBySystemLanguage;
            Dictionary<string, SystemLanguage> languageMap = EncodingMaps.SystemLanguageByCode;
        }

        public override string GetCode(SystemLanguage language) {
            Dictionary<SystemLanguage, string> map = EncodingMaps.CodeBySystemLanguage;
            if (map.ContainsKey(language)) return map[language];
            return UnknownLanguageCode;
        }

        public override SystemLanguage GetSystemLanguage(string code) {
            Dictionary<string, SystemLanguage> map = EncodingMaps.SystemLanguageByCode;
            if (map.ContainsKey(code)) return map[code];
            return UnknownSystemLanguage;
        }
    }
}