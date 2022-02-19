namespace Krugames.LocalizationSystem.Linkers {
    /// <summary>
    /// Tells developer, that current linker is linker for custom system or component
    /// Custom components and systems are usually are parts of game-end logic code that will be used
    /// only in current project and not used across projects as universal system, or plugin system
    /// Do not use inheritance from this class for other purposes!*** to keep understanding clear.
    /// </summary>
    public abstract class CustomLinker : Linker {
    }
}