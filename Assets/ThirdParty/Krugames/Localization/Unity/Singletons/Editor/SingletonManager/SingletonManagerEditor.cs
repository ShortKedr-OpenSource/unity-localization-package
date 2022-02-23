using Core.Unity.Singletons.Editor;
using Krugames.Core.Unity.Singletons.Editor.SingletonManager.Elements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Toolbar = Krugames.Core.Unity.Singletons.Editor.SingletonManager.Elements.Toolbar;

namespace Krugames.Core.Unity.Singletons.Editor.SingletonManager {
    public class SingletonManagerEditor : EditorWindow {

        private static readonly Vector2 DefaultSize = new Vector2(400, 500);
        private static readonly Vector2 MinSize = new Vector2(400, 400);

        private Vector2 _mainScrollPosition = Vector2.zero;
        
        #region VisualElements
        private Toolbar _toolbar;
        private SingletonList _singletonList;
        #endregion
        
        [MenuItem("Tools/Singleton Manager")]
        private static void ShowWindow() {
            var window = GetWindow<SingletonManagerEditor>(false, "Singleton Manager", true);
            window.minSize = MinSize;

            #region Position MultiScreen Bug Solution
            Resolution scaledResolution = Screen.currentResolution;
            scaledResolution.width = (int)(scaledResolution.width / EditorGUIUtility.pixelsPerPoint);
            scaledResolution.height = (int)(scaledResolution.height / EditorGUIUtility.pixelsPerPoint);
            
            if (window.position.x < -window.position.width ||
                window.position.y < -window.position.height ||
                window.position.x > scaledResolution.width ||
                window.position.y > scaledResolution.height) {
                window.position = new Rect(
                    (scaledResolution.width/2f) - (DefaultSize.x/2f), 
                    (scaledResolution.height/2f) - (DefaultSize.y/2f), 
                    DefaultSize.x, 
                    DefaultSize.y);
            }
            #endregion

            window.Show();
        }

        private void CreateGUI() {
            
            VisualElement root = rootVisualElement;
            root.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            
            root.Add(_toolbar = new Toolbar());
            root.Add(_singletonList = new SingletonList());
            
            _singletonList.SetSingletonTypes(ScriptableSingletonUtility.GetAllSingletonTypes());

            _toolbar.SearchField.RegisterCallback<ChangeEvent<string>>(SearchFieldChangeCallback);
            _toolbar.RefreshButton.clickable.clicked += RefreshAction;
            _toolbar.UpdateAssetsButton.clickable.clicked += UpdateAssetsAction;

        }

        private void SearchFieldChangeCallback(ChangeEvent<string> changeEvent) {
            _singletonList.SearchForSingleton(changeEvent.newValue);
        }

        private void RefreshAction() {
            _singletonList.UpdateSelf();
        }

        private void UpdateAssetsAction() {
            EditorUtility.DisplayProgressBar("Updating Singleton Assets", "Processing assemblies...", 0f);
            ScriptableSingletonUtility.UpdateAllSingletonAssets();
            EditorUtility.ClearProgressBar();
            _singletonList.SetSingletonTypes(ScriptableSingletonUtility.GetAllSingletonTypes());
            
            _toolbar.SearchField.SetValueWithoutNotify(string.Empty);
            _singletonList.CancelSearchResult();
            _singletonList.UpdateSelf();
        }
    }
}