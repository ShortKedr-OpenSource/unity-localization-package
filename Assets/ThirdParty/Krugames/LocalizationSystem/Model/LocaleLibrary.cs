using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Models.Dynamic;
using Krugames.LocalizationSystem.Models.Interfaces;
using UnityEditor;
using UnityEngine;

//TODO Change Base Locale method;
//TODO Add base language

namespace Krugames.LocalizationSystem.Models {
    /// <summary>
    /// Presents main Localization static library.
    /// Library can be extended at runtime with use of DynamicLocale
    /// Localization structure presents in this class
    /// </summary>
    public class LocaleLibrary : Unity.Singletons.ScriptableSingleton<LocaleLibrary>, ICacheCarrier {
        
        private const int DefaultDynamicBuffer = 4;

        [SerializeField] private Locale baseLocale;
        [SerializeField] private Locale[] staticLocales;

        private SystemLanguage _currentLanguage = SystemLanguage.English;
        private ILocale _currentLocale = null;

        private List<ILocale> _validLocales = null;

        private List<Locale> _validStaticLocales = null;

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
        
        /// <summary>
        /// Return current used language
        /// </summary>
        public SystemLanguage CurrentLanguage {
            get {
                if (!_wasInitialized) InitializeInternal();
                return _currentLanguage;
            }
        }

        /// <summary>
        /// Return currently used locale. This locale has language of currently selected language
        /// </summary>
        public ILocale CurrentLocale {
            get {
                if (!_wasInitialized) InitializeInternal();
                return _currentLocale;
            }
        }

        /// <summary>
        /// Return all supported (working) languages
        /// </summary>
        public SystemLanguage[] SupportedLanguages {
            get {
                if (!_wasInitialized) InitializeInternal();
                return _supportedLanguages.ToArray();
            }
        }

        /// <summary>
        /// Return base working locale. This locale can be DynamicLocale or StaticLocale
        /// </summary>
        public ILocale BaseValidLocale {
            get {
                if (!_wasInitialized) InitializeInternal();
                return (_validLocales.Count > 0) ? _validLocales[0] : null;
            }
        }

        /// <summary>
        /// Returns all working locales
        /// </summary>
        public ILocale[] ValidLocales {
            get {
                if (!_wasInitialized) InitializeInternal();
                return _validLocales.ToArray();
            }
        }

        /// <summary>
        /// Return all static working locales
        /// </summary>
        public Locale[] StaticLocales {
            get {
                if (!_wasInitialized) InitializeInternal();
                return _validStaticLocales.ToArray();
            }
        }
        
        /// <summary>
        /// Return all dynamic working locales;
        /// </summary>
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
        
        /// <summary>
        /// Initialize LocaleLibrary. Can be used manually if Localization Settings for Auto Initialize is off.
        /// </summary>
        public void Initialize() {
            if (!_wasInitialized) InitializeInternal();
        }
        
        private void InitializeInternal() {

            int buffer = 1 + staticLocales.Length + DefaultDynamicBuffer;
            
            _validLocales = new List<ILocale>(buffer);

            _validStaticLocales = new List<Locale>(buffer);

            _dynamicLocales = new List<DynamicLocale>(DefaultDynamicBuffer);
            _dynamicLocaleCache = new HashSet<DynamicLocale>();

            _supportedLanguages = new List<SystemLanguage>(buffer);
            _supportedLanguagesCache = new HashSet<SystemLanguage>();

            if (baseLocale != null) {
                _validLocales.Add(baseLocale);
                _validStaticLocales.Add(baseLocale);
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
                _validStaticLocales.Add(staticLocales[i]);
                _supportedLanguages.Add(language);
                _supportedLanguagesCache.Add(language);
                
                staticLocales[i].Initialize();
            }

            _wasInitialized = true;
            
            RebuildCache();

            if (_validLocales.Count > 0) {
                _currentLocale = _validLocales[0];
                _currentLanguage = _validLocales[0].Language;
            } else {
                Debug.LogWarning("No valid Locales available, " +
                                 "LocalizationSystem can not work properly in this scenario");
            }

            OnInitialized?.Invoke(this);
        }
        
        /// <summary>
        /// Rebuild library Cache. Note, this operation can be heavy. Usually Cache don't need to be rebuild
        /// </summary>
        public void RebuildCache() {
            if (!_wasInitialized) InitializeInternal();
            
            _localeByLanguageDict = new Dictionary<SystemLanguage, ILocale>(_validLocales.Count);

            for (int i = 0; i < _validLocales.Count; i++) {
                _localeByLanguageDict.Add(_validLocales[i].Language, _validLocales[i]);
            }
        }

        /// <summary>
        /// Add new dynamic locale to library. Use to add your own locales at runtime.
        /// If game don't use any static locales, first dynamic locale will be set as base
        /// </summary>
        /// <param name="dynamicLocale">dynamic locale to add</param>
        /// <returns>true if locale was added</returns>
        public bool AddDynamicLocale(DynamicLocale dynamicLocale) {
            
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                Debug.LogError("Dynamic Library Update can be performed only in runtime!");
                return false;
            }
#endif
            
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
        
        /// <summary>
        /// Remove dynamic locale from library. Use to remove locales at runtime.
        /// This method removes only dynamic locales. Static locales can not be removed
        /// </summary>
        /// <param name="dynamicLocale">dynamic locale to remove</param>
        /// <returns>true if dynamic locale was removed</returns>
        public bool RemoveDynamicLocale(DynamicLocale dynamicLocale) {
            
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) {
                Debug.LogError("Dynamic Library Update can be performed only in runtime!");
                return false;
            }
#endif
            
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

        
        /// <summary>
        /// Check if specified dynamic locale is contained in library
        /// </summary>
        /// <param name="dynamicLocale">dynamic locale to check</param>
        /// <returns>true if specified dynamic locale contained in library</returns>
        public bool ContainsDynamicLocale(DynamicLocale dynamicLocale) {
            if (!_wasInitialized) InitializeInternal();
            if (dynamicLocale == null) return false;
            return _dynamicLocaleCache.Contains(dynamicLocale);
        }

        /// <summary>
        /// Change current (working) localization language
        /// </summary>
        /// <param name="language">new language</param>
        /// <returns>true if language was changed</returns>
        public bool SetLanguage(SystemLanguage language) {
            if (!_wasInitialized) InitializeInternal();
            if (_currentLanguage == language) return false;
            if (_supportedLanguagesCache.Contains(language)) {
                SystemLanguage oldLanguage = _currentLanguage;
                _currentLanguage = language;
                _currentLocale = _localeByLanguageDict[language];
                OnLanguageChanged?.Invoke(this, oldLanguage, language);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if specified language is supported by library.
        /// </summary>
        /// <param name="language">language to check</param>
        /// <returns>true if specified language is supported by library</returns>
        public bool SupportsLanguage(SystemLanguage language) {
            if (!_wasInitialized) InitializeInternal();
            return _supportedLanguagesCache.Contains(language);
        }

        public ILocale GetLocale(SystemLanguage language) {
            if (!_wasInitialized) InitializeInternal();
            if (_localeByLanguageDict.ContainsKey(language)) {
                return _localeByLanguageDict[language];
            }
            return null;
        }

        /// <summary>
        /// Get locale term from current (working) locale
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <returns>returns LocaleTerm reference if it's exists, otherwise return null</returns>
        public LocaleTerm GetTerm(string term) {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTerm(term);
        }
        
        /// <summary>
        /// Get locale term of specified type from current (working) locale
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <param name="type">target term type</param>
        /// <returns>returns LocaleTerm reference if it's exists and has specified type, otherwise return null</returns>
        public LocaleTerm GetTerm(string term, Type type) {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTerm(term, type);
        }

        /// <summary>
        /// Get locale term of specified type from current (working) locale
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <typeparam name="TTermType">target term type</typeparam>
        /// <returns>return TTermType reference if it's exists and match specified type, otherwise return null</returns>
        public TTermType GetTerm<TTermType>(string term) where TTermType : LocaleTerm {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTerm<TTermType>(term);
        }

        /// <summary>
        /// Get locale term from specified language' locale 
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <param name="language">target locale language</param>
        /// <returns>return LocaleTerm reference if locale of specified language exists and term also exists,
        /// otherwise return null</returns>
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

        /// <summary>
        /// Get locale term of specified type from specified language' locale
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <param name="type">target term type</param>
        /// <param name="language">target locale language</param>
        /// <returns>returns LocaleTerm reference if locale of specified language exists,
        /// and term of target type also exists, otherwise return null</returns>
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
        
        /// <summary>
        /// Get locale term of specified type from specified language' locale
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <param name="language">target locale language</param>
        /// <typeparam name="TTermType">target term type</typeparam>
        /// <returns>returns TTermType reference if locale of specified language exists
        /// and term of target type also exists, otherwise return null</returns>
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

        /// <summary>
        /// Get locale term value from current (working) locale 
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <returns>returns object reference if term exists</returns>
        public object GetTermValue(string term) {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTermValue(term);
        }

        /// <summary>
        /// Get locale term value of specified type from current (working) locale 
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <param name="type">target term value type</param>
        /// <returns>returns object reference if term of target type exists, otherwise return null</returns>
        public object GetTermValue(string term, Type type) {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTermValue(term, type);
        }

        /// <summary>
        /// Get locale term value of specified type from current (working) locale
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <typeparam name="TTermValueType">target term value type</typeparam>
        /// <returns>returns TTermValueType reference if term of target type exists, otherwise return null</returns>
        public TTermValueType GetTermValue<TTermValueType>(string term) {
            if (!_wasInitialized) InitializeInternal();
            return _currentLocale.GetTermValue<TTermValueType>(term);
        }
        
        /// <summary>
        /// Get locale term value from specified language' locale
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <param name="language">target locale language</param>
        /// <returns>returns object reference if target locale exists and term also exists, otherwise return null</returns>
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

        /// <summary>
        /// Get locale term value of specified type from specified language' locale
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <param name="type">target term value type</param>
        /// <param name="language">target locale language</param>
        /// <returns>return object reference if target locale exists
        /// and term of target type also exists, otherwise return null</returns>
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

        /// <summary>
        /// Get locale term value of specified type from specified language' locale
        /// </summary>
        /// <param name="term">term name (identifier)</param>
        /// <param name="language">target locale language</param>
        /// <typeparam name="TTermValueType">target term value type</typeparam>
        /// <returns>return TTermValueType reference if target locale exists
        /// and term of target type also exists, otherwise return null</returns>
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