using Krugames.LocalizationSystem.Models;
using UnityEngine;

namespace Krugames.LocalizationSystem.Unity.Tools {
    public class LocalizationSwitcher : MonoBehaviour {
#if UNITY_EDITOR
        public SystemLanguage language;
        private SystemLanguage previousLanguage;
        
        private void Start() {
            language = Localization.CurrentLanguage;
            previousLanguage = language;
        }

        private void Update() {
            if (language != previousLanguage) {
                if (!Localization.SetLanguage(language)) {
                    language = Localization.CurrentLanguage;
                }
                previousLanguage = language;
            }
        }
#endif
    }
}

