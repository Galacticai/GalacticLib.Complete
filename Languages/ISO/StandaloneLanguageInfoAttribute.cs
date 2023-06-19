using System.ComponentModel;

namespace GalacticLib.Languages.ISO;

/// <summary> Standalone language info <see cref="Attribute"/> </summary>
[AttributeUsage(AttributeTargets.Field)]
public class LanguageInfoAttribute : DescriptionAttribute {
    public LanguageInfoAttribute(string description)
            : base(description) { }
}