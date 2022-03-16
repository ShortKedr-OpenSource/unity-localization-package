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
    public class LocaleLibrary : ScriptableSingleton<LocaleLibrary>, ICacheCarrier {

        private const int DefaultDynamicBuffer = 4;
        
        [SerializeField] private Locale baseLocale;
        [SerializeField] private Locale[] staticLocales;

        private SystemLanguage currentLanguage = SystemLanguage.English;
        
        private List<DynamicLocale> _dynamicLocales;
        private HashSet<DynamicLocale> _dynamicLocaleCache;
        
        private HashSet<SystemLanguage> _existsLanguagesCache = new HashSet<SystemLanguage>();
        private List<SystemLanguage> _supportedLanguages = new List<SystemLanguage>();

        private Dictionary<SystemLanguage, ILocale> _localeByLanguageDict = new Dictionary<SystemLanguage, ILocale>();

        private bool _wasInitialized = false;
        

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
            _wasInitialized = false;
#endif
            if (LocalizationSettings.AutoInitialize) Initialize();
        }

        public void Initialize() {
            
            //TODO Initialization
            
            RebuildCache();
            
            _wasInitialized = true;
        }
        
        public void RebuildCache() {
        }

        public bool AddDynamicLocale(DynamicLocale dynamicLocale) {
            throw new NotImplementedException();
        }
        
        public bool RemoveDynamicLocale(DynamicLocale dynamicLocale) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Change current localization language
        /// </summary>
        /// <param name="language">target language</param>
        /// <returns>true if language was changed. This method will also
        /// return true if current language and target language are equal</returns>
        public bool SetLanguage(SystemLanguage language) {
            return false;
        }

        
    }
}