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

        private SystemLanguage _currentLanguage = SystemLanguage.English;
        private ILocale _currentLocale = null;

        private List<ILocale> _validLocales = null;
        
        private List<DynamicLocale> _dynamicLocales = null;
        private HashSet<DynamicLocale> _dynamicLocaleCache = null;

        private List<SystemLanguage> _supportedLanguages = null;
        private HashSet<SystemLanguage> _supportedLanguagesCache = null;
        
        private Dictionary<SystemLanguage, ILocale> _localeByLanguageDict = null;

        private bool _wasInitialized = false;

        
        public delegate void CallbackDelegate(LocaleLibrary localeLibrary);
        public delegate void LanguageChangeDelegate(LocaleLibrary localeLibrary, SystemLanguage oldLanguage, SystemLanguage newLanguage);

        public event CallbackDelegate OnInitialized;
        public event LanguageChangeDelegate OnLanguageChanged;
        
            
        public SystemLanguage CurrentLanguage {
            get {
                if (!_wasInitialized) InitializeInternal();
                return _currentLanguage;
            }
        }

        public ILocale CurrentLocale {
            get {
                if (!_wasInitialized) InitializeInternal();
                return _currentLocale;
            }
        }

        public SystemLanguage[] SupportedLanguages {
            get {
                if (!_wasInitialized) InitializeInternal();
                return _supportedLanguages.ToArray();
            }
        }

        public ILocale BaseValidLocale {
            get {
                if (!_wasInitialized) InitializeInternal();
                return (_validLocales.Count > 0) ? _validLocales[0] : null;
            }
        }

        public ILocale[] ValidLocales {
            get {
                if (!_wasInitialized) InitializeInternal();
                return _validLocales.ToArray();
            }
        }

        public DynamicLocale[] DynamicLocales {
            get {
                if (!_wasInitialized) InitializeInternal();
                return _dynamicLocales.ToArray();
            }
        }


        private void OnEnable() {
#if UNITY_EDITOR
            _wasInitialized = false;
#endif
        }

        private void DebugUnsupportedLanguage(SystemLanguage language) {
            Debug.LogWarning($"Language \"{language}\" is unsupported. " +
                             $"Add locale for specified language to make it supportable");
        }
        
        public void Initialize() {
            if (!_wasInitialized) InitializeInternal();
        }
        
        private void InitializeInternal() {

            int buffer = 1 + staticLocales.Length + DefaultDynamicBuffer;
            
            _validLocales = new List<ILocale>(buffer);

            _dynamicLocales = new List<DynamicLocale>(DefaultDynamicBuffer);
            _dynamicLocaleCache = new HashSet<DynamicLocale>();

            _supportedLanguages = new List<SystemLanguage>(buffer);
            _supportedLanguagesCache = new HashSet<SystemLanguage>();

            if (baseLocale != null) {
                _validLocales.Add(baseLocale);
                _supportedLanguages.Add(baseLocale.Language);
                _supportedLanguagesCache.Add(baseLocale.Language);
            } else {
                Debug.LogWarning("Base locale is null. First valid locale will be used as Base instead");
            }
            
            for (int i = 0; i < staticLocales.Length; i++) {
                if (staticLocales[i] == null) {
                    Debug.LogWarning("Static locale is null. Reference will be skipped");
                    continue;
                }

                SystemLanguage language = staticLocales[i].Language;
                if (_supportedLanguagesCache.Contains(language)) {
                    Debug.LogWarning($"{language} language of \"{staticLocales[i].name}\" locale is already used. " +
                                   $"Locale will be skipped. Language or locale duplication is not allowed");
                    continue;
                }

                _validLocales.Add(staticLocales[i]);
                _supportedLanguages.Add(language);
                _supportedLanguagesCache.Add(language);
                
                staticLocales[i].Initialize();
            }

            RebuildCache();

            if (_validLocales.Count > 0) {
                _currentLocale = _validLocales[0];
                _currentLanguage = _validLocales[0].Language;
            } else {
                Debug.LogWarning("No valid Locales available, " +
                                 "LocalizationSystem can not work properly in this scenario");
            }

            _wasInitialized = true;
            
            OnInitialized?.Invoke(this);
        }
        
        public void RebuildCache() {

            _localeByLanguageDict = new Dictionary<SystemLanguage, ILocale>(_validLocales.Count);

            for (int i = 0; i < _validLocales.Count; i++) {
                _localeByLanguageDict.Add(_validLocales[i].Language, _validLocales[i]);
            }
        }

        public bool AddDynamicLocale(DynamicLocale dynamicLocale) {
            if (!_wasInitialized) InitializeInternal();
            
            if (dynamicLocale == null) return false;

            SystemLanguage language = dynamicLocale.Language;
            if (_supportedLanguagesCache.Contains(language)) {
                Debug.LogWarning("Dynamic locale uses language that already exists. This locale will not be added");
                return false;
            }

            if (_dynamicLocaleCache.Contains(dynamicLocale)) {
                Debug.LogWarning("Specified dynamic locale already added. Locales can not be added twice");
                return false;
            }

            _validLocales.Add(dynamicLocale);
            
            _dynamicLocales.Add(dynamicLocale);
            _dynamicLocaleCache.Add(dynamicLocale);

            _supportedLanguages.Add(language);
            _supportedLanguagesCache.Add(language);
            
            _localeByLanguageDict.Add(language, dynamicLocale);
            return true;
        }
        
        public bool RemoveDynamicLocale(DynamicLocale dynamicLocale) {
            if (!_wasInitialized) InitializeInternal();
            
            if (dynamicLocale == null) return false;
            if (!_dynamicLocaleCache.Contains(dynamicLocale)) return false;

            SystemLanguage language = dynamicLocale.Language;
            
            _validLocales.Remove(dynamicLocale);
            
            _dynamicLocales.Remove(dynamicLocale);
            _dynamicLocaleCache.Remove(dynamicLocale);

            _supportedLanguages.Remove(language);
            _supportedLanguagesCache.Remove(language);
            
            _localeByLanguageDict.Remove(language);
            
            return true;
        }

        public bool ContainsDynamicLocale(DynamicLocale dynamicLocale) {
            if (!_wasInitialized) InitializeInternal();
            if (dynamicLocale == null) return false;
            return _dynamicLocaleCache.Contains(dynamicLocale);
        }

        /// <summary>
        /// Change current localization language
        /// </summary>
        /// <param name="language">target language</param>
        /// <returns>true if language was changed. This method will also
        /// return true if current language and target language are equal</returns>
        public bool SetLanguage(SystemLanguage language) {
            if (!_wasInitialized) InitializeInternal();
            if (_currentLanguage == language) return true;
            if (_supportedLanguagesCache.Contains(language)) {
                SystemLanguage oldLanguage = _currentLanguage;
                _currentLanguage = language;
                _currentLocale = _localeByLanguageDict[language];
                OnLanguageChanged?.Invoke(this, oldLanguage, language);
                return true;
            }
            return false;
        }

        public bool SupportsLanguage(SystemLanguage language) {
            if (!_wasInitialized) InitializeInternal();
            return _supportedLanguagesCache.Contains(language);
        }

        public LocaleTerm GetTerm(string term) {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTerm(term);
        }

        public LocaleTerm GetTerm(string term, Type type) {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTerm(term, type);
        }

        public TTermType GetTerm<TTermType>(string term) where TTermType : LocaleTerm {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTerm<TTermType>(term);
        }

        public LocaleTerm GetTerm(string term, SystemLanguage language) {
            if (!_wasInitialized) InitializeInternal();
            if (_supportedLanguagesCache.Contains(language)) {
                return _localeByLanguageDict[language].GetTerm(term);
            }
#if UNITY_EDITOR
            DebugUnsupportedLanguage(language);
#endif
            return null;
        }

        public LocaleTerm GetTerm(string term, Type type, SystemLanguage language) {
            if (!_wasInitialized) InitializeInternal();
            if (_supportedLanguagesCache.Contains(language)) {
                return _localeByLanguageDict[language].GetTerm(term, type);
            }
#if UNITY_EDITOR
            DebugUnsupportedLanguage(language);
#endif
            return null;
        }
        
        public TTermType GetTerm<TTermType>(string term, SystemLanguage language) where TTermType : LocaleTerm {
            if (!_wasInitialized) InitializeInternal();
            if (_supportedLanguagesCache.Contains(language)) {
                return _localeByLanguageDict[language].GetTerm<TTermType>(term);
            }
#if UNITY_EDITOR
            DebugUnsupportedLanguage(language);
#endif
            return null;
        }

        public object GetTermValue(string term) {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTermValue(term);
        }

        public object GetTermValue(string term, Type type) {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTermValue(term, type);
        }

        public TTermValueType GetTermValue<TTermValueType>(string term) {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTermValue<TTermValueType>(term);
        }
        
        public object GetTermValue(string term, SystemLanguage language) {
            if (!_wasInitialized) InitializeInternal();
            if (_supportedLanguagesCache.Contains(language)) {
                return _localeByLanguageDict[language].GetTermValue(term);
            }
#if UNITY_EDITOR
            DebugUnsupportedLanguage(language);
#endif
            return null;
        }

        public object GetTermValue(string term, Type type, SystemLanguage language) {
            if (!_wasInitialized) InitializeInternal();
            if (_supportedLanguagesCache.Contains(language)) {
                return _localeByLanguageDict[language].GetTermValue(term, type);
            }
#if UNITY_EDITOR
            DebugUnsupportedLanguage(language);
#endif
            return null;
        }

        public TTermValueType GetTermValue<TTermValueType>(string term, SystemLanguage language) {
            if (!_wasInitialized) InitializeInternal();
            if (_supportedLanguagesCache.Contains(language)) {
                return _localeByLanguageDict[language].GetTermValue<TTermValueType>(term);
            }
#if UNITY_EDITOR
            DebugUnsupportedLanguage(language);
#endif
            return default;
        }

    }
}