using Krugames.LocalizationSystem.Editor.Styles;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.PlainView {
    public class TermElementTableHeader : Toolbar {

        private const string HeaderContainerClassName = nameof(TermElementTableHeader)+"_HeaderContainer";
        private const string TermLabelClassName = nameof(TermElementTableHeader)+"_TermLabel";
        private const string ValueLabelClassName = nameof(TermElementTableHeader)+"_ValueLabel";
        private const string NoteLabelClassName = nameof(TermElementTableHeader)+"_NoteLabel";
        private const string PropsPlaceholderClassName = nameof(TermElementTableHeader)+"_PropsPlaceholder";
        private const string ScrollerPlaceholderClassName = nameof(TermElementTableHeader)+"_ScrollerPlaceholder";

        private readonly Box _headerContainer;
        private readonly Label _termHeadLabel;
        private readonly Label _valueHeadLabel;
        private readonly Label _noteHeadLabel;
        private readonly Button _propsPlaceholder;
        private readonly Scroller _scrollerPlaceholder;
        
        public Label TermHeadLabel => _termHeadLabel;

        public Label ValueHeadLabel => _valueHeadLabel;

        public Label NoteHeadLabel => _noteHeadLabel;

        public TermElementTableHeader() {
            //TODO review, style imports many times, cuz of hierarchy
            //styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);

            style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
            style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);

            _headerContainer = new Box(); 
            _termHeadLabel = new Label("term");
            _valueHeadLabel = new Label("value");
            _noteHeadLabel = new Label("note");
            _propsPlaceholder = new Button();
            _scrollerPlaceholder = new Scroller();

            _headerContainer.AddToClassList(HeaderContainerClassName);
            _termHeadLabel.AddToClassList(TermLabelClassName);
            _valueHeadLabel.AddToClassList(ValueLabelClassName);
            _noteHeadLabel.AddToClassList(NoteLabelClassName);
            _propsPlaceholder.AddToClassList(PropsPlaceholderClassName);
            _scrollerPlaceholder.AddToClassList(ScrollerPlaceholderClassName);
            
            _headerContainer.Add(_termHeadLabel);
            _headerContainer.Add(_valueHeadLabel);
            _headerContainer.Add(_noteHeadLabel);
            _headerContainer.Add(_propsPlaceholder);
            Add(_headerContainer);
            Add(_scrollerPlaceholder);
        }
    }
}