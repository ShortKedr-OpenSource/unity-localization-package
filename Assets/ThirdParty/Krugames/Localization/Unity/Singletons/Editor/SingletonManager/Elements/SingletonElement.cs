using System;
using Core.Unity.Singletons.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.Core.Unity.Singletons.Editor.SingletonManager.Elements {
    public class SingletonElement : Box {

        private Type _singletonType; 
        
        private Label _nameLabel;
        private Label _statusLabel;
        
        private Box _actionGroup;

        private Button _createAssetButton;
        private Button _selectAssetButton;
        private Button _propertiesButton;

        public Type SingletonType => _singletonType;
        
        public Label NameLabel => _nameLabel;
        public Label StatusLabel => _statusLabel;
        
        public Box ActionGroup => _actionGroup;
        
        public Button SelectAssetButton => _selectAssetButton;

        public Button CreateAssetButton => _createAssetButton;

        public Button PropertiesButton => _propertiesButton;

        public SingletonElement(Type singletonType) : base(){
            SetType(singletonType);

            style.flexGrow = 0f;
            style.justifyContent = new StyleEnum<Justify>(Justify.FlexStart);
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            style.alignItems = new StyleEnum<Align>(Align.Stretch);

            style.marginTop = 5f;
            style.marginBottom = 5f;

            style.paddingTop = 6f;
            style.paddingBottom = 6f;
            style.paddingLeft = 6f;
            style.paddingRight = 6f;

            style.borderBottomLeftRadius = 4f;
            style.borderBottomRightRadius = 4f;
            style.borderTopLeftRadius = 4f;
            style.borderTopRightRadius = 4f;

            style.borderBottomWidth = 1f;

            style.width = new StyleLength(StyleKeyword.Auto);
            style.height = new StyleLength(StyleKeyword.Auto);

            _nameLabel = new Label() {
                name = "nameLabel",
                style = {
                    width = new StyleLength(StyleKeyword.Auto),
                    height = new StyleLength(StyleKeyword.Auto),
                    marginBottom = 4f,
                    fontSize = new StyleLength(13f),
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold),
                }
            };

            _statusLabel = new Label() {
                name = "statusLabel",
                style = {
                    width = new StyleLength(StyleKeyword.Auto),
                    height = new StyleLength(StyleKeyword.Auto),
                }
            };
            
            _actionGroup = new Box() {
                name = "actionGroup",
                style = {
                    backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0f)),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    justifyContent = new StyleEnum<Justify>(Justify.FlexEnd),
                    width = new StyleLength(StyleKeyword.Auto),
                    height = new StyleLength(StyleKeyword.Auto),
                }
            };
            
            _createAssetButton = new Button() {
                name = "createAssetButton",
                text = "Create Asset",
            };
            _createAssetButton.clickable.clicked += CreateAction;
            
            _selectAssetButton = new Button() {
                name = "selectAssetButton",
                text = "Select",
            };
            _selectAssetButton.clickable.clicked += PingAction;
            
            _propertiesButton = new Button() {
                name = "propertiesButton",
                text = "Properties",
            };
            _propertiesButton.clickable.clicked += PropertiesAction;
            
            _actionGroup.Add(_createAssetButton);
            _actionGroup.Add(_selectAssetButton);
            _actionGroup.Add(_propertiesButton);
            
            this.Add(_nameLabel);
            this.Add(_statusLabel);
            this.Add(_actionGroup);
            
            UpdateSelf();
        }

        public void SetType(Type singletonType) {
            if (ScriptableSingletonUtility.IsTypeScriptableSingleton(singletonType)) {
                _singletonType = singletonType;
            } else {
                _singletonType = null;
            }
        }

        public void UpdateSelf() {
            if (_singletonType == null) {
                _nameLabel.text = "[None]";
                _statusLabel.text = "No data";
                
                return;
            }

            bool assetExists = ScriptableSingletonUtility.IsSingletonAssetExist(_singletonType);

            _actionGroup.Clear();
            if (assetExists) {
                _actionGroup.Add(_selectAssetButton);
                _actionGroup.Add(_propertiesButton);
            } else {
                _actionGroup.Add(_createAssetButton);
            }

            _nameLabel.text = _singletonType.Name;
            _statusLabel.text = (assetExists) ? "Asset file is found" : "Asset file not exists yet. Create one first";
        }

        public void CreateAction() {
            if (_singletonType == null) return;
            ScriptableSingletonUtility.UpdateSingletonAsset(_singletonType);
        }
        
        public void PingAction() {
            if (_singletonType == null) return;
            var asset = ScriptableSingletonUtility.GetAsset(_singletonType);
            if (asset != null) {
                EditorGUIUtility.PingObject(asset);
                Selection.activeObject = asset;
            }
        }

        public void PropertiesAction() {
            if (_singletonType == null) return;
            var asset = ScriptableSingletonUtility.GetAsset(_singletonType);
            if (asset != null) {
                //TODO open properties in new undocked window
                AssetDatabase.OpenAsset(asset);
            }
        }
    }
}