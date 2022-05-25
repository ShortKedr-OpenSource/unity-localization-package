using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class PagerTooltipToolbar : Toolbar {
        
        private const string PreviousButtonClassName = nameof(PagerTooltipToolbar)+"_Previous";
        private const string NextButtonClassName = nameof(PagerTooltipToolbar)+"_Next";
        private const string CurrentPageClassName = nameof(PagerTooltipToolbar)+"_CurrentField";
        private const string PagesInfoClassName = nameof(PagerTooltipToolbar)+"_PageInfoLabel";
        private const string TooltipClassName = nameof(PagerTooltipToolbar) + "_TooltipLabel";

        private int _pageCount;
        private int _currentPage;

        private ToolbarButton _previousPageButton;
        private ToolbarButton _nextPageButton;
        private IntegerField _currentPageField;
        private Label _pagesInfoLabel;
        private Label _tooltipLabel;

        public delegate void PageChangeDelegate(PagerTooltipToolbar self, int newPage);
        public event PageChangeDelegate OnPageChange;
        
        
        public int PageCount => _pageCount;

        public int CurrentPage {
            get => _currentPage;
            set {
                int previousPage = _currentPage;
                _currentPage = RulePageValue(value);
                _currentPageField.SetValueWithoutNotify(_currentPage);
                if (previousPage != _currentPage) OnPageChange?.Invoke(this, _currentPage);
            }
        }


        public PagerTooltipToolbar() : this(0) {
        }

        public PagerTooltipToolbar(int pageCount) {
            //TODO review, style imports many times, cuz of hierarchy
            //styleSheets.Add(LocalizationEditorStyles.LocalizationEditorStyle);

            _previousPageButton = new ToolbarButton(PreviousPage);
            _nextPageButton = new ToolbarButton(NextPage);
            _currentPageField = new IntegerField();
            _pagesInfoLabel = new Label();
            _tooltipLabel = new Label();
            
            _currentPageField.isDelayed = true;
            _currentPageField.RegisterCallback<ChangeEvent<int>>(CurrentPageFieldChangeAction);
            
            _previousPageButton.AddToClassList(PreviousButtonClassName);
            _nextPageButton.AddToClassList(NextButtonClassName);
            _currentPageField.AddToClassList(CurrentPageClassName);
            _pagesInfoLabel.AddToClassList(PagesInfoClassName);
            _tooltipLabel.AddToClassList(TooltipClassName);
            
            Add(_tooltipLabel);
            Add(_previousPageButton);
            Add(_currentPageField);
            Add(_pagesInfoLabel);
            Add(_nextPageButton);

            _pageCount = pageCount;
            CurrentPage = 1;
            _currentPageField.SetValueWithoutNotify(_currentPage);
            UpdateInfoLabel();
        }

        private void CurrentPageFieldChangeAction(ChangeEvent<int> evt) {
            CurrentPage = evt.newValue;
        }

        public void SetPageCount(int pageCount) {
            SetPageCountWithoutNotify(pageCount);
            OnPageChange?.Invoke(this, _currentPage);
        }

        public void SetPageCountWithoutNotify(int pageCount) {
            _pageCount = pageCount;
            ChangePageWithoutNotify(_currentPage);
            UpdateInfoLabel();
        }

        public void ChangePageWithoutNotify(int newPage) {
            _currentPage = RulePageValue(newPage);
            _currentPageField.SetValueWithoutNotify(_currentPage);
        }

        public void SetTooltip(string text) {
            _tooltipLabel.text = text;
        }

        private int RulePageValue(int pageValue) {
            if (_pageCount == 0) pageValue = 0;
            if (pageValue > _pageCount) pageValue = _pageCount;
            else if (pageValue < 1) pageValue = 1;
            return pageValue;
        }
        
        private void NextPage() {
            CurrentPage += 1;
        }

        private void PreviousPage() {
            CurrentPage -= 1;
        }

        private void UpdateInfoLabel() {
            _pagesInfoLabel.text = $"/ {_pageCount} ";
        }
    }
}