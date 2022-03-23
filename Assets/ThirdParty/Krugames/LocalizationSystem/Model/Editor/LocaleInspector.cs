using Krugames.LocalizationSystem.Editor.Serialization.Serializers;
using Krugames.LocalizationSystem.Models;
using Krugames.LocalizationSystem.Models.Terms;
using Krugames.LocalizationSystem.Models.Utility.Editor;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor {
    [CustomEditor(typeof(Locale))]
    public class LocaleInspector : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI(); //TODO remove

            Locale locale = (Locale)target;
            
            if (GUILayout.Button("AddTerm")) {
                
                string[] possibleTerms = new string[] {
                    "String",
                    "Sprite",
                    "Texture",
                    "AudioClip",
                };

                Rect popupPosition = new Rect(Event.current.mousePosition, new Vector3(100f, 100f));
                PopupWindow.Show(popupPosition, new PopupExample());
            }

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
    
    public class PopupExample : PopupWindowContent
    {
        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 150);
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("Popup Options Example", EditorStyles.boldLabel);
            GUILayout.Button("String");
            GUILayout.Button("Sprite");
            GUILayout.Button("AudioClip");
            GUILayout.Button("Texture");
        }

        public override void OnOpen()
        {
            Debug.Log("Popup opened: " + this);
        }

        public override void OnClose()
        {
            Debug.Log("Popup closed: " + this);
        }
    }
}