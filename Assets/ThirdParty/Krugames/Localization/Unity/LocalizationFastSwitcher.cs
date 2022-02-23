using System;
using Krugames.LocalizationSystem.Models;
using UnityEngine;

namespace Krugames.LocalizationSystem.Tools {
    public class LocalizationFastSwitcher : MonoBehaviour {
#if UNITY_EDITOR
        public SystemLanguage language;
        private SystemLanguage previousLanguage;
        
        private void Start() {
            language = Localization.CurrentUsedLanguage;
            previousLanguage = language;
        }

        private void Update() {
            if (language != previousLanguage) {
                Localization.SetLanguage(language);
                previousLanguage = language;
            }
        }
#endif
    }
}

