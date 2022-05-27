using Krugames.LocalizationSystem.Models;
using TMPro;
using UnityEngine;

namespace Krugames.LocalizationSystem.Linkers.Native {
    [AddComponentMenu("Localization/Native Linkers/TMP Text Linker")]
    public class LocalizatedTMPLinker : NativeLinker {
        //TODO add Term selector
        //TODO add not persistent Odin Integration
        
        [SerializeField] private string term = "none"; 
        [SerializeField] private TMP_Text targetText;
        [SerializeField] private TextCase textCase = TextCase.NoChanges;

        public override void UpdateContent() {
            string t =  Localization.GetTermValue<string>(term);
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