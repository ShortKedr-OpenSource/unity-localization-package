using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//TODO add old way where string[] defines was not available
namespace Krugames.LocalizationSystem.Editor.Package {
    public static class PackageSymbolDefiner {
        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void DefineSymbolIfNotPresented() {
            DefineSymbolModern();
            //TODO implement for multiple Unity versions
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

        private static void DefineSymbolDeprecated() {
            throw new NotImplementedException();
        }

    }
}