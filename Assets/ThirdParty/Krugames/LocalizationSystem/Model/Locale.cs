using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Models.Interfaces;
using Krugames.LocalizationSystem.Models.Locators;
using Krugames.LocalizationSystem.Models.Structs;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models {
    
    [CreateAssetMenu(fileName = "Locale", menuName = "Localization/Locale", order = 0)]
    public class Locale : ScriptableObject, ILocale, ICacheCarrier, ILocaleGettableLayout, ILocaleSettableLayout {

        private const int DefaultBuffer = 64;

        [SerializeField] private SystemLanguage language = SystemLanguage.English;
        [SerializeField] private List<LocaleTerm> terms = new List<LocaleTerm>(0);

        private HashSet<Type> _supportedTermTypesCache = null;
        private Type[] _supportedTermTypes = Array.Empty<Type>();

        private HashSet<Type> _supportedValueTypesCache = null;
        private Type[] _supportedValueTypes = Array.Empty<Type>();

        private HashSet<Type> _containedTermTypesCache = null;
        private HashSet<Type> _containedValueTypesCache = null;

        private Dictionary<Type, Type> _termTypeToValueTypeDict = null;
        private Dictionary<Type, Type> _valueTypeToTermTypeDict = null;

        private Dictionary<string, LocaleTerm> _termDict = null;
        private Dictionary<Type, List<LocaleTerm>> _termListByType = null;
        private Dictionary<Type, Dictionary<string, LocaleTerm>> _termDictByType = null;

        private bool _wasInitialized = false;

        public SystemLanguage Language => language;
        public Type[] SupportedTermTypes {
            get {
                if (!_wasInitialized) Initialize();
                return _supportedTermTypes;
            }
        }

        public Type[] SupportedValueTypes {
            get {
                if (!_wasInitialized) Initialize();
                return _supportedValueTypes;
            }
        }


        private void OnEnable() {
#if UNITY_EDITOR
            _wasInitialized = false;
#endif
            if (LocalizationSettings.AutoInitialize) Initialize();
        }

        private void DebugWrongTerm(string term) {
            Debug.LogWarning($"Can not find term \"{term}\" in Localization. Term must be added to locales first!");
        }
        
        public void Initialize() {

            var buildData = LocaleTermLocator.BuildData;

            _supportedTermTypesCache = new HashSet<Type>();
            List<Type> supportedTermTypes = new List<Type>(buildData.Length);

            _supportedValueTypesCache = new HashSet<Type>();
            List<Type> supportedValueTypes = new List<Type>(buildData.Length);

            _termTypeToValueTypeDict = new Dictionary<Type, Type>(buildData.Length);
            _valueTypeToTermTypeDict = new Dictionary<Type, Type>(buildData.Length);            
            
            for (int i = 0; i < buildData.Length; i++) {
                Type termType = buildData[i].TermType;
                Type valueType = buildData[i].ValueType;

                if (_supportedTermTypesCache.Contains(termType)) {
                    Debug.LogWarning($"Term Type is already added to Locale: {termType.Name}");
                    continue;
                }

                if (_supportedValueTypesCache.Contains(valueType)) {
                    Debug.LogWarning($"Value Type is already added to Locale: {valueType.Name}");
                    continue;
                }

                _termTypeToValueTypeDict.Add(termType, valueType);
                _valueTypeToTermTypeDict.Add(valueType, termType);
                
                _supportedTermTypesCache.Add(termType);
                _supportedValueTypesCache.Add(valueType);

                supportedTermTypes.Add(termType);
                supportedValueTypes.Add(valueType);
            }
            _supportedTermTypes = supportedTermTypes.ToArray();
            _supportedValueTypes = supportedValueTypes.ToArray();

            RebuildCache();

            _wasInitialized = true;
        }

        public void RebuildCache() {
            _termDict = new Dictionary<string, LocaleTerm>(terms.Count);
            _termListByType = new Dictionary<Type, List<LocaleTerm>>(_supportedTermTypes.Length);
            _termDictByType = new Dictionary<Type, Dictionary<string, LocaleTerm>>(_supportedTermTypes.Length);

            _containedTermTypesCache = new HashSet<Type>();
            _containedValueTypesCache = new HashSet<Type>();

            for (int i = 0; i < _supportedTermTypes.Length; i++) {
                _termListByType.Add(_supportedTermTypes[i], new List<LocaleTerm>(DefaultBuffer));
                _termDictByType.Add(_supportedTermTypes[i], new Dictionary<string, LocaleTerm>());
            }
            
            for (int i = 0; i < terms.Count; i++) {

                string term = terms[i].Term;
                
                if (_termDict.ContainsKey(term)) {
                    Debug.LogError($"Term with key \"{terms[i].Term}\" is already exists. Remove double terms from Locales to fix this problem");
                    continue;
                }
                
                Type termType = terms[i].GetType();

                if (!_supportedTermTypesCache.Contains(termType)) {
                    Debug.LogWarning($"Term type \"{termType.Name}\" is not supported by Localization System. " +
                                     "Register locale term type to fix this problem. See RegisterLocaleTerm attribute\n");
                    continue;
                }
                
                if (!_termTypeToValueTypeDict.ContainsKey(termType)) {
                    Debug.LogError($"Value type for \"{termType.Name}\" is not found");
                }
                Type valueType = _termTypeToValueTypeDict[termType];

                _containedTermTypesCache.Add(termType);
                _containedValueTypesCache.Add(valueType);

                _termDict.Add(term, terms[i]);
                _termListByType[termType].Add(terms[i]);
                _termDictByType[termType].Add(term, terms[i]);
            }
        }

        public LocaleTerm GetTerm(string term) {
            if (!_wasInitialized) Initialize();
            if (_termDict.ContainsKey(term)) {
#if UNITY_EDITOR
                DebugWrongTerm(term);
#endif
                return _termDict[term];
            }
            return null;
        }

        public LocaleTerm GetTerm(string term, Type type) {
            if (!_wasInitialized) Initialize();
            if (!_termDictByType.ContainsKey(type)) {
#if UNITY_EDITOR
                DebugWrongTerm(term);
#endif
                return null;
            }
            
            var typedDict = _termDictByType[type];
            if (typedDict.ContainsKey(term)) return typedDict[term];
#if UNITY_EDITOR
            DebugWrongTerm(term);
#endif      
            return null;
        }

        public TTermType GetTerm<TTermType>(string term) where TTermType : LocaleTerm {
            if (!_wasInitialized) Initialize();
            
            Type type = typeof(TTermType);
            if (!_termDictByType.ContainsKey(type)) {
#if UNITY_EDITOR
                DebugWrongTerm(term);
#endif
                return null;
            }

            var typedDict = _termDictByType[type];
            if (typedDict.ContainsKey(term)) return (TTermType) typedDict[term];
#if UNITY_EDITOR
            DebugWrongTerm(term);
#endif
            return null;
        }

        public object GetTermValue(string term) {
            return GetTerm(term)?.Value;
        }

        public object GetTermValue(string term, Type type) {

            if (!_valueTypeToTermTypeDict.ContainsKey(type)) {
#if UNITY_EDITOR
                DebugWrongTerm(term);
#endif
                return null;
            }
            Type termType = _valueTypeToTermTypeDict[type];

            return GetTerm(term, termType)?.Value;
        }

        public TTermValueType GetTermValue<TTermValueType>(string term) {
            if (!_wasInitialized) Initialize();
            
            Type valueType = typeof(TTermValueType);
            if (!_valueTypeToTermTypeDict.ContainsKey(valueType)) {
#if UNITY_EDITOR
                DebugWrongTerm(term);
#endif
                return default;
            }
            
            Type termType = _valueTypeToTermTypeDict[valueType];
            if (!_termDictByType.ContainsKey(termType)) {
#if UNITY_EDITOR
                DebugWrongTerm(term);
#endif
                return default;
            }

            var typedDict = _termDictByType[termType];
            if (typedDict.ContainsKey(term)) return (TTermValueType)(typedDict[term].Value);
#if UNITY_EDITOR
            DebugWrongTerm(term);
#endif
            return default;
        }

        public LocaleTerm[] GetTerms() {
            if (!_wasInitialized) Initialize();
            return terms.ToArray();
        }

        public TTermType[] GetTerms<TTermType>() where TTermType : LocaleTerm {
            if (!_wasInitialized) Initialize();
            
            Type type = typeof(TTermType);
            if (!_termListByType.ContainsKey(type)) return Array.Empty<TTermType>();

            List<LocaleTerm> typedList = _termListByType[type];
            TTermType[] resultArray = new TTermType[typedList.Count];
            for (int i = 0; i < typedList.Count; i++) resultArray[i] = (TTermType)typedList[i];
            return resultArray;
        }

        /// <summary>
        /// Checks if locale supports specific term type 
        /// </summary>
        /// <param name="termType">specific term type to check</param>
        /// <returns>true if locale supports terms of specified type</returns>
        public bool SupportsTermType(Type termType) {
            if (!_wasInitialized) Initialize();
            return _supportedTermTypesCache.Contains(termType);
        }
        
        /// <summary>
        /// Checks if locale supports specific value type
        /// </summary>
        /// <param name="valueType">specific value type to check</param>
        /// <returns>true if locale supports terms of specified type</returns>
        public bool SupportsValueType(Type valueType) {
            if (!_wasInitialized) Initialize();
            return _supportedValueTypesCache.Contains(valueType);
        }

        /// <summary>
        /// Checks if locale contains specific term type
        /// </summary>
        /// <param name="termType">specific term type to check</param>
        /// <returns>true if locale contains terms of specified type</returns>
        public bool ContainsTermType(Type termType) {
            if (!_wasInitialized) Initialize();
            return _containedTermTypesCache.Contains(termType);
        }

        /// <summary>
        /// Checks if locale contains specific value type
        /// </summary>
        /// <param name="valueType">specific value type to check</param>
        /// <returns>true if locale contains values of specified type</returns>
        public bool ContainsValueType(Type valueType) {
            if (!_wasInitialized) Initialize();
            return _containedValueTypesCache.Contains(valueType);
        }


        public TermStructureInfo[] GetLayout() {
            TermStructureInfo[] layout = new TermStructureInfo[terms.Count];
            for (int i = 0; i < terms.Count; i++) {
                layout[i] = new TermStructureInfo() {
                    termName = terms[i].Term,
                    termType = terms[i].GetType(),
                };
            }
            return layout;
        }

        public void SetLayout(TermStructureInfo[] layout) {
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                Debug.LogError("Static Locale Layout can not be changed in runtime!");
                return;
            }
            
            //TODO use LocaleUtility to change layout
            
#else
            Debug.LogError("Static Locale Layout can not be changed in runtime!");
            return;
#endif
        }
    }
}