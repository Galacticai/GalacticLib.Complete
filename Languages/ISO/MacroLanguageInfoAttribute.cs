namespace GalacticLib.Languages.ISO;

/// <summary> Macro language info <see cref="Attribute"/> which may have child languages linked to it (<see cref="ChildrenCodes"/>) </summary>
[AttributeUsage(AttributeTargets.Field)]
public class MacroLanguageInfoAttribute : LanguageInfoAttribute {
    public MacroLanguageCode MacroCode { get; }
    /// <summary> Individual <see cref="LanguageCode"/>s under this <see cref="MacroLanguageName"/> </summary>
    public LanguageCode[] ChildrenCodes { get; }
    public MacroLanguageInfoAttribute(MacroLanguageCode macroCode, string description, params LanguageCode[] childrenCodes)
                : base(description) {
        MacroCode = macroCode;
        ChildrenCodes = childrenCodes;
    }

    public MacroLanguageName MacroName => MacroCode.ToMacroName();
    public LanguageCode Code => MacroCode.ToDistinctCode();
}
