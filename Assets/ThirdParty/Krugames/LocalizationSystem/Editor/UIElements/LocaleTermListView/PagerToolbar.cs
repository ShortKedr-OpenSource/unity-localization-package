using System.Data;
using Krugames.LocalizationSystem.Editor.Styles;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UIElements {
    //TODO encapsulate pager to tree:            -> ToolbarPager;
    //TODO                                     /
    //TODO                             Pager --
    //TODO                                     \
    //TODO                                       -> PagerElement;
    public class PagerToolbar : Toolbar {
        
        private const string PreviousButtonClassName = nameof(PagerToolbar)+"_Previous";
        private const string NextButtonClassName = nameof(PagerToolbar)+"_Next";
        private const string CurrentPageClassName = nameof(PagerToolbar)+"_CurrentField";
        private const string PagesInfoClassName = nameof(PagerToolbar)+"_InfoLabel";
        
        private int _pageCount;
        private int _currentPage;

        private ToolbarButton _previousPageButton;
        private ToolbarButton _nextPageButton;
        private IntegerField _currentPageField;
        private Label _pagesInfoLabel;

        public delegate void PageChangeDelegate(PagerToolbar self, int newPage);
        public event PageChangeDelegate OnPageChange;
        

        public int CurrentPage {
            get => _currentPage;
            set {
                int previousPage = _currentPage;
                _currentPage = RulePageValue(value);
                _currentPageField.SetValueWithoutNotify(_currentPage);
                if (previousPage != _currentPage) OnPageChange?.Invoke(this, _currentPage);
            }
        }


        public PagerToolbar() : this(0) {
        }

        public PagerToolbar(int pageCount) {
            styleSheets.Add(LocalizationEditorStyles.GlobalStyle);

            _previousPageButton = new ToolbarButton(PreviousPage);
            _nextPageButton = new ToolbarButton(NextPage);
            _currentPageField = new IntegerField();
            _pagesInfoLabel = new Label();

            _currentPageField.isDelayed = true;
            _currentPageField.RegisterCallback<ChangeEvent<int>>(CurrentPageFieldChangeAction);
            
            _previousPageButton.AddToClassList(PreviousButtonClassName);
            _nextPageButton.AddToClassList(NextButtonClassName);
            _currentPageField.AddToClassList(CurrentPageClassName);
            _pagesInfoLabel.AddToClassList(PagesInfoClassName);
            
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
            _currentPage = RulePageValue(_currentPage);
            _currentPageField.SetValueWithoutNotify(_currentPage);
            UpdateInfoLabel();
        }

        public void ChangePageWithoutNotify(int newPage) {
            _currentPage = RulePageValue(newPage);
            _currentPageField.SetValueWithoutNotify(_currentPage);
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