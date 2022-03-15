using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Unity.Singletons;
using Krugames.LocalizationSystem.Models.Dynamic;
using Krugames.LocalizationSystem.Models.Interfaces;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models {
    /// <summary>
    /// Presents main Localization static library.
    /// Localization structure presents in this class
    /// </summary>
    //TODO change approach to LocaleLibraries + LocalizationService, if multiple LocaleLibraries will be required
    public class LocaleLibrary : ScriptableSingleton<LocaleLibrary> {

        [SerializeField] private Locale baseLocale;
        [SerializeField] private Locale[] staticLocales;
        private List<DynamicLocale> _dynamicLocales;

        private HashSet<SystemLanguage> _existsLanguagesCache = new HashSet<SystemLanguage>();
        private List<SystemLanguage> _supportedLanguages = new List<SystemLanguage>();

        private Dictionary<SystemLanguage, ILocale> _localeByLanguageDict = new Dictionary<SystemLanguage, ILocale>();

        public Locale BaseLocale => baseLocale;
        public Locale[] StaticLocales => staticLocales;
        public DynamicLocale[] DynamicLocales => _dynamicLocales.ToArray();

        public SystemLanguage[] SupportedLanguages => _supportedLanguages.ToArray();

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

        public bool AddDynamicLocale(DynamicLocale dynamicLocale) {
            throw new NotImplementedException();
        }
    }
}