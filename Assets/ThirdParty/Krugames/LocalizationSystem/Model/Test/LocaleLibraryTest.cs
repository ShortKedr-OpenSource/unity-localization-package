using Krugames.LocalizationSystem.Models.Terms;
using UnityEngine;

namespace Krugames.LocalizationSystem.Models {
    public class LocaleLibraryTest : MonoBehaviour {
        [SerializeField] private LocaleLibrary localeLibrary;

        private void Start() {
            
            localeLibrary.Initialize();
            
            localeLibrary.OnInitialized += OnInitialized;
            localeLibrary.OnLanguageChanged += LocaleLibraryOnOnLanguageChanged;
            
            AddDynamicLocales();
            
            Debug.Log("Current Language: " + localeLibrary.CurrentLanguage);
            Debug.Log("Current Locale: " + localeLibrary.CurrentLocale);

            // Dynamic locales
            var dynamicLocales = localeLibrary.DynamicLocales;
            var dynamicLocalesPrint = "Dynamic Locales:\n";
            for (int i = 0; i < dynamicLocales.Length; i++) {
                dynamicLocalesPrint += $"{dynamicLocales[i]} ({dynamicLocales[i].Language})\n";
            }
            Debug.Log(dynamicLocalesPrint);
            
            // Static locales
            var staticLocale = localeLibrary.StaticLocales;
            var staticLocalesPrint = "Static Locales:\n";
            for (int i = 0; i < staticLocale.Length; i++) {
                staticLocalesPrint += $"{staticLocale[i]} ({staticLocale[i].Language})\n";
            }
            Debug.Log(staticLocalesPrint);
            
            // Supported Languages
            var supportedLanguages = localeLibrary.SupportedLanguages;
            var supportedLanguagesPrint = "Supported Languages:\n";
            for (int i = 0; i < supportedLanguages.Length; i++) {
                supportedLanguagesPrint += $"{supportedLanguages[i]}\n";
            }
            Debug.Log(supportedLanguagesPrint);
            
            var validLocales = localeLibrary.ValidLocales;
            var validLocalePrint = "Valid Locales:\n";
            for (int i = 0; i < validLocales.Length; i++) {
                validLocalePrint += $"{validLocales[i]} ({validLocales[i].GetType()})\n";
            }
            Debug.Log(validLocalePrint);

            var baseValidLocale = localeLibrary.BaseValidLocale;
            Debug.Log($"Base Valid Locale: {baseValidLocale} ({baseValidLocale.Language})");
            
            // have result
            Debug.Log("Q1: "+localeLibrary.GetLocale(SystemLanguage.English));
            Debug.Log("Q2: "+localeLibrary.GetLocale(SystemLanguage.Russian));
            Debug.Log("Q3: "+localeLibrary.GetLocale(SystemLanguage.German));
            
            // null
            Debug.Log("Q4: "+localeLibrary.GetLocale(SystemLanguage.Chinese));
            Debug.Log("Q5: "+localeLibrary.GetLocale(SystemLanguage.Greek));

            // false
            var langChange = localeLibrary.SetLanguage(SystemLanguage.Russian);
            Debug.Log("Q6: " + langChange);
            
            // true
            langChange = localeLibrary.SetLanguage(SystemLanguage.English);
            Debug.Log("Q7: " + langChange);
            
            // true
            langChange = localeLibrary.SetLanguage(SystemLanguage.German);
            Debug.Log("Q8: " + langChange);
            
            // false
            langChange = localeLibrary.SetLanguage(SystemLanguage.Chinese);
            Debug.Log("Q9: " + langChange);
            
            // false
            langChange = localeLibrary.SetLanguage(SystemLanguage.German);
            Debug.Log("Q10: " + langChange);
            
            // true
            Debug.Log("Q11:"+ localeLibrary.SupportsLanguage(SystemLanguage.English));
            Debug.Log("Q12:"+ localeLibrary.SupportsLanguage(SystemLanguage.Russian));
            Debug.Log("Q13:"+ localeLibrary.SupportsLanguage(SystemLanguage.German));
            
            // false
            Debug.Log("Q14:"+ localeLibrary.SupportsLanguage(SystemLanguage.Chinese));
            Debug.Log("Q15:"+ localeLibrary.SupportsLanguage(SystemLanguage.Greek));
            Debug.Log("Q16:"+ localeLibrary.SupportsLanguage(SystemLanguage.Dutch));

            // have result
            localeLibrary.SetLanguage(SystemLanguage.English);
            var audioClip1 = localeLibrary.GetTerm("test_audio_clip_1");
            var audioClip2 = localeLibrary.GetTerm("test_audio_clip_2");
            var string1 = localeLibrary.GetTerm("test_string_1");
            var string2 = localeLibrary.GetTerm("test_string_2");
            var string3 = localeLibrary.GetTerm("test_string_3");
            var texture = localeLibrary.GetTerm("test_texture");
            var sprite = localeLibrary.GetTerm("test_sprite");
            var some = localeLibrary.GetTerm("some_term");
            Debug.Log($"Q17 English: \n" +
                      $"audioClip1: {audioClip1}\n" + //haveResult
                      $"audioClip2: {audioClip2}\n" + //haveResult
                      $"string1: {string1}\n" + //haveResult
                      $"string2: {string2}\n" + //haveResult
                      $"string3: {string3}\n" + //haveResult
                      $"texture: {texture}\n" + //haveResult
                      $"sprite: {sprite}\n" + //haveResult
                      $"some: {some}"); // null
            
            //haveResult
            audioClip1 = localeLibrary.GetTerm("test_audio_clip_1", typeof(AudioClipTerm));
            audioClip2 = localeLibrary.GetTerm("test_audio_clip_2", typeof(AudioClipTerm));
            string1 = localeLibrary.GetTerm("test_string_1", typeof(StringTerm));
            string2 = localeLibrary.GetTerm("test_string_2", typeof(StringTerm));
            string3 = localeLibrary.GetTerm("test_string_3", typeof(StringTerm));
            texture = localeLibrary.GetTerm("test_texture", typeof(TextureTerm));
            sprite = localeLibrary.GetTerm("test_sprite", typeof(SpriteTerm));
            some = localeLibrary.GetTerm("some_term", typeof(StringTerm));
            Debug.Log($"Q18 English: \n" +
                      $"audioClip1: {audioClip1}\n" + //haveResult
                      $"audioClip2: {audioClip2}\n" + //haveResult
                      $"string1: {string1}\n" + //haveResult
                      $"string2: {string2}\n" + //haveResult
                      $"string3: {string3}\n" + //haveResult
                      $"texture: {texture}\n" + //haveResult
                      $"sprite: {sprite}\n" + //haveResult
                      $"some: {some}\n"); //null
            
            //haveResult
            audioClip1 = localeLibrary.GetTerm<AudioClipTerm>("test_audio_clip_1");
            audioClip2 = localeLibrary.GetTerm<AudioClipTerm>("test_audio_clip_2");
            string1 = localeLibrary.GetTerm<StringTerm>("test_string_1");
            string2 = localeLibrary.GetTerm<StringTerm>("test_string_2");
            string3 = localeLibrary.GetTerm<StringTerm>("test_string_3");
            texture = localeLibrary.GetTerm<TextureTerm>("test_texture");
            sprite = localeLibrary.GetTerm<SpriteTerm>("test_sprite");
            some = localeLibrary.GetTerm<StringTerm>("some_term");
            Debug.Log($"Q19 English: \n" +
                      $"audioClip1: {audioClip1}\n" + //haveResult
                      $"audioClip2: {audioClip2}\n" + //haveResult
                      $"string1: {string1}\n" + //haveResult
                      $"string2: {string2}\n" + //haveResult
                      $"string3: {string3}\n" + //haveResult
                      $"texture: {texture}\n" + //haveResult
                      $"sprite: {sprite}\n" + //haveResult
                      $"some: {some}\n"); // null

            //haveResult
            var audioClip1Value = localeLibrary.GetTermValue("test_audio_clip_1");
            var audioClip2Value = localeLibrary.GetTermValue("test_audio_clip_2");
            var string1Value = localeLibrary.GetTermValue("test_string_1");
            var string2Value = localeLibrary.GetTermValue("test_string_2");
            var string3Value = localeLibrary.GetTermValue("test_string_3");
            var textureValue = localeLibrary.GetTermValue("test_texture");
            var spriteValue = localeLibrary.GetTermValue("test_sprite");
            var someValue = localeLibrary.GetTermValue("some_term");
            Debug.Log($"Q20 English: \n" +
                      $"audioClip1: {audioClip1Value}\n" + //haveResult
                      $"audioClip2: {audioClip2Value}\n" + //haveResult
                      $"string1: {string1Value}\n" + //haveResult
                      $"string2: {string2Value}\n" + //haveResult
                      $"string3: {string3Value}\n" + //haveResult
                      $"texture: {textureValue}\n" + //haveResult
                      $"sprite: {spriteValue}\n" + //haveResult
                      $"someValue: {someValue}\n"); //null
            
            //haveResult
            audioClip1Value = localeLibrary.GetTermValue("test_audio_clip_1", typeof(AudioClip));
            audioClip2Value = localeLibrary.GetTermValue("test_audio_clip_2", typeof(AudioClip));
            string1Value = localeLibrary.GetTermValue("test_string_1", typeof(string));
            string2Value = localeLibrary.GetTermValue("test_string_2", typeof(string));
            string3Value = localeLibrary.GetTermValue("test_string_3", typeof(string));
            textureValue = localeLibrary.GetTermValue("test_texture", typeof(Texture));
            spriteValue = localeLibrary.GetTermValue("test_sprite", typeof(Sprite));
            someValue = localeLibrary.GetTermValue("some_term", typeof(string));
            Debug.Log($"Q21 English: \n" +
                      $"audioClip1: {audioClip1Value}\n" + //haveResult
                      $"audioClip2: {audioClip2Value}\n" + //haveResult
                      $"string1: {string1Value}\n" + //haveResult
                      $"string2: {string2Value}\n" + //haveResult
                      $"string3: {string3Value}\n" + //haveResult
                      $"texture: {textureValue}\n" + //haveResult
                      $"sprite: {spriteValue}\n" + //haveResult
                      $"someValue: {someValue}"); //null
            
            //haveResult
            audioClip1Value = localeLibrary.GetTermValue<AudioClip>("test_audio_clip_1");
            audioClip2Value = localeLibrary.GetTermValue<AudioClip>("test_audio_clip_2");
            string1Value = localeLibrary.GetTermValue<string>("test_string_1");
            string2Value = localeLibrary.GetTermValue<string>("test_string_2");
            string3Value = localeLibrary.GetTermValue<string>("test_string_3");
            textureValue = localeLibrary.GetTermValue<Texture>("test_texture");
            spriteValue = localeLibrary.GetTermValue<Sprite>("test_sprite");
            someValue = localeLibrary.GetTermValue<string>("some_term");
            Debug.Log($"Q22 English: \n" +
                      $"audioClip1: {audioClip1Value}\n" + //haveResult
                      $"audioClip2: {audioClip2Value}\n" + //haveResult
                      $"string1: {string1Value}\n" + //haveResult
                      $"string2: {string2Value}\n" + //haveResult
                      $"string3: {string3Value}\n" + //haveResult
                      $"texture: {textureValue}\n" + //haveResult
                      $"sprite: {spriteValue}\n" + //haveResult
                      $"someValue: {someValue}"); //null
            
            
            // null
            some = localeLibrary.GetTerm("test_string_1", typeof(SpriteTerm));
            Debug.Log("Q23: " + some);
            
            // null
            some = localeLibrary.GetTerm<SpriteTerm>("test_string_1");
            Debug.Log("Q24: " + some);
            
            // null
            someValue = localeLibrary.GetTermValue("test_string_1", typeof(Sprite));
            Debug.Log("Q25: " + some);
            
            // null
            someValue = localeLibrary.GetTermValue<Sprite>("test_string_1");
            Debug.Log("Q26: " + some);
            
            // have result
            localeLibrary.SetLanguage(SystemLanguage.Russian);
            audioClip1 = localeLibrary.GetTerm("test_audio_clip_1");
            audioClip2 = localeLibrary.GetTerm("test_audio_clip_2");
            string1 = localeLibrary.GetTerm("test_string_1");
            string2 = localeLibrary.GetTerm("test_string_2");
            string3 = localeLibrary.GetTerm("test_string_3");
            texture = localeLibrary.GetTerm("test_texture");
            sprite = localeLibrary.GetTerm("test_sprite");
            some = localeLibrary.GetTerm("some_term");
            Debug.Log($"Q27 Russian: \n" +
                      $"audioClip1: {audioClip1}\n" + //haveResult
                      $"audioClip2: {audioClip2}\n" + //haveResult
                      $"string1: {string1}\n" + //haveResult
                      $"string2: {string2}\n" + //haveResult
                      $"string3: {string3}\n" + //haveResult
                      $"texture: {texture}\n" + //haveResult
                      $"sprite: {sprite}\n" + //haveResult
                      $"some: {some}"); // null
            
            //haveResult
            audioClip1 = localeLibrary.GetTerm("test_audio_clip_1", typeof(AudioClipTerm));
            audioClip2 = localeLibrary.GetTerm("test_audio_clip_2", typeof(AudioClipTerm));
            string1 = localeLibrary.GetTerm("test_string_1", typeof(StringTerm));
            string2 = localeLibrary.GetTerm("test_string_2", typeof(StringTerm));
            string3 = localeLibrary.GetTerm("test_string_3", typeof(StringTerm));
            texture = localeLibrary.GetTerm("test_texture", typeof(TextureTerm));
            sprite = localeLibrary.GetTerm("test_sprite", typeof(SpriteTerm));
            some = localeLibrary.GetTerm("some_term", typeof(StringTerm));
            Debug.Log($"Q28 Russian: \n" +
                      $"audioClip1: {audioClip1}\n" + //haveResult
                      $"audioClip2: {audioClip2}\n" + //haveResult
                      $"string1: {string1}\n" + //haveResult
                      $"string2: {string2}\n" + //haveResult
                      $"string3: {string3}\n" + //haveResult
                      $"texture: {texture}\n" + //haveResult
                      $"sprite: {sprite}\n" + //haveResult
                      $"some: {some}\n"); //null
            
            //haveResult
            audioClip1 = localeLibrary.GetTerm<AudioClipTerm>("test_audio_clip_1");
            audioClip2 = localeLibrary.GetTerm<AudioClipTerm>("test_audio_clip_2");
            string1 = localeLibrary.GetTerm<StringTerm>("test_string_1");
            string2 = localeLibrary.GetTerm<StringTerm>("test_string_2");
            string3 = localeLibrary.GetTerm<StringTerm>("test_string_3");
            texture = localeLibrary.GetTerm<TextureTerm>("test_texture");
            sprite = localeLibrary.GetTerm<SpriteTerm>("test_sprite");
            some = localeLibrary.GetTerm<StringTerm>("some_term");
            Debug.Log($"Q29 Russian: \n" +
                      $"audioClip1: {audioClip1}\n" + //haveResult
                      $"audioClip2: {audioClip2}\n" + //haveResult
                      $"string1: {string1}\n" + //haveResult
                      $"string2: {string2}\n" + //haveResult
                      $"string3: {string3}\n" + //haveResult
                      $"texture: {texture}\n" + //haveResult
                      $"sprite: {sprite}\n" + //haveResult
                      $"some: {some}\n"); // null

            //haveResult
            audioClip1Value = localeLibrary.GetTermValue("test_audio_clip_1");
            audioClip2Value = localeLibrary.GetTermValue("test_audio_clip_2");
            string1Value = localeLibrary.GetTermValue("test_string_1");
            string2Value = localeLibrary.GetTermValue("test_string_2");
            string3Value = localeLibrary.GetTermValue("test_string_3");
            textureValue = localeLibrary.GetTermValue("test_texture");
            spriteValue = localeLibrary.GetTermValue("test_sprite");
            someValue = localeLibrary.GetTermValue("some_term");
            Debug.Log($"Q30 Russian: \n" +
                      $"audioClip1: {audioClip1Value}\n" + //haveResult
                      $"audioClip2: {audioClip2Value}\n" + //haveResult
                      $"string1: {string1Value}\n" + //haveResult
                      $"string2: {string2Value}\n" + //haveResult
                      $"string3: {string3Value}\n" + //haveResult
                      $"texture: {textureValue}\n" + //haveResult
                      $"sprite: {spriteValue}\n" + //haveResult
                      $"someValue: {someValue}\n"); //null
            
            //haveResult
            audioClip1Value = localeLibrary.GetTermValue("test_audio_clip_1", typeof(AudioClip));
            audioClip2Value = localeLibrary.GetTermValue("test_audio_clip_2", typeof(AudioClip));
            string1Value = localeLibrary.GetTermValue("test_string_1", typeof(string));
            string2Value = localeLibrary.GetTermValue("test_string_2", typeof(string));
            string3Value = localeLibrary.GetTermValue("test_string_3", typeof(string));
            textureValue = localeLibrary.GetTermValue("test_texture", typeof(Texture));
            spriteValue = localeLibrary.GetTermValue("test_sprite", typeof(Sprite));
            someValue = localeLibrary.GetTermValue("some_term", typeof(string));
            Debug.Log($"Q31 Russian: \n" +
                      $"audioClip1: {audioClip1Value}\n" + //haveResult
                      $"audioClip2: {audioClip2Value}\n" + //haveResult
                      $"string1: {string1Value}\n" + //haveResult
                      $"string2: {string2Value}\n" + //haveResult
                      $"string3: {string3Value}\n" + //haveResult
                      $"texture: {textureValue}\n" + //haveResult
                      $"sprite: {spriteValue}\n" + //haveResult
                      $"someValue: {someValue}"); //null
            
            //haveResult
            audioClip1Value = localeLibrary.GetTermValue<AudioClip>("test_audio_clip_1");
            audioClip2Value = localeLibrary.GetTermValue<AudioClip>("test_audio_clip_2");
            string1Value = localeLibrary.GetTermValue<string>("test_string_1");
            string2Value = localeLibrary.GetTermValue<string>("test_string_2");
            string3Value = localeLibrary.GetTermValue<string>("test_string_3");
            textureValue = localeLibrary.GetTermValue<Texture>("test_texture");
            spriteValue = localeLibrary.GetTermValue<Sprite>("test_sprite");
            someValue = localeLibrary.GetTermValue<string>("some_term");
            Debug.Log($"Q32 Russian: \n" +
                      $"audioClip1: {audioClip1Value}\n" + //haveResult
                      $"audioClip2: {audioClip2Value}\n" + //haveResult
                      $"string1: {string1Value}\n" + //haveResult
                      $"string2: {string2Value}\n" + //haveResult
                      $"string3: {string3Value}\n" + //haveResult
                      $"texture: {textureValue}\n" + //haveResult
                      $"sprite: {spriteValue}\n" + //haveResult
                      $"someValue: {someValue}"); //null
            
            // null
            some = localeLibrary.GetTerm("test_string_1", typeof(SpriteTerm));
            Debug.Log("Q33: " + some);
            
            // null
            some = localeLibrary.GetTerm<SpriteTerm>("test_string_1");
            Debug.Log("Q34: " + some);
            
            // null
            someValue = localeLibrary.GetTermValue("test_string_1", typeof(Sprite));
            Debug.Log("Q35: " + some);
            
            // null
            someValue = localeLibrary.GetTermValue<Sprite>("test_string_1");
            Debug.Log("Q36: " + some);

        }

        private void LocaleLibraryOnOnLanguageChanged(LocaleLibrary localelibrary, SystemLanguage oldlanguage, SystemLanguage newlanguage) {
            Debug.Log($"Language was changed from \"{oldlanguage}\" to \"{newlanguage}\"");
        }

        private void OnInitialized(LocaleLibrary localelibrary) {
            Debug.Log("Locale library initialized");
        }

        private void AddDynamicLocales() {
            
        }
    }
}