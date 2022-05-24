using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace Krugames.LocalizationSystem.Unity.Singletons {
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
        
        protected const string ResourceFolder = "Resources";
        protected const string SingletonFolder = "Localization";

#if UNITY_EDITOR
        protected const string AssetsFolder = "Assets";
        protected const string AssetExtension = ".asset";
#endif

        private static TEndType _instance;
        public static TEndType Instance {
            get {
                if (_instance == null) {
                    
#if UNITY_EDITOR
                    if (!EditorApplication.isPlaying) {
                        _instance = AssetDatabase.LoadAssetAtPath<TEndType>(AssetPath);
                    } else {
                        _instance = Resources.Load<TEndType>(ResourcePath);
                    }
#else
                    _instance = Resources.Load<TEndType>(ResourcePath);
#endif
#if UNITY_EDITOR
                    if (_instance == null) {
                        CreateAssetIfNotExists();
                        _instance = AssetDatabase.LoadAssetAtPath<TEndType>(AssetPath);
                    }          
#endif
                }
                return _instance;
            }
        }

        public static readonly string ResourcePath = SingletonFolder + "/" + typeof(TEndType).Name;
#if UNITY_EDITOR
        public static readonly string AssetPath = $"{AssetsFolder}/{ResourceFolder}/{SingletonFolder}/" +
                                                  $"{typeof(TEndType).Name}{AssetExtension}";
#endif


#if UNITY_EDITOR
        //TODO improve, add IO Layer of Confidence (if AssetDatabase Fails)
        public static void CreateAssetIfNotExists() {

            // Phase 1 of confidence
            TEndType asset = AssetDatabase.LoadAssetAtPath<TEndType>(AssetPath);

            // Phase 2 of confidence
            if (asset == null) {
                ImportAssetOptions importParams = ImportAssetOptions.ForceUpdate | 
                                                  ImportAssetOptions.ForceSynchronousImport |
                                                  ImportAssetOptions.DontDownloadFromCacheServer;
                AssetDatabase.ImportAsset(AssetPath, importParams);
                asset = AssetDatabase.LoadAssetAtPath<TEndType>(AssetPath);
            }
            
            // Phase 3 of confidence
            bool assetFileExists = false;
            if (asset == null) {
                string directory = Directory.GetCurrentDirectory().Replace("\\", "/");
                string fullAssetPath = directory + "/" + AssetPath;
                assetFileExists = File.Exists(fullAssetPath);
            }

            if (asset == null && !assetFileExists) {
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
                } else {
                    Debug.LogError($"Unable to create {typeof(TEndType).Name} singleton!'{AssetPath}'");
                }
            }
        }
#endif
    }
}