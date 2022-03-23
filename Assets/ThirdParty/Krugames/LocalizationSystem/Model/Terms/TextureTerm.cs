using Krugames.LocalizationSystem.Models.Attributes;
using Krugames.LocalizationSystem.Models.Terms;
using UnityEngine;

[assembly: RegisterLocaleTerm(typeof(TextureTerm), "Texture")]

namespace Krugames.LocalizationSystem.Models.Terms {
    [CreateAssetMenu(fileName = "TextureTerm", menuName = "Localization/Terms/Texture", order = 0)]
    public class TextureTerm : LocaleTerm<Texture> {
    }
}