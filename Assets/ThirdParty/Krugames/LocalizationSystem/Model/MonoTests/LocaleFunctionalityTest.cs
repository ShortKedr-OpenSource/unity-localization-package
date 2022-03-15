using System;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models.MonoTests {
    
    public class LocaleFunctionalityTest : MonoBehaviour {
        [SerializeField] private Locale locale;

        private void Start() {
            Debug.Log(locale.Language);
            
        }
    }
}