using System;
using Krugames.LocalizationSystem.Editor.Serialization.Locators;
using Krugames.LocalizationSystem.Implementation;
using UnityEngine;

namespace Krugames.LocalizationSystem.Editor.UI {
    public class LocaleSerializerSelector : SelectorListPopup {
        public delegate void SelectDelegate(Type selectedSerializerType);

        private static LocaleSerializerLocator.LocaleSerializerBuildData[] _buildData = LocaleSerializerLocator.BuildData;

        public static LocaleSerializerSelector Create(SelectDelegate callback) {

            Element[] elements = new Element[_buildData.Length];
            for (int i = 0; i < elements.Length; i++) {
                elements[i].Name = _buildData[i].Name;
                var ind = i;
                elements[i].ClickAction = () => callback?.Invoke(_buildData[ind].SerializerType);
            }
            
            return new LocaleSerializerSelector("Serializers", elements);
        }
        
        private LocaleSerializerSelector(string title, Element[] elements) : base(title, elements) {
        }

        public override Vector2 GetWindowSize() {
            return new Vector2(175, 250);
        }
    }
}