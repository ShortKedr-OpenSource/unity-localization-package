using System;
using Krugames.LocalizationSystem.Common.Extensions;
using Krugames.LocalizationSystem.Models.Interfaces;
using Krugames.LocalizationSystem.Models.Structs;
using Krugames.LocalizationSystem.Models.Validation;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Krugames.LocalizationSystem.Models.Utility.Editor {
    public static class LocaleUtility {
        
        public static LocaleTerm AddLocaleTerm(Locale locale, string termName, Type termType, bool preventDuplicate = true) {
            
            if (Application.isPlaying) {
                Debug.LogError("Static Locale can not be changed in runtime!");
                return null;
            }

            string localePath = AssetDatabase.GetAssetPath(locale);
            
            bool validType = termType.IsInheritor(LocaleTermUtility.BaseType) && !termType.IsAbstract;
            bool canBeAdded = (!preventDuplicate) || locale.GetTerm(termName) == null;
            bool isAsset = AssetDatabase.IsMainAsset(locale);
            
            if (!validType) {
                Debug.LogError($"Term of type{termType} can not be added. Specified type is not valid. " +
                                 $"TermType must be inheritor of {nameof(LocaleTerm)}");
                return null;
            }

            if (!isAsset) {
                Debug.LogError($"Term can not be added to non asset locale. Locale must be an asset!");
                return null;
            }
            
            if (!canBeAdded) {
                Debug.LogError($"Term with identifier \"{termName}\" is already exists and can not be added");
                return null;
            }

            LocaleTerm termObject = (LocaleTerm)ScriptableObject.CreateInstance(termType);
            termObject.name = termName;

            SerializedObject serializedTermObject = new SerializedObject(termObject);
            serializedTermObject.FindProperty("term").stringValue = termName;
            serializedTermObject.ApplyModifiedPropertiesWithoutUndo();
            
            AssetDatabase.AddObjectToAsset(termObject, locale);
#pragma warning disable CS0618
            locale.AddTerm(termObject);
#pragma warning restore CS0618
            AssetDatabase.ImportAsset(localePath);

            return termObject;
        }

        public static bool RemoveLocaleTerm(Locale locale, LocaleTerm term) {
            
            //TODO implement
            
            if (Application.isPlaying) {
                Debug.LogError("Static Locale can not be changed in runtime!");
                return false;
            }

            if (term == null) return false;
                
            string termPath = AssetDatabase.GetAssetPath(term);

            bool isAsset = AssetDatabase.IsMainAsset(locale);
            bool isSubAsset = AssetDatabase.IsSubAsset(term);
            bool isLocaleChild = AssetDatabase.LoadAssetAtPath<Locale>(termPath) == locale;

            if (!isAsset) {
                Debug.LogError($"Term can not be removed from non asset locale. Locale must be an asset!");
                return false;
            }
            
            if (!isSubAsset) {
                Debug.LogError("Non sub-asset term can not be removed from locale!");
            }

            if (isLocaleChild) {
                Debug.LogError("Term asset can not be removed from another locale!");
            }

#pragma warning disable CS0618
            locale.RemoveTerm(term);
#pragma warning restore CS0618
            AssetDatabase.RemoveObjectFromAsset(term);
            Object.DestroyImmediate(term);
            AssetDatabase.ImportAsset(termPath);

            return true;
        }

        public static void SetLayout(Locale locale, TermStructureInfo[] layout) {
#pragma warning disable CS0618
            locale.SetLayout(layout);
#pragma warning restore CS0618
        }
        
        public static bool SetLanguage(Locale locale, SystemLanguage newLanguage) {
#pragma warning disable CS0618
            return locale.SetLanguage(newLanguage);
#pragma warning restore CS0618
        }

        public static bool SetTerms(Locale locale, LocaleTerm[] terms) {
#pragma warning disable CS0618
            return locale.SetTerms(terms);
#pragma warning restore CS0618
        }
        
        public static void RenameTermSubAsset(Locale locale, LocaleTerm term) {
            if (!AssetDatabase.IsSubAsset(term)) return;
            string termPath = AssetDatabase.GetAssetPath(term);
            if (AssetDatabase.LoadAssetAtPath<Locale>(termPath) != locale) return;
            term.name = term.Term;
            AssetDatabase.ImportAsset(termPath);
        }
    }
}