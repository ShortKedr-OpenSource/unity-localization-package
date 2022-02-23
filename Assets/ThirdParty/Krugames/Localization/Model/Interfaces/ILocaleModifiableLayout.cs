using Krugames.LocalizationSystem.Models.Structs;

namespace Krugames.LocalizationSystem.Models.Interfaces {
    public interface ILocaleModifiableLayout {
        public TermStructureInfo[] GetLayout();
        public TermStructureInfo[] SetLayout();
    }
}