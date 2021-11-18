using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;

namespace Krugames.LocalizationSystem {
    [CreateAssetMenu(fileName = "LocalizationLibrary", menuName = "Localization/Library")]
    public class Localization : ScriptableObject {

        public const string LocalizationLibraryPath = "Localization/LocalizationLibrary";
        public const string ErrorTermMessage = "error_term";
        public const string LastUsedLanguageKey = "localization_last_used_language";
        public const bool UseLastUsedLanguage = true;

        private static Localization instance;
        public static Localization Instance {
            get {
                if (instance == null) {
                    instance = Resources.Load<Localization>(LocalizationLibraryPath);
                    instance?.Initialize();
#if UNITY_EDITOR
                    if (instance == null) {
                        Debug.Log("There is no LocalizationLibrary at path\"Resources/"+LocalizationLibraryPath+
                                  "/\". Please create one via Context Menu");
                    }
#endif
                }
                return instance;
            }
        }

        [SerializeField]
        private SystemLanguage currentUsedLanguage = SystemLanguage.English;

        public static SystemLanguage CurrentUsedLanguage => Instance.currentUsedLanguage;

        [Serializable]
        public struct LocaleLoadData {
            public TextAsset localeAsset;
            public SystemLanguage referencedLanguage;
        }

        [Tooltip("First element will be used as fallback element")]
        public LocaleLoadData[] localeAssets;

        public Dictionary<SystemLanguage, Locale> locales = new Dictionary<SystemLanguage, Locale>();

        private static Locale currentLocale;

        public delegate void LanguageChangeDelegate();

        private List<LanguageChangeDelegate> languageChangeUpdateCallbacks = new List<LanguageChangeDelegate>();

        private SystemLanguage[] avaliableLanguages;

        public static SystemLanguage[] AvaliableLanguages => Instance.avaliableLanguages;

        private void Initialize() {

            avaliableLanguages = new SystemLanguage[localeAssets.Length];
            locales = new Dictionary<SystemLanguage, Locale>();
            Locale locale;
            for (int i = 0; i < localeAssets.Length; i++) {
                locale = LoadLocale(localeAssets[i]);
                locales.Add(locale.Language, locale);
                avaliableLanguages[i] = locale.Language;
            }

            if (locales.ContainsKey(Application.systemLanguage)) {
                currentUsedLanguage = Application.systemLanguage;
            } else if (localeAssets.Length > 0){
                currentUsedLanguage = localeAssets[0].referencedLanguage;
            }
            
            for (int i = 0; i < languageChangeUpdateCallbacks.Count; i++) {
                languageChangeUpdateCallbacks[i]?.Invoke();
            }

            if (UseLastUsedLanguage) {
                if (PlayerPrefs.HasKey(LastUsedLanguageKey)) {
                    SetLanguage_Private((SystemLanguage)Enum.Parse(typeof(SystemLanguage), PlayerPrefs.GetString(LastUsedLanguageKey)));
                }
            }
        }

        private Locale LoadLocale(LocaleLoadData data) {
            return Locale.FromJson(data.localeAsset.text, data.referencedLanguage);
        }

        public static string GetTerm(string name) {
            if (currentLocale == null || currentLocale.Language != Instance.currentUsedLanguage) { 
                bool result = Instance.locales.TryGetValue(Instance.currentUsedLanguage, out currentLocale);
                if (!result) {
                    Debug.LogWarning("Locale \"" + Instance.currentUsedLanguage + "\" not found");
                    return ErrorTermMessage;
                }
            }

            string term = "";
            bool termResult = currentLocale.Terms.TryGetValue(name, out term);
            if (!termResult) {
                Debug.LogWarning("Term \"" + name + "\" not found");
                return ErrorTermMessage;
            } else {
                return term;
            }
        }

        public static string GetTermFromLanguage(string name, SystemLanguage language) {

            Locale locale;
            bool result = Instance.locales.TryGetValue(language, out locale);
            if (!result) {
                Debug.LogWarning("Locale \"" + Instance.currentUsedLanguage + "\" not found");
                return ErrorTermMessage;
            } else {
                string term = "";
                bool termResult = locale.Terms.TryGetValue(name, out term);
                if (!termResult) {
                    Debug.LogWarning("Term \"" + name + "\" not found");
                    return ErrorTermMessage;
                } else {
                    return term;
                }
            }
            
        }

        private void SetLanguage_Private(SystemLanguage language) {
            if (locales.ContainsKey(language)) {
                currentUsedLanguage = language;
            } else if (localeAssets.Length > 0) {
                currentUsedLanguage = localeAssets[0].referencedLanguage;
            }
            
            for (int i = 0; i < languageChangeUpdateCallbacks.Count; i++) {
                languageChangeUpdateCallbacks[i]?.Invoke();
            }
            PlayerPrefs.SetString(LastUsedLanguageKey, language.ToString());
        }

        public static void SetLanguage(SystemLanguage language) {
            Instance.SetLanguage_Private(language);
        }
        
        private void AddLanguageUpdateCallback_Private([NotNull]LanguageChangeDelegate method) {
            languageChangeUpdateCallbacks.Add(method);
        }
        
        private void RemoveLanguageUpdateCallback_Private([NotNull]LanguageChangeDelegate method) {
            languageChangeUpdateCallbacks.Remove(method);
        }

        public static void AddLanguageUpdateCallback([NotNull]LanguageChangeDelegate method) {
            Instance.AddLanguageUpdateCallback_Private(method);
        }

        public static void RemoveLanguageUpdateCallback([NotNull] LanguageChangeDelegate method) {
            Instance.RemoveLanguageUpdateCallback_Private(method);
        }

        public static void Reinitialize() {

            Localization library = Instance;
            if (library != null) {
                library.Initialize();
            } else {
                Debug.Log("There is no LocalizationLibrary at path\"Resources/"+LocalizationLibraryPath+
                          "/\". Please create one via Context Menu");
            }
        }
    }
}
