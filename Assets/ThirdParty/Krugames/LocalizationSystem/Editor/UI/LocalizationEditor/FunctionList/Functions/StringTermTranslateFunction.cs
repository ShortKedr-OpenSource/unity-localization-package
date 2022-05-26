using System;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Terms;
using Krugames.LocalizationSystem.Translation;
using Krugames.LocalizationSystem.Translation.RapidApi;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.Functions {
    public class StringTermTranslateFunction : IFunction {
        
        public static readonly StringTermTranslateFunction Invalid = 
            new StringTermTranslateFunction(null, default, null, default);
        
        private StringTerm _fromTerm;
        private SystemLanguage _fromLanguage;
        private StringTerm _toTerm;
        private SystemLanguage _toLanguage;

        public delegate void SuccessDelegate(StringTerm changedTerm);
        public event SuccessDelegate OnSuccess;
        
        public StringTerm FromTerm => _fromTerm;

        public SystemLanguage FromLanguage => _fromLanguage;

        public StringTerm ToTerm => _toTerm;

        public SystemLanguage ToLanguage => _toLanguage;

        public bool IsValid => _fromTerm != null && _toTerm != null && _fromLanguage != _toLanguage;
        
        public string Description => $"Translate from {_fromLanguage} to {_toLanguage}";
        
        public StringTermTranslateFunction(StringTerm fromTerm, SystemLanguage fromLanguage, StringTerm toTerm, SystemLanguage toLanguage) {
            _fromTerm = fromTerm;
            _fromLanguage = fromLanguage;
            _toTerm = toTerm;
            _toLanguage = toLanguage;
        }

        public void Execute() {
            //TODO GetCurrentTranslator, not concrete
            StringTranslator translator = new RapidApiGoogleTranslator();
            translator.Translate(_fromTerm.SmartValue, _fromLanguage, _toLanguage, TranslateSuccessCallback, TranslateFailCallback);
        }

        private void TranslateFailCallback(string data, SystemLanguage from, SystemLanguage to) {
            Debug.LogError($"Unable to translate \"{data}\" from {from} to {to}");
        }

        private void TranslateSuccessCallback(string translatedData, SystemLanguage language, SystemLanguage translatedFrom) {
            _toTerm.SetSmartValue(translatedData);
            EditorUtility.SetDirty(_toTerm);
            OnSuccess?.Invoke(_toTerm);
        }
    }
}