using System;
using Krugames.LocalizationSystem.Editor.Package;
using Krugames.LocalizationSystem.Editor.Serialization.Locators;
using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models.Locators;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor {
    public class LocaleTermSelector : PopupWindowContent {
        
        private static LocaleTermLocator.LocaleTermBuildData[] _buildData = LocaleTermLocator.BuildData;
        private static LocaleSerializerLocator.LocaleSerializerBuildData[] _serializerBuildData =
            LocaleSerializerLocator.BuildData;

        private HeaderTitleElement _headerTitleElement;
        private VisualElement _mainGroup;
        private VisualElement _listGroup;

        public override void OnOpen() {

            VisualElement root = editorWindow.rootVisualElement;
            root.styleSheets.Add(LocalizationEditorStyles.GlobalStyle);

            root.style.marginTop = 0;
            root.style.marginBottom = 0;
            root.style.marginLeft = 0;
            root.style.marginRight = 0;
            root.Add(_mainGroup = new Box() {
                style = {
                    flexGrow = 1,

                    borderTopColor = Color.blue,
                    borderBottomColor = Color.blue,
                    borderLeftColor = Color.blue,
                    borderRightColor = Color.blue,
                    
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                }
            });

            _mainGroup.Add(_headerTitleElement = new HeaderTitleElement("Locale terms"));
            
            for (int i = 0; i < _buildData.Length; i++) {
                _mainGroup.Add(new ListElement(){text=_buildData[i].Name});
            }
        }
        
        public override Vector2 GetWindowSize() {
            return new Vector2(200, 300);
        }

        public override void OnGUI(Rect rect) {
            //do nothing since we implement it with UIElements
        }

        private class ListElement : Button { //TODO move to global scope
            public ListElement() : this(null){
            }

            public ListElement(Action clickEvent) : base(clickEvent) {
            }
        }

        private class HeaderTitleElement : Toolbar { //TODO move to global scope

            private Label _titleLabel;

            public string Title {
                get => _titleLabel.text;
                set => _titleLabel.text = value;
            }

            public HeaderTitleElement(string title) {
                
                style.fontSize = 12;
                style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
                style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
                style.height = 28;
                
                Add(_titleLabel = new Label(title) {
                    style = {
                        flexGrow = 1,
                    }
                });
            }
        }
    }
}