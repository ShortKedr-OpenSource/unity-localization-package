using UnityEngine;

namespace Krugames.LocalizationSystem.Linkers {
    /// <summary>
    /// Base class for all Localization System linkers
    /// Linker - class that helps automatically link static content to localization system,
    /// and update it with proper localized content, happens on localization update
    /// </summary>
    public abstract class Linker : MonoBehaviour {

        /// <summary>
        /// Update linked content. Happens if localization event was
        /// caused in runtime and on awake of current linker.
        /// Usually it's happens after language change in runtime mode
        /// </summary>
        public abstract void UpdateContent();

        /// <summary>
        /// Links content to localization system on Awake
        /// </summary>
        private void Link() {
            Localization.AddLanguageUpdateCallback(UpdateContent);
        }

        /// <summary>
        /// Unlinks content from localization system on Destroy
        /// </summary>
        private void Unlink() {
            Localization.RemoveLanguageUpdateCallback(UpdateContent);
        }
        
        private void Awake() {
            UpdateContent();
            Link();
        }

        private void OnDestroy() {
            Unlink();
        }
    }
}