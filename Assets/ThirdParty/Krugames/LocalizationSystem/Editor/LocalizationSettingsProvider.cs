using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor {
    public class LocalizationSettingsProvider {

        [SettingsProvider]
        private static SettingsProvider CreateLocalizationSettingsProvider() {
            var provider = new SettingsProvider("Project/Localization", SettingsScope.Project) {
                label = "Localization",
                guiHandler = delegate(string searchContext) {
                    var settings = LocalizationSettings.Instance;
                    var serializedSettings = new SerializedObject(settings);
                    EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("autoInitialize"), GUILayout.ExpandWidth(true));
                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("useSystemLanguageAsDefault"));
                    EditorGUILayout.PropertyField(serializedSettings.FindProperty("loadLastUsedLanguageAsCurrent"));
                    EditorGUILayout.EndVertical();
                    serializedSettings.ApplyModifiedProperties();
                },
                keywords = new HashSet<string>() {
                    "Auto Initialize",
                    "Use System Language As Default",
                    "Load Last Used Language As Current"
                }
            };
            return provider;
        }
    }
}