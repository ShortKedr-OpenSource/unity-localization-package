using Krugames.LocalizationSystem.Models.Attributes;
using Krugames.LocalizationSystem.Models.Terms;
using UnityEngine;

[assembly: RegisterLocaleTerm(typeof(RegisteredNotUsedTerm))]

namespace Krugames.LocalizationSystem.Models.Terms {
    public class RegisteredNotUsedTerm : LocaleTerm<Quaternion>{
    }
}