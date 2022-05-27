using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Terms;
using UnityEngine;
using UnityEngine.UI;

namespace Krugames.LocalizationSystem.Linkers.Native {
    [AddComponentMenu("Localization/Native Linkers/Text Linker")]
    public class LocalizatedTextLinker : NativeLinker {
        //TODO add Term selector
        //TODO add not persistent Odin Integration
        
        [SerializeField] private string term = "none"; 
        [SerializeField] private Text targetText;
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