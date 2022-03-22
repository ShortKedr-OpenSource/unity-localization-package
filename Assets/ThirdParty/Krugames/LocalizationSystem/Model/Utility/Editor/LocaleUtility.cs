using System;
using Krugames.LocalizationSystem.Common.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Krugames.LocalizationSystem.Models.Utility.Editor {
    public static class LocaleUtility {
        public static LocaleTerm AddLocaleTerm(Locale targetLocale, string termName, Type termType) {
            
            if (Application.isPlaying) {
                Debug.LogError("Static Locale can not be changed in runtime!");
                return null;
            }
            
            bool validType = termType.IsInheritor(LocaleTermUtility.BaseType) && !termType.IsAbstract;
            bool canBeAdded = targetLocale.GetTerm(termName) == null;
            bool isAsset = AssetDatabase.IsMainAsset(targetLocale);
            
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
            
            AssetDatabase.AddObjectToAsset(termObject, targetLocale);
            targetLocale.AddTerm(termObject);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(termObject));

            return termObject;
        }

        public static bool RemoveLocaleTerm(Locale targetLocale, string termName) {
            
            if (Application.isPlaying) {
                Debug.LogError("Static Locale can not be changed in runtime!");
                return false;
            }
            
            bool isAsset = AssetDatabase.IsMainAsset(targetLocale);
            
            if (!isAsset) {
                Debug.LogError($"Term can not be removed from non asset locale. Locale must be an asset!");
                return false;
            }

            LocaleTerm localeTerm = targetLocale.GetTerm(termName);
            
            if (localeTerm == null) return false;

            AssetDatabase.RemoveObjectFromAsset(localeTerm);
            Object.DestroyImmediate(localeTerm);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(targetLocale));

            return true;
        }
    }
}