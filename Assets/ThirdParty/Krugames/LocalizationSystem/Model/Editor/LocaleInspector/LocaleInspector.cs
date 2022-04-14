using Krugames.LocalizationSystem.Editor.Serialization.Serializers;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Utility.Editor;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor {
    [CustomEditor(typeof(Locale))]
    public class LocaleInspector : UnityEditor.Editor {

        private Rect _addTermRect;
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI(); //TODO remove

            Locale locale = (Locale)target;
            
            GUILayout.Space(4);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add term", GUILayout.MinWidth(225), GUILayout.MinHeight(26))) {
                PopupWindow.Show(_addTermRect, new LocaleTermSelector());
            }
            if (Event.current.type == EventType.Repaint) _addTermRect = GUILayoutUtility.GetLastRect();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(16);

            if (GUILayout.Button("Remove Term")) {
                LocaleUtility.RemoveLocaleTerm(locale, "new_term");
            }

            if (GUILayout.Button("Export JSON")) {
                LocaleJsonSerializer serializer = new LocaleJsonSerializer(Formatting.Indented);
                string json = serializer.SerializeSmart(locale);
                Debug.Log(json);
            }

            if (GUILayout.Button("Export XML")) {
                LocaleXmlSerializer serializer = new LocaleXmlSerializer();
                string xml = serializer.SerializeSmart(locale);
                Debug.Log(xml);
            }
            
            if (GUILayout.Button("Export YAML")) {
                LocaleYamlSerializer serializer = new LocaleYamlSerializer();
                string yaml = serializer.SerializeSmart(locale);
                Debug.Log(yaml);
            }
            
            if (GUILayout.Button("Export CSV")) {
                LocaleCsvSerializer serializer = new LocaleCsvSerializer();
                string csv = serializer.SerializeSmart(locale);
                Debug.Log(csv);
            }
        }

    }
}