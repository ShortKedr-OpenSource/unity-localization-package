using System.Collections.Generic;
using Krugames.LocalizationSystem.Models.Interfaces;
using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class FunctionGroup : Box {
        
        private const string TittleClassName = nameof(FunctionGroup) + "_Tittle";
        private const string ContainerClassName = nameof(FunctionGroup) + "_Container";
        
        private int _priority = 0;

        private List<FunctionElement> _functionElements = new List<FunctionElement>(16);

        private Label _tittleLabel;
        private Box _functionContainer;
        
        public int Priority => _priority;

        public FunctionElement[] FunctionElements => _functionElements.ToArray();

        public FunctionGroup(string tittle, int priority = 0) : this(tittle, null, priority) {
        }

        public FunctionGroup(string tittle, FunctionElement[] functionElements, int priority = 0) {
            _priority = 0;

            _tittleLabel = new Label(tittle);
            _functionContainer = new Box();
            
            _tittleLabel.AddToClassList(TittleClassName);
            _functionContainer.AddToClassList(ContainerClassName);
            
            Add(_tittleLabel);
            Add(_functionContainer);

            if (functionElements != null) AddFunctionElement(functionElements);
        }

        private void SortFunctionElements() {
            _functionElements.Sort(SortElementComparer);
            Update();
        }

        private int SortElementComparer(FunctionElement x, FunctionElement y) {
            int xPriority = x.Priority;
            int yPriority = y.Priority;
            if (xPriority < yPriority) return -1;
            if (xPriority > yPriority) return 1;
            return 0;
        }
        
        public void AddFunctionElement(FunctionElement element) {
            _functionElements.Add(element);
            SortFunctionElements();
        }

        public void AddFunctionElement(params FunctionElement[] elements) {
            _functionElements.AddRange(elements);
            SortFunctionElements();
        }

        public void RemoveFunctionElement(FunctionElement element) {
            _functionElements.Remove(element);
            SortFunctionElements();
        }

        public void RemoveFunctionElement(params FunctionElement[] elements) {
            for (int i = 0; i < elements.Length; i++) {
                _functionElements.Remove(elements[i]);
            }
            SortFunctionElements();
        }

        public void SetTittle(string text) {
            _tittleLabel.text = text;
        }

        public virtual void Update() {
            _functionContainer.Clear();
            for (int i = 0; i < _functionElements.Count; i++) {
                if (_functionElements[i].Function.IsValid) _functionContainer.Add(_functionElements[i]);
            }
        }

        public bool HaveFunctions() {
            for (int i = 0; i < _functionElements.Count; i++) {
                if (_functionElements[i].Function.IsValid) return true;
            }
            return false;
        }
    }

    public class FunctionGroup<TSourceType> : FunctionGroup {

        private TSourceType _source;
        public TSourceType Source => _source;

        public FunctionGroup(TSourceType source, int priority = 0) 
            : this(source, null, priority) {
        }

        public FunctionGroup(TSourceType source, FunctionElement[] functionElements, int priority = 0) 
            : base(string.Empty, functionElements, priority) {
        }

        public void SetSource(TSourceType source) {
            _source = source;
            SetTittle($"Options for {_source}:");
        }
    }
}