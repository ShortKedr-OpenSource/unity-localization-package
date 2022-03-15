using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Krugames.LocalizationSystem.Models.Attributes;
using Krugames.LocalizationSystem.Models.Utility.Editor;
using Krugames.LocalizationSystem.Translation.Attributes;
using Debug = UnityEngine.Debug;

namespace Krugames.LocalizationSystem.Models.Locators {
    public static class LocaleTermLocator {
        
        private const int DefaultBuffer = 32;
        
        private static RegisterLocaleTermAttribute[] _locators;
        private static LocaleTermBuildData[] _buildData;
        
        private static bool _isInitialized = false;

        /// <summary>
        /// Returns LocaleTerm register attributes
        /// </summary>
        public static RegisterLocaleTermAttribute[] Locators {
            get {
                if (!_isInitialized) Initialize();
                return _locators;
                //TODO Gather info once, return many of the same
            }
        }

        /// <summary>
        /// Returns LocaleTerm full build data, that made from register data.
        /// Build data is preferred to use compare to Locators
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
            _isInitialized = true;
        }
        
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
    }
}