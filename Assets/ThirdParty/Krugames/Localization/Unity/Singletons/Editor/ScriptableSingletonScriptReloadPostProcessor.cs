using System;
using System.Reflection;
using UnityEditor;

namespace Krugames.Core.Unity.Singletons.Editor {
    
    public static class ScriptableSingletonScriptReloadPostProcessor {
         
        [UnityEditor.Callbacks.DidReloadScripts(1)]
        private static void PostProcess() {
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++) PostProcessAssembly(assemblies[i]);
        }
        
        private static void PostProcessAssembly(Assembly assembly) {
            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++) PostProcessType(types[i]);
        }
        
        private static void PostProcessType(Type type) {
            
            bool notAbstract = !type.IsAbstract;
            bool hasBaseType = type.BaseType != null;
            bool isGenericBaseType = hasBaseType && type.BaseType.IsGenericType;
            bool isScriptableSingletonInheritor = isGenericBaseType && 
                                                  type.BaseType.GetGenericTypeDefinition() == typeof(ScriptableSingleton<>);

            bool match = notAbstract && isScriptableSingletonInheritor;
            if (!match) return;

            PropertyInfo propertyInfo = type.BaseType.GetProperty("Instance");

            if (propertyInfo != null) {
                var instance = propertyInfo.GetValue(null);
                if (instance == null) {
                    MethodInfo methodInfo = type.BaseType.GetMethod("CreateAsset");
                    methodInfo?.Invoke(null, new object[0]);  
                }
                //TODO Debug Symbols
            }
            //TODO Debug Symbols
        }
    }
}