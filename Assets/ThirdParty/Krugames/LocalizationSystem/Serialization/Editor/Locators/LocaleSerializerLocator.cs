using System;
using System.Collections.Generic;
using System.Reflection;
using Krugames.LocalizationSystem.Editor.Serialization.Attributes;

namespace Krugames.LocalizationSystem.Editor.Serialization.Locators {
    public static class LocaleSerializerLocator {

        private const int DefaultBuffer = 8;

        private static RegisterLocaleSerializerAttribute[] _locators;
        private static LocaleSerializerBuildData[] _buildData;

        private static bool _isInitialized = false;

        /// <summary>
        /// Return LocaleSerializer register attributes
        /// </summary>
        public static RegisterLocaleSerializerAttribute[] Locators {
            get {
                if (!_isInitialized) Initialize();
                return _locators;
            }
        }

        /// <summary>
        /// Return LocaleSerializer full build data, that was made from register data.
        /// BuildData is preferred to use, compare to Locators
        /// </summary>
        public static LocaleSerializerBuildData[] BuildData {
            get {
                if (!_isInitialized) Initialize();
                return _buildData;
            }    
        }
        
        private static void Initialize() {
            List<RegisterLocaleSerializerAttribute> locators = new List<RegisterLocaleSerializerAttribute>(DefaultBuffer);
            List<LocaleSerializerBuildData> buildData = new List<LocaleSerializerBuildData>(DefaultBuffer);
            
            foreach (ICustomAttributeProvider assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                var attributes = assembly.GetCustomAttributes(typeof(RegisterLocaleSerializerAttribute), false);

                for (int i = 0; i < attributes.Length; i++) {
                    if (attributes[i] is RegisterLocaleSerializerAttribute registrar) {
                        if (!registrar.IsValid) continue;
                        locators.Add(registrar);
                        buildData.Add(new LocaleSerializerBuildData(registrar));
                    }
                }
            }

            _locators = locators.ToArray();
            _buildData = buildData.ToArray();
            _isInitialized = true;
        }

        public class LocaleSerializerBuildData {
            private RegisterLocaleSerializerAttribute _registerAttribute;

            public Type SerializerType => _registerAttribute.SerializerType;
            public string Name => _registerAttribute.Name;

            public LocaleSerializerBuildData(RegisterLocaleSerializerAttribute registerAttribute) {
                _registerAttribute = registerAttribute;
            }
        }
    }
}