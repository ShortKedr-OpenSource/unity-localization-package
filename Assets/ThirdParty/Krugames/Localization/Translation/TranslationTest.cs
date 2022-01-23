using Krugames.LocalizationSystem.Translation.YandexCloud;
using UnityEngine;

namespace Krugames.LocalizationSystem.Translation {
    public class TranslationTest : MonoBehaviour {
        private void Start() {
            new YandexTranslator().Translate("Hello", SystemLanguage.Afrikaans, SystemLanguage.Afrikaans,
                (text, language) => {
                    
                },
                (translate, from, to) => {
                    
                });
        }
    }
}