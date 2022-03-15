using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Krugames.LocalizationSystem.Unity.Singletons;
using UnityEngine;

namespace Krugames.LocalizationSystem.Unity.Singletons.Editor {
    /// <summary>
    /// Editor Utility for managing ScriptableSingleton 
    /// </summary>
    public class ScriptableSingletonUtility {

        public static bool DebugMode = false;

        public static Type[] GetAllSingletonTypes() {

            List<Type> singletonTypes = new List<Type>(16);
            
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++) {
                Type[] types = assemblies[i].GetTypes();
                for (int j = 0; j < types.Length; j++) {
                    if (IsTypeScriptableSingleton(types[j])) singletonTypes.Add(types[j]);
                }
            }

            return singletonTypes.ToArray();
        }

        public static string GetSingletonName(Type type) {
            if (IsTypeScriptableSingleton(type)) {
                return type.Name;
            } else {
                return "[None]";
            }
        }

        public static bool IsTypeScriptableSingleton(Type type) {
            bool notAbstract = !type.IsAbstract;
            bool hasBaseType = type.BaseType != null;
            bool isGenericBaseType = hasBaseType && type.BaseType.IsGenericType;
            bool isScriptableSingletonInheritor = isGenericBaseType &&
                                                  type.BaseType.GetGenericTypeDefinition() == typeof(Krugames.LocalizationSystem.Unity.Singletons.ScriptableSingleton<>).GetGenericTypeDefinition();
            
            bool match = notAbstract && isScriptableSingletonInheritor;
            return match;
        }

        public static bool IsSingletonAssetExist(Type singletonType) {
            if (IsTypeScriptableSingleton(singletonType)) {
                PropertyInfo propertyInfo = singletonType.BaseType.GetProperty("Instance");
                if (propertyInfo != null) {
                    var instance = propertyInfo.GetValue(null);
                    if (instance != null) return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Update ScriptableSingleton assets for all existing assemblies in current app domain
        /// </summary>
        public static void UpdateAllSingletonAssets() {
            if (DebugMode) Debug.Log("[ScriptableSingleton PostProcess] Start PostProcessing");
            
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++) UpdateSingletonAssetsForAssembly(assemblies[i]);
            
            if (DebugMode) Debug.Log("[ScriptableSingleton PostProcess] End PostProcessing");
        }
        
        /// <summary>
        /// Update ScriptableSingleton assets for whole assembly
        /// </summary>
        /// <param name="assembly"></param>
        public static void UpdateSingletonAssetsForAssembly(Assembly assembly) {
            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++) {
                UpdateSingletonAsset(types[i]);
            }
        }
        /// <summary>
        /// Updating ScriptableSingleton asset for specific type.
        /// If there is no asset then new asset will be created.
        /// Specified type must be type of <code>ScriptableSingleton</code>
        /// </summary>
        /// <param name="type">specific ScriptableSingleton type</param>
        public static void UpdateSingletonAsset(Type type) {
            
            bool notAbstract = !type.IsAbstract;
            bool hasBaseType = type.BaseType != null;
            bool isGenericBaseType = hasBaseType && type.BaseType.IsGenericType;
            bool isScriptableSingletonInheritor = isGenericBaseType &&
                                                  type.BaseType.GetGenericTypeDefinition() == typeof(ScriptableSingleton<>).GetGenericTypeDefinition();
            
            bool match = notAbstract && isScriptableSingletonInheritor;
            if (!match) return;

            PropertyInfo propertyInfo = type.BaseType.GetProperty("Instance");
            
            if (propertyInfo != null) {
                var instance = propertyInfo.GetValue(null);
                if (instance == null) {
                    MethodInfo methodInfo = type.BaseType.GetMethod("CreateAssetIfNotExists");
                    methodInfo?.Invoke(null, new object[0]);  
                } else {
                    if (DebugMode) Debug.LogWarning($"{type.Name} Instance asset is already exists");
                }
            } else {
                if (DebugMode) Debug.LogError($"{type.Name} \"Instance\" not found");
            }
        }

        public static T GetAsset<T>() where T : ScriptableSingleton<T> {
            Type type = typeof(T);
            
            PropertyInfo propertyInfo = type.BaseType?.GetProperty("Instance");

            if (propertyInfo != null) {
                var instance = propertyInfo.GetValue(null);
                return (T) instance;
            }
            return null;
        }
        
        public static ScriptableObject GetAsset(Type type) {
            if (!IsTypeScriptableSingleton(type)) return null;
            
            //TODO change binding flags to Flat Hierarchy
            PropertyInfo propertyInfo = type.BaseType?.GetProperty("Instance");

            if (propertyInfo != null) {
                var instance = propertyInfo.GetValue(null);
                if (instance is ScriptableObject unityObject) return unityObject;
                return null;
            }
            return null;
        }
    }
}