using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Translation.Attributes;

namespace Krugames.LocalizationSystem.Translation.Utility {
    public class TranslatorLocator {
        private static bool _wasLocatorInitialized = false;
        
        private static RegisterTranslatorAttribute[] _allLocators = Array.Empty<RegisterTranslatorAttribute>();
        private static Dictionary<Type, RegisterTranslatorAttribute[]> _locatorsByBaseType =
            new Dictionary<Type, RegisterTranslatorAttribute[]>(16);

        public static RegisterTranslatorAttribute[] Locators {
            get {
                if (!_wasLocatorInitialized) Initialize();
                RegisterTranslatorAttribute[] newLocatorArray = new RegisterTranslatorAttribute[_allLocators.Length];
                Array.Copy(_allLocators, newLocatorArray, _allLocators.Length);
                return newLocatorArray;
            }
        }

        public static RegisterTranslatorAttribute[] GetLocators(Type translatorBaseType) {
            if (!_wasLocatorInitialized) Initialize();
            
            if (translatorBaseType == RegisterTranslatorAttribute.BaseType) return Locators;

            throw new NotImplementedException();
        }

        public static RegisterTranslatorAttribute[] GetLocators<TBaseType>() {
            Type baseTranslatorType = typeof(TBaseType);
            return GetLocators(baseTranslatorType);
        }

        private static void Initialize() {
            //TODO implement;
            
            _wasLocatorInitialized = true;
        }
    }
}