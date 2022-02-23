using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.Core.Unity.Singletons.Editor.SingletonManager.Elements {
    public class Toolbar : UnityEditor.UIElements.Toolbar {

        private Box _leftGroup;
        private Box _centerGroup;
        private Box _rightGroup;
        
        private Button _updateAssetsButton;
        private Button _refreshButton;
        private Label _searchLabel;
        private TextField _searchField;

        public Box LeftGroup => _leftGroup;

        public Box CenterGroup => _centerGroup;

        public Box RightGroup => _rightGroup;

        public Button UpdateAssetsButton => _updateAssetsButton;

        public Button RefreshButton => _refreshButton;

        public Label SearchLabel => _searchLabel;

        public TextField SearchField => _searchField;

        public Toolbar() : base() {

            style.flexGrow = 0f;
            style.justifyContent = new StyleEnum<Justify>(Justify.FlexStart);
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            style.alignItems = new StyleEnum<Align>(Align.Center);

            style.height = new StyleLength(StyleKeyword.Auto);
            
            _leftGroup = new Box() {
                name = "leftGroup",
                style = {
                    backgroundColor = new StyleColor(new Color(0f,0f,0f,0f)),
                    flexGrow = 1f,
                    justifyContent = new StyleEnum<Justify>(Justify.FlexStart),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    height = new StyleLength(StyleKeyword.Auto),
                    alignItems = new StyleEnum<Align>(Align.Center),
                },
            };

            _centerGroup = new Box() {
                name = "centerGroup",
                style = {
                    backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0f)),
                    flexGrow = 1f,
                    justifyContent = new StyleEnum<Justify>(Justify.Center),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    height = new StyleLength(StyleKeyword.Auto),
                    alignItems = new StyleEnum<Align>(Align.Center),
                }
            };
            
            _rightGroup = new Box() {
                name = "rightGroup",
                style = {
                    backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0f)),
                    flexGrow = 1f,
                    justifyContent = new StyleEnum<Justify>(Justify.FlexEnd),
                    flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row),
                    height = new StyleLength(StyleKeyword.Auto),
                    alignItems = new StyleEnum<Align>(Align.Center),
                }
            };

            _updateAssetsButton = new Button() {
                name = "updateAssetsButton",
                text = "Update Assets",
            };
            
            _refreshButton = new Button() {
                name = "refreshButton",
                text = "Refresh",
            };

            _searchLabel = new Label("Search:") {
                name = "searchLabel",
                style = {
                    width = new StyleLength(StyleKeyword.Auto)
                }
            };

            _searchField = new TextField(512, false, false, '*') {
                name = "searchField",
                style = {
                    maxWidth = 150f,
                    minWidth = 150f,
                    flexGrow = 0f,
                    flexShrink = 0f,
                }
            };
            
            _leftGroup.Add(_updateAssetsButton);
            _leftGroup.Add(_refreshButton);
            
            _rightGroup.Add(_searchLabel);
            _rightGroup.Add(_searchField);
            
            this.Add(_leftGroup);
            this.Add(_centerGroup);
            this.Add(_rightGroup);
        }
    }
}