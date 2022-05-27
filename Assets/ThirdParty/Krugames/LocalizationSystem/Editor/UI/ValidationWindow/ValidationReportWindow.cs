using Krugames.LocalizationSystem.Editor.Styles;
using Krugames.LocalizationSystem.Models.Validation;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


//TODO report null check;
//TODO bad data safety (nulls)
namespace Krugames.LocalizationSystem.Editor.UI.ValidationWindow {
    public class ValidationReportWindow : EditorWindow {

        private const string DescriptionClassName = nameof(ValidationReportWindow) + "_Description";
        private const string DescriptionLabelClassName = nameof(ValidationReportWindow) + "_DescriptionLabel";
        
        private static readonly Vector2 WindowSize = new Vector2(350, 500); 
        
        private ValidationReport _report;
        private string _description;

        private VisualElement _root;
        private VisualElement _descriptionElement;
        private ScrollView _scrollView;
        
        public static ValidationReportWindow CreateWindow (ValidationReport report, string description) {
            ValidationReportWindow editorWindow = EditorWindow.CreateInstance<ValidationReportWindow>();
            editorWindow.titleContent = new GUIContent("Validation report");
            editorWindow.minSize = WindowSize;
            editorWindow.maxSize = WindowSize;
            editorWindow._report = report;
            editorWindow._description = description;
            return editorWindow;
        }

        public static void ShowReport(ValidationReport report, string description) {
            CreateWindow(report, description).ShowUtility();
        }

        private void CreateGUI() {
            _root = rootVisualElement;
            _root.styleSheets.Add(LocalizationEditorStyles.GlobalStyle);

            _descriptionElement = new HelpBox(_description, HelpBoxMessageType.None) {
                style = {
                    flexGrow = 0,
                    borderTopWidth = 0,
                    borderBottomWidth = 0,
                    borderLeftWidth = 0,
                    borderRightWidth = 0,
                    marginTop = 4,
                    marginBottom = 4,
                }
            };
            _descriptionElement.AddToClassList(DescriptionClassName);
            _descriptionElement.Q<Label>().AddToClassList(DescriptionLabelClassName);
            
            _scrollView = new ScrollView(ScrollViewMode.Vertical) {
                style = {
                    flexGrow = 1,
                    fontSize = 12,
                }
            };

            if (_report != null) {
                for (int i = 0; i < _report.Errors.Length; i++) {
                    _scrollView.Add(new ValidationErrorElement(_report.Errors[i]));
                }
            }

            _root.Add(_descriptionElement);
            _root.Add(_scrollView);
        }
    }
}