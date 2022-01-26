using System;
using System.Collections.Generic;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models {
    public class Locale : ScriptableObject {

        private static readonly Type BaseTermType = typeof(LocaleTerm);
        
        [SerializeField] private SystemLanguage language = SystemLanguage.English;
        [SerializeField] private List<LocaleTerm> terms = new List<LocaleTerm>(0);

        private Dictionary<string, LocaleTerm> _termsDict = null;

        private Dictionary<Type, List<LocaleTerm>> _termsByTypeDict = null;


        private void OnEnable() {
            Initialize();
        }

        public void Initialize() {
            RebuildCache();
        }

        public void RebuildCache() {
            _termsDict = new Dictionary<string, LocaleTerm>(terms.Count);
            for (int i = 0; i < terms.Count; i++) {
                if (_termsDict.ContainsKey(terms[i].Term)) {
                    Debug.LogError($"Term with key \"{terms[i].Term}\" is already exists. Remove double terms from Locales to fix this problem");
                    continue;
                }
                _termsDict.Add(terms[i].Term, terms[i]);
            }
        }

        private TTermType GetTerm<TTermType>(string term) where TTermType : LocaleTerm {
            throw new NotImplementedException();
        }

        private LocaleTerm GetTerm(string term, Type type) {
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

        public LocaleTerm GetTerm(string term) {
            if (_termsDict == null) RebuildCache();
            
            if ()
            throw new NotImplementedException();
        }

    }
}