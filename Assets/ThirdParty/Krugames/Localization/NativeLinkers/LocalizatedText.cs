using System;
using Krugames.LocalizationSystem.Models;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace Krugames.LocalizationSystem.Linkers {
    public class LocalizatedText : MonoBehaviour {
        public string term = "some_term";
        public Text text;
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
            if (text == null) return;
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