using System.Diagnostics;
using Krugames.LocalizationSystem.Models.Terms;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Krugames.LocalizationSystem.Models.Utility.Editor {
    public class UtilityTest : MonoBehaviour {
        private void Start() {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var stringType = LocaleTermUtility.GetValueTypeOfGenericTermType<StringTerm>();
            var spriteType = LocaleTermUtility.GetValueTypeOfGenericTermType<SpriteTerm>();
            var audioClipType = LocaleTermUtility.GetValueTypeOfGenericTermType<AudioClipTerm>();
            var textureType = LocaleTermUtility.GetValueTypeOfGenericTermType<TextureTerm>();
            stopwatch.Stop();
            Debug.Log("Time ms: " + stopwatch.Elapsed.TotalMilliseconds);
            Debug.Log("Ticks: " + stopwatch.ElapsedTicks);
            
            Debug.Log(stringType);
            Debug.Log(spriteType);
            Debug.Log(audioClipType);
            Debug.Log(textureType);
        }
    }
}