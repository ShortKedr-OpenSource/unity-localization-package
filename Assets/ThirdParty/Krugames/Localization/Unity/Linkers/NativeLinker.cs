namespace Krugames.LocalizationSystem.Linkers {
    /// <summary>
    /// Tells developer that current linker is native linker.
    /// Native Linkers always links Unity' base systems and components
    /// to localization system.
    /// Do not use inheritance from this class for other purposes!*** to keep understanding clear.
    /// </summary>
    public abstract class NativeLinker : Linker {
    }
}