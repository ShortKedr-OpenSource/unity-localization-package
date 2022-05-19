using System;
using System.Collections.Generic;
using System.Reflection;
using Krugames.LocalizationSystem.Models.Attributes;
using Krugames.LocalizationSystem.Models.Utility;

namespace Krugames.LocalizationSystem.Models.Locators {
    public static class LocaleTermLocator {
        
        public class LocaleTermBuildData {
            
            private RegisterLocaleTermAttribute _registerAttribute;
            public readonly Type ValueType;
            
            public Type TermType => _registerAttribute.TermType;
            public string Name => _registerAttribute.Name;

            public LocaleTermBuildData(RegisterLocaleTermAttribute registerAttribute) {
                _registerAttribute = registerAttribute;
                ValueType = LocaleTermUtility.GetValueTypeOfGenericTermType(registerAttribute.TermType);
            }
        }
        
        private const int DefaultBuffer = 32;
        
        private static RegisterLocaleTermAttribute[] _locators;
        private static LocaleTermBuildData[] _buildData;

        private static Dictionary<Type, Type> _termTypeByValueType;

        private static bool _isInitialized = false;

        /// <summary>
        /// Returns LocaleTerm register attributes
        /// </summary>
        public static RegisterLocaleTermAttribute[] Locators {
            get {
                if (!_isInitialized) Initialize();
                return _locators;
            }
        }

        /// <summary>
        /// Returns LocaleTerm full build data, that was made from register data.
        /// BuildData is preferred to use, compare to Locators
        /// </summary>if (_isInitialized) Initialize();
        public static LocaleTermBuildData[] BuildData {
            get {
                if (!_isInitialized) Initialize();
                return _buildData;
            }
        }

        private static void Initialize() {
            
            List<RegisterLocaleTermAttribute> locators = new List<RegisterLocaleTermAttribute>(DefaultBuffer);
            List<LocaleTermBuildData> buildData = new List<LocaleTermBuildData>(DefaultBuffer);
            
            foreach (ICustomAttributeProvider assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                var attributes = assembly.GetCustomAttributes(typeof(RegisterLocaleTermAttribute), false);

                for (int i = 0; i < attributes.Length; i++) {
                    if (attributes[i] is RegisterLocaleTermAttribute registrator) {
                        if (!registrator.IsValid) continue;
                        locators.Add(registrator);
                        buildData.Add(new LocaleTermBuildData(registrator));
                    }
                }
            }

            _locators = locators.ToArray();
            _buildData = buildData.ToArray();

            _termTypeByValueType = new Dictionary<Type, Type>(_buildData.Length);
            for (int i = 0; i < _buildData.Length; i++) {
                _termTypeByValueType.Add(_buildData[i].ValueType, _buildData[i].TermType);
            }
            
            _isInitialized = true;
        }

        public static Type GetTermTypeByValueType(Type valueType) {
            if (!_isInitialized) Initialize();
            if (_termTypeByValueType.ContainsKey(valueType)) return _termTypeByValueType[valueType];
            return null;
        }
    }
}