#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Krugames.LocalizationSystem.Common.Editor {
    /// <summary>
    /// Provides additional functionality for asset objects in addition to AssetDatabase
    /// </summary>
    public class AssetHelper : AssetPostprocessor {

        private static Dictionary<Object, HashSet<Object>> _subAssetReferenceCache = new Dictionary<Object, HashSet<Object>>(16);

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths) {

            for (int i = 0; i < importedAssets.Length; i++) {
                UpdateSubAssetReferenceCache(importedAssets[i]);    
            }
        }

        private static void UpdateSubAssetReferenceCache(string mainAssetPath) {
            //TODO rework, since multiple main assets can be loaded
            //BUG current approach generates bug, when multiple main assets are loaded, see todo above
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(mainAssetPath);
            if (assets.Length == 0) return;
            Object mainAsset = null;
            List<Object> subAssets = new List<Object>(assets.Length-1);
            for (int i = 0; i < assets.Length; i++) {
                if (mainAsset == null && AssetDatabase.IsMainAsset(assets[i])) {
                    mainAsset = assets[i];
                    continue;
                }
                subAssets.Add(assets[i]);
            }

            if (mainAsset == null) {
                Debug.LogError("Main asset not found at given path!");
                return;
            }

            if (!_subAssetReferenceCache.ContainsKey(mainAsset)) {
                _subAssetReferenceCache.Add(mainAsset, new HashSet<Object>());
            }

            var subAssetCache = _subAssetReferenceCache[mainAsset];
            subAssetCache.Clear();
            for (int i = 0; i < subAssets.Count; i++) subAssetCache.Add(subAssets[i]);
        }

        private static void UpdateSubAssetReferenceCache(Object asset) {
            UpdateSubAssetReferenceCache(AssetDatabase.GetAssetPath(asset));
        }

        public static bool IsSubAssetOf(Object subAsset, Object mainAsset) {
            if (!AssetDatabase.IsMainAsset(mainAsset)) return false;
            if (!AssetDatabase.IsSubAsset(subAsset)) return false;
            if (!_subAssetReferenceCache.ContainsKey(mainAsset)) UpdateSubAssetReferenceCache(mainAsset);
            return _subAssetReferenceCache[mainAsset].Contains(subAsset);
        }

        public static Object[] GetSubAssets(Object asset) {
            return GetSubAssets(AssetDatabase.GetAssetPath(asset), out _);
        }

        public static Object[] GetSubAssets(string path) {
            return GetSubAssets(path, out _ );
        }
        
        public static Object[] GetSubAssets(string path, out Object mainAsset) {
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
            mainAsset = null;
            List<Object> subAssets = new List<Object>(assets.Length-1);
            for (int i = 0; i < assets.Length; i++) {
                if (mainAsset == null && AssetDatabase.IsMainAsset(assets[i])) {
                    mainAsset = assets[i];
                    continue;
                }
                subAssets.Add(assets[i]);
            }
            return subAssets.ToArray();
        }
    }
}
#endif