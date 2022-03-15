using UnityEditor;

namespace Krugames.LocalizationSystem.Unity.Singletons.Editor.Postprocessor {
    public class ScriptableSingletonScriptReloadPostprocessor {
        
        //BUG git recreation bug
        //[UnityEditor.Callbacks.DidReloadScripts]
        private static void Postprocess() {
            EditorUtility.DisplayProgressBar("Updating Singleton Assets", "Processing assemblies...", 0f);
            ScriptableSingletonUtility.UpdateAllSingletonAssets();
            EditorUtility.ClearProgressBar();
        }
    }
}