using UnityEngine;

namespace Krugames.LocalizationSystem.Models {
    public class LocalizationTopLayerTest : MonoBehaviour {

        [SerializeField] private LocaleLibrary localeLibrary;
        
        [SerializeField] private string testString1;
        [SerializeField] private string testString2;
        [SerializeField] private string testString3;

        [SerializeField] private Texture testTexture;
        [SerializeField] private Sprite testSprite;
        
        [SerializeField] private AudioClip testAudioClip1;
        [SerializeField] private AudioClip testAudioClip2;
        
        private void Start() {
            UpdateValues();
            Localization.AddLanguageUpdateCallback(UpdateValues);
        }

        private void UpdateValues() {

            testString1 = localeLibrary.GetTermValue<string>("test_string_1");
            testString2 = localeLibrary.GetTermValue<string>("test_string_2");
            testString3 = localeLibrary.GetTermValue<string>("test_string_3");

            testTexture = localeLibrary.GetTermValue<Texture>("test_texture");
            testSprite = localeLibrary.GetTermValue<Sprite>("test_sprite");

            testAudioClip1 = localeLibrary.GetTermValue<AudioClip>("test_audio_clip_1");
            testAudioClip2 = localeLibrary.GetTermValue<AudioClip>("test_audio_clip_2");

            //TODO update localeLibrary to Localization request
        }
    }
}