using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//TODO add old way where string[] defines was not available
namespace Krugames.LocalizationSystem.Editor.Package {
    public static class PackageSymbolDefiner {

        //TODO 
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void DefineSymbolIfNotPresented() {
            DefineSymbolModern();
        }

        private static void DefineSymbolModern() {
            string[] symbols;
            PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, out symbols);
            List<string> symbolsList = new List<string>(symbols);
            if (!symbolsList.Contains(PackageVariables.DefineSymbol)) {
                symbolsList.Add(PackageVariables.DefineSymbol);
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbolsList.ToArray());
        }
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void DefineSymbolDeprecated() {
            //TODO impl
            Debug.Log(EditorGUIUtility.labelWidth);
        }

    }
}