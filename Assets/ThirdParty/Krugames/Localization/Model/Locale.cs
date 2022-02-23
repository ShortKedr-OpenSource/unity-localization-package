using System;
using System.Collections.Generic;
using Krugames.LocalizationSystem.Models.Attributes;
using Krugames.LocalizationSystem.Models.Interfaces;
using Krugames.LocalizationSystem.Models.Utility;
using UnityEditor;
using UnityEngine;

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

            Type termType;
            for (int i = 0; i < termLocators.Length; i++) {
                termType = termLocators[i].TermType;
                if (supportedTypesHash.Contains(termType)) {
                    Debug.LogWarning($"Type is already added to Locale: {termType.Name}");
                    continue;
                }
                supportedTypesHash.Add(termType);
                supportedTypes.Add(termType);
            }
            _supportedTypes = supportedTypes.ToArray();


            _termListByType = new Dictionary<Type, List<LocaleTerm>>(supportedTypes.Count);
            _termDictByType = new Dictionary<Type, Dictionary<string, LocaleTerm>>(supportedTypes.Count);
            for (int i = 0; i < supportedTypes.Count; i++) {
                _termListByType.Add(supportedTypes[i], new List<LocaleTerm>(TermListBuffer));
                _termDictByType.Add(supportedTypes[i], new Dictionary<string, LocaleTerm>(TermDictBuffer));
            }

            _termDict = new Dictionary<string, LocaleTerm>(Mathf.Max(terms.Count, TermDictBuffer));
            for (int i = 0; i < terms.Count; i++) {
                _termDict.Add(terms[i].Term, terms[i]);
                termType = terms[i].GetType();
                //if (_termListByType)
            }
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
        }

        public LocaleTerm GetTerm(string term) {
            if (_termDict == null) RebuildCache();
            
            //if ()
            throw new NotImplementedException();
        }

        public LocaleTerm GetTerm(string term, Type type) {
            bool isValidType = type != null && type.IsAssignableFrom(BaseTermType);
            bool isValidTerm = !string.IsNullOrEmpty(term);

            if (!isValidType) {
                Debug.LogError("Term type is invalid. Term type must be inherited from " + nameof(LocaleTerm));
                return null;
            }

            if (isValidTerm) {
                Debug.LogError("Term can not be null or empty");
                return null;
            }

            throw new NotImplementedException();
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