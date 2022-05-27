using Krugames.LocalizationSystem;
using Krugames.LocalizationSystem.Linkers;
using UnityEngine;
using UnityEngine.UI;

namespace ThirdParty.Krugames.LocalizationSystem.Unity.Linkers.Native {
    [AddComponentMenu("Localization/Native Linkers/Raw Image Linker")]
    public class LocalizatedRawImageLinker : NativeLinker {

        [SerializeField] private string term = "none";
        [SerializeField] private RawImage rawImage;
        
        public override void UpdateContent() {
            rawImage.texture = Localization.GetTermValue<Texture>(term);
        }
    }
}