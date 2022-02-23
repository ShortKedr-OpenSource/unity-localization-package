using System;
using UnityEngine;

namespace Krugames.LocalizationSystem.Translation {

    public abstract class Translator {

        public delegate void AbstractTranslationSuccessDelegate(object translatedData, SystemLanguage language, SystemLanguage translatedFrom);
        public delegate void AbstractTranslationFailDelegate(object data, SystemLanguage from, SystemLanguage to);
        
        public abstract void Translate(object data, SystemLanguage from, SystemLanguage to,
            AbstractTranslationSuccessDelegate successCallback, AbstractTranslationFailDelegate failCallback);
    }

    public abstract class Translator<TDataType> : Translator {
        
        public delegate void TranslationSuccessDelegate(TDataType translatedData, SystemLanguage language, SystemLanguage translatedFrom);
        public delegate void TranslationFailDelegate(TDataType data, SystemLanguage from, SystemLanguage to);

        protected static readonly Type VALUE_TYPE = typeof(TDataType);

        public override void Translate(object data, SystemLanguage from, SystemLanguage to, AbstractTranslationSuccessDelegate successCallback,
            AbstractTranslationFailDelegate failCallback) {

            void SuccessCallback(TDataType _translatedData, SystemLanguage _language, SystemLanguage _translatedFrom) {
                successCallback?.Invoke(_translatedData, _language, _translatedFrom);
            }

            void FailCallback(TDataType _data, SystemLanguage _from, SystemLanguage _to) {
                failCallback?.Invoke(_data, _from, _to);
            }
            
            if (data is TDataType revealedData) {
                Translate(revealedData, from, to, SuccessCallback, FailCallback);
            } else {
                Debug.LogError("Unable to translate data.");
            }
            
        }

        public abstract void Translate(TDataType data, SystemLanguage from, SystemLanguage to, 
            TranslationSuccessDelegate successCallback, TranslationFailDelegate failCallback);

    }
}