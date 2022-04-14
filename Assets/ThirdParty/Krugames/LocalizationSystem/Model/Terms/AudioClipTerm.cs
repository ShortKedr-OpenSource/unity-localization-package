using System;
using Krugames.LocalizationSystem.Models.Attributes;
using Krugames.LocalizationSystem.Models.Terms;
using UnityEngine;

[assembly: RegisterLocaleTerm(typeof(AudioClipTerm))]

namespace Krugames.LocalizationSystem.Models.Terms {
    [CreateAssetMenu(fileName = "AudioClipTerm", menuName = "Localization/Terms/AudioClip", order = 0)]
    public class AudioClipTerm : LocaleTerm<AudioClip> {
    }
}