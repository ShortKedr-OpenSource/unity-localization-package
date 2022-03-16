using Krugames.LocalizationSystem.Models.Structs;

namespace Krugames.LocalizationSystem.Models.Interfaces {
    public interface ILocaleSettableLayout {
        public void SetLayout(TermStructureInfo[] layout);
    }
}