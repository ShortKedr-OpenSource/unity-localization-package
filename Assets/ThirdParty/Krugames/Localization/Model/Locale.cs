using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Models.Attributes;
using Krugames.LocalizationSystem.Models.Interfaces;
using Krugames.LocalizationSystem.Models.Locators;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Krugames.LocalizationSystem.Models {
    
    [CreateAssetMenu(fileName = "Locale", menuName = "Localization/Locale", order = 0)]
    public class Locale : ScriptableObject, ILocale {

        private const int TermListBuffer = 64;
        private const int TermDictBuffer = 64;
        
        private static readonly Type BaseTermType = typeof(LocaleTerm);
        
        [SerializeField] private SystemLanguage language = SystemLanguage.English;
        [SerializeField] private List<LocaleTerm> terms = new List<LocaleTerm>(0);

        private Dictionary<string, LocaleTerm> _termDict = null;

        private Dictionary<Type, List<LocaleTerm>> _termListByType = null;
        private Dictionary<Type, Dictionary<string, LocaleTerm>> _termDictByType = null;

        private Dictionary<Type, Type> _termTypeToValueTypeDict = null;
        
        private HashSet<Type> _supportedTypesExistCache = null;
        private Type[] _supportedTypes = Array.Empty<Type>();

        public SystemLanguage Language => language;
        
        private void OnEnable() {
            Initialize();
        }

        public void Initialize() {
            RebuildCache();

            RegisterLocaleTermAttribute[] termLocators = LocaleTermLocator.LocaleTermLocators;
            HashSet<Type> supportedTypesHash = new HashSet<Type>();
            List<Type> supportedTypes = new List<Type>(termLocators.Length);

            for (int i = 0; i < termLocators.Length; i++) {
                if (!termLocators[i].IsValid) {
                    Debug.Log("Register data is not Valid. Term type can not be null and must be " +
                              $"inherited from {BaseTermType.Name}");
                    continue;
                }
                Type termType = termLocators[i].TermType;
                if (supportedTypesHash.Contains(termType)) {
                    Debug.LogWarning($"Type is already added to Locale: {termType.Name}");
                    continue;
                }
                supportedTypesHash.Add(termType);
                supportedTypes.Add(termType);
            }
            _supportedTypes = supportedTypes.ToArray();
            //TODO test;

            RebuildCache();
        }

        public void RebuildCache() {
            _termDict = new Dictionary<string, LocaleTerm>(terms.Count);
            for (int i = 0; i < terms.Count; i++) {
                if (_termDict.ContainsKey(terms[i].Term)) {
                    Debug.LogError($"Term with key \"{terms[i].Term}\" is already exists. Remove double terms from Locales to fix this problem");
                    continue;
                }
                _termDict.Add(terms[i].Term, terms[i]);
            }
            
            _termListByType = new Dictionary<Type, List<LocaleTerm>>(_supportedTypes.Length);
            _termDictByType = new Dictionary<Type, Dictionary<string, LocaleTerm>>(_supportedTypes.Length);
            for (int i = 0; i < _supportedTypes.Length; i++) {
                _termListByType.Add(_supportedTypes[i], new List<LocaleTerm>(TermListBuffer));
                _termDictByType.Add(_supportedTypes[i], new Dictionary<string, LocaleTerm>(TermDictBuffer));
            }

            _termDict = new Dictionary<string, LocaleTerm>(Mathf.Max(terms.Count, TermDictBuffer));
            for (int i = 0; i < terms.Count; i++) {
                Type termType = terms[i].GetType();
                string term = terms[i].Term;

                if (!SupportsTermType(termType)) {
                    Debug.LogWarning($"Term type \"{termType.Name}\" is not supported by Localization System. " +
                                     "Register locale term type to fix this problem. See RegisterLocaleTerm attribute\n");
                    continue;
                }
                if (_termDict.ContainsKey(term)) {
                    Debug.LogError($"Term with key \"{terms[i].Term}\" is already exists. Remove double terms from Locales to fix this problem");
                    continue;
                }

                _termDict.Add(term, terms[i]);
                _termDictByType[termType].Add(term, terms[i]);
                _termListByType[termType].Add(terms[i]);
            }
        }

        public LocaleTerm GetTerm(string term) {
            if (_termDict == null) RebuildCache();
            
            //if ()
            throw new NotImplementedException();
        }

        public LocaleTerm GetTerm(string term, Type type) {
            if (SupportsTermType(type)) {
                Debug.LogWarning($"Term type \"{type.Name}\" is not supported by Localization System. " +
                                 "Register locale term type to fix this problem. See RegisterLocaleTerm attribute\n");
                return null;
            }

            bool isValidTerm = !string.IsNullOrEmpty(term);
            if (!isValidTerm) {
                Debug.LogError("Term can not be null or empty");
                return null;
            }

            if (_termDictByType[type].ContainsKey(term)) {
                //TODO
            }

            return null;
        }

        public TTermType GetTerm<TTermType>(string term) where TTermType : LocaleTerm {
            throw new NotImplementedException();
        }

        public object GetTermValue(string term) {
            return GetTerm(term).Value;
        }

        public object GetTermValue(string term, Type type) {
            throw new NotImplementedException();
        }

        public TTermValueType GetTermValue<TTermValueType>(string term) {
            throw new NotImplementedException();
        }
        
        public bool SupportsTermType(Type type) {
            if (_supportedTypesExistCache != null) {
                return _supportedTypesExistCache.Contains(type);
            }
            throw new NullReferenceException("Locale was not Initialized yet!");
        }
    }
}