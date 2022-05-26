namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor {
    public interface IFunction {
        public bool IsValid { get; }
        public string Description { get; }
        public void Execute();
        
        //public string Tooltip { get; } //TODO review
    }
}