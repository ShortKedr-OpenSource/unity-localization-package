using Krugames.LocalizationSystem.Common.Editor.UnityInternal;
using Krugames.LocalizationSystem.Models;

namespace Krugames.LocalizationSystem.Editor.UI.LocalizationEditor.Functions {
    public class TermOpenPropertiesFunction : IFunction {

        public static readonly TermOpenPropertiesFunction Invalid = new TermOpenPropertiesFunction(null);
        
        private LocaleTerm _term;

        
        public LocaleTerm Term => _term;

        public bool IsValid => _term != null;

        public string Description => $"Open properties";

        public TermOpenPropertiesFunction(LocaleTerm term) {
            _term = term;
        }

        public void Execute() {
            EditorInternalUtility.OpenPropertyEditor(_term);
        }
    }
}