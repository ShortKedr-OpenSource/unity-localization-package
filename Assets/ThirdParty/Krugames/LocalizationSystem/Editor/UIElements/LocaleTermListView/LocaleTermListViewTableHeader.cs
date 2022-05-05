using System;
using Krugames.LocalizationSystem.Editor.Styles;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    public class LocaleTermListViewTableHeader : Toolbar {

        private const string TermLabelClassName = nameof(LocaleTermListViewTableHeader)+"_TermLabel";
        private const string ValueLabelClassName = nameof(LocaleTermListViewTableHeader)+"_ValueLabel";
        
        private readonly Label _termHeadLabel;
        private readonly Label _valueHeadLabel;

        
        public Label TermHeadLabel => _termHeadLabel;

        public Label ValueHeadLabel => _valueHeadLabel;

        public LocaleTermListViewTableHeader() {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);

            _termHeadLabel = new Label("term") {
                style = {
                    minWidth = EditorGUIUtility.labelWidth,
                }
            };

            _valueHeadLabel = new Label("value");
            
            _termHeadLabel.AddToClassList(TermLabelClassName);
            _valueHeadLabel.AddToClassList(ValueLabelClassName);
            
            Add(_termHeadLabel);
            Add(_valueHeadLabel);
        }
    }
}