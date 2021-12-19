using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Krugames.Core.Unity.Singletons {
    /// <summary>
    /// Realization of ScriptableObject but as singleton unit.
    /// Presented realization don't mean "only-one" rule.
    /// Instance will be always presented as one instance,
    /// but you still can create other instances via UnityEngine system
    /// <para>All instances will be located in </para>
    /// </summary>
    /// <typeparam name="TEndType">type of inheritor</typeparam>
    public abstract class ScriptableSingleton<TEndType> : ScriptableObject 
        where TEndType : ScriptableSingleton<TEndType> {
        
        protected const string AssetsFolder = "Assets";
        protected const string ResourceFolder = "Resources";
        protected const string SingletonFolder = "Singletons";

        public const string AssetExtension = ".asset";
        
        private static TEndType _instance;
        public static TEndType Instance {
            get {
                if (_instance == null) {
                    
#if UNITY_EDITOR
                    _instance = AssetDatabase.LoadAssetAtPath<TEndType>(AssetPath);
#else
                    _instance = Resources.Load<TEndType>(ResourcePath);
#endif
#if UNITY_EDITOR
                    if (_instance == null) {
                        CreateAsset();
                        _instance = AssetDatabase.LoadAssetAtPath<TEndType>(AssetPath);
                    }          
#endif
                }
                return _instance;
            }
        }

        public static readonly string ResourcePath = SingletonFolder + "/" + typeof(TEndType).Name;
        public static readonly string AssetPath = $"{AssetsFolder}/{ResourceFolder}/{SingletonFolder}/" +
                                                  $"{typeof(TEndType).Name}{AssetExtension}";

        
#if UNITY_EDITOR
        public static void CreateAsset(){
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            
            TEndType asset = AssetDatabase.LoadAssetAtPath<TEndType>(AssetPath);

            if (asset == null) {
                var newAsset = ScriptableObject.CreateInstance(typeof(TEndType)); 

                if (!AssetDatabase.IsValidFolder($"{AssetsFolder}/{ResourceFolder}")){
                    AssetDatabase.CreateFolder(AssetsFolder, ResourceFolder);
                }
                
                if (!AssetDatabase.IsValidFolder($"{AssetsFolder}/{ResourceFolder}/{SingletonFolder}")){
                    AssetDatabase.CreateFolder($"{AssetsFolder}/{ResourceFolder}", SingletonFolder);
                }
                
                AssetDatabase.CreateAsset(newAsset, AssetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                asset = AssetDatabase.LoadAssetAtPath<TEndType>(AssetPath);
                if (asset != null) {
                    Debug.Log($"{typeof(TEndType).Name} singleton asset was created in '{AssetPath}'");
                }
                
            }
        }
#endif
    }
}