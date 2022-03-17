using System;
using Krugames.LocalizationSystem.Models.Terms;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models {
    public class LocaleTest : MonoBehaviour {
        [SerializeField] private Locale locale;

        public void Start() {
            Debug.Log("Language: " + locale.Language);

            Type[] supportedTermTypes = locale.SupportedTermTypes;
            string supportedTermTypesPrint = "Supported Term Types:\n";
            for (int i = 0; i < supportedTermTypes.Length; i++) 
                supportedTermTypesPrint += (supportedTermTypes[i]) + "\n";
            Debug.Log(supportedTermTypesPrint);
            
            Type[] supportedValueTypes = locale.SupportedValueTypes;
            string supportedValueTypesPrint = "Supported Value Types:\n";
            for (int i = 0; i < supportedValueTypes.Length; i++) 
                supportedValueTypesPrint += (supportedValueTypes[i]) + "\n";
            Debug.Log(supportedValueTypesPrint);

            Debug.Log($"Supports term {nameof(StringTerm)}: " + locale.SupportsTermType(typeof(StringTerm)));
            Debug.Log($"Supports term {nameof(SpriteTerm)}: " + locale.SupportsTermType(typeof(SpriteTerm)));
            Debug.Log($"Supports term {nameof(TextureTerm)}: " + locale.SupportsTermType(typeof(TextureTerm)));
            Debug.Log($"Supports term {nameof(AudioClipTerm)}: " + locale.SupportsTermType(typeof(AudioClipTerm)));
            Debug.Log($"Supports term {nameof(UnregisteredTestTerm)}: " + locale.SupportsTermType(typeof(UnregisteredTestTerm)));
            Debug.Log($"Supports term {nameof(RegisteredNotUsedTerm)}: " + locale.SupportsTermType(typeof(RegisteredNotUsedTerm)));

            Debug.Log($"Supports value {nameof(String)}: " + locale.SupportsValueType(typeof(string)));
            Debug.Log($"Supports value {nameof(Sprite)}: " + locale.SupportsValueType(typeof(Sprite)));
            Debug.Log($"Supports value {nameof(Texture)}: " + locale.SupportsValueType(typeof(Texture)));
            Debug.Log($"Supports value {nameof(AudioClip)}: " + locale.SupportsValueType(typeof(AudioClip)));
            Debug.Log($"Supports value {nameof(Vector2)}: " + locale.SupportsValueType(typeof(Vector2)));
            Debug.Log($"Supports value {nameof(Quaternion)}: " + locale.SupportsValueType(typeof(Quaternion)));

            Debug.Log($"Contains term {nameof(StringTerm)}: " + locale.ContainsTermType(typeof(StringTerm)));
            Debug.Log($"Contains term {nameof(SpriteTerm)}: " + locale.ContainsTermType(typeof(SpriteTerm)));
            Debug.Log($"Contains term {nameof(TextureTerm)}: " + locale.ContainsTermType(typeof(TextureTerm)));
            Debug.Log($"Contains term {nameof(AudioClipTerm)}: " + locale.ContainsTermType(typeof(AudioClipTerm)));
            Debug.Log($"Contains term {nameof(UnregisteredTestTerm)}: " + locale.ContainsTermType(typeof(UnregisteredTestTerm)));
            Debug.Log($"Contains term {nameof(RegisteredNotUsedTerm)}: " + locale.ContainsTermType(typeof(RegisteredNotUsedTerm)));
            
            Debug.Log($"Contains value {nameof(String)}: " + locale.ContainsValueType(typeof(string)));
            Debug.Log($"Contains value {nameof(Sprite)}: " + locale.ContainsValueType(typeof(Sprite)));
            Debug.Log($"Contains value {nameof(Texture)}: " + locale.ContainsValueType(typeof(Texture)));
            Debug.Log($"Contains value {nameof(AudioClip)}: " + locale.ContainsValueType(typeof(AudioClip)));
            Debug.Log($"Contains value {nameof(Vector2)}: " + locale.ContainsValueType(typeof(Vector2)));
            Debug.Log($"Contains value {nameof(Quaternion)}: " + locale.ContainsValueType(typeof(Quaternion)));

            
            // have result
            LocaleTerm stringTerm = locale.GetTerm("test_string");
            Debug.Log("Q1: " + stringTerm + " | " + stringTerm.Value);

            // have result
            stringTerm = locale.GetTerm("test_string", typeof(StringTerm));
            Debug.Log("Q2: " + stringTerm + " | " + stringTerm.Value);

            // have result
            StringTerm typedStringTerm = locale.GetTerm<StringTerm>("test_string");
            Debug.Log("Q3: " + typedStringTerm + " | " + typedStringTerm.Value);

            // null
            stringTerm = locale.GetTerm("some_shit");
            Debug.Log("Q4: " + stringTerm);
            
            // null
            stringTerm = locale.GetTerm("test_string", typeof(Vector2));
            Debug.Log("Q5: " + stringTerm);

            // null
            var typedTextureTerm = locale.GetTerm<TextureTerm>("test_string");
            Debug.Log("Q6: " + typedStringTerm);

            
            // have result
            object stringValue = locale.GetTermValue("test_string");
            Debug.Log("Q7: " + stringValue);

            // have result
            stringValue = locale.GetTermValue("test_string", typeof(string));
            Debug.Log("Q8: " + stringValue);

            // have result
            string typedStringValue = locale.GetTermValue<string>("test_string");
            Debug.Log("Q9: " + typedStringValue);

            // null
            stringValue = locale.GetTermValue("some_shit");
            Debug.Log("Q10: " + stringValue);
            
            // null
            stringValue = locale.GetTermValue("test_string", typeof(Vector2));
            Debug.Log("Q11: " + stringValue);
            
            // null
            var typedTextureValue = locale.GetTermValue<Texture>("test_string");
            Debug.Log("Q12: " + typedTextureValue);
            
            // all null
            Debug.Log("Q13: " + locale.GetTerm("fake_term"));
            Debug.Log("Q14: " + locale.GetTerm("fake_term", typeof(StringTerm)));
            Debug.Log("Q15: " + locale.GetTerm<StringTerm>("fake_term"));
            Debug.Log("Q16: " + locale.GetTermValue("fake_term"));
            Debug.Log("Q17: " + locale.GetTermValue("fake_term", typeof(string)));
            Debug.Log("Q18: " + locale.GetTermValue<string>("fake_term"));
            
            // have result
            var audioClipTerm = locale.GetTerm<AudioClipTerm>("test_audio_clip");
            Debug.Log("Q19: " + audioClipTerm.SmartValue);
            AudioSource.PlayClipAtPoint(audioClipTerm.SmartValue, Vector3.zero);

            // have result
            var audioClipValue = locale.GetTermValue<AudioClip>("test_audio_clip");
            Debug.Log("Q20: " + audioClipValue);
            AudioSource.PlayClipAtPoint(audioClipValue, Vector3.zero);
        }
    }
}