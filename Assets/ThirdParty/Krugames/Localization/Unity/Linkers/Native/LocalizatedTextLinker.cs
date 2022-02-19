using Krugames.LocalizationSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Krugames.LocalizationSystem.Linkers.Native {
    public class LocalizatedTextLinker : NativeLinker {
        //TODO add Term selector
        //TODO add not persistent Odin Integation
        
        [SerializeField] private string term; 
        [SerializeField] private Text targetText;
        [SerializeField] private TextCase textCase = TextCase.NoChanges;

        public override void UpdateContent() {
            string t =  Localization.GetTerm(term);
            if (targetText == null) return;
            if (textCase == TextCase.NoChanges) {
                targetText.text = t;
            } else if (textCase == TextCase.UpperCase) {
                targetText.text = t.ToUpper();
            }  else if (textCase == TextCase.LowerCase) {
                targetText.text = t.ToLower();
            }
        }
    }
}