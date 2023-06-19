namespace GalacticLib.Languages.ISO;

/// <summary> Child language info <see cref="Attribute"/> which is linked to a macro lanugage (<see cref="MacroCode"/>) </summary>
[AttributeUsage(AttributeTargets.Field)]
public class ChildLanguageInfoAttribute : LanguageInfoAttribute {
    public MacroLanguageCode MacroCode { get; }
    public ChildLanguageInfoAttribute(MacroLanguageCode macroCode, string description)
            : base(description) {
        MacroCode = macroCode;
    }
}
