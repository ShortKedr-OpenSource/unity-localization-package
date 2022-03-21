using System.Collections;
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
        [SerializeField] private AudioClip testAudioClip3;
        
        private void Start() {
            UpdateValues();
            Localization.AddLanguageUpdateCallback(UpdateValues);
        }

        private void UpdateValues() {

            testString1 = Localization.GetTermValue<string>("test_string_1");
            testString2 = Localization.GetTermValue<string>("test_string_2");
            testString3 = Localization.GetTermValue<string>("test_string_3");

            testTexture = Localization.GetTermValue<Texture>("test_texture");
            testSprite = Localization.GetTermValue<Sprite>("test_sprite");

            testAudioClip1 = Localization.GetTermValue<AudioClip>("test_audio_clip_1");
            testAudioClip2 = Localization.GetTermValue<AudioClip>("test_audio_clip_2");
            testAudioClip3 = Localization.GetTermValue<AudioClip>("test_audio_clip_3");

            StartCoroutine(PlayVoices());
        }

        IEnumerator PlayVoices() {
            AudioSource.PlayClipAtPoint(testAudioClip1, Vector3.zero);
            yield return new WaitForSecondsRealtime(testAudioClip1.length);
            AudioSource.PlayClipAtPoint(testAudioClip2, Vector3.zero);
            yield return new WaitForSecondsRealtime(testAudioClip2.length);
            AudioSource.PlayClipAtPoint(testAudioClip3, Vector3.zero);
        }
    }
}