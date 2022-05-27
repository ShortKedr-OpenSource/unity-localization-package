using Krugames.LocalizationSystem;
using Krugames.LocalizationSystem.Linkers;
using UnityEngine;

namespace ThirdParty.Krugames.LocalizationSystem.Unity.Linkers.Native {
    [AddComponentMenu("Localization/Native Linkers/Sprite Renderer Linker")]
    public class LocalizatedSpriteRendererLinker : NativeLinker {

        [SerializeField] private string term = "none";
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public override void UpdateContent() {
            spriteRenderer.sprite = Localization.GetTermValue<Sprite>(term);
        }
    }
}