using Krugames.LocalizationSystem;
using Krugames.LocalizationSystem.Linkers;
using UnityEngine;

namespace ThirdParty.Krugames.LocalizationSystem.Unity.Linkers.Native {
    [AddComponentMenu("Localization/Native Linkers/Audio Source Linker")]
    public class LocalizatedAudioSourceLinker : NativeLinker {

        [SerializeField] private string term = "none";
        [SerializeField] private AudioSource audioSource;
        
        public override void UpdateContent() {
            audioSource.clip = Localization.GetTermValue<AudioClip>(term);
        }
    }
}