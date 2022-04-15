using System;
using Krugames.LocalizationSystem.Editor.Serialization.Editors;
using Krugames.LocalizationSystem.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor {
    [CustomEditor(typeof(Locale))]
    public class LocaleInspector : UnityEditor.Editor {

        private VisualElement _rootElement;
        
        private Rect _addTermBtnRect;
        private Rect _exportBtnRect;
        private Rect _importBtnRect;
        
        private LocaleTermSelector _termSelector;
        private LocaleSerializerSelector _serializerSelector;

        private Locale _locale;
        private SerializedProperty _languageProp;
        private SerializedProperty _termsProp;

        public override VisualElement CreateInspectorGUI() {
            _locale = (Locale) target;
            _languageProp = serializedObject.FindProperty("language");
            _termsProp = serializedObject.FindProperty("terms");

            _rootElement = new VisualElement();

            _rootElement.Add(new Button(){text="adata"});
            
            //return _rootElement;
            return null;
        }
        
        

        public override void OnInspectorGUI() {
           //base.OnInspectorGUI(); //TODO remove

            EditorGUILayout.PropertyField(_languageProp);
            EditorGUILayout.Separator();
            for (int i = 0; i < _termsProp.arraySize; i++) {
                var element = _termsProp.GetArrayElementAtIndex(i);
                LocaleTerm localeTerm = (LocaleTerm)element.objectReferenceValue;
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button($"{localeTerm.Term}", GUILayout.MinWidth(200))) {
                    AssetDatabase.OpenAsset(localeTerm);
                };
                GUILayout.Label(localeTerm.GetType().Name);
                GUILayout.Button("R", GUILayout.MaxWidth(18), GUILayout.MaxHeight(18));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Separator();
            serializedObject.ApplyModifiedProperties();
            
            GUILayout.Space(4);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add term", GUILayout.MinWidth(225), GUILayout.MinHeight(26))) {
                if (_termSelector == null) _termSelector = LocaleTermSelector.Create(AddTerm);
                PopupWindow.Show(_addTermBtnRect, _termSelector);
            }
            if (Event.current.type == EventType.Repaint) _addTermBtnRect = GUILayoutUtility.GetLastRect();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            //GUILayout.Space(16);

            /*if (GUILayout.Button("Remove Term")) {
                LocaleUtility.RemoveLocaleTerm(locale, "new_term");
            }*/

            GUILayout.Space(4);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Export", GUILayout.MinWidth(112.5f), GUILayout.MinHeight(26))) {
                if (_serializerSelector == null) _serializerSelector = LocaleSerializerSelector.Create(Export);
                PopupWindow.Show(_exportBtnRect, _serializerSelector);
            }
            if (Event.current.type == EventType.Repaint) _exportBtnRect = GUILayoutUtility.GetLastRect();
            if (GUILayout.Button("Import", GUILayout.MinWidth(112.5f), GUILayout.MinHeight(26))) {
                if (_serializerSelector == null) _serializerSelector = LocaleSerializerSelector.Create(Export);
                PopupWindow.Show(_importBtnRect, _serializerSelector);
            }
            if (Event.current.type == EventType.Repaint) _importBtnRect = GUILayoutUtility.GetLastRect();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            
        }

        private void AddTerm(Type termType, Type valueType) {
            Debug.Log($"TermType: {termType}; ValueType: {valueType}");
        }
        
        private void Export(Type serializerType) {
            Debug.Log($"SerializerType: {serializerType}");
        }

    }
}