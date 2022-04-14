using Krugames.LocalizationSystem.Models.Attributes;
using Krugames.LocalizationSystem.Models.Terms;
using UnityEngine;

[assembly: RegisterLocaleTerm(typeof(SpriteTerm))]

namespace Krugames.LocalizationSystem.Models.Terms {
    [CreateAssetMenu(fileName = "SpriteTerm", menuName = "Localization/Terms/Sprite", order = 0)]
    public class SpriteTerm : LocaleTerm<Sprite> {
    }
}