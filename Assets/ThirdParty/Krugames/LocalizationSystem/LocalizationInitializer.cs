using UnityEngine;

namespace Krugames.LocalizationSystem {
    internal static class LocalizationInitializer {
        
        [RuntimeInitializeOnLoadMethod]
        public static void Initialize() {
            Debug.Log("Omaewa mu mou shinderu");
        }
    }
}