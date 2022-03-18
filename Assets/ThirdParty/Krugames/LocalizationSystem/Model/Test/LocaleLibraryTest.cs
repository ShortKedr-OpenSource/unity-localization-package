using UnityEngine;

namespace Krugames.LocalizationSystem.Models {
    public class LocaleLibraryTest : MonoBehaviour {
        [SerializeField] private LocaleLibrary localeLibrary;

        private void Start() {
            Debug.Log("Current Language: " + localeLibrary.CurrentLanguage);
            Debug.Log("Current Locale: " + localeLibrary.CurrentLocale);

            var supportedLanguages = localeLibrary.SupportedLanguages;
            var supportedLanguagesPrint = "Supported Languages:\n";
            for (int i = 0; i < supportedLanguages.Length; i++) {
                supportedLanguagesPrint += $"{supportedLanguages[i]}\n";
            }
            Debug.Log(supportedLanguagesPrint);
            
            var dynamicLocales = localeLibrary.DynamicLocales;
            var dynamicLocalesPrint = "Dynamic Locales:\n";
            for (int i = 0; i < dynamicLocales.Length; i++) {
                dynamicLocalesPrint += $"{dynamicLocales[i]} ({dynamicLocales[i].Language})\n";
            }
            Debug.Log(dynamicLocalesPrint);
            
            Debug.Log("Base Valid Locale: " + localeLibrary.BaseValidLocale);

            var validLocales = localeLibrary.ValidLocales;
            var validLocalePrint = "Valid Locales:\n";
            for (int i = 0; i < validLocales.Length; i++) {
                validLocalePrint += $"{validLocales[i]} ({validLocales[i].GetType()})";
            }

            //have result
            Debug.Log("Q1: " + localeLibrary.SupportsLanguage(SystemLanguage.English));
        }
    }
}