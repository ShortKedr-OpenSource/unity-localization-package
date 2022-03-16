using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Unity.Singletons;
using Krugames.LocalizationSystem.Models.Dynamic;
using Krugames.LocalizationSystem.Models.Interfaces;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models {
    /// <summary>
    /// Presents main Localization static library.
    /// Library can be extended at runtime with use of DynamicLocale
    /// Localization structure presents in this class
    /// </summary>
    public class LocaleLibrary : ScriptableSingleton<LocaleLibrary> {

        private const int DefaultDynamicBuffer = 4;
        
        [SerializeField] private Locale baseLocale;
        [SerializeField] private Locale[] staticLocales;

        private List<DynamicLocale> _dynamicLocales;
        private HashSet<DynamicLocale> _dynamicLocaleCache;
        
        private HashSet<SystemLanguage> _existsLanguagesCache = new HashSet<SystemLanguage>();
        private List<SystemLanguage> _supportedLanguages = new List<SystemLanguage>();

        private Dictionary<SystemLanguage, ILocale> _localeByLanguageDict = new Dictionary<SystemLanguage, ILocale>();
        
        

        public bool IsValid {
            get {
                HashSet<SystemLanguage> languagesCache = new HashSet<SystemLanguage>();
                languagesCache.Add(baseLocale.Language);
                for (int i = 0; i < _supportedLanguages.Count; i++) {
                    if (languagesCache.Contains(_supportedLanguages[i])) return false;
                    languagesCache.Add(_supportedLanguages[i]);
                }
                return true;
            }
        }

        private void OnEnable() {
#if UNITY_EDITOR
            //TODO reset initialization if in editor
#endif
        }

        public bool AddDynamicLocale(DynamicLocale dynamicLocale) {
            throw new NotImplementedException();
        }
        
        public bool RemoveDynamicLocale(DynamicLocale dynamicLocale) {
            throw new NotImplementedException();
        }
    }
}