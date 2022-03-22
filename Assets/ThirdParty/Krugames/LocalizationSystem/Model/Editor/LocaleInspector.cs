using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Terms;
using Krugames.LocalizationSystem.Models.Utility.Editor;
using UnityEditor;
using UnityEngine;

namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor {
    [CustomEditor(typeof(Locale))]
    public class LocaleInspector : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI(); //TODO remove

            Locale locale = (Locale)target;
            
            if (GUILayout.Button("AddTerm")) {
                LocaleUtility.AddLocaleTerm(locale, "new_term", typeof(AudioClipTerm));
            }
            
            if (GUILayout.Button("Remove Term")) {
                LocaleUtility.RemoveLocaleTerm(locale, "new_term");
            }
        }
    }
}