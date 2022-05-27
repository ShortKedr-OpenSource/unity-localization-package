using Krugames.LocalizationSystem;
using Krugames.LocalizationSystem.Linkers;
using UnityEngine;
using UnityEngine.UI;

namespace ThirdParty.Krugames.LocalizationSystem.Unity.Linkers.Native {
    [AddComponentMenu("Localization/Native Linkers/Image Linker")]
    public class LocalizatedImageLinker : NativeLinker {
        
        [SerializeField] private string term = "none";
        [SerializeField] private Image image;

        public override void UpdateContent() {
            image.sprite = Localization.GetTermValue<Sprite>(term);
        }
    }
}