using Krugames.LocalizationSystem.Unity.Singletons;
using UnityEngine;

namespace Krugames.LocalizationSystem {
    public class LocalizationSettings : ScriptableSingleton<LocalizationSettings> {

        [SerializeField] private bool autoInitialize = false;

        public static bool AutoInitialize => Instance.autoInitialize;
    }
}