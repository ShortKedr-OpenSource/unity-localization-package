using Krugames.LocalizationSystem.Models.Structs;

namespace Krugames.LocalizationSystem.Models.Interfaces {
    public interface ILocaleGettableLayout {
        public TermStructureInfo[] GetLayout();
    }
}