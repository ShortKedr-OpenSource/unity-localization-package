using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class FunctionGroupsList : TittledContentBox {
        
        private List<FunctionGroup> _functionGroups = new List<FunctionGroup>(16);
        
        private ScrollView _scrollView;
        public FunctionGroup[] FunctionGroups => _functionGroups.ToArray();

        public FunctionGroupsList(string tittle, FunctionGroup[] groups = null) : base(tittle) {

            _scrollView = new ScrollView(ScrollViewMode.Vertical) {
                style = {
                    flexGrow = 1f,
                }
            };
            
            Content.Add(_scrollView);
            
            if (groups != null) AddFunctionGroup(groups);
        }

        private void SortFunctionGroups() {
            _functionGroups.Sort(SortGroupComparer);
            Update();
        }

        private int SortGroupComparer(FunctionGroup x, FunctionGroup y) {
            int xPriority = x.Priority;
            int yPriority = y.Priority;
            if (xPriority < yPriority) return -1;
            if (xPriority > yPriority) return 1;
            return 0;
        }
        
        public void AddFunctionGroup(FunctionGroup group) {
            _functionGroups.Add(group);
            SortFunctionGroups();
        }
        
        public void AddFunctionGroup(params FunctionGroup[] groups) {
            _functionGroups.AddRange(groups);
            SortFunctionGroups();
        }

        public void RemoveFunctionGroup(FunctionGroup group) {
            _functionGroups.Remove(group);
            SortFunctionGroups();
        }

        public void RemoveFunctionGroup(params FunctionGroup[] groups) {
            for (int i = 0; i < groups.Length; i++) {
                _functionGroups.Remove(groups[i]);
            }
            SortFunctionGroups();
        }

        public void Update() {
            _scrollView.Clear();
            for (int i = 0; i < _functionGroups.Count; i++) {
                if (_functionGroups[i].HaveFunctions()) {
                    _functionGroups[i].Update();
                    _scrollView.Add(_functionGroups[i]);
                }
            }
        }
    }
}