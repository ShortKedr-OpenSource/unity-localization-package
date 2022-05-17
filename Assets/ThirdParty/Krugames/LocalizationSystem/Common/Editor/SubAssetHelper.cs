using System;
using UnityEditor;

namespace ThirdParty.Krugames.LocalizationSystem.Common.Editor {
    public class SubAssetHelper : AssetPostprocessor {
        
        //TODO create assetCache
        //TODO create singleton
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths) {
            
            throw new NotImplementedException();
            //TODO update data to already linked main asset objects
        }

        public static bool IsChildOf(UnityEngine.Object subAsset, UnityEngine.Object mainAsset) {
            if (!AssetDatabase.IsMainAsset(mainAsset)) return false;
            if (!AssetDatabase.IsSubAsset(subAsset)) return false;
            
            //TODO make check;
            throw new NotImplementedException();
        }
    }
}