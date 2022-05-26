using UnityEngine.UIElements;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public class FunctionElement : Button {

        private int _priority;
        private IFunction _function;
        
        public IFunction Function => _function;
        
        public int Priority => _priority;

        public FunctionElement(IFunction function, int priority = 0) {
            _priority = priority;
            clicked += Event_OnClick;
            SetFunction(function);
        }

        private void Event_OnClick() {
            _function?.Execute();
        }
        
        public void Update() {
            if (_function == null) return;
            text = _function.Description;
        }

        public void SetFunction(IFunction function) {
            _function = function;
            Update();
        }
    }
    
    public class FunctionElement<TFunctionType> : FunctionElement where TFunctionType : IFunction {

        private TFunctionType _function;
        public new TFunctionType Function => _function;

        public FunctionElement(TFunctionType function, int priority = 0) : base(function, priority) {
            _function = function;
        }

        public void SetFunction(TFunctionType function) {
            base.SetFunction(function);
            _function = function;
        }
    }
}