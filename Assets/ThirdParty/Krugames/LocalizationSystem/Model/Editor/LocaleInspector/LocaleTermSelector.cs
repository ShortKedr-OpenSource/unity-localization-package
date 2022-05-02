using System;
using Krugames.LocalizationSystem.Models.Locators;

//TODO move to SpecialElements folder
namespace ThirdParty.Krugames.LocalizationSystem.Model.Editor {
    public class LocaleTermSelector : SelectorListPopup {

        public delegate void SelectDelegate(Type selectedTermType, Type valueType);
        
        private static LocaleTermLocator.LocaleTermBuildData[] _buildData = LocaleTermLocator.BuildData;

        public static LocaleTermSelector Create(SelectDelegate callback) {

            Element[] elements = new Element[_buildData.Length];
            for (int i = 0; i < elements.Length; i++) {
                elements[i].Name = _buildData[i].Name;
                var ind = i;
                elements[i].ClickAction = () => callback?.Invoke(_buildData[ind].TermType, _buildData[ind].ValueType);
            }
            
            return new LocaleTermSelector("Locale terms", elements);
        }
        
        private LocaleTermSelector(string title, Element[] elements) : base(title, elements) {
        }
    }
}