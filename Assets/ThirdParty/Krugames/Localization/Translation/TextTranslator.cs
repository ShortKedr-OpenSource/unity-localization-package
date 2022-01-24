using UnityEngine;

namespace Krugames.LocalizationSystem.Translation {

    public delegate void TranslationSuccessDelegate(string translatedText, SystemLanguage language);
    public delegate void TranslationFailDelegate(string textToTranslate, SystemLanguage from, SystemLanguage to);
    
    public abstract class TextTranslator {
        
        public abstract void Translate(string textToTranslate, SystemLanguage from, SystemLanguage to, 
            TranslationSuccessDelegate successCallback, TranslationFailDelegate failCallback);
        
    }
}