using Krugames.LocalizationSystem.Editor.Styles;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.PlainView {
    public class PlaintTermElementTableHeader : Toolbar {

        private const string HeaderContainerClassName = nameof(PlaintTermElementTableHeader)+"_HeaderContainer";
        private const string TermLabelClassName = nameof(PlaintTermElementTableHeader)+"_TermLabel";
        private const string ValueLabelClassName = nameof(PlaintTermElementTableHeader)+"_ValueLabel";
        private const string NoteLabelClassName = nameof(PlaintTermElementTableHeader)+"_NoteLabel";
        private const string PropsPlaceholderClassName = nameof(PlaintTermElementTableHeader)+"_PropsPlaceholder";
        private const string ScrollerPlaceholderClassName = nameof(PlaintTermElementTableHeader)+"_ScrollerPlaceholder";

        private readonly Box _headerContainer;
        private readonly Label _termHeadLabel;
        private readonly Label _valueHeadLabel;
        private readonly Label _noteHeadLabel;
        private readonly Button _propsPlaceholder;
        private readonly Scroller _scrollerPlaceholder;
        
        public Label TermHeadLabel => _termHeadLabel;

        public Label ValueHeadLabel => _valueHeadLabel;

        public Label NoteHeadLabel => _noteHeadLabel;

        public PlaintTermElementTableHeader() {

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