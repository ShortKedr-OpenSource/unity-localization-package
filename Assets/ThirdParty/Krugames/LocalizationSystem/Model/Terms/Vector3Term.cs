using Krugames.LocalizationSystem.Models.Attributes;
using Krugames.LocalizationSystem.Models.Terms;
using UnityEngine;

[assembly: RegisterLocaleTerm(typeof(Vector3Term))]

namespace Krugames.LocalizationSystem.Models.Terms {
    [CreateAssetMenu(fileName = "Vector3Term", menuName = "Localization/Terms/Vector3", order = 0)]
    public class Vector3Term : LocaleTerm<Vector3> {
    }
}