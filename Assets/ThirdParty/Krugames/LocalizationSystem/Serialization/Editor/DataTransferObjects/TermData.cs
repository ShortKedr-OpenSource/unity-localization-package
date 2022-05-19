using System;
using System.ComponentModel;
using System.IO;
using Krugames.LocalizationSystem.Common.Extensions;
using Krugames.LocalizationSystem.Editor.Serialization.Utility;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Utility;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor.Serialization.DataTransferObjects {
    [Serializable]
    public struct TermData {
        public string term;
        public object value;
        public string valueType;
        public bool isAsset;

        public TermData(string term, object value, bool isAsset, Type valueType) {
            this.term = term;
            this.value = (JsonPrimitiveChecker.IsPrimitive(value)) ? value : JsonConvert.SerializeObject(value);
            this.isAsset = isAsset;
            this.valueType = valueType.AssemblyQualifiedName;
        }

        public TermData(LocaleTerm localeTerm) {
            Type termType = localeTerm.GetType();
            Type valueType = null;
            if (termType.IsInheritor(LocaleTermUtility.BaseGenericType)) {
                valueType = LocaleTermUtility.GetValueTypeOfGenericTermType(termType);
            } else {
                valueType = localeTerm.Value.GetType();
            }

            object value = localeTerm.Value;
            bool isAsset = false;
            if (value is UnityEngine.Object unityObject && AssetDatabase.Contains(unityObject)) {
                string path = AssetDatabase.GetAssetPath(unityObject);
                value = path;
                isAsset = true;
            }
            
            this.term = localeTerm.Term;
            this.value = (JsonPrimitiveChecker.IsPrimitive(value)) ? value : JsonUtility.ToJson(value);
            this.isAsset = isAsset;
            this.valueType = valueType.AssemblyQualifiedName;
        }

        public LocaleTerm CreateTermInstance() {
            Type valType = Type.GetType(valueType);
            bool isPrimitive = JsonPrimitiveChecker.IsPrimitive(valType);
            
            object termValue;

            if (isAsset) {
                if (value == null) termValue = null;
                else {
                    string path = value.ToString();
                    termValue = AssetDatabase.LoadAssetAtPath(path, valType);
                    if (termValue == null) {
                        string[] guids = AssetDatabase.FindAssets(Path.GetFileName(path));
                        if (guids.Length > 0) {
                            termValue = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), valType);
                        }
                    }
                }
            } else if (isPrimitive) {
                termValue = value;
            } else {
                termValue = JsonUtility.FromJson(value.ToString(), valType);
            }
            
            Type termType = LocaleTermUtility.GetTermTypeByValueType(valType);
            LocaleTerm termObject = (LocaleTerm) ScriptableObject.CreateInstance(termType);
            termObject.name = term;
            termObject.SetValue(termValue);
            
            SerializedObject serializedTermObject = new SerializedObject(termObject);
            serializedTermObject.FindProperty("term").stringValue = term;
            serializedTermObject.ApplyModifiedPropertiesWithoutUndo();

            return termObject;
        }
    }
}