using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Krugames.LocalizationSystem.Translation.LanguageEncoding {
    /// <summary>
    /// Language encoding for ISO 639-1 standard
    /// </summary>
    public class Iso639_1LanguageEncoding : SystemLanguageEncoding {

        public static readonly SystemLanguageCodePair[] RelationPairs = new[] {
            new SystemLanguageCodePair(SystemLanguage.Afrikaans, ""),
            new SystemLanguageCodePair(SystemLanguage.Arabic, ""),
            new SystemLanguageCodePair(SystemLanguage.Basque, ""),
            new SystemLanguageCodePair(SystemLanguage.Belarusian, ""),
            new SystemLanguageCodePair(SystemLanguage.Bulgarian, ""),
            new SystemLanguageCodePair(SystemLanguage.Catalan, ""),
            new SystemLanguageCodePair(SystemLanguage.Chinese, ""),
            new SystemLanguageCodePair(SystemLanguage.Czech, ""),
            new SystemLanguageCodePair(SystemLanguage.Danish, ""),
            new SystemLanguageCodePair(SystemLanguage.Dutch, ""),
            new SystemLanguageCodePair(SystemLanguage.English, ""),
            new SystemLanguageCodePair(SystemLanguage.Estonian, ""),
            new SystemLanguageCodePair(SystemLanguage.Faroese, ""),
            new SystemLanguageCodePair(SystemLanguage.Finnish, ""),
            new SystemLanguageCodePair(SystemLanguage.French, ""),
            new SystemLanguageCodePair(SystemLanguage.German, ""),
            new SystemLanguageCodePair(SystemLanguage.Greek, ""),
            new SystemLanguageCodePair(SystemLanguage.Hebrew, ""),
            new SystemLanguageCodePair(SystemLanguage.Icelandic, ""),
            new SystemLanguageCodePair(SystemLanguage.Indonesian, ""),
            new SystemLanguageCodePair(SystemLanguage.Italian, ""),
            new SystemLanguageCodePair(SystemLanguage.Japanese, ""),
            new SystemLanguageCodePair(SystemLanguage.Korean, ""),
            new SystemLanguageCodePair(SystemLanguage.Latvian, ""),
            new SystemLanguageCodePair(SystemLanguage.Lithuanian, ""),
            new SystemLanguageCodePair(SystemLanguage.Norwegian, ""),
            new SystemLanguageCodePair(SystemLanguage.Polish, ""),
            new SystemLanguageCodePair(SystemLanguage.Portuguese, ""),
            new SystemLanguageCodePair(SystemLanguage.Romanian, ""),
            new SystemLanguageCodePair(SystemLanguage.Russian, ""),
            new SystemLanguageCodePair(SystemLanguage.SerboCroatian, ""),
            new SystemLanguageCodePair(SystemLanguage.Slovak, ""),
            new SystemLanguageCodePair(SystemLanguage.Slovenian, ""),
            new SystemLanguageCodePair(SystemLanguage.Spanish, ""),
            new SystemLanguageCodePair(SystemLanguage.Swedish, ""),
            new SystemLanguageCodePair(SystemLanguage.Thai, ""),
            new SystemLanguageCodePair(SystemLanguage.Turkish, ""),
            new SystemLanguageCodePair(SystemLanguage.Ukrainian, ""),
            new SystemLanguageCodePair(SystemLanguage.Vietnamese, ""),
            new SystemLanguageCodePair(SystemLanguage.ChineseSimplified, ""),
            new SystemLanguageCodePair(SystemLanguage.ChineseTraditional, ""),
            new SystemLanguageCodePair(SystemLanguage.Hungarian, ""),
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