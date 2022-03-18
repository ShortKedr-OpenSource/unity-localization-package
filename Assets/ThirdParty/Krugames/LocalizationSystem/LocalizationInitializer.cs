using Krugames.LocalizationSystem.Models;
using UnityEngine;

namespace Krugames.LocalizationSystem {
    //TODO rework initializer to ordered type, if execution order is matter
    //TODO change localeLibrary to Localization
    internal static class LocalizationInitializer {
        
        private static readonly string LanguageSaveKey = "LocalizationSystem_LastLang"; 
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize() {
            LocaleLibrary.Instance.OnInitialized += OnInitialized;
            LocaleLibrary.Instance.OnLanguageChanged += OnLanguageChange;
            if (LocalizationSettings.AutoInitialize) LocaleLibrary.Instance.Initialize();
        }

        private static void OnInitialized(LocaleLibrary localeLibrary) {
            
            bool isFirstSession = !PlayerPrefs.HasKey(LanguageSaveKey);

            if (isFirstSession) {
                if (LocalizationSettings.UseSystemLanguageAsDefault) {
                    Localization.SetLanguage(Application.systemLanguage);
                }
            } else {
                if (LocalizationSettings.LoadLastUsedLanguageAsCurrent) {
                    Localization.SetLanguage((SystemLanguage)PlayerPrefs.GetInt(LanguageSaveKey));
                } else if (LocalizationSettings.UseSystemLanguageAsDefault) {
                    Localization.SetLanguage(Application.systemLanguage);
                }
            }
        }

        private static void OnLanguageChange(LocaleLibrary library, SystemLanguage oldLanguage, SystemLanguage newLanguage) {
            Debug.Log("Language was changed");
        }
    }
}