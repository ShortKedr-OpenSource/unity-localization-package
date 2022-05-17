using System;
using System.Collections.Generic;
using System.Reflection;
using Krugames.LocalizationSystem.Editor.Serialization.Attributes;

namespace Krugames.LocalizationSystem.Editor.Serialization.Locators {
    public static class LocaleSerializerLocator {

        public class LocaleSerializerBuildData {
            private RegisterLocaleSerializerAttribute _registerAttribute;

            public Type SerializerType => _registerAttribute.SerializerType;
            public string Name => _registerAttribute.Name;
            public string Extension => _registerAttribute.Extension;

            public LocaleSerializerBuildData(RegisterLocaleSerializerAttribute registerAttribute) {
                _registerAttribute = registerAttribute;
            }
        }
        
        private const int DefaultBuffer = 8;

        private static RegisterLocaleSerializerAttribute[] _locators;
        private static LocaleSerializerBuildData[] _buildData;

        private static Dictionary<Type, LocaleSerializerBuildData> _buildDataBySerializerType;
        
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
            
            _buildDataBySerializerType = new Dictionary<Type, LocaleSerializerBuildData>(buildData.Count);
            for (int i = 0; i < _buildData.Length; i++) {
                _buildDataBySerializerType.Add(_buildData[i].SerializerType, _buildData[i]);
            }
            
            _isInitialized = true;
        }

        public static LocaleSerializerBuildData GetBuildData(Type serializerType) {
            if (_buildDataBySerializerType.ContainsKey(serializerType))
                return _buildDataBySerializerType[serializerType];
            return null;
        }
    }
}