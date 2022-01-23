using Krugames.LocalizationSystem.Models;
using TMPro;
using UnityEngine;

namespace Krugames.LocalizationSystem.Linkers {
    public class LocalizatedTMP : MonoBehaviour {
        public string term = "localizated_term";
        public TMP_Text text;
        public TextCase textCase = TextCase.NoChanges;
        
        private void Awake() {
            AssignLocalizationTerms();
            Localization.AddLanguageUpdateCallback(AssignLocalizationTerms);
        }

        private void OnDestroy() {
            Localization.RemoveLanguageUpdateCallback(AssignLocalizationTerms);
        }
        
        private void AssignLocalizationTerms() {
            string t =  Localization.GetTerm(term);
            if (textCase == TextCase.NoChanges) {
                text.text = t;
            } else if (textCase == TextCase.UpperCase) {
                text.text = t.ToUpper();
            }  else if (textCase == TextCase.LowerCase) {
                text.text = t.ToLower();
            }
        }
    }
}