using Krugames.LocalizationSystem;
using UnityEngine;

namespace Example.SimpleExample {
    public class LanguageSwitch : MonoBehaviour {

        public void SetLanguage(SystemLanguage language) {
            Localization.SetLanguage(language);
        }

        public void SetEnglish() => SetLanguage(SystemLanguage.English);
        public void SetRussian() => SetLanguage(SystemLanguage.Russian);
        public void SetGerman() => SetLanguage(SystemLanguage.German);
    }
}
